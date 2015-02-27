(function () {
    'use strict';

    angular.module('app').controller('requestCtrl',
        ['$scope', '$location', 'travelRequestService', requestCtrl]);

    function requestCtrl($scope, $location, travelRequestService) {
        travelRequestService.getTravelRequests()
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
                }, 'All', 0, 'fa-bars', 'View both open and archived travel requests.'),
                current: new Filter(function () {
                    return _.filter($scope.allRequests, function (req) {
                        return req.IsArchived == false && req.IsDeleted == false && req.IsApproved == 0;
                    });
                }, 'Awaiting', 1, 'fa-inbox', 'View requests that are awaiting approval.'),
                approved: new Filter(function () {
                    return _.filter($scope.allRequests, function (req) {
                        return req.IsApproved == 2 && req.IsDeleted == false;
                    });
                }, 'Approved', 2, 'fa-check', 'View approved travel requests.'),
                rejected: new Filter(function () {
                    return _.filter($scope.allRequests, function (req) {
                        return req.IsApproved == 1 && req.IsDeleted == false;
                    });
                }, 'Rejected', 3, 'fa-times', 'View rejected travel requests.'),
                archived: new Filter(function () {
                    return _.filter($scope.allRequests, function (req) {
                        return req.IsArchived == true && req.IsDeleted == false;
                    });
                }, 'Archived', 4, 'fa-archive', 'View archived travel requests.'),
                deleted: new Filter(function () {
                    return _.filter($scope.allRequests, function (req) {
                        return req.IsDeleted == true;
                    });
                }, 'Deleted', 5, 'fa-ban', 'View deleted travel requests.')
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
    
    angular.module('app').controller('requestStatsCtrl',
        ['$scope', '$location', 'travelRequestService', requestStatsCtrl]);

    function requestStatsCtrl($scope, $location, travelRequestService) {

        $scope.awaiting = [];
        $scope.approved = [];
        $scope.denied = [];
        $scope.total = 0;
        travelRequestService.getTravelRequests()
        .then(function (query) {
            var items = query.results;
            angular.forEach(items, function (value, key) {
                if(value.IsApproved == '0')
                {
                    $scope.awaiting.push(value);
                }
                else if (value.IsApproved == '1') {
                    $scope.approved.push(value);
                }
                else if (value.IsApproved == '2') {
                    $scope.denied.push(value);
                }
                $scope.total++;
            });
            console.log($scope);
        });
    }

    angular.module('app').controller('requestDetailCtrl',
        ['$scope', '$route', 'travelRequestService', requestDetailCtrl]);

    function requestDetailCtrl($scope, $route, travelRequestService) {
        travelRequestService.getTravelRequestByHash($route.current.params.Hash)
        .then(function (query) {
            $scope.request = query.results[0];
            
            $scope.c = "a73d1a5e-b640-467e-8583-e4b52cfae437";
            travelRequestService.getCountryById($scope.request.CountryID)
            .then(function (result) {
                $scope.request.Country = result.results[0];
                show($scope.request);
            });
            return query.results[0];
        })
        .then(function (request) {
            return travelRequestService.getEmployeeByObjectGuid(request.SuperiorID);
        })
        .then(function (employee) {
            $scope.supervisorName = employee.userName;
        });

        function show(request) {
            var showFooter = false;
            
            if($scope.request.SuperiorID == ownGuid){
                showFooter = true;
            }
            
            if ($scope.request.Country.Name == "Romania" && ownGuid == $scope.c) {
                showFooter = true;
                if ($scope.request.TravelRequestApproval.COOApproved != 0) {
                    showFooter = false;
                }
            } else {
                if ($scope.request.TravelRequestApproval.HasApproved != 0) {
                    showFooter = false;
                }
            }
            if (showFooter) {
                $scope.TRArequest = $scope.request.TravelRequestApproval;
                if ($scope.request.IsApproved == 0) {
                    $("footer").show();
                }
            }
            
        }

        $scope.onApprove = function () {
            $scope.mode = 'approve';

           
        };
        $scope.onReject = function () {
            $scope.mode = 'reject';
        };
        $scope.onApproveConfirm = function () {
            $scope.mode = 'approveConfirmed';                
            $scope.TRArequest.Flag = true;
            if (ownGuid == $scope.c && $scope.request.Country.Name == "Romania" && $scope.c != $scope.request.SuperiorID) {
                $scope.TRArequest.COOApproved = 2;
            } else {
                $scope.TRArequest.HasApproved = 2;
            }
            $scope.TRArequest.Note = angular.copy($scope.comments);
            $scope.TRArequest.ApprovedBy = ownGuid;
            travelRequestService.saveChanges($scope.TRArequest, undefined, angular.noop, angular.noop);
            reloadPage();
        };

        $scope.onRejectConfirm = function () {
            $scope.mode = 'rejectConfirmed';
            $scope.TRArequest.Flag = true;
            if (ownGuid == $scope.c && $scope.request.Country.Name == "Romania" && $scope.c != $scope.request.SuperiorID) {
                $scope.TRArequest.COOApproved = 1;
            } else {
                $scope.TRArequest.HasApproved = 1;
            }
            $scope.TRArequest.ApprovedBy = ownGuid;
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
            //Maybe set a time out, test if that has to be done.

            /*$("#page-reload-notification").html("This page will reload in:  to set the state");
            window.setTimeout(function () { document.location.reload(true); }, 10000);*/
        }
    }
})();
