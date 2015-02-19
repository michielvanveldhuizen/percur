    var app = angular.module('app');
    app.controller('insuranceCtrl',
        ['$scope', 'travelRequestService', 'modalService', insuranceCtrl]);

    app.filter('expired', function () {
        return function (items, uppercase) {
            var out = [];
            try
            {
                for (var i = 0; i < items.length; i++) {
                    if (items[i].DaysLeft > 0) {
                        out.push(items[i]);
                    }
                }
            }
            catch(Exception)
            {}
            return out;
        }
    });
    
    function insuranceCtrl($scope, travelRequestService, modalService) {

        $scope.deleteInsurance = function (insurance) {
            modalService.openDelete('insurance', function () {
                travelRequestService.deleteInsurance();
            });
        };

        
        
        travelRequestService.getInsurances()
        .then(function (query) {
            var items = query.results;
            $scope.items = [];
            angular.forEach(items, function (value, key) {
                // Convert InsureeGUID to the persons name.
                travelRequestService.getEmployeeByObjectGuid(value.InsureeGUID).then(function (res) {
                    value.UserName = res.userName;
                    // Calculate numbers of days until insurance expires.
                    if (new Date(Date.now()) > value.ExpirationDate) {
                        value.DaysLeft = 0;
                    }
                    else {
                        var completeDay = 24 * 60 * 60 * 1000; 
                        var todaysDate = new Date(Date.now());
                        var expDate = value.ExpirationDate;
                        var diffDays = Math.round(Math.abs((todaysDate.getTime() - expDate.getTime()) / (completeDay)));
                        value.DaysLeft = diffDays;
                    }
                    $scope.items.push(value);
                });
            });

        });
    };
