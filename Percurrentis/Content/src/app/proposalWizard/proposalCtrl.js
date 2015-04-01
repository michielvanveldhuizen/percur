(function () {
    'use strict';

    angular.module('app').controller('proposalCtrl',
        ['$scope', '$route', 'wizard', '$location', 'travelRequestService', 'modalService', proposalCtrl]);

    function proposalCtrl($scope, $route, wizard, $location, travelRequestService, modalService) {
        travelRequestService.getTravelRequestByHash($route.current.params.Hash)
        .then(function (query) {
            $scope.itinerary = query.results[0];
        })
        .then(function () {
            travelRequestService.createTravelProposal()
            .then(function (request) {
                $scope.proposal = request;
                // Check if already exists and give options.
                $scope.proposal.TravelRequestID = $scope.itinerary.Id;
            });
        });

        // And the ferries as well
        travelRequestService.getFerries().then(function (query) {
            var uniqueResults = [];
            var alreadyHad = [];
            var uniqueResultsPlate = [];
            var alreadyHadPlate = [];

            //Without the time out javascript always have enough time to get the addresses
            //The timeout doesn't matter for the load time because it's used on the second page of the travel request and loaded during the first
            setTimeout(function () {
                $.each(query.results, function (i, el) {
                    try {
                        if (el.LicensePlate != null) {
                            if (typeof alreadyHadPlate[el.LicensePlate] == typeof undefined) {
                                var x = { LicensePlate: el.LicensePlate };
                                uniqueResultsPlate.push(x);
                                alreadyHadPlate[el.LicensePlate] = true;
                            }
                        }
                        if (el.DepartureAddress.AddressName != null) {
                            if (typeof alreadyHad[el.DepartureAddress.AddressName] == typeof undefined) {
                                var x = { Name: el.DepartureAddress.AddressName };
                                uniqueResults.push(x);
                                alreadyHad[el.DepartureAddress.AddressName] = true;
                            }
                        }
                        if (el.DestinationAddress.AddressName != null) {
                            if (typeof alreadyHad[el.DestinationAddress.AddressName] == typeof undefined) {
                                var x = { Name: el.DestinationAddress.AddressName };
                                uniqueResults.push(x);
                                alreadyHad[el.DestinationAddress.AddressName] = true;
                            }
                        }
                    } catch (err) { };
                });

                $scope.ferryAddresses = uniqueResults;
                $scope.ferryLicensePlates = uniqueResultsPlate;
            }, 1000);

        });

        // Load all the airports again -.-'
        travelRequestService.getAirports().then(function (query) {
            $scope.airports = query.results;
        });

        // Load all the addresses again -.-'
        travelRequestService.getAddresses().then(function (query) {
            $scope.addresses = query.results;
        });

        // Load all the countries again -.-'
        travelRequestService.getCountries().then(function (query) {
            $scope.countries = query.results;
        });

        $scope.currentlyAdding = false;
        $scope.currentEntity;
        $scope.currentIndex;
        $scope.type = "";
        $scope.templateUrl = '/TravelAgency/Content/src/app/proposalWizard/partials/index.tpl.html';

        

        // Save when proposal is completed.
        $scope.saveProposal = function () {
            // Maybe check if the proposal isn't empty?
            $scope.proposal.IsApproved = false;
            
            modalService.open("Confirm ready?", "Are you sure you are done editing this proposal?",
                    function yes() {
                        travelRequestService.saveChanges($scope.proposal,
                                            undefined,
                                            function succes() {
                                                $location.path('/Proposal/');
                                            },
                                            function failure(error) {
                                                console.log("Failed: " + error);
                                            });
                    },
                    function no() {
                        // No actions needed.
                    },
                    '/TravelAgency/Content/src/app/modal/editBtnSet.tpl.html');
           
        }

        // Discard the current proposal.
        $scope.discardProposal = function () {
            modalService.open("Discard changes?", "Do you wish to discard this complete proposal?",
                    function yes() {
                        $location.path('/Itinerary/');
                    },
                    function no() {
                        // No actions needed.
                    },
                    '/TravelAgency/Content/src/app/modal/discardBtnSet.tpl.html');
            
        }

        // Include correct partial and create new entity
        $scope.updateTemplateUrl = function(type, request)
        {
            // Check if editing mode is enabled already
            if (!$scope.currentlyAdding) {
                 $scope.currentlyAdding = true;
                // Create copy entity of 'type' type
                $scope.attach(type, $scope.proposal, request);
                $scope.type = type;
                $scope.templateUrl = '/TravelAgency/Content/src/app/proposalWizard/partials/' + type + '.tpl.html';
                $scope.currentRequest = request;
            }
            else
            {
                modalService.open("Discard changes?", "You are currently still editing an option. Do you wish to discard this one and create a new one?",
                    function yes() {
                        // detach current
                        $scope.currentlyAdding = false;
                        $scope.detach($scope.proposal);
                        $scope.currentEntity = null;
                        $scope.currentIndex = null;
                        $scope.type = null;
                        $scope.updateTemplateUrl(type, request);
                    },
                    function no() {
                        // No actions needed.
                    },
                    '/TravelAgency/Content/src/app/modal/discardBtnSet.tpl.html');
            }
        }


        $scope.attach = function (type, proposal, request) {
            switch (type) {
                case 'Accommodation':
                    travelRequestService.addAccommodation(proposal);
                    // Setting the index
                    $scope.currentIndex = $scope.proposal.Accommodations.length - 1;
                    $scope.currentEntity = $scope.proposal.Accommodations[$scope.currentIndex];
                    // Copying all this stuff by hand because it will screw up otherwise... :(

                    $scope.currentEntity.Address.AddressName = request.Address.AddressName;
                    $scope.currentEntity.Address.Street = request.Address.Street;
                    $scope.currentEntity.Address.PostalCode = request.Address.PostalCode;
                    $scope.currentEntity.Address.City = request.Address.City;
                    $scope.currentEntity.Address.StateProvince = request.Address.StateProvince;
                    $scope.currentEntity.Address.CountryRegionID = request.Address.CountryRegionID;
                    $scope.currentEntity.ParentID = request.Id;

                    $scope.currentEntity.TravelRequestID = 0;
                    $scope.currentEntity.TravelRequest = null;

                    $scope.currentEntity.CheckInDate = request.CheckInDate;
                    $scope.currentEntity.CheckOutDate = request.CheckOutDate;

                    $scope.currentEntity.Cost = request.Cost;
                    $scope.currentEntity.CostSecondary = request.CostSecondary;
                    $scope.currentEntity.SecondaryCurrency = request.SecondaryCurrency;
                    break;

                case 'Flight':
                    travelRequestService.addFlight(proposal);
                    // Setting the index
                    $scope.currentIndex = $scope.proposal.FlightRequests.length - 1;
                    $scope.currentEntity = $scope.proposal.FlightRequests[$scope.currentIndex];
                    // Copying all this stuff by hand because it will screw up otherwise... :(
                    $scope.currentEntity.DepartureDate = request.DepartureDate;
                    $scope.currentEntity.HasLargeCabinLuggage = request.HasLargeCabinLuggage;
                    $scope.currentEntity.HasSpecialEquipment = request.HasSpecialEquipment;
                    $scope.currentEntity.LargeLuggageCount = request.LargeLuggageCount;
                    $scope.currentEntity.IsOnlineCheckIn = request.IsOnlineCheckIn;
                    $scope.currentEntity.FlyerMemberCardID = request.FlyerMemberCardID;
                    $scope.currentEntity.ParentID = request.Id;
                    // DepartureAddress
                    $scope.currentEntity.DepartureAddressID = request.DepartureAddressID;
                    $scope.currentEntity.DepartureAddress = request.DepartureAddress;
                    // DestinationAddress
                    $scope.currentEntity.DestinationAddressID = request.DestinationAddressID;
                    $scope.currentEntity.DestinationAddress = request.DestinationAddress;


                    $scope.currentEntity.TravelRequestID = 0;
                    $scope.currentEntity.TravelRequest = null;

                    $scope.currentEntity.FlyerMemberCard.Id = request.FlyerMemberCard.Id;
                    $scope.currentEntity.FlyerMemberCard.FMCNumber = request.FlyerMemberCard.FMCNumber;


                    $scope.currentEntity.Cost = request.Cost;
                    $scope.currentEntity.CostSecondary = request.CostSecondary;
                    $scope.currentEntity.SecondaryCurrency = request.SecondaryCurrency;
                    break;
                case 'Ferry':
                    travelRequestService.addFerry(proposal);

                    $scope.currentIndex = $scope.proposal.FerryRequests.length - 1;
                    $scope.currentEntity = $scope.proposal.FerryRequests[$scope.currentIndex];

                    $scope.currentEntity.TravelRequestID = 0;

                    $scope.currentEntity.CarHeight = request.CarHeight;
                    $scope.currentEntity.CarLength = request.CarLength;
                    $scope.currentEntity.LicensePlate = request.LicensePlate;

                    $scope.currentEntity.DepartureDate = request.DepartureDate;
                    // DepartureAddress
                    $scope.currentEntity.DepartureAddressID = request.DepartureAddressID;
                    $scope.currentEntity.DepartureAddress = request.DepartureAddress;
                    // DestinationAddress
                    $scope.currentEntity.DestinationAddressID = request.DestinationAddressID;
                    $scope.currentEntity.DestinationAddress = request.DestinationAddress;

                    $scope.currentEntity.Cost = request.Cost;
                    $scope.currentEntity.CostSecondary = request.CostSecondary;
                    $scope.currentEntity.SecondaryCurrency = request.SecondaryCurrency;

            }
        }


        $scope.detach = function (proposal) {
            
            switch ($scope.type) {
                case 'Accommodation':
                    travelRequestService.removeAccommodation($scope.proposal, proposal);
                    break;
                case 'Flight':
                    travelRequestService.removeFlight($scope.proposal, proposal);
                    break;
                case 'Ferry':
                    travelRequestService.removeFerry($scope.proposal, proposal);
                    break;
                default:
                    console.log('Defaulted.');
                    break;
            }
        }

     

        // Item is complete, add it to the proposal
        $scope.commitItem = function()
        {
            $scope.currentlyAdding = false;
            $scope.type = "";
            $scope.currentEntity = null;
            $scope.currentIndex = null;
            // Revert to default template
            $scope.templateUrl = '/TravelAgency/Content/src/app/proposalWizard/partials/index.tpl.html';
        }
    };

    angular.module('app').controller('proposalListCtrl',
        ['$scope', '$route', 'wizard', '$location', 'travelRequestService', 'modalService', proposalListCtrl]);

    function proposalListCtrl($scope, $route, wizard, $location, travelRequestService, modalService) {
        travelRequestService.getProposals()
        .then(function (query) {
            $scope.proposals = query.results;
        });
    }

    angular.module('app').controller('proposalDetailCtrl',
        ['$scope', '$route', 'wizard', '$location', 'travelRequestService', 'modalService', proposalDetailCtrl]);

    function proposalDetailCtrl($scope, $route, wizard, $location, travelRequestService, modalService) {
        $scope.costs = [];
        $scope.totalCost = 0;
        $scope.selectedOptions = [];
        $scope.approvalMode = 'false';
        $scope.mode = 'init';
        travelRequestService.getProposalById($route.current.params.Id)
        .then(function (query) {
            $scope.proposal = query.results[0];
            travelRequestService.getTravelRequestById($scope.proposal.TravelRequestID)
            .then(function (query) {
                $scope.itinerary = query.results[0];

                console.log($scope.itinerary);
            })
            .then(function (query) {
                travelRequestService.getEmployeeByObjectGuid($scope.proposal.TravelRequest.SuperiorID)
                .then(function (employee) {
                    $scope.supervisorName = employee.userName;
                })
            });
        });


        // Load all the addresses again -.-'
        travelRequestService.getAddresses().then(function (query) {
            $scope.addresses = query.results;
        });

        // Set selected item per group.
        $scope.select = function (iets, id) {
            if ($scope.proposal.IsApproved == '0') {
                $scope.costs[iets.ParentID] = iets.Cost;
                $scope.updateTotal();
                $scope.selectedOptions[iets.ParentID] = iets.Id;
                var selector = "#option_" + iets.ParentID + "_" + id;
                jQuery(selector).removeClass("disabled");
                jQuery(selector).find("i.check").addClass("fa fa-check text-success");
                jQuery(".group_" + iets.ParentID + ":not(" + selector + ")").addClass("disabled");
                jQuery(".group_" + iets.ParentID + ":not(" + selector + ")").find("i.check").removeClass("fa fa-check text-success");

                $scope.checkCompletion();
            }
        }

        $scope.checkCompletion = function () {
            var hasEmpty = false;
            angular.forEach($scope.selectedOptions, function (value, key) {
                
                if(value == "")
                {
                    hasEmpty = true;
                }
            });
            if(!hasEmpty)
            {
                $scope.approvalMode = true;
            }
        };

        $scope.updateTotal = function()
        {
            var sum = 0;
            angular.forEach($scope.costs, function (value, key) {
                sum += value;
            });
            $scope.totalCost = sum;
        }

        /* Handle approval form */
        $scope.onApprove = function () {
            $scope.mode = 'approve';
        };
        $scope.onReject = function () {
            $scope.mode = 'reject';
        };

        $scope.onCancel = function () {
            $scope.mode = 'init';
        };


        //When approved is pressed in the approving dialog
        $scope.onApproveConfirm = function () {
            $scope.mode = 'approveConfirmed';
            $scope.proposal.IsApproved = 2;
            // Insert logic



            /*$scope.TRArequest.Flag = true;

            //Check if self is COO
            if (ownGuid == $scope.c && $scope.request.Country.Name == "Romania" && $scope.c != $scope.request.SuperiorID) {
                $scope.TRArequest.COOApproved = 2;
            } else {
                $scope.TRArequest.HasApproved = 2;
            }
            $scope.TRArequest.Note = angular.copy($scope.comments);
            $scope.TRArequest.ApprovedBy = ownGuid;
            travelRequestService.saveChanges($scope.TRArequest, undefined, function (result) {
                reloadPage();
            }, angular.noop);*/

        };
    }
})();