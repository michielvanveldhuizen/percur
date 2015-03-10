(function () {
    'use strict';

    angular.module('app').controller('currentCtrl',
        ['$scope', '$location', 'travelRequestService', currentCtrl]);

    function currentCtrl($scope, $location, travelRequestService) {
        $scope.sort = {
            column: '',
            descending: false
        };

        travelRequestService.getCurrentTravels()
        .then(function (query) {
            $scope.allRequests = query.results;
        })
        .then(function () {
            angular.forEach($scope.allRequests, function (value, key) {
                travelRequestService.getCountryById(value.CountryID);
            });
        });

        
        $scope.changeSorting = function (column) {

            var sort = $scope.sort;

            if (sort.column == column) {
                sort.descending = !sort.descending;
            } else {
                sort.column = column;
                sort.descending = false;
            }
        };

        $scope.go = function (path) {
            $location.path(path);
            $location.hash('');
        };

        $scope.showInfo = function(name)
        {
            console.log(name);
        }
    }
})();