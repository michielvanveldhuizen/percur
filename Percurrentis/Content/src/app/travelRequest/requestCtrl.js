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

    angular.module('app').controller('requestDetailCtrl',
        ['$scope', '$route', 'travelRequestService', requestDetailCtrl]);

    function requestDetailCtrl($scope, $route, travelRequestService) {
        var request;

        travelRequestService.getTravelRequestById(parseInt($route.current.params.requestId, 10))
        .then(function (query) {
            $scope.request = query.results[0];
            return query.results[0];
        })
        .then(function (request) {
            return travelRequestService.getEmployeeByObjectGuid(request.SuperiorID);
        })
        .then(function (employee) {
            $scope.supervisorName = employee.userName;
        });

        console.log($scope);
    }
})();
