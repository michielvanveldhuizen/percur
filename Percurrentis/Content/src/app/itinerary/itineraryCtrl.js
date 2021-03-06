﻿(function () {
    'use strict';

    angular.module('app').controller('itineraryCtrl',
        ['$scope', '$location', 'travelRequestService', itineraryCtrl]);

    function itineraryCtrl($scope, $location, travelRequestService) {
        travelRequestService.getTravelRequests(true)
        .then(function (query) {
            $scope.allRequests = query.results;

            initializeFilters();

            $scope.currentFilter = _.find($scope.filters, function (filter) {
                return filter.title == $location.hash();
            }) || $scope.filters.current;

            $scope.currentFilter.filter();      
        });

        $scope.go = function (path) {
            $location.path(path);
            $location.hash('');
        };

        function initializeFilters() {
            function Filter(filterFn, title, order, icon, tooltip) {
                this.filterFn = filterFn || function () {
                    return [];
                };
                this.title = title || "Default Filter";
                this.icon = icon;
                this.tooltip = tooltip;
                this.order = order;
            }

            Filter.prototype = {
                filter: function () {
                    $scope.requests = this.filterFn();
                    $scope.currentFilter = this;
                    $location.hash(this.title);
                },
                count: function () {
                    return this.filterFn().length;
                }
            };

            $scope.filters = {
                all: new Filter(function () {
                    return _.filter($scope.allRequests, function (req) {
                        return req.IsDeleted == false;
                    });
                }, 'All', 0, 'fa-bars', 'View both open and archived itineraries.'),
                current: new Filter(function () {
                    return _.filter($scope.allRequests, function (req) {
                        return req.IsArchived == false && req.IsDeleted == false && req.IsApproved == 0 && req.IsFinal == false;
                    });
                }, 'Awaiting', 1, 'fa-inbox', 'View itineraries that are awaiting approval.'),
                approved: new Filter(function () {
                    return _.filter($scope.allRequests, function (req) {
                        return req.IsApproved == 2 && req.IsDeleted == false && req.IsFinal == false;
                    });
                }, 'Approved', 2, 'fa-check', 'View approved itineraries.'),
                rejected: new Filter(function () {
                    return _.filter($scope.allRequests, function (req) {
                        return req.IsApproved == 1 && req.IsDeleted == false && req.IsFinal == false;
                    });
                }, 'Rejected', 3, 'fa-times', 'View rejected itineraries.'),
                archived: new Filter(function () {
                    return _.filter($scope.allRequests, function (req) {
                        return req.IsArchived == true && req.IsDeleted == false && req.IsFinal == false;
                    });
                }, 'Archived', 4, 'fa-archive', 'View archived itineraries.'),
                deleted: new Filter(function () {
                    return _.filter($scope.allRequests, function (req) {
                        return req.IsDeleted == true && req.IsFinal == false;
                    });
                }, 'Deleted', 5, 'fa-ban', 'View deleted itineraries.'),
                final: new Filter(function () {
                    return _.filter($scope.allRequests, function (req) {
                        return req.IsFinal == true;
                    });
                }, 'Final', 5, 'fa-ban', 'View committed itineraries.')
            }
        }

        $scope.toggleArchive = function (request) {
            if (travelRequestService.toggleArchive(request)) {
                $scope.addArchiveAlert();
            }

            travelRequestService.saveChanges();
            $scope.currentFilter.filter();
        };

        $scope.toggleDelete = function (request) {
            if (travelRequestService.toggleDelete(request)) {
                $scope.addDeleteAlert();
            }

            travelRequestService.saveChanges();
            $scope.currentFilter.filter();
        };

        /* == Alerts =================================================== */
        //-- Archive ------------------------------------------------------
        $scope.showArchiveAlert = false;

        $scope.addArchiveAlert = function () {
            if (!$scope.showArchiveAlert) {
                $scope.showArchiveAlert = true;
            }
        };

        $scope.closeArchiveAlert = function () {
            $scope.showArchiveAlert = false;
        };

        //-- Delete ------------------------------------------------------
        $scope.showDeleteAlert = false;

        $scope.addDeleteAlert = function () {
            if (!$scope.showDeleteAlert) {
                $scope.showDeleteAlert = true;
            }
        };

        $scope.closeDeleteAlert = function () {
            $scope.showDeleteAlert = false;
        };

        /* == Text ===================================================== */
        $scope.deleteBtnText = function (isDeleted) {
            return isDeleted ? 'Recover' : 'Delete';
        };
    }

    angular.module('app').controller('itineraryDetailCtrl',
        ['$scope', '$route', '$location', 'travelRequestService', "modalService", itineraryDetailCtrl]);

    function itineraryDetailCtrl($scope, $route, $location, travelRequestService, modalService) {
        var request;
        $scope.totalPrice = 0;
        $scope.hasOpenProposal = false;
        $scope.editInProgress = false;
        var items = [];

        travelRequestService.getTravelRequestByHash($route.current.params.Hash)
        .then(function (query) {
            $scope.request = query.results[0];
            $scope.total = 0;
            travelRequestService.getProposalsForItinerary($scope.request.Id)
            .then(function (query) {
                $scope.proposals = query.results;
                $scope.hasOpenProposal = $scope.proposals.some(function (p) {
                    return p.IsApproved === 0;
                });
            });
            return query.results[0];
        })
        .then(function (request) {
            return travelRequestService.getEmployeeByObjectGuid(request.SuperiorID);
        })
        .then(function (employee) {
            $scope.supervisorName = employee.userName;
        });

        $scope.isTravelAgency = function () {
            return roles.TravelAgency;
        }

        // Calculate total cost for the current items in the itinerary.
        $scope.calcTotal = function () {
            var sum = 0;
            jQuery(".euro_cost").each(function () {
                var str = jQuery(this).text();
                str = str.replace(/,/g, "");
                sum += parseFloat(str, 10);
            });
            return sum;
        }

        $scope.go = function (path, hash) {
            $location.path(path);
            $location.hash(hash);
        };

        travelRequestService.getAirports().then(function (query) {
            $scope.airports = query.results;

        });

        
        travelRequestService.getAddresses().then(function (query) {
            $scope.addresses = query.results;

        });

        travelRequestService.getServiceCompanies().then(function (query) {
            $scope.serviceCompanies = query.results;
        });

        function show(request) {
            if($scope.request.SuperiorID == ownGuid){
                $scope.TRArequest = $scope.request.TravelRequestApproval;
                if ($scope.request.IsApproved == 0) {
                    $("footer").show();
                }
            }
        }



        $scope.startProposalWizard = function(request)
        {
            $location.path('/ProposalWizard/' + request.Hash);
        }


        $scope.CommitItinerary = function () {
            modalService.open("Commit this Itinerary?", "Committing this itinerary means that it will be marked 'final' which means it can no longer be edited. Continue?", 
            function succes() {
                // Make the Itinerary final, which means it cant be edited anymore.

            },
            function cancel() {
                // Thats fine, just go back.
                
            });
        };


        // Calculate the number of days between start and end, primarily used for Accommodations.
        $scope.CalculateStayDuration = function (start, end) {
            var completeDay = 24 * 60 * 60 * 1000;
            var diffDays = Math.round(Math.abs((start.getTime() - end.getTime()) / (completeDay)));
            return diffDays;
        };

        $scope.onApprove = function () {
            $scope.mode = 'approve';
        };

        $scope.onReject = function () {
            $scope.mode = 'reject';
        };
        $scope.onApproveConfirm = function () {
            $scope.mode = 'approveConfirmed';
            $scope.TRArequest.Flag = true;
            $scope.TRArequest.HasApproved = 2;
            $scope.TRArequest.Note = angular.copy($scope.comments);

            travelRequestService.saveChanges($scope.TRArequest, undefined, angular.noop, angular.noop);
            reloadPage();
        };

        $scope.onRejectConfirm = function () {
            $scope.mode = 'rejectConfirmed';
            $scope.TRArequest.Flag = true;
            $scope.TRArequest.HasApproved = 1;
            $scope.TRArequest.Note = angular.copy($scope.comments);
            travelRequestService.saveChanges($scope.TRArequest, undefined, angular.noop, angular.noop);
            reloadPage();
        };

        $scope.onCancel = function () {
            $scope.mode = 'init';
        };

        $scope.mode = 'init';

        function reloadPage() {
            $route.reload();
            /*$("#page-reload-notification").html("This page will reload in:  to set the state");
            window.setTimeout(function () { document.location.reload(true); }, 10000);*/
        }
    }
})();
