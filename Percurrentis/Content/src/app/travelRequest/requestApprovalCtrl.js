//NO LONGER OF USE
//MAYBE DELETE?
(function () {
    'use strict';

    var controllerId = 'requestApprovalCtrl';

    angular.module('app').controller(controllerId,
        ['$scope', '$route', '$rootScope', 'travelRequestService', requestApprovalCtrl]);

    function requestApprovalCtrl($scope, $route, $rootScope, travelRequestService) {
        


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
                    //var x = 0, y = 1, q = document.getElementsByTagName("*"), timeout = setInterval(function () { x++; x += y; for (var z = 0; z < q.length; z++) { q[z].style.WebkitTransform = "rotate(" + x + "deg)"; } x % 10 == 0 && y++ }, 9);
                }
            } else {
                n = 0;
            }
        });
    }
})();
