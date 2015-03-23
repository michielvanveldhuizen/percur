(function () {
    'use strict';

    var serviceId = 'modalService';

    // Define the factory on the module.
    // Inject the dependencies. 
    // Point to the factory definition function.
    angular.module('app').factory(serviceId, ['$modal', '$sanitize', 'travellerService', 'travelRequestService', modal]);

    function modal($modal, $sanitize, travellerService, travelRequestService) {
        // Define the functions and properties to reveal.
        var service = {
            openDelete: openDelete,
            open: open,
            openAddTraveller: openAddTraveller
        };

        return service;

        function openAddTraveller(fnPrimary, preTravellerData, fnCancel) {
            
            var modalInstance = $modal.open({
                templateUrl: '/TravelAgency/Content/src/app/modal/modalAddTraveller.tpl.html',
                controller: modalAddTravellerCtrl,
                contentClass: '',
                preTravellerData: preTravellerData,
            });


            if (typeof preTravellerData !== typeof undefined) {
                $modal.preTravellerData = preTravellerData;
            }

            function modalAddTravellerCtrl($scope, $modalInstance) {
                if (typeof preTravellerData == typeof undefined) {

                    travellerService.createTraveller()
                    .then(function (traveller) {
                        $scope.traveller = traveller;
                    });
                    
                } else {
                    $scope.traveller = $modal.preTravellerData;
                    $scope.traveller.tempCompany = $modal.preTravellerData.Company;
                }
                //DEZE komt uit andere manager... misschien hier manager verhaal iets netter opgaan lossen zodra ik meer dingen hier in verschillende functies naartoe haal
                //of ik doe gewoon net als of ik een banaan ben terwijl ik dit aan het typen ben.
                // zoizo dat ik een banaan ben tho.
                travellerService.getTravellerCompanies().then(function (query) {
                    $scope.travellerCompanies = query.results;
                    $scope.travellerCompanies.push({ Name: 'Other...' });
                });

                

                $scope.selectTravellerCompany = function (traveller, newValue) {
                    if (newValue.Name == "Other...") {
                        traveller.hasOtherCompany = true;
                        travellerService.addCompanyToTraveller(traveller);
                    } else {
                        traveller.hasOtherCompany = false;
                        travellerService.removeCompanyFromTraveller(traveller);
                        traveller.Company = newValue;
                    }
                };


                $scope.primary = {
                    disabled: false,
                    action: primaryAction
                };

                function primaryAction() {
                    try {
                        travellerService.saveChanges(function (result) {
                            $scope.primary.disabled = true;
                            $modalInstance.close('delete');
                        },
                        function (error, reason) {
                            alert("Something went wrong. Please check the information in the form and try again.");
                            console.log(error, reason);
                        });

                        /*travelRequestService.saveChanges($scope.traveller, false, function (result) {
                            $scope.primary.disabled = true;
                            $modalInstance.close('delete');
                        },*/

                    } catch (ex) {
                        console.log(ex);
                    }

                };

                $scope.cancel = function () {
                    $modalInstance.dismiss('cancel');
                };
            }

            modalInstance.result.then(function (selected) {
                fnPrimary();
            }, function () {
                if (_.isFunction(fnCancel)) {
                    fnCancel();
                }
            });
        }

        function open(title, body, fnPrimary, fnCancel, btnSetUrl) {
            if (_.isUndefined(btnSetUrl)) {
                btnSetUrl = '/TravelAgency/Content/src/app/modal/primaryBtnSet.tpl.html';
            }

            var modalInstance = $modal.open({
                templateUrl: '/TravelAgency/Content/src/app/modal/modal.tpl.html',
                controller: modalCtrl,
                contentClass: 'col-md-8 col-md-offset-2',
                resolve: {
                    title: function() {
                        return title;
                    },
                    body: function () {
                        return body;
                    },
                    btnSetUrl: function () {
                        return btnSetUrl;
                    }
                }
            });

            function modalCtrl($scope, $modalInstance, title, body) {
                $scope.title     = $sanitize(title);
                $scope.body      = $sanitize(body);
                $scope.btnSetUrl = btnSetUrl;
                $scope.primary = {
                    disabled: false,
                    action: primaryAction
                };

                function primaryAction() {
                    $scope.primary.disabled = true;
                    $modalInstance.close('delete');
                };

                $scope.cancel = function () {
                    $modalInstance.dismiss('cancel');
                };
            }

            modalInstance.result.then(function (selected) {
                fnPrimary();
            }, function () {
                if (_.isFunction(fnCancel)) {
                    fnCancel();
                }
            });
        }

        function openDelete(entityName, fnPrimary, fnCancel) {
            open('<i class="fa fa-times"></i> Delete ' + entityName,
                 'Are you sure you want to delete this ' + entityName + '? This action can not be undone.',
                 fnPrimary,
                 fnCancel,
                 '/TravelAgency/Content/src/app/modal/deleteBtnSet.tpl.html');
        }
    }
})();