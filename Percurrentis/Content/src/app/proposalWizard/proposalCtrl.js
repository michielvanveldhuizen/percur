(function () {
    'use strict';

    angular.module('app').controller('proposalCtrl',
        ['$scope', '$route', 'wizard', '$location', 'travelRequestService', proposalCtrl]);

    function proposalCtrl($scope, $route, wizard, $location, travelRequestService, modalService) {
        travelRequestService.getTravelRequestByHash($route.current.params.Hash)
        .then(function (query) {
            $scope.itinerary = query.results;
        });

        $scope.type = "";
        
        $scope.templateUrl = '/TravelAgency/Content/src/app/proposalWizard/partials/index.tpl.html';
        

        $scope.hoi = function (ding) {
            console.log($scope.type);
        }

        $scope.updateTemplateUrl = function(type, request)
        {
            $scope.currentRequest = request;
            $scope.type = type;
            $scope.templateUrl = '/TravelAgency/Content/src/app/proposalWizard/partials/' + type + '.tpl.html';
        }
        console.log($scope);
    };
})();