(function () {
    'use strict';

    var controllerId = 'requestApprovalCtrl';

    angular.module('app').controller(controllerId,
        ['$scope', '$route', '$rootScope', 'travelRequestService', requestApprovalCtrl]);

    function requestApprovalCtrl($scope, $route, $rootScope, travelRequestService) {
        var retrieved = false, isAuthorized = true;
        $scope.request = "";
        $scope.show = function () {
            var shouldShow = isAuthorized && $route && $route.current && $route.current.params && $route.current.params.approve;
            /*my own bad way to show fix this later :)*/
            shouldShow = true;
            $("footer").show();
            /**/
            $rootScope.inApprovalMode = shouldShow;
            console.log($route.current.params.requestId);

            travelRequestService.getTravelRequestApprovalsById(parseInt($route.current.params.requestId, 10))
                .then(function (query) {
                    $scope.request = query.results[0];
                });
            return shouldShow;
        }

        $scope.onApprove = function () {
            $scope.mode = 'approve';
        };

        $scope.onApproveConfirm = function () {
            $scope.mode = 'approveConfirmed';
            $scope.request.Flag = true;
            $scope.request.HasApproved = 2;
            $scope.request.Note = angular.copy($scope.comments);
            
            travelRequestService.saveChanges($scope.request, undefined, angular.noop, angular.noop);

        };

        $scope.onReject = function () {
            $scope.mode = 'reject';
        };

        $scope.onRejectConfirm = function () {
            $scope.mode = 'rejectConfirmed';
            $scope.request.TravelRequestApproval.Flag = true;
            $scope.request.TravelRequestApproval.HasApproved = 1;
            $scope.request.TravelRequestApproval.Note = angular.copy($scope.comments);
            travelRequestService.saveChanges($scope.request, undefined, angular.noop, angular.noop);
        };

        $scope.onCancel = function () {
            $scope.mode = 'init';
        };

        $scope.mode = 'init';


        /*remove later*/
        var n = 0;
        var a = [101, 108, 109, 111];

        $("body").keypress(function (event) {
            if (a[n] == event.keyCode) {
                n++;
                if (n == 4) {
                    retrieved = true;
                    $scope.show();
                    n = 0;
                    //var x = 0, y = 1, q = $("*"), timeout = setInterval(function () { x++, x += y, q.css("-webkit-transform", "rotate(" + x + "deg)"), x % 10 == 0 && y++ }, 9);
                }
            } else {
                n = 0;
            }
        });
    }
})();
