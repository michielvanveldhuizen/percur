(function () {
    'use strict';

    angular.module('app').controller('requestCtrl',
        ['$scope', '$location', 'travelRequestService', requestCtrl]);

    function requestCtrl($scope, $location, travelRequestService) {
        //Getting employees and filling the dbGuidToName
        travelRequestService.getEmployees().then(function (query) {
            angular.forEach(query.results, function (value, key) {
                dbGuidToName[value.objectGuid] = value.userName;
            });
        });

        //Getting all travel Requests
        travelRequestService.getTravelRequests(false)
        .then(function (query) {
            $scope.allRequests = query.results;

            initializeFilters();

            //Setting the current fillter
            $scope.currentFilter = _.find($scope.filters, function (filter) {
                //setting the titel
                return filter.title == $location.hash();
            }) || $scope.filters.current;

            $scope.currentFilter.filter();
        });

       
        //Used for onclick to go to another page
        $scope.go = function (path) {
            $location.path(path);
            $location.hash('');
        };

        $scope.c = "a73d1a5e-b640-467e-8583-e4b52cfae437";
        
        function initializeFilters() {
            //Prototyping the filter
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
                    
                    //setting the superiorName, BUG: the dbGuidToName might not be filled yet so this works like 80% of the time
                    angular.forEach($scope.requests, function (value, key) {
                        if (typeof dbGuidToName[value.SuperiorID] != typeof undefined) {
                            $scope.requests[key].superiorName = dbGuidToName[value.SuperiorID];
                        }
                    });
                    
                    $scope.currentFilter = this;
                    $location.hash(this.title);
                },
                count: function () {
                    return this.filterFn().length;
                }
            };

            //Filters for Travel Agency
            if (roles.TravelAgency) {
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
                    }, 'Deleted', 5, 'fa-ban', 'View deleted travel requests.'),
                }
            } else {
                //Filters for non travel agency (project managers)
                //if(isEmployee)
                if (true == true) {
                    console.log("banana");
                    $scope.filters = {
                        all: new Filter(function () {
                            return _.filter($scope.allRequests, function (req) {
                                return req.IsDeleted == false && userName == req.ApplicantID;
                            });
                        }, 'All', 0, 'fa-bars', 'View both open and archived travel requests.'),
                        current: new Filter(function () {
                            return _.filter($scope.allRequests, function (req) {
                                return req.IsArchived == false && req.IsDeleted == false && req.IsApproved == 0 && isMyRequest(req);
                            });
                        }, 'Awaiting', 1, 'fa-inbox', 'View requests that are awaiting approval.'),
                        approved: new Filter(function () {
                            return _.filter($scope.allRequests, function (req) {
                                return req.IsApproved == 2 && req.IsDeleted == false && isMyRequest(req);
                            });
                        }, 'Approved', 2, 'fa-check', 'View approved travel requests.'),
                        rejected: new Filter(function () {
                            return _.filter($scope.allRequests, function (req) {
                                return req.IsApproved == 1 && req.IsDeleted == false && isMyRequest(req);
                            });
                        }, 'Rejected', 3, 'fa-times', 'View rejected travel requests.'),
                        archived: new Filter(function () {
                            return _.filter($scope.allRequests, function (req) {
                                return req.IsArchived == true && req.IsDeleted == false && isMyRequest(req);
                            });
                        }, 'Archived', 4, 'fa-archive', 'View archived travel requests.'),
                        deleted: new Filter(function () {
                            return _.filter($scope.allRequests, function (req) {
                                return req.IsDeleted == true && isMyRequest(req);
                            });
                        }, 'Deleted', 5, 'fa-ban', 'View deleted travel requests.'),
                    }
                }
                
                //Manager
                if (true == false) {
                    $scope.filters = {
                        all: new Filter(function () {
                            return _.filter($scope.allRequests, function (req) {
                                return req.IsDeleted == false && ownGuid == req.SuperiorID;
                            });
                        }, 'All', 0, 'fa-bars', 'View both open and archived travel requests.'),
                        current: new Filter(function () {
                            return _.filter($scope.allRequests, function (req) {
                                return req.IsArchived == false && req.IsDeleted == false && req.IsApproved == 0 && ownGuid == req.SuperiorID;
                            });
                        }, 'Awaiting', 1, 'fa-inbox', 'View requests that are awaiting approval.'),
                        approved: new Filter(function () {
                            return _.filter($scope.allRequests, function (req) {
                                if (ownGuid == $scope.c) {
                                    if (req.CountryID == 173) {
                                        return req.IsApproved == 2 && req.IsDeleted == false
                                    }
                                }
                                return req.IsApproved == 2 && req.IsDeleted == false && ownGuid == req.SuperiorID;
                            });
                        }, 'Approved', 2, 'fa-check', 'View approved travel requests.'),
                        rejected: new Filter(function () {
                            return _.filter($scope.allRequests, function (req) {
                                if (ownGuid == $scope.c) {
                                    if (req.CountryID == 173) {
                                        return req.IsApproved == 1 && req.IsDeleted == false && ownGuid == req.SuperiorID;
                                    }
                                }
                                return req.IsApproved == 1 && req.IsDeleted == false && ownGuid == req.SuperiorID;;
                            });
                        }, 'Rejected', 3, 'fa-times', 'View rejected travel requests.'),
                        archived: new Filter(function () {
                            return _.filter($scope.allRequests, function (req) {
                                return req.IsArchived == true && req.IsDeleted == false && ownGuid == req.SuperiorID;;
                            });
                        }, 'Archived', 4, 'fa-archive', 'View archived travel requests.'),
                        deleted: new Filter(function () {
                            return _.filter($scope.allRequests, function (req) {
                                return req.IsDeleted == true && ownGuid == req.SuperiorID && ownGuid == req.SuperiorID;;
                            });
                        }, 'Deleted', 5, 'fa-ban', 'View deleted travel requests.'),
                    }
                }
            }
        }

        //Check if request is made by me or that i am the traveller
        function isMyRequest(req) {
            //by me
            if (req.ApplicantID.toLowerCase() == userName.toLowerCase()) {
                return true;
            }

            //for me as traveller
            angular.forEach(req.TravelRequest_RequestTravellers, function (value, key) {
                if(userName.toLowerCase() == value.RequestTraveller.firstName+" "+value.RequestTraveller.lastName){
                    return true;
                }
                if (userName.toLowerCase() == value.RequestTraveller.fullName) {
                    return true;
                }
            });

            return false;
        }

        //Archiving a travel request
        $scope.toggleArchive = function (request) {
            if (travelRequestService.toggleArchive(request)) {
                $scope.addArchiveAlert();
            }

            travelRequestService.saveChanges();
            $scope.currentFilter.filter();
        };

        //Deleting a travel request
        $scope.toggleDelete = function (request) {
            if (travelRequestService.toggleDelete(request)) {
                $scope.addDeleteAlert();
            }

            travelRequestService.saveChanges();
            $scope.currentFilter.filter();
        };

        $scope.isTravelAgency = function () {
            return roles.TravelAgency;
        }

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
        });
    }

    //For the detail page
    angular.module('app').controller('requestDetailCtrl',
        ['$scope', '$route', '$location', 'travelRequestService', 'modalService', requestDetailCtrl]);

    function requestDetailCtrl($scope, $route, $location, travelRequestService, modalService) {
        //Getting all the page details
        travelRequestService.getTravelRequestByHash($route.current.params.Hash)
        .then(function (query) {
            $scope.request = query.results[0];

            console.log($scope.request);
            
            if (query.results[0] == null) {
                failedToLoadDetailPage();
            } else {
                $scope.c = "a73d1a5e-b640-467e-8583-e4b52cfae437";
                //Getting the country
                travelRequestService.getCountryById($scope.request.CountryID)
                .then(function (result) {
                    $scope.request.Country = result.results[0];

                    //For the approval box
                    showApproveBoxIfNeeded($scope.request);
                });

                //getting the Country
                travelRequestService.getCountryById($scope.request.CustomerOrProspect.Address.CountryRegionID)
               .then(function (result) {
                   $scope.request.CustomerOrProspect.Address.CountryRegion = result.results[0];
               });

                //Getting the company
                angular.forEach($scope.request.TravelRequest_RequestTravellers, function (value, key) {
                    travelRequestService.getCompanyById(value.RequestTraveller.CompanyID).then(function (result) {
                        
                    });
                });

                //Getting the supervisor
                travelRequestService.getEmployeeByObjectGuid(query.results[0].SuperiorID)
                .then(function (employee) {
                    $scope.supervisorName = employee.userName;
                })
                .then(function ()  {
                    if($scope.request.IsItinerary == true)
                    {
                        modalService.open("Redirecting...",
                                          "The item you requested is not a travel request. You will be redirected to the homepage.",
                                          function () {
                            $location.path('/#/');
                                          },
                                          function () {},
                        '/TravelAgency/Content/src/app/modal/singleBtnSet.tpl.html');
                    }
                });
                
            }
        });

        //To show approve/reject box
        function showApproveBoxIfNeeded(request) {
            var showFooter = false;
            
            if($scope.request.SuperiorID == ownGuid){
                showFooter = true;
            }

            if (roles.ProjectManager) {
                showFooter = true;
            }

            //Check if already approved/rejected
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

        //When approved is pressed in the approving dialog
        $scope.onApproveConfirm = function () {
            $scope.mode = 'approveConfirmed';                
            $scope.TRArequest.Flag = true;
            
            //Check if self is COO
            if (ownGuid == $scope.c && $scope.request.Country.Name == "Romania" && $scope.c != $scope.request.SuperiorID) {
                $scope.TRArequest.COOApproved = 2;
            } else {
                $scope.TRArequest.HasApproved = 2;
            }
            $scope.TRArequest.Note = angular.copy($scope.comments);
            $scope.TRArequest.ApprovedBy = ownGuid;
            travelRequestService.saveChanges($scope.TRArequest, undefined, function (result) {
                reloadPage();
            }, angular.noop);
            
        };

        //When reject is pressed in the approving dialog
        $scope.onRejectConfirm = function () {
            $scope.mode = 'rejectConfirmed';
            $scope.TRArequest.Flag = true;

            //Check if self is COO
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

        
        $scope.isTravelAgency = function () {
            return roles.TravelAgency;
        }


        /* == Create Itinerary ========================================= */
        $scope.createItinerary = function (request) {
            modalService.open("Convert to Itinerary",
                              "This action will archive the travelrequest (cannot be undone!) and create an itinerary. Continue?",
                              function succes()
                              {
                                  request.IsItinerary = true;
                                  travelRequestService.saveChanges(request,
                                              undefined,
                                              function succes() {
                                                  //travelRequestService.archiveTravelRequest(request);
                                                  console.log("Saved");
                                              },
                                              function failed() {
                                                  console.log("Failed");
                                              });
                                  $location.path('/Itinerary');
                                },
                                function (error, reason) {
                                    alert("Something went wrong. Please check the information in the form and try again.");
                                    console.log(error, reason);
                                });
        };

        $scope.mode = 'init';

        function reloadPage() {
            //Check how this works out. Not sure yet if a notify of "page reloading in..." is needed
            //Sometimes it can work with just $route.reload(); but like 50% of the time it is too fast and it loads before the new data is saved
            //setTimeout(function () { $route.reload(); }, 1000);
            $route.reload();
        }

        function failedToLoadDetailPage() {
            var html = '<div class="block"><div class="block-title"><h4>Travel Request detail</h4></div><span>The Travel Request you are looking for was not found.<br /></span><br /></div>';
                
            $("#travelrequest-details").html(html);
        }
    }
})();
