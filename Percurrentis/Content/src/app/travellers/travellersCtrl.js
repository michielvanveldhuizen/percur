(function () {
    'use strict';
    var app = angular.module('app');
    app.controller('travellersCtrl',
        ['$scope', '$location', '$route', 'travellerService', 'modalService', travellersCtrl]);

    //LIST PAGE
    function travellersCtrl($scope, $location, $route, travellerService, modalService) {
        //Start loading bar
        NProgress.start();

        //Search input text default value
        $scope.search = "Search by name...";
        
        travellerService.getDepartments()
        .then(function (departmentQuery) {
            var departments = departmentQuery.results;
            departments.push("Other");
            travellerService.getTravellers()
            .then(function (query) {
                $scope.allTravellers = query.results;
                $scope.AfterSearchTravellers = $scope.allTravellers;

                initializeFilters();

                //Setting the current fillter

                $scope.currentFilter = _.find($scope.filters, function (filter) {
                    //setting the titel
                    return filter.title == $location.hash();
                }) || $scope.filters.current;

                $scope.currentFilter.filter();

                //end loading bar
                NProgress.done();
            });
        });

        //Search function for the input field at the top
        $scope.searchChange = function () {
            var tempList = [];

            //Search filter on Family/First/Full name adding them to the list
            for (var x in $scope.allTravellers) {
                var add = false;

                if ($scope.allTravellers[x].FamilyName.toLowerCase().indexOf($scope.search.toLowerCase()) >= 0) {
                    add = true;
                }
                if ($scope.allTravellers[x].FirstName.toLowerCase().indexOf($scope.search.toLowerCase()) >= 0) {
                    add = true;
                }
                if ($scope.allTravellers[x].FullName.toLowerCase().indexOf($scope.search.toLowerCase()) >= 0) {
                    add = true;
                }
                
                if (add) {
                    tempList.push($scope.allTravellers[x]);
                }
            }
            $scope.AfterSearchTravellers = tempList;
            $scope.currentFilter.filter();
        }

        function initializeFilters() {
            if ($location.hash() == "") {
                $location.hash("All");
            }
            //Prototyping the filter
            function Filter(filterFn, title, order, icon, tooltip) {
                this.filterFn = filterFn || function () {
                    return [];
                };
                this.title = title || "Default Filter";
                this.icon = icon;
                this.tooltip = tooltip;
                this.order = order;
            }

            //Prototype filter
            Filter.prototype = {
                filter: function () {
                    $scope.travellers = this.filterFn();

                    $scope.currentFilter = this;
                    $location.hash(this.title);
                },
                count: function () {
                    return this.filterFn().length;
                }
            };
            
            //Filters for the travellers
            $scope.filters = {
                all: new Filter(function () {
                    return _.filter($scope.AfterSearchTravellers, function (req) {
                        return req.IsDeleted == false;
                    });
                }, 'All', 0, 'fa-bars', 'View all Travellers.'),
                deleted: new Filter(function () {
                    return _.filter($scope.AfterSearchTravellers, function (req) {
                        return req.IsDeleted == true;
                    });
                }, 'Deleted', 1, 'fa-ban', 'View deleted Travellers.'),
            }            
        }
        
        //return if someone is travelAgency
        $scope.isTravelAgency = function () {
            return roles.TravelAgency;
        }

        //Deleting a travel request
        $scope.toggleDelete = function (request) {
            if (toggleDelete(request)) {
                $scope.addDeleteAlert();
            }

            travellerService.saveChanges(function () {
                //success
            }, function () {
                //not success
            });
            $scope.currentFilter.filter();
        };
        $scope.showDeleteAlert = false;

        //Dialog in the top
        $scope.addDeleteAlert = function () {
            if (!$scope.showDeleteAlert) {
                $scope.showDeleteAlert = true;
            }
        };

        //close function for the dialog in the top
        $scope.closeDeleteAlert = function () {
            $scope.showDeleteAlert = false;
        };

        //Setting bool true/false
        function toggleDelete(request) {
            return request.IsDeleted = !request.IsDeleted;
        }

        //used to go the detail page
        $scope.go = function (path) {
            $location.path(path);
            $location.hash('');
        };
    }

    //DETAIL PAGE
    angular.module('app').controller('travellersDetailCtrl',
        ['$scope', '$route', '$location', 'travellerService', 'modalService', travellersDetailCtrl]);

    function travellersDetailCtrl($scope, $route, $location, travellerService, modalService) {
        //start loading bar
        NProgress.start();

        $scope.ShowNotes = false;

        //used to go the detail page
        $scope.go = function (path) {
            $location.path(path);
            $location.hash('');
        };

        var urlArray = $location.url().split("/");
        var lastParam = urlArray[urlArray.length - 1];
        
        $scope.showTravelRequests = true;

        //TO show dates as dd-mm-yyyy
        Date.prototype.ddmmyyyy = function () {
            var yyyy = this.getFullYear().toString();
            var mm = (this.getMonth() + 1).toString(); // getMonth() is zero-based
            var dd = this.getDate().toString();
            var returnString = (dd[1] ? dd : "0" + dd[0]) + "-" + (mm[1] ? mm : "0" + mm[0]) + "-" + yyyy;
            if (returnString == "01-01-1900") {
                return "n/a";
            } else {
                return returnString;
            }
        };
        
        //gets traveller data and set old data to nothing to not keep connected to a previous traveller.
        travellerService.getTravellerById(lastParam).then(function (query) {

            $scope.request = query.results[0];
            if (typeof $scope.request == typeof undefined) {
                $location.path("/TravelAgency/#/Travellers/");
                $location.hash();
            }

            //ShowingNotes or not
            if ($scope.request.Note != null) {
                if ($scope.request.Note.length > 0) {
                    $scope.ShowNotes = true;
                }
            }

            

            //Date showing as dd-mm-yyyy
            $scope.request.PassportExpiryDateShow = $scope.request.PassportExpiryDate.ddmmyyyy();
            $scope.request.DateOfBirthShow = $scope.request.DateOfBirth.ddmmyyyy();

            //showing at setting traveldocument or not
            if ($scope.request.TravelDocument !== null) {
                $scope.request.ShowTravelDocument = true;
                $scope.request.TravelDocumentLocation = "Picture/TravelDocument/" + $scope.request.TravelDocument;
            } else {
                $scope.request.ShowTravelDocument = false;
            }

            //end loading bar
            NProgress.done();
        });

        //Getting the travelRequests
        travellerService.getTravellerRequests(lastParam).then(function (query) {
            $scope.travelRequests = query.results;
            if (query.results.length == 0) {
                $scope.showTravelRequests = false;
            }
        });

        //get link based on if itinerary
        $scope.getLinkType = function (data) {
            if(data.IsItinerary)
            {
                return "Itinerary/" + data.Hash;
            }
            else
            {
                return "Request/" + data.Hash;
            }
        }
    }

    //ADD/edit PAGE
    angular.module('app').controller('travellersAddCtrl',
        ['$scope', '$route', '$location', 'fileUpload', 'travellerService', travellersAddCtrl]);

    function travellersAddCtrl($scope, $route, $location, fileUpload, travellerService) {
        //start loading bar
        NProgress.start();
        $scope.PageTitle = "Traveller";
        var isAdd = true;
        var travellerID;

        $('.textarea-editor').wysihtml5();

        $scope.go = function (path) {
            $location.path(path);
            $location.hash('');
        };

        //Get TravellerId from url param if its there, also set isAdd to false if not
        if (angular.isDefined($route.current.params.id)) {
            isAdd = false;
            travellerID = $route.current.params.id;
        }
        
        $scope.isAdd = isAdd;
        

        //Get Traveller
        if (isAdd) {
            //If is add new traveller
            $scope.PageTitle = "Add New Traveller";
            travellerService.createTraveller()
            .then(function (traveller) {
                travellerService.addAddress(traveller);

                traveller.DateOfBirth = new Date();
                traveller.PassportExpiryDate = new Date();

                $scope.traveller = traveller;

                NProgress.done();
            });
        } else {
            //if is edit traveller
            travellerService.getTravellerById(travellerID)
            .then(function (query) {
                
                var traveller = query.results[0];

                //If no address found, create address entity
                if (traveller.AddressID == null) {
                    travellerService.addAddress(traveller);
                }
    
                $scope.traveller = traveller;
                $scope.PageTitle = "Edit Traveller " + traveller.FullName;

                if (traveller.TravelDocument != null) {
                    var travelDocumentLocation = "Picture/TravelDocument/" + traveller.TravelDocument;
                    document.getElementById("travel-document-preview").innerHTML = "<img class='img-responsive' src='" + travelDocumentLocation + "' />";
                }

                NProgress.done();
            });
        }


        //genders
        var genders = ['Male', 'Female'];
        $scope.Genders = genders;

        //Set of companies.
        travellerService.getTravellerCompanies().then(function (query) {
            $scope.travellerCompanies = query.results;
            //Used to self fillin a company
            $scope.travellerCompanies.push({ Name: 'Other...' });
        });

        //SetDepartments
        travellerService.getDepartments().then(function (query) {
            $scope.departments = query.results;
            $scope.departments.push('Other');
        });

        //Getting the countries
        travellerService.getCountries().then(function (query) {
            $scope.countries = query.results;
        })


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


        $scope.confirmSave = function () {
            var doSave = true;

            //giving an error when there is a file selected but not uploaded
            if ($("#file-upload-selector").val().length != 0 && $("#upload-button").html() == "Upload file") {
                doSave = false;
                $("#save-error").show(500);
                setTimeout(function () {
                    document.getElementById("save-error").style.display = "None";
                }, 5000);
            }

            if (doSave) {
                NProgress.start();
                try {
                    //Setting the travelDocument name to save it in the entity if its not empty
                    if (typeof travellerService.sTravelDocument != typeof undefined) {
                        fileUpload.deleteOldFile($scope.traveller.TravelDocument);

                        $scope.traveller.TravelDocument = travellerService.sTravelDocument;
                        travellerService.sTravelDocument = undefined;
                    }

                    //Standard save changes to update the entities
                    travellerService.saveChanges(function (result) {
                        NProgress.done();
                        //$(".text-success").show(500);
                        /*var x = 5;
                        //countdown for redirect
                        var interval = setInterval(function () {
                            x--;
                            $("#countdown-redirect").html(x);
                            if (x == 0) {
                                clearInterval(interval);
                            }
                        }, 1000);*/
                        //redirect after 5sec
						window.location.href = window.location.origin + "" + window.location.pathname + "#/Travellers/"+$scope.traveller.Id;
                        /*var timeout = setTimeout(function () {
                            window.location.href = window.location.origin + "" + window.location.pathname + "#/Travellers/"+$scope.traveller.Id;
                        }, 5000);*/
                    },
                    function (error, reason) {
                        alert("Something went wrong. Please check the information in the form and try again.");
                        console.log(error, reason);
                        NProgress.done();
                    });
                } catch (ex) {
                    NProgress.done();
                    console.log(ex);
                }
            }
        };
    }

    //fileModel
    app.directive('fileModel', ['$parse', function ($parse) {
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
    app.service('fileUpload', ['$http', 'travellerService', function ($http, travellerService) {
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
                
                var travelDocumentLocation = "FileUpload/TravelDocuments/" + data;

                document.getElementById("travel-document-preview").innerHTML = "<img class='img-responsive' src='" + travelDocumentLocation + "' />";
                document.getElementById("upload-button").innerHTML = "File saved!";
                document.getElementById("save-error").style.display = "None";
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

    app.controller('uploadCtrl', ['$scope', 'fileUpload', function ($scope, fileUpload) {
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