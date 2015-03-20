(function () {
    'use strict';
    var app = angular.module('app');
    app.controller('travellersCtrl',
        ['$scope', '$location', '$route', 'travelRequestService', 'modalService', travellersCtrl]);

    function travellersCtrl($scope, $location, $route, travelRequestService, modalService) {
        travelRequestService.getTravellers()
            .then(function (query) {
                $scope.travellers = query.results;
            });

        $scope.showTravellerAdding = function () {
            modalService.openAddTraveller(function () {
                $route.reload();
            });
        }

        $scope.go = function (path) {
            $location.path(path);
            $location.hash('');
        };
    }

    angular.module('app').controller('travellersDetailCtrl',
        ['$scope', '$location', '$route', 'travelRequestService', 'modalService', travellersDetailCtrl]);

    function travellersDetailCtrl($scope, $route, $location, travelRequestService, modalService) {

        var urlArray = $route.url().split("/");
        var lastParam = urlArray[urlArray.length - 1];
        var oldData = "";
        
        travelRequestService.getTravellerById(lastParam).then(function (query) {
            $scope.request = query.results[0];
            oldData = {};
            jQuery.extend(oldData, query.results[0]);
        });

        $scope.showTravellerEditing = function () {
            travelRequestService.getTravellerById(lastParam).then(function (query) {
                $scope.request = query.results[0];
                oldData = {};
                jQuery.extend(oldData, query.results[0]);
            
                modalService.openAddTraveller(function () {
                    //Saved Maybe a note for that?
                }, $scope.request,
                function () {
                    $scope.request = oldData;
                    oldData = "";
                });
            });
        }
    }
})();