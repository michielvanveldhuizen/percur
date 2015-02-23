    var app = angular.module('app');
    app.controller('insuranceCtrl',
        ['$scope', 'travelRequestService', 'modalService', insuranceCtrl]);

    app.filter('expired', function () {
        return function (items, exp) {
            var outExp = [];
            var outVal = [];
            try
            {
                for (var i = 0; i < items.length; i++) {
                    if (items[i].DaysLeft > 0) {
                        outVal.push(items[i]);
                    }
                    else {
                        outExp.push(items[i]);
                    }
                }
            }
            catch (Exception) {}
            if (exp == "no")
            {
                return outVal;
            }
            else {
                return outExp;
            }
        }
    });
    
    function insuranceCtrl($scope, travelRequestService, modalService) {

        $scope.deleteInsurance = function (insurance) {
            modalService.openDelete('insurance', function () {
                travelRequestService.deleteInsurance();
            });
        };
        
        // todo: change to actual validation
        $scope.checkInsuranceNumber = function (input) {
            if (input.length > 9) {
                return "Invalid InsuranceNumber";
            }
        };

        //
        $scope.validateDate = function (input) {
            if(input === undefined)
            {
                return "Invalid Date";
            }
        };

        //
        $scope.saveEdits = function (input) {
            // Do some stuff here pls
       };

        // Calculate numbers of days until insurance expires.
        updateDaysLeft = function (eDate) {
            if (new Date(Date.now()) > eDate) {
                return 0;
            }
            else {
                var completeDay = 24 * 60 * 60 * 1000;
                var todaysDate = new Date(Date.now());
                var expDate = eDate;
                var diffDays = Math.round(Math.abs((todaysDate.getTime() - expDate.getTime()) / (completeDay)));
                return diffDays;
            }
        };

        
        
        travelRequestService.getInsurances()
        .then(function (query) {
            var items = query.results;
            $scope.items = [];
            angular.forEach(items, function (value, key) {
                // Convert InsureeGUID to the persons name.
                travelRequestService.getEmployeeByObjectGuid(value.InsureeGUID).then(function (res) {
                    value.UserName = res.userName;
                    value.DaysLeft = (updateDaysLeft(value.ExpirationDate));
                    $scope.items.push(value);
                });
            });

        });
    };
