(function () {
    'use strict';

    var serviceId = 'modalService';

    // Define the factory on the module.
    // Inject the dependencies. 
    // Point to the factory definition function.
    var myModalApp = angular.module('app').factory(serviceId, ['$modal', '$sanitize', 'fileUpload', 'travellerService', modal]);

    function modal($modal, $sanitize,fileUpload, travellerService) {
        // Define the functions and properties to reveal.
        var service = {
            openDelete: openDelete,
            open: open,
            openAddTraveller: openAddTraveller
        };

        var sTravelDocument = "";
        
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

                //SetDepartments
                travellerService.getDepartments().then(function (query) {
                    $scope.departments = query.results;
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
                        
                        //Setting the travelDocument name to save it in the entity if its not empty
                        if (typeof travellerService.sTravelDocument != typeof undefined) {
                            fileUpload.deleteOldFile($scope.traveller.TravelDocument);


                            $scope.traveller.TravelDocument = travellerService.sTravelDocument;
                        }

                        //Standard save changes to update the entities
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

    //fileModel
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

    //Uploading travelDocument file
    myModalApp.service('fileUpload', ['$http', 'travellerService', function ($http, travellerService) {
        this.uploadFileToUrl = function (file, uploadUrl, request) {
            var fd = new FormData();
            fd.append('file', file);

            //Uploading the file
            $http.post(uploadUrl, fd, {
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            })
            .success(function (data) {
                //Setting the data to the travellerService and updating the button text
                data = data.substring(1, data.length - 1);
                travellerService.sTravelDocument = data;
                document.getElementById("upload-button").innerHTML = "File saved!";
            })
            .error(function () {
                document.getElementById("upload-error").innerHTML = "Couldn't upload the file";
            });
        }

        this.deleteOldFile = function (id) {
            var fd = new FormData();
            fd.append('delID', id);
            var uploadUrl = "api/FileUpload";
            //Uploading the file
            $http.post(uploadUrl, fd, {
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            })
        }
    }]);

    myModalApp.controller('myCtrl', ['$scope', 'fileUpload', function ($scope, fileUpload) {
        $scope.uploadFile = function () {
            //Check that its only a image
            var check = false;
            var file = $scope.myFile;
            var whiteList = ["jpg", "png", "jpeg"];
            if (typeof file != typeof undefined) {
                if (typeof file.name != typeof undefined) {
                    var splitFileName = file.name.split(".");
                    if (typeof splitFileName[1] != typeof undefined) {
                        for (var extention in whiteList) {
                            if (splitFileName[1].toLowerCase() == whiteList[extention]) {
                                check = true;
                            }
                        }
                    }
                }
            }
            //To upload or not to upload
            if (check) {   
                var uploadUrl = "api/FileUpload";
                fileUpload.uploadFileToUrl(file, uploadUrl, $scope.request);
                
                document.getElementById("upload-error").innerHTML = "";
            } else {
                //document.getElementById("upload-error").innerHTML = "Please upload a PNG, JPG or JPEG file!";
                $("#upload-error").html("Please upload a PNG, JPG or JPEG file!").show(500);
                setTimeout(function () {
                    $("#upload-error").hide(500);
                }, 5000);
                
            }
        };

    }]);
})();