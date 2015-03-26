(function () {
    'use strict';

    var serviceId = 'modalService';

    // Define the factory on the module.
    // Inject the dependencies. 
    // Point to the factory definition function.
    var myModalApp = angular.module('app').factory(serviceId, ['$modal', '$sanitize','travellerService', modal]);

    function modal($modal, $sanitize, travellerService) {
        // Define the functions and properties to reveal.
        var service = {
            openDelete: openDelete,
            open: open,
            openAddTraveller: openAddTraveller
        };

        return service;

        //Special modal just for adding travellers
        function openAddTraveller(fnPrimary, preTravellerData, fnCancel) {
            
            var modalInstance = $modal.open({
                templateUrl: '/TravelAgency/Content/src/app/modal/modalAddTraveller.tpl.html',
                controller: modalAddTravellerCtrl,
                contentClass: '',
                //Is used to edit a traveller
                preTravellerData: preTravellerData,
            });

            if (typeof preTravellerData !== typeof undefined) {
                $modal.preTravellerData = preTravellerData;
            }

            function modalAddTravellerCtrl($scope, $modalInstance) {
                $scope.onFileSelect = function ($files) {
                    //$files: an array of files selected, each file has name, size, and type.
                    for (var i = 0; i < $files.length; i++) {
                        var $file = $files[i];
                        angularFileUpload.upload({
                            url: '',
                            file: $file,
                            progress: function (e) { }
                        }).then(function (data, status, headers, config) {
                            // file is uploaded successfully
                            console.log(data);
                        });
                    }
                }

                //Create a new entity if preTravellerData is not defined
                if (typeof preTravellerData == typeof undefined) {
                    travellerService.createTraveller()
                    .then(function (traveller) {
                        $scope.traveller = traveller;
                    });
                    
                } else {
                    //Use PreTravellerData as data to edit
                    $scope.traveller = $modal.preTravellerData;
                    $scope.traveller.tempCompany = $modal.preTravellerData.Company;
                }

                //Set of companies.
                travellerService.getTravellerCompanies().then(function (query) {
                    $scope.travellerCompanies = query.results;
                    //Used to self fillin a company
                    $scope.travellerCompanies.push({ Name: 'Other...' });
                });

                

                $scope.selectTravellerCompany = function (traveller, newValue) {
                    if (newValue.Name == "Other...") {
                        //Used to self fillin a company
                        traveller.hasOtherCompany = true;
                        travellerService.addCompanyToTraveller(traveller);
                    } else {
                        //Connect to an company already in the database
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

        //Standard way to open a modal.
        function open(title, body, fnPrimary, fnCancel, btnSetUrl) {
            if (_.isUndefined(btnSetUrl)) {
                btnSetUrl = '/TravelAgency/Content/src/app/modal/primaryBtnSet.tpl.html';
            }

            var modalInstance = $modal.open({
                templateUrl: '/TravelAgency/Content/src/app/modal/modal.tpl.html',
                //controller Function
                controller: modalCtrl,
                contentClass: 'col-md-8 col-md-offset-2',
                //Variables that will be useable in the template
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
                //Parsing the variables for the template
                $scope.title     = $sanitize(title);
                $scope.body      = $sanitize(body);
                $scope.btnSetUrl = btnSetUrl;
                $scope.primary = {
                    disabled: false,
                    action: primaryAction
                };

                //Parsing functions for the tempalte
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

        //Opens the modal which askes if you are sure you want to delete stuff
        function openDelete(entityName, fnPrimary, fnCancel) {
            open('<i class="fa fa-times"></i> Delete ' + entityName,
                 'Are you sure you want to delete this ' + entityName + '? This action can not be undone.',
                 fnPrimary,
                 fnCancel,
                 '/TravelAgency/Content/src/app/modal/deleteBtnSet.tpl.html');
        }
    }

    myModalApp.directive('fileModel', ['$parse', function ($parse) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                var model = $parse(attrs.fileModel);
                var modelSetter = model.assign;

                element.bind('change', function () {
                    scope.$apply(function () {
                        modelSetter(scope, element[0].files[0]);
                    });
                });
            }
        };
    }]);

    myModalApp.service('fileUpload', ['$http', function ($http) {
        this.uploadFileToUrl = function (file, uploadUrl) {
            var fd = new FormData();
            fd.append('file', file);
            $http.post(uploadUrl, fd, {
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            })
            .success(function () {
            })
            .error(function () {
            });
        }
    }]);

    myModalApp.controller('fileUploadCtrl', ['$scope', 'fileUpload', function ($scope, fileUpload) {

        $scope.uploadFile = function () {
            var file = $scope.myFile;
            console.log('file is ' + JSON.stringify(file));
            var uploadUrl = "/TravelAgency/api/FileUpload";
            fileUpload.uploadFileToUrl(file, uploadUrl);
        };

    }]);
})();