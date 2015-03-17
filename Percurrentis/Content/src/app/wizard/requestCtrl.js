(function () {
    'use strict';

    var controllerId = 'requestWizardCtrl';

    angular.module('app').controller(controllerId,
        [
            '$scope',
            '$route',
            '$q',
            '$location',
            'activeProfile',
            'wizard',
            'modalService',
            'entityManagerFactory',
            'travelRequestService',
            requestCtrl
        ]);

    function requestCtrl(
        $scope,
        $route,
        $q,
        $location,
        activeProfile,
        wizard,
        modal,
        entityManagerFactory,
        travelRequestService) {

        $scope.onNextClicked = function () {
            $(':input').parent().addClass('has-visited');
        };

        travelRequestService.createTravelRequest()
        .then(function (request) {
            request.ApplicantID = activeProfile.username;
            $scope.model = request;
        });

        travelRequestService.getAddressTypes().then(function (query) {
            $scope.addressTypes = query.results;
        });


        //////////////////////////////////////////////////////////
        // Steps
        
        var steps = [
            {
                step: 'General Details',
                code: 'general',
                templateUrl: '/TravelAgency/Content/src/app/wizard/partials/generalDetails.tpl.html',
                isValid: function () {
                    return $scope.model.entityAspect.validateEntity() &&
                        $scope.model.CustomerOrProspect.entityAspect.validateEntity();
                }
            },
            {
                step: 'Personal Details',
                code: 'personal',
                templateUrl: '/TravelAgency/Content/src/app/wizard/partials/personalDetails.tpl.html',
                isValid: function () {
                    return _.every($scope.model.RequestTravellers, function (val)
                    {
                        return val.entityAspect.validateEntity();
                    });
                }
            },
            {
                step: 'Travel Options',
                code: 'options',
                templateUrl: '/TravelAgency/Content/src/app/wizard/partials/options.tpl.html'
            },
            {
                step: 'Flights',
                code: 'flights',
                enabled: false,
                templateUrl: '/TravelAgency/Content/src/app/wizard/partials/flights.tpl.html',
                isValid: function () {
                    return _.every($scope.model.FlightRequests, function (val) {
                        return val.entityAspect.validateEntity();
                    });
                }
            },
            {
                step: 'Ferry',
                code: 'ferry',
                enabled: false,
                templateUrl: '/TravelAgency/Content/src/app/wizard/partials/ferry.tpl.html',
                isValid: function () {
                    return _.every($scope.model.FerryRequests, function (val) {
                        return val.entityAspect.validateEntity();
                    });
                }
            },
            {
                step: 'Eurotunnel',
                code: 'eurotunnel',
                enabled: false,
                templateUrl: '/TravelAgency/Content/src/app/wizard/partials/eurotunnel.tpl.html',
                isValid: function () {
                    return _.every($scope.model.EuroTunnelRequests, function (val) {
                        return val.entityAspect.validateEntity();
                    });
                }
            },
            {
                step: 'Rental Car',
                code: 'rentalcar',
                enabled: false,
                templateUrl: '/TravelAgency/Content/src/app/wizard/partials/rentalcar.tpl.html',
                isValid: function () {
                    return _.every($scope.model.RentalCarRequests, function (val) {
                        return val.entityAspect.validateEntity();
                    });
                }
            },
            {
                step: 'Taxi',
                code: 'taxi',
                enabled: false,
                templateUrl: '/TravelAgency/Content/src/app/wizard/partials/taxi.tpl.html',
                isValid: function () {
                    return _.every($scope.model.TaxiRequests, function (val) {
                        return val.entityAspect.validateEntity();
                    });
                }
            },
            {
                step: 'Accommodation',
                code: 'accommodation',
                enabled: false,
                templateUrl: '/TravelAgency/Content/src/app/wizard/partials/accommodation.tpl.html',
                isValid: function () {
                    return _.every($scope.model.Accommodations, function (val) {
                        return val.entityAspect.validateEntity();
                    });
                }
            },
            {
                step: 'Confirm Request',
                code: 'confirm',
                templateUrl: '/TravelAgency/Content/src/app/wizard/partials/confirm.tpl.html'
            },
            {
                step: 'Cancel Request',
                code: 'cancel',
                enabled: false,
                templateUrl: '/TravelAgency/Content/src/app/wizard/partials/cancel.tpl.html'
            }
        ];


        //////////////////////////////////////////////////////////
        // Init

        var m_wizard = wizard();

        m_wizard.init(steps, {
            maySubmit: function () {
                return true;
            },
            onSubmit: function () {
                console.log($scope.model);
                // clean up entities that should be empty
                if (!$scope.options.hasFlight) {
                    _.each($scope.model.FlightRequests, function (v) {
                        travelRequestService.removeFlight($scope.model, v);
                    });
                }
                if (!$scope.options.hasFerry) {
                    _.each($scope.model.FerryRequests, function (v) {
                        travelRequestService.removeFerry($scope.model, v);
                    });
                }
                if (!$scope.options.hasEurotunnel) {
                    _.each($scope.model.EuroTunnelRequests, function (v) {
                        travelRequestService.removeEurotunnel($scope.model, v);
                    });
                }
                if (!$scope.options.hasRentalCar) {
                    _.each($scope.model.RentalCarRequests, function (v) {
                        travelRequestService.removeRentalcar($scope.model, v);
                    });
                }
                if (!$scope.options.hasTaxi) {
                    _.each($scope.model.TaxiRequests, function (v) {
                        travelRequestService.removeTaxi($scope.model, v);
                    });
                }
                if (!$scope.options.hasAccommodation) {
                    _.each($scope.model.Accommodations, function (v) {
                        travelRequestService.removeAccommodation($scope.model, v);
                    });
                }

                try {
                    travelRequestService.saveChanges($scope.model, $scope.options, function (result) {
                        $location.path('/Request/Submitted');
                    },
                    function (error, reason) {
                        alert("Something went wrong. Please check the information in the form and try again.");
                        console.log(error, reason);
                    });
                } catch (ex) {
                    console.log(ex);
                }
            },
            onNext: function () {
                window.scrollTo(0, 0);
            }
        });

        $scope.wizard = m_wizard;
        
        $scope.cancelRequest = function () {
            if ($scope.wizard.current().code == 'confirm') {
                $scope.wizard.jump('cancel');
            } else if ($scope.wizard.current().code == 'cancel') {
                $location.path('/Request/Cancelled');
            }
        };

        //////////////////////////////////////////////////////////
        // General

        $scope.toggleCustomerAddress = function (enabled) {
            console.log("Is this ever used? enabled = " + enabled);
            travelRequestService.toggleCustomerAddress($scope.model, enabled);
        };

        travelRequestService.getEmployees().then(function (query) {
            $scope.employees = query.results;
        });

        travelRequestService.getTravellers().then(function (query) {
            $scope.travellers = query.results;
        });

        $scope.setPreUsedCompany = function () {
            travelRequestService.setPreUsedCompany($scope.model);
        }

        //$scope.model isn't active right away sometimes
        function setModelDates() {
            if (typeof $scope.model != typeof undefined && typeof $scope.countries != typeof undefined) {
                var today = new Date();
                today.setDate(today.getDate() + 0);
                today.setHours(12, 0, 0);
                $scope.model.DepartureDate = today;

                var tomorrow = new Date();
                tomorrow.setDate(tomorrow.getDate() + 1);
                tomorrow.setHours(12, 0, 0);
                $scope.model.ReturnDate = tomorrow;

                $scope.CheckInDate = today;
                $scope.CheckOutDate = tomorrow;

                _.each($scope.countries, function (c) {
                    if (c.Name == "Romania") {
                        $scope.model.Country = c;
                        $scope.model.CountryID = c.Id;
                    }
                });
            } else {
                setTimeout(function () {
                    setModelDates();
                },300);
            }
        }
        setModelDates();

        travelRequestService.getCountries().then(function (query) {
            $scope.countries = query.results;
        });

        travelRequestService.getAirports().then(function (query) {
            $scope.airports = query.results;
        });

        travelRequestService.getCompanies().then(function (query) {
            $scope.companies = query.results;
        });

        
        travelRequestService.getCompanies().then(function (query) {
            var uniqueResults = [];
            var alreadyHad = [];
            
            $.each(query.results, function (i, el) {
                if (uniqueResults.length < 1) {
                    var copiedObject = {};
                    jQuery.extend(copiedObject, el);
                    copiedObject.Address = null;
                    copiedObject.Id = -1;
                    copiedObject.Name = "";
                    uniqueResults.push(copiedObject);
                }
                if (typeof alreadyHad[el.Name] == typeof undefined) {
                    uniqueResults.push(el);
                    alreadyHad[el.Name] = true;
                }
            });
            $scope.uniqueCompanies = uniqueResults;
        });
             
        //////////////////////////////////////////////////////////
        // Personal

        travelRequestService.getTravellerCompanies().then(function (query) {
            $scope.travellerCompanies = query.results;
            $scope.travellerCompanies.push({ Name: 'Other...' });
            dbCompanies = query.results;
        });

        travelRequestService.getAddresses().then(function (query) {
            dbAddresses = query.results;
        });

        $scope.addTraveller = function () {
            travelRequestService.addTraveller($scope.model);
        };

        $scope.deleteTraveller = function (traveller) {
            modal.openDelete('traveller', function () {
                travelRequestService.deleteTraveller($scope.model, traveller);
            });
        };

        $scope.selectTravellerCompany = function (traveller, newValue) {
            if (newValue.Name == "Other...") {
                traveller.hasOtherCompany = true;
                travelRequestService.addCompanyToTraveller(traveller);
            } else {
                traveller.hasOtherCompany = false;
                travelRequestService.removeCompanyFromTraveller(traveller);
                traveller.Company = newValue;
            }
        };


        //////////////////////////////////////////////////////////
        // Options

        $scope.options = {
            'hasFlight':            false,
            'hasFerry':             false,
            'hasEurotunnel':        false,
            'hasRentalCar':         false,
            'hasTaxi':              false,
            'hasAccommodation':     false,
            'knownCustomerAddress': true 
        };


        //////////////////////////////////////////////////////////
        // Flights

        $scope.addFlight = function (departureFlight) {
            travelRequestService.addFlight($scope.model, departureFlight);
        };
        
        $scope.deleteFlight = function (flight) {
            modal.openDelete('flight', function () {
                travelRequestService.removeFlight($scope.model, flight);
            });
        };

        $scope.differentFlyerMemberCardOnChange = function (flight) {
            travelRequestService.changeFlyerMemberCard($scope.model, flight, !flight.DifferentFlyerMemberCard);
        };


        //////////////////////////////////////////////////////////
        // Ferry

        $scope.addFerry = function (departure) {
            travelRequestService.addFerry($scope.model, departure);
        };

        $scope.deleteFerry = function (ferry) {
            modal.openDelete('ferry', function () {
                travelRequestService.removeFerry($scope.model, ferry);
            });
        };

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

        //////////////////////////////////////////////////////////
        // Eurotunnel

        $scope.isEurotunnelToFolkstone = function (eurotunnel) {
            return eurotunnel.DestinationAddress.AddressName == "Eurotunnel Folkstone";
        };

        $scope.addEurotunnel = function (departure) {
            travelRequestService.addEurotunnel($scope.model, departure);
        };

        $scope.eurotunnelChangeDestination = function (eurotunnel, val) {
            if (eurotunnel.DestinationAddress.AddressName != val) {
                var tempAddress               = eurotunnel.DepartureAddress;
                eurotunnel.DepartureAddress   = eurotunnel.DestinationAddress;
                eurotunnel.DestinationAddress = tempAddress;
            }
        };

        $scope.deleteEurotunnel = function (eurotunnel) {
            modal.openDelete('eurotunnel', function () {
                travelRequestService.removeEurotunnel($scope.model, eurotunnel);
            });
        };


        //////////////////////////////////////////////////////////
        // Rental Car

        $scope.addRentalCar = function () {
            travelRequestService.addRentalcar($scope.model);
        };

        // RentalEnd may not be be on an earlier date as the rentalStart. So,
        // when the start date changes to beyond the end date, also set the
        // end date to the new start date.
        $scope.onRentalCarStartDateChanged = function (rentalcar) {
            if (rentalcar.rentalStart > rentalcar.rentalEnd) {
                rentalcar.rentalEnd = rentalcar.rentalStart;
            }
        };

        $scope.deleteRentalCar = function (rentalcar) {
            modal.openDelete('rental car', function () {
                travelRequestService.removeRentalcar($scope.model, rentalcar);
            });
        };


        travelRequestService.getRentalCarRequests().then(function (query) {
            var uniqueResults = [];
            //Without the time out javascript always have enough time to get the addresses
            //The timeout doesn't matter for the load time because it's used on the second page of the travel request and loaded during the first
            setTimeout(function () {
                $.each(query.results, function (i, el) {
                    uniqueResults.concat(getUniqueAddresses(el.Address, uniqueResults));
                });
                $scope.model.TempRentalCarAddress = [];
                $scope.uniqueRentalCar = uniqueResults;
            }, 1000);
        });

        $scope.setPreUsedRentalCar = function (index) {
            travelRequestService.setPreUsedRentalCar($scope.model, index);
        }

        //////////////////////////////////////////////////////////
        // Taxi

        $scope.addTaxi = function () {
            travelRequestService.addTaxi($scope.model);
        };
        
        $scope.deleteTaxi = function (taxi) {
            modal.openDelete('taxi', function () {
                travelRequestService.removeTaxi($scope.model, taxi);
            });
        };

        travelRequestService.getTaxiRequests().then(function (query) {
            var uniqueResults = [];
            
            //Without the time out javascript always have enough time to get the addresses
            //The timeout doesn't matter for the load time because it's used on the second page of the travel request and loaded during the first
            setTimeout(function () {
                $.each(query.results, function (i, el) {
                    uniqueResults.concat(getUniqueAddresses(el.DepartureAddress, uniqueResults));
                    uniqueResults.concat(getUniqueAddresses(el.DestinationAddress, uniqueResults));
                });
                $scope.model.TempTaxiDeparture = [];
                $scope.model.TempTaxiDestination = [];
                $scope.uniqueTaxiRequests = uniqueResults;
            }, 1000);
        });

        $scope.setPreUsedTaxiDeparture = function (index) {
            travelRequestService.setPreUsedTaxiDeparture($scope.model,index);
        }

        $scope.setPreUsedTaxiDestination = function (index) {
            travelRequestService.setPreUsedTaxiDestination($scope.model, index);
        }


        //////////////////////////////////////////////////////////
        // Accommodation

        $scope.addAccommodation = function () {
            console.log("add");
            travelRequestService.addAccommodation($scope.model);
        };

        $scope.deleteAccommodation = function (accommodation) {
            modal.openDelete('accommodation', function () {
                travelRequestService.removeAccommodation($scope.model, accommodation);
            });
        };

        travelRequestService.getAccommodationRequests().then(function (query) {
            var uniqueResults = [];
            //Without the time out javascript always have enough time to get the addresses
            //The timeout doesn't matter for the load time because it's used on the second page of the travel request and loaded during the first
            setTimeout(function () {
                $.each(query.results, function (i, el) {
                    uniqueResults.concat(getUniqueAddresses(el.Address, uniqueResults));
                });
                $scope.model.TempAccommodation = [];
                $scope.uniqueAccommodations = uniqueResults;
            }, 1000);
        });

        $scope.setPreUsedAccommodations = function (index) {
            travelRequestService.setPreUsedAccommodation($scope.model, index);
        }

        //To get the unique Addresses for the preFilledIn Select boxes
        function getUniqueAddresses(result, uniqueResults) {
            if (uniqueResults.length < 1) {
                var copiedObject = {};
                jQuery.extend(copiedObject, result);
                copiedObject.AddressName = "";
                copiedObject.Id = -1;
                copiedObject.City = "";
                copiedObject.CountryRegion = "";
                copiedObject.PostalCode = "";
                copiedObject.StateProvince = "";
                copiedObject.Street = "";
                uniqueResults.push(copiedObject);
            }

            var addDestinationAddress = true;
            for (var x = 0; x < uniqueResults.length; x++) {
                if (uniqueResults[x].AddressName == result.AddressName) {
                    if (uniqueResults[x].City == result.City) {
                        addDestinationAddress = false;
                    }
                }
            }
            if (addDestinationAddress == true) {
                var copiedObject = {};
                jQuery.extend(copiedObject, result);
                var showName = "";
                if (copiedObject.AddressName != null) {
                    showName += copiedObject.AddressName + ", ";
                }
                if (copiedObject.City != null) {
                    showName += copiedObject.City + ", ";
                }
                if (copiedObject.CountryRegion != null) {
                    showName += copiedObject.CountryRegion.Name;
                }
                copiedObject.ShowName = showName;
                if (showName != "") {
                    uniqueResults.push(copiedObject);
                }
            }
        }
    }
})();
