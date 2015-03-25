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

        // Testgeval
        $scope.hoi = function (ding) {
            console.log($scope);
        }

        // Save when proposal is completed.
        $scope.saveProposal = function () {
            console.log(travelRequestService.hasChanges($scope.proposal));
            travelRequestService.saveChanges($scope.proposal,
                                             undefined,
                                             function succes() {
                                                 $location.path('/Proposal/');
                                             },
                                             function failure(error) {
                                                 console.log("Failed: " + error);
                                             });
        }

        // Save when proposal is completed.
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

        $scope.attach = function(type, proposal, request)
        {
            switch(type)
            {
                case 'Accommodation':
                    travelRequestService.addAccommodation(proposal);
                    // Setting the index
                    $scope.currentIndex                          = $scope.proposal.Accommodations.length - 1;
                    $scope.currentEntity                         = $scope.proposal.Accommodations[$scope.currentIndex];
                    // Copying all this stuff by hand because it will screw up otherwise... :(
                    $scope.currentEntity.TravelProposalID        = $scope.proposal.Id;
                    $scope.currentEntity.Address.AddressName     = request.Address.AddressName;
                    $scope.currentEntity.Address.Street          = request.Address.Street;
                    $scope.currentEntity.Address.PostalCode      = request.Address.PostalCode;
                    $scope.currentEntity.Address.City            = request.Address.City;
                    $scope.currentEntity.Address.StateProvince   = request.Address.StateProvince;
                    $scope.currentEntity.Address.CountryRegionID = request.Address.CountryRegionID;
                    $scope.currentEntity.ParentID                = request.Id;

                    $scope.currentEntity.CheckInDate             = request.CheckInDate;
                    $scope.currentEntity.CheckOutDate            = request.CheckOutDate;

                    $scope.currentEntity.Cost                    = request.Cost;
                    $scope.currentEntity.CostSecondary           = request.CostSecondary;
                    $scope.currentEntity.SecondaryCurrency       = request.SecondaryCurrency;
                    $scope.currentEntity.Note                    = request.Note;
                    break;
                case 'Flight':
                    travelRequestService.addFlight(proposal);
                    console.log(request);
                    // Setting the index
                    $scope.currentIndex                               = $scope.proposal.FlightRequests.length - 1;
                    $scope.currentEntity                              = $scope.proposal.FlightRequests[$scope.currentIndex];
                    // Copying all this stuff by hand because it will screw up otherwise... :(
                    $scope.currentEntity.DepartureDate                = request.DepartureDate;
                    $scope.currentEntity.HasLargeCabinLuggage         = request.HasLargeCabinLuggage;
                    $scope.currentEntity.HasSpecialEquipment          = request.HasSpecialEquipment;
                    $scope.currentEntity.LargeLuggageCount            = request.LargeLuggageCount;
                    $scope.currentEntity.IsOnlineCheckIn              = request.IsOnlineCheckIn;
                    $scope.currentEntity.FlyerMemberCardID            = request.FlyerMemberCardID;
                    $scope.currentEntity.ParentID                     = request.Id;
                    // DepartureAddress
                    $scope.currentEntity.DepartureAddressID           = request.DepartureAddressID;
                    $scope.currentEntity.DepartureAddress             = request.DepartureAddress;
                    // DestinationAddress
                    $scope.currentEntity.DestinationAddressID         = request.DestinationAddressID;
                    $scope.currentEntity.DestinationAddress           = request.DestinationAddress;
                    

                    $scope.currentEntity.TravelRequestID = 0;

                    $scope.currentEntity.FlyerMemberCard.Id = request.FlyerMemberCard.Id;
                    $scope.currentEntity.FlyerMemberCard.FMCNumber = request.FlyerMemberCard.FMCNumber;

                    
                    $scope.currentEntity.Cost = request.Cost;
                    $scope.currentEntity.CostSecondary = request.CostSecondary;
                    $scope.currentEntity.SecondaryCurrency = request.SecondaryCurrency;
                    $scope.currentEntity.Note = request.Note;
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
            console.log($scope.proposals);
        });
    }

    angular.module('app').controller('proposalDetailCtrl',
        ['$scope', '$route', 'wizard', '$location', 'travelRequestService', 'modalService', proposalDetailCtrl]);

    function proposalDetailCtrl($scope, $route, wizard, $location, travelRequestService, modalService) {
        travelRequestService.getProposalById($route.current.params.Id)
        .then(function (query) {
            $scope.proposal = query.results[0];
            travelRequestService.getTravelRequestById($scope.proposal.TravelRequestID)
            .then(function (query) {
                $scope.itinerary = query.results[0]; 
            });
        });

        // Load all the addresses again -.-'
        travelRequestService.getAddresses().then(function (query) {
            $scope.addresses = query.results;
        });
    }
})();