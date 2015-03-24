(function () {
    'use strict';
    var app = angular.module('app');
    app.controller('travellersCtrl',
        ['$scope', '$location', '$route', 'travellerService', 'modalService', travellersCtrl]);

    function travellersCtrl($scope, $location, $route, travellerService, modalService) {
        travellerService.getTravellers()
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
        ['$scope', '$location', '$route', 'travellerService', 'modalService', travellersDetailCtrl]);

    function travellersDetailCtrl($scope, $route, $location, travellerService, modalService) {

        var urlArray = $route.url().split("/");
        var lastParam = urlArray[urlArray.length - 1];
        var oldData = "";
        
        //gets traveller data and set old data to nothing to not keep connected to a previous traveller.
        travellerService.getTravellerById(lastParam).then(function (query) {
            $scope.request = query.results[0];
            oldData = {};
            jQuery.extend(oldData, query.results[0]);
        });

        //Shows modal to edit a traveller.
        $scope.showTravellerEditing = function () {
            travellerService.getTravellerById(lastParam).then(function (query) {
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