var app = angular.module('app');
app.controller('travellersCtrl',
    ['$scope', 'travelRequestService', 'modalService', travellersCtrl]);

function travellersCtrl($scope, travelRequestService, modalService) {
    travelRequestService.getTravellers()
        .then(function (query) {
            $scope.travellers = query.results;
        });

    $scope.addTraveller = function () {
        console.log("IMABANANAAAAA");
    }
}