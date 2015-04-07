(function () {
    'use strict';

    var serviceId = 'travelRequestService';

    angular.module('app').factory(serviceId, ['$q', 'breeze',
        travelRequestService]);

    function travelRequestService($q, breeze) {
        var serviceName = 'breeze/TravelRequest';

        //console.log(breeze);
        
        var manager = new breeze.EntityManager(serviceName);

        //console.log(manager);

        //To use a function first declare it here 
        var service = {
            archiveTravelRequest: archiveTravelRequest,
            createTravelRequest: createTravelRequest,
            createTravelProposal: createTravelProposal,
            createTraveller: createTraveller,
            getTravelRequests: getTravelRequests,
            getProposals: getProposals,
            getProposalById: getProposalById,
            getProposalIdFromItinerary: getProposalIdFromItinerary,
            getTravelRequestById: getTravelRequestById,
            getTravelRequestByHash: getTravelRequestByHash,
            getCurrentTravels: getCurrentTravels,
            getTravelRequestApprovalsById: getTravelRequestApprovalsById,
            hashToID: hashToID,
            createCopy: createCopy,
            toggleDelete: toggleDelete,
            toggleArchive: toggleArchive,
            getAddresses: getAddresses,
            getRequestTravellers:getRequestTravellers,
            getAddressTypes: getAddressTypes,
            getCountries: getCountries,
            getCountryById: getCountryById,
            getCompanyById:getCompanyById,
            getInsurances: getInsurances,
            getEmployees: getEmployees,
            getEmployeeByObjectGuid: getEmployeeByObjectGuid,
            getTravellerById:getTravellerById,
            getAirports: getAirports,
            getCompanies: getCompanies,
            getFerries: getFerries,
            getTaxiRequests: getTaxiRequests,
            getAccommodationRequests: getAccommodationRequests,
            getRentalCarRequests:getRentalCarRequests,
            setPreUsedCompany: setPreUsedCompany,
            setPreUsedTaxiDeparture: setPreUsedTaxiDeparture,
            setPreUsedTaxiDestination: setPreUsedTaxiDestination,
            setPreUsedAccommodation: setPreUsedAccommodation,
            setPreUsedRentalCar:setPreUsedRentalCar,
            toggleCustomerAddress: toggleCustomerAddress,
            addTraveller: addTraveller,
            deleteTraveller: deleteTraveller,
            getTravellerCompanies: getTravellerCompanies,
            getTravellers:getTravellers,
            addCompanyToTraveller: addCompanyToTraveller,
            removeCompanyFromTraveller: removeCompanyFromTraveller,
            addFlight: addFlight,
            removeFlight: removeFlight,
            changeFlyerMemberCard: changeFlyerMemberCard,
            addFerry: addFerry,
            removeFerry: removeFerry,
            addEurotunnel: addEurotunnel,
            removeEurotunnel: removeEurotunnel,
            addRentalcar: addRentalcar,
            getServiceCompanies: getServiceCompanies,
            removeRentalcar: removeRentalcar,
            addTaxi: addTaxi,
            removeTaxi: removeTaxi,
            addAccommodation: addAccommodation,
            removeAccommodation: removeAccommodation,
            getExchangeRates: getExchangeRates,
            hasChanges: hasChanges,
            saveChanges: saveChanges,
            manager: manager
        };


        return service;

        /* == Create ======================================================= */

        // creating a TravelProposal
        function createTravelProposal() {
            var deferred = $q.defer();

            if (!manager.metadataStore.hasMetadataFor(serviceName)) {
                manager.fetchMetadata()
                .then(function () {

                    deferred.resolve(buildTravelRequest());
                }, function (error) {
                    console.log('Error while fetching metadata', error);
                    deferred.reject(error);
                });
            } else {
                deferred.resolve(buildTravelRequest());
            }

            return deferred.promise;

            function buildTravelRequest() {
                var deferred = $q.defer();

                var request = manager.createEntity('TravelProposal');
                console.log('hier');
                console.log(request);

                var query = breeze.EntityQuery
                    .from('AddressTypes')
                    .take(1);

                manager.executeQuery(query).then(function (q) {
                    deferred.resolve(request);
                }, function (error) {
                    console.log('Error while creating travel ' +
                        'request (fetching address types)', error);
                    deferred.reject(error);
                });

                return deferred.promise;
            }
        }

        //creating a TravelRequest
        function createTravelRequest() {
            manager = new breeze.EntityManager(serviceName);
            var deferred = $q.defer();

            if (!manager.metadataStore.hasMetadataFor(serviceName)) {
                manager.fetchMetadata()
                .then(function () {

                    deferred.resolve(buildTravelRequest());
                }, function (error) {
                    console.log('Error while fetching metadata', error);
                    deferred.reject(error);
                });
            } else {
                deferred.resolve(buildTravelRequest());
            }

            return deferred.promise;

            function buildTravelRequest() {
                var deferred = $q.defer();

                var request = manager.createEntity('TravelRequest');

                request.CustomerOrProspect =
                    manager.createEntity('Company');

                request.CustomerOrProspect.Address =
                    manager.createEntity('Address');

                addTraveller(request);
                addFlight(request);
                addFerry(request);
                addEurotunnel(request);
                addRentalcar(request);
                addTaxi(request);
                addAccommodation(request);

                var query = breeze.EntityQuery
                    .from('AddressTypes')
                    .take(1);

                manager.executeQuery(query).then(function (q) {
                    request.CustomerOrProspect.Address.AddressType =
                        _.first(q.results);

                    deferred.resolve(request);
                }, function (error) {
                    console.log('Error while creating travel ' +
                        'request (fetching address types)', error);
                    deferred.reject(error);
                });

                return deferred.promise;
            }
        }

        /* == Read ========================================================= */
        function getTravelRequests(isItin) {
            manager = new breeze.EntityManager(serviceName);

            var query = new breeze.EntityQuery('TravelRequests')
                .where('IsItinerary', 'eq', isItin)
                .orderBy('CreatedDate desc');

            var promise = manager.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                throw error;
            }
        }

        function getProposalIdFromItinerary(id)
        {
            manager = new breeze.EntityManager(serviceName);

            var promise = manager.fetchEntityByKey('Proposals', id)
            .catch(queryFailed);

            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                throw error;
            }
        }

        function getCurrentTravels() {
            manager = new breeze.EntityManager(serviceName);
            var today = new Date();

            var query = new breeze.EntityQuery('TravelRequests')
                .where('IsFinal', 'eq', true)
                .where('DepartureDate', '<=', today)
                .where('ReturnDate' ,'>=' , today);

            var promise = manager.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                throw error;
            }
        }

        function getServiceCompanies() {
            manager = new breeze.EntityManager(serviceName);

            var query = new breeze.EntityQuery('ServiceCompanies');
            var promise = manager.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                throw error;
            }
        }

        function hashToID(hash)
        {
            getTravelRequestByHash(hash, false)
            .then(function (bla) {
                return bla.results[0].Id;
            });
        }

        function getProposals() {
            manager = new breeze.EntityManager(serviceName);

            //terugvindcomment
            var query = new breeze.EntityQuery('Proposals');
            var promise = manager.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                throw error;
            }
        }

        function getProposalById(id) {
            manager = new breeze.EntityManager(serviceName);
            
            //terugvindcomment
            var query = new breeze.EntityQuery('Proposals')
                .where('Id', 'eq', parseInt(id));
            var promise = manager.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                throw error;
            }
        }

        function getInsurances() {
            manager = new breeze.EntityManager(serviceName);

            //terugvindcomment
            var query = new breeze.EntityQuery('Insurances')
                .orderBy('ExpirationDate asc');
            var promise = manager.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                throw error;
            }
        }

        function getTravelRequestByHash(hash, isItinerary) {
            manager = new breeze.EntityManager(serviceName);

            var query = new breeze.EntityQuery('TravelRequests')
                .where('Hash', 'eq', hash);

            var promise = manager.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                throw error;
            }
        }

        function getTravelRequestById(id) {
            manager = new breeze.EntityManager(serviceName);

            var query = new breeze.EntityQuery('TravelRequests')
                .where('Id', 'eq', parseInt(id));

            var promise = manager.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                throw error;
            }
        }

        function getTravelRequestApprovalsById(id) {
            manager = new breeze.EntityManager(serviceName);

            var query = new breeze.EntityQuery('TravelRequestApprovals')
                .where('TravelRequestID', 'eq', id);

            var promise = manager.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                throw error;
            }
        }


        /* == Delete ======================================================= */
        function toggleDelete(request) {
            return request.IsDeleted = !request.IsDeleted;
        }

        /* == Archival ===================================================== */
        function toggleArchive(request) {
            return request.IsArchived = !request.IsArchived;
        }

        /* == Lookup lists ================================================= */
        function getAddressTypes() {
            var deferred = $q.defer();

            var query = breeze.EntityQuery
                .from('AddressTypes');

            manager.executeQuery(query).then(function (q) {
                deferred.resolve(q.results);
            }, function (error) {
                console.log('Error while fetching address types', error);
                deferred.reject(error);
            });

            return deferred.promise;
        }

        function getExchangeRates() {
            var query = breeze.EntityQuery
                .from('ExchangeRate');

            var promise = manager.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                throw error;
            }
        }


        function createCopy(source)
        {
            var entity = manager.createEntity(source.entityType.shortName);
            jQuery.extend(true, entity, source);
            return entity;
        }

        function getAddresses() {
            var query = breeze.EntityQuery
                .from('Address');

            var promise = manager.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                throw error;
            }
        }

        function getRequestTravellers() {
            var query = breeze.EntityQuery
                .from('RequestTraveller');

            var promise = manager.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                throw error;
            }
        }

        function getTravellerCompanies() {
            var query = breeze.EntityQuery
                .from('Company')
                .where('DefaultCompany', '==', true)
                .expand("Address");
                

            var promise = manager.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                throw error;
            }
        }

        function getTravellers() {
            var query = breeze.EntityQuery
                .from('RequestTraveller');


            var promise = manager.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                throw error;
            }
        }

        function getCountries() {
            var query = breeze.EntityQuery
                .from('CountryInformation');

            var promise = manager.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                throw error;
            }
        }

        function getCountryById(id) {
            var query = breeze.EntityQuery
                .from('CountryInformation')
                .where("Id","==",id);

            var promise = manager.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                throw error;
            }
        }

        function getCompanyById(id) {
            var query = breeze.EntityQuery
                .from('Company')
                .where("Id", "==", id)
                .expand("Address");;

            var promise = manager.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                throw error;
            }
        }

        function getEmployees() {
            var query = breeze.EntityQuery
                .from('ADusers');

            var promise = manager.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                alert(error.message);
                throw error;
            }
        }

        function getEmployeeByObjectGuid(objectGuid) {
            var promise = $q.defer();

            if (!objectGuid) {
                promise.reject('No identifier given for the employee!');
            }

            var query = breeze.EntityQuery
                        .from('ADusers');

            manager.executeQuery(query).then(function (val) {
                promise.resolve(_.find(val.results, function (val) { return val.objectGuid == objectGuid }));
            }).catch(queryFailed);

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                throw error;
            }
            return promise.promise;
        }

        function getAirports() {
            var p1 = new breeze.Predicate("IATA_FAA", "!=", "");
            var p2 = breeze.Predicate("ICAO", "!=", "");
            var predicate = p1.and(p2);

            var query = breeze.EntityQuery
                .from('AirportInformation')
                .where(predicate);
            

            var promise = manager.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                alert(error.message);
                throw error;
            }
        }

        function getFerries() {
            var query = breeze.EntityQuery
            .from('FerryRequest');

            var promise = manager.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                throw error;
            }
        }

        function getTaxiRequests() {
            var query = breeze.EntityQuery
            .from('TaxiRequest');

            var promise = manager.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                throw error;
            }
        }

        function getAccommodationRequests() {
            var query = breeze.EntityQuery
            .from('AccommodationRequest');

            var promise = manager.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                throw error;
            }
        }

        function getRentalCarRequests() {
            var query = breeze.EntityQuery
           .from('RentalCarRequest');

            var promise = manager.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                throw error;
            }
        }

        function getTravellerById(id) {
            var query = breeze.EntityQuery
                .from('RequestTraveller')
                .where("Id", "==", id);

            var promise = manager.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                throw error;
            }
        }


        function getCompanies() {
            var query = breeze.EntityQuery
            .from('Company')
            .expand("Address");

            var promise = manager.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                throw error;
            }
        }

        /* == Child Entities =============================================== */
        function toggleCustomerAddress(request, enabled) {
            if (enabled) {
                // attach new address to request
                request.CustomerOrProspect.Address = manager.createEntity('Address');
            } else if (request.CustomerOrProspect.Address) {
                // detach address from request
                manager.detachEntity(request.CustomerOrProspect.Address);
            }
        }

        // -- traveller ----------------------------------------------------//
        function addTraveller(request) {
            //create the connectionTable
            var connectTable = manager.createEntity('TravelRequest_RequestTraveller')

            request.TravelRequest_RequestTravellers.push(connectTable);
        }

        function deleteTraveller(request, traveller) {
            request.TravelRequest_RequestTravellers.splice(_.indexOf(request.TravelRequest_RequestTravellers, traveller), 1);
            manager.detachEntity(traveller);
        }

        function addCompanyToTraveller(traveller) {
            traveller.Company = manager.createEntity('Company');
        }

        function createTraveller() {
            return manager.createEntity('RequestTraveller')
        }
        //Did you know that it doesn't matter what gender you are to get it cold on antarctica #theMoreYouBanana
        function removeCompanyFromTraveller(traveller) {
            if (traveller.Company && !traveller.Company.DefaultCompany) {
                manager.detachEntity(traveller.Company);
            }
        }



        // -- flight -------------------------------------------------------//
        function addFlight(request, departureFlight) {
            var entity = manager.createEntity('FlightRequest');
            entity.Title = "Flight";

            if (departureFlight) {
                entity.DepartureDate = departureFlight.DepartureDate;
                entity.HasLargeCabinLuggage = departureFlight.HasLargeCabinLuggage;
                entity.HasSpecialEquipment = departureFlight.HasSpecialEquipment;
                entity.LargeLuggageCount = departureFlight.LargeLuggageCount;
                entity.IsOnlineCheckIn = departureFlight.IsOnlineCheckIn;
                entity.FlyerMemberCard = departureFlight.FlyerMemberCard;
                entity.DepartureAddress = departureFlight.DestinationAddress; // v
                entity.DestinationAddress = departureFlight.DepartureAddress;   // ^
                entity.ServiceCompany = departureFlight.ServiceCompany;

                entity.IsReturnFlight = true;
                entity.Title = "Return Flight";
            } else {
                if (request.FlightRequests[0]) {
                    entity.FlyerMemberCard = request.FlightRequests[0].FlyerMemberCard;
                } else {
                    entity.FlyerMemberCard = manager.createEntity('FlyerMemberCard');
                    entity.DifferentFlyerMemberCard = true;
                }

                var departure = new Date();
                departure.setHours(0, 0, 0);

                entity.DepartureDate = departure;
                entity.LargeLuggageCount = 0;
                entity.IsOnlineCheckIn = true;
                entity.DepartureAddress = manager.createEntity('Address');
                entity.DestinationAddress = manager.createEntity('Address');
            }

            
            entity.DepartureDate.setHours(0, 0, 0);

            request.FlightRequests.push(entity);
        }

        function removeFlight(request, flight, force) {
            if (flight == request.FlightRequests[0] || force) {
                manager.detachEntity(flight.FlyerMemberCard);
            }
            request.FlightRequests.splice(_.indexOf(request.FlightRequests, flight), 1);
            manager.detachEntity(flight.DepartureAddress);
            manager.detachEntity(flight.DestinationAddress);
            
            manager.detachEntity(flight);
        }

        function changeFlyerMemberCard(request, flight, shouldAttachNewCard) {
            if (shouldAttachNewCard) {
                flight.FlyerMemberCard = manager.createEntity('FlyerMemberCard');
            } else {
                manager.detachEntity(flight.FlyerMemberCard);
                flight.FlyerMemberCard = request.FlightRequests[0].FlyerMemberCard;
            }
        }

        // -- ferry --------------------------------------------------------//
        function addFerry(request, departure) {

            if (departure) {
                var entity = manager.createEntity('FerryRequest');
                entity.DepartureDate = departure.DepartureDate;
                entity.DepartureAddress = departure.DestinationAddress;
                entity.DestinationAddress = departure.DepartureAddress;
                entity.LicensePlate = departure.LicensePlate;
                entity.CarHeight = departure.CarHeight;
                entity.CarLength = departure.CarLength;
                entity.IsReturnFerry = true;
                entity.Title = "Return Ferry"
                request.FerryRequests.push(entity);
                return;
            }

            var entity = manager.createEntity('FerryRequest');
            entity.DepartureAddress = manager.createEntity('Address');
            entity.DestinationAddress = manager.createEntity('Address');
            entity.DepartureDate = new Date();
            entity.DepartureDate.setHours(0, 0, 0);
            entity.CarHeight = entity.CarLength = 0;
            entity.Title = "Ferry";

            request.FerryRequests.push(entity);
        }

        function removeFerry(request, ferry) {
            request.FerryRequests.splice(_.indexOf(request.FerryRequests, ferry), 1);
            manager.detachEntity(ferry.DepartureAddress);
            manager.detachEntity(ferry.DestinationAddress);
            manager.detachEntity(ferry);
        }

        // -- eurotunnel ---------------------------------------------------//
        function addEurotunnel(request, departure) {
            if (departure) {
                var entity = manager.createEntity('EuroTunnelRequest');
                entity.DepartureDate = departure.DepartureDate;
                entity.DepartureAddress = departure.DestinationAddress;
                entity.DestinationAddress = departure.DepartureAddress;
                entity.LicensePlate = departure.LicensePlate;
                entity.Title = "Return Eurotunnel";
                entity.IsReturnEuroTunnel = true;
                request.EuroTunnelRequests.push(entity);
                return;
            }

            var folkstoneQuery = breeze.EntityQuery
                .from('Address')
                .where('AddressName', '==', 'Eurotunnel Folkstone')
                .take(1);

            var coquellesQuery = breeze.EntityQuery
                .from('Address')
                .where('AddressName', '==', 'Eurotunnel Coquelles')
                .take(1);

            manager.executeQuery(folkstoneQuery).then(function (folkstoneQuery) {
                manager.executeQuery(coquellesQuery).then(function (coquellesQuery) {
                    var entity = manager.createEntity('EuroTunnelRequest');
                    entity.DepartureAddress = coquellesQuery.results[0];
                    entity.DestinationAddress = folkstoneQuery.results[0];
                    entity.DepartureDate = new Date();
                    entity.DepartureDate.setHours(0, 0, 0);
                    entity.Title = "Eurotunnel";
                    request.EuroTunnelRequests.push(entity);
                })
            });
        }

        function removeEurotunnel(request, eurotunnel) {
            request.EuroTunnelRequests.splice(_.indexOf(request.EuroTunnelRequests, eurotunnel), 1);
            manager.detachEntity(eurotunnel);
        }

        // -- rental car ---------------------------------------------------//
        function addRentalcar(request) {
            var entity = manager.createEntity('RentalCarRequest');

            entity.Address = manager.createEntity('Address');
            entity.StartDate = new Date();
            entity.StartDate.setHours(0, 0, 0);
            entity.EndDate = new Date();
            entity.EndDate.setHours(0, 0, 0);
            //entity.Driver = request.RequestTravellers[0];

            request.RentalCarRequests.push(entity);
        }

        function removeRentalcar(request, rentalcar) {
            request.RentalCarRequests.splice(_.indexOf(request.RentalCarRequests, rentalcar), 1);
            manager.detachEntity(rentalcar.Address);
            manager.detachEntity(rentalcar);
        }

        function setPreUsedRentalCar(request, index) {
            var TempRentalCarAddress = {};
            jQuery.extend(TempRentalCarAddress, request.TempRentalCarAddress[index]);

            request.RentalCarRequests[index].Address.AddressName   = TempRentalCarAddress.AddressName;
            request.RentalCarRequests[index].Address.Street        = TempRentalCarAddress.Street;
            request.RentalCarRequests[index].Address.City          = TempRentalCarAddress.City;
            request.RentalCarRequests[index].Address.PostalCode    = TempRentalCarAddress.PostalCode;
            request.RentalCarRequests[index].Address.StateProvince = TempRentalCarAddress.StateProvince;
            try {
                request.RentalCarRequests[index].Address.CountryRegion = TempRentalCarAddress.CountryRegion;
            } catch (err) { };
        }

        // -- taxi ---------------------------------------------------------//
        function addTaxi(request) {
            var entity = manager.createEntity('TaxiRequest');
            entity.DepartureDate = new Date();
            entity.DepartureDate.setHours(0, 0, 0);
            entity.DepartureAddress = manager.createEntity('Address');
            entity.DestinationAddress = manager.createEntity('Address');
            request.TaxiRequests.push(entity);
        }

        function removeTaxi(request, taxi) {
            request.TaxiRequests.splice(_.indexOf(request.TaxiRequests, taxi), 1);
            manager.detachEntity(taxi.DepartureAddress);
            manager.detachEntity(taxi.DestinationAddress);
            manager.detachEntity(taxi);
        }

        function setPreUsedTaxiDeparture(request,index) {
            var TempTaxiDeparture = {};
            jQuery.extend(TempTaxiDeparture, request.TempTaxiDeparture[index]);

            request.TaxiRequests[index].DepartureAddress.AddressName        = TempTaxiDeparture.AddressName;
            request.TaxiRequests[index].DepartureAddress.Street             = TempTaxiDeparture.Street;
            request.TaxiRequests[index].DepartureAddress.City               = TempTaxiDeparture.City;
            request.TaxiRequests[index].DepartureAddress.PostalCode         = TempTaxiDeparture.PostalCode;
            request.TaxiRequests[index].DepartureAddress.StateProvince      = TempTaxiDeparture.StateProvince;
            try {
                request.TaxiRequests[index].DepartureAddress.CountryRegion = TempTaxiDeparture.CountryRegion;
            } catch (err) { };
        }

        function setPreUsedTaxiDestination(request, index) {
            var TempTaxiDestination = {};
            jQuery.extend(TempTaxiDestination, request.TempTaxiDestination[index]);

            request.TaxiRequests[index].DestinationAddress.AddressName      = TempTaxiDestination.AddressName;
            request.TaxiRequests[index].DestinationAddress.Street           = TempTaxiDestination.Street;
            request.TaxiRequests[index].DestinationAddress.City             = TempTaxiDestination.City;
            request.TaxiRequests[index].DestinationAddress.PostalCode       = TempTaxiDestination.PostalCode;
            request.TaxiRequests[index].DestinationAddress.StateProvince    = TempTaxiDestination.StateProvince;
            try {
                request.TaxiRequests[index].DestinationAddress.CountryRegion = TempTaxiDestination.CountryRegion;
            } catch (err) { };
        }

        

        // -- accommodation ------------------------------------------------//
        function addAccommodation(request) {
            var entity = manager.createEntity('Accommodation');
            entity.Address = manager.createEntity('Address');
            request.Accommodations.push(entity);
        }

        function removeAccommodation(request, accommodation) {
            request.Accommodations.splice(_.indexOf(request.Accommodations, accommodation), 1);
            manager.detachEntity(accommodation.Address);
            manager.detachEntity(accommodation);
        }

        function setPreUsedAccommodation(request, index) {
            var TempAccommodation = {};

            jQuery.extend(TempAccommodation, request.TempAccommodation[index]);

            request.Accommodations[index].Address.AddressName = TempAccommodation.AddressName;
            request.Accommodations[index].Address.Street = TempAccommodation.Street;
            request.Accommodations[index].Address.City = TempAccommodation.City;
            request.Accommodations[index].Address.PostalCode = TempAccommodation.PostalCode;
            request.Accommodations[index].Address.StateProvince = TempAccommodation.StateProvince;
            try {
                request.Accommodations[index].Address.CountryRegion = TempAccommodation.CountryRegion;
            } catch (err) { };
           
        }

        // -- Company ------------------------------------------------------//
        function setPreUsedCompany(request) {            
            var TempCompany = {};
            jQuery.extend(TempCompany, request.TempCompany);

            request.CustomerOrProspect.Name = TempCompany.Name;

            request.CustomerOrProspect.Address.AddressName      = TempCompany.Address.AddressName;
            request.CustomerOrProspect.Address.Street           = TempCompany.Address.Street;
            request.CustomerOrProspect.Address.City             = TempCompany.Address.City;
            request.CustomerOrProspect.Address.PostalCode       = TempCompany.Address.PostalCode;
            request.CustomerOrProspect.Address.StateProvince    = TempCompany.Address.StateProvince;
            request.CustomerOrProspect.Address.CountryRegion    = TempCompany.Address.CountryRegion;
        }

        /* == Update ======================================================= */
        function hasChanges() {
            return manager.hasChanges();
        }

        function handleSaveValidationError(error) {
            var message = 'Not saved due to validation error';
            try {
                var firstErr = error.entityErrors[0];
                message += ': ' + firstErr.errorMessage;
            } catch (e) { }
            return message;
        }



        function archiveTravelRequest(original) {
            var archive = manager.createEntity('ArchivedTravelRequest');
            archive.Content = "Doe maar iets";
            saveChanges(archive,
                        undefined,
                        function succes() {
                            //console.log("Saved");
                        },
                        function failed() {
                            //console.log("Failed");
                        });
        }

        function saveChanges(request, options, onSuccess, onFailure) {
            //console.log(request);
            if (options) {
                if (!options.hasFlight) {
                    _.forEach(request.FlightRequests, function (val) {
                        removeFlight(request, val, true);
                    });
                }

                if (!options.hasFerry) {
                    _.forEach(request.FerryRequests, function (val) {
                        removeFerry(request, val);
                    });
                }

                if (!options.hasEurotunnel) {
                    _.forEach(request.EuroTunnelRequests, function (val) {
                        removeEurotunnel(request, val);
                    });
                }

                if (!options.hasRentalCar) {
                    _.forEach(request.RentalCarRequests, function (val) {
                        removeRentalcar(request, val);
                    });
                }

                if (!options.hasTaxi) {
                    _.forEach(request.TaxiRequests, function (val) {
                        removeTaxi(request, val);
                    });
                }

                if (!options.hasAccommodation) {
                    _.forEach(request.Accommodations, function (val) {
                        removeAccommodation(request, val);
                    });
                }
            }
            
                //Checks if records already exist in the database. If so connecting them to the object and detach the Entity
                //CustomerOrProspect
                /*var companyCheck = 0;
                if (typeof request.CustomerOrProspect !== typeof undefined) {
    
                    _.forEach(dbCompanies, function (val) {
                        if (val.Name != null && val.Name == request.CustomerOrProspect.Name) {
                            if (val.Address.City != null && val.Address.Street == request.CustomerOrProspect.Address.Street) {
                                if (val.Address.City != null && val.Address.City == request.CustomerOrProspect.Address.City) {
                                    companyCheck = val.Id;
                                }
                            }
                        }
                    });
    
                    if (companyCheck != 0) {
                        manager.detachEntity(request.CustomerOrProspect.Address);
                        manager.detachEntity(request.CustomerOrProspect);
                        request.CustomerOrProspectID = companyCheck;
                    }
    
    
                    //If the filled in is already in the database-> connect ID to that ID and try to delete the old entity so it isn't made in the database
                    _.forEach(dbAddresses, function (val) {
                        if (options.hasFerry) {
                            var x = 0;
                            _.forEach(request.FerryRequests, function (f) {
                                if (val.AddressName != null && val.AddressName == f.DepartureAddress.AddressName) {
                                    try {
                                        manager.detachEntity(f.DepartureAddress);
                                    } catch (err) { }
                                    request.FerryRequests[_.indexOf(request.FerryRequests, f)].DepartureAddressID = val.Id;
                                }
                                if (val.AddressName != null && val.AddressName == f.DestinationAddress.AddressName) {
                                    try {
                                        manager.detachEntity(f.DestinationAddress);
                                    } catch (err) { }
                                    request.FerryRequests[_.indexOf(request.FerryRequests, f)].DestinationAddressID = val.Id;
                                }
                            });
                        }
                        if (options.hasTaxi) {
                            _.forEach(request.TaxiRequests, function (t) {
                                if (val.Street != null && val.Street == t.DepartureAddress.Street) {
                                    if (val.City != null && val.City == t.DepartureAddress.City) {
                                        if (val.AddressName != null && val.AddressName == t.DepartureAddress.AddressName) {
                                            try {
                                                manager.detachEntity(t.DepartureAddress);
                                            } catch (err) { }
                                            request.TaxiRequests[_.indexOf(request.TaxiRequests, t)].DepartureAddressID = val.Id;
                                        }
                                    }
                                }
                                if (val.Street != null && val.Street == t.DestinationAddress.Street) {
                                    if (val.City != null && val.City == t.DestinationAddress.City) {
                                        if (val.AddressName != null && val.AddressName == t.DestinationAddress.AddressName) {
                                            try {
                                                manager.detachEntity(t.DestinationAddress);
                                            } catch (err) { }
                                            request.TaxiRequests[_.indexOf(request.TaxiRequests, t)].DestinationAddressID = val.Id;
                                        }
                                    }
                                }
                            });
                        }
                        if (options.hasRentalCar) {
                            _.forEach(request.RentalCarRequests, function (r) {
                                if (val.Street != null && val.Street == r.Address.Street) {
                                    if (val.City != null && val.City == r.Address.City) {
                                        try {
                                            manager.detachEntity(r.Address);
                                        } catch (err) { }
                                        request.RentalCarRequests[_.indexOf(request.RentalCarRequests, r)].AddressID = val.Id;
                                    }
                                }
                            });
                        }
                        if (options.hasAccommodation) {
                            _.forEach(request.Accommodations, function (a) {
                                if (val.Street != null && val.Street == a.Address.Street) {
                                    if (val.City != null && val.City == a.Address.City) {
                                        try {
                                            manager.detachEntity(a.Address);
                                        } catch (err) { }
                                        request.Accommodations[_.indexOf(request.Accommodations, a)].AddressID = val.Id;
                                    }
                                }
                            });
                        }
                    });
                }*/
            
            return manager.saveChanges()
                .then(saveSucceeded)
                .catch(saveFailed);

            function saveSucceeded(result) {
                //console.log('Successfully saved all changes');
                onSuccess(result);
                //initiateNotificationStep();
            }

            function saveFailed(error) {
                var reason = error.message;
                var detail = error.detail;

                if (error.entityErrors) {
                    handleSaveValidationError(error);
                } else if (detail && detail.ExceptionType &&
                    detail.ExceptionType.indexOf('OptimisticConcurrencyException') !== -1) {
                    reason = 'Another user, perhaps the server itself, may have deleted '
                    + 'the request you are currenty working on. Please try saving again, '
                    + 'or reload this web page.';
                } else {
                    reason = 'Failed to save changes: ' + reason + ' You may have to reload this web page.';
                }
                console.log(error, reason);

                onFailure(error, reason);
            }

            function initiateNotificationStep() {
                $.get("/TravelAgency/api/TravelRequestApproval",function () {
                    console.log("success");
                })
                .done(function () {
                    console.log("second success");
                })
                .fail(function () {
                    console.log("error");
                })
                .always(function () {
                    console.log("finished");
                });
            }
        }
    }
})();