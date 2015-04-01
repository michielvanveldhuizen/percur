(function () {
    'use strict';

    var serviceId = 'travellerService';

    angular.module('app').factory(serviceId, ['$q', 'breeze',
        travellerService]);

    function travellerService($q, breeze) {
        var serviceName = 'breeze/TravelRequest';
        
        var managerTraveller = new breeze.EntityManager(serviceName);

        //To use a function first declare it here 
        var service = {
            createTraveller: createTraveller,
            getTravellerCompanies: getTravellerCompanies,
            addCompanyToTraveller: addCompanyToTraveller,
            deleteTraveller: deleteTraveller,
            removeCompanyFromTraveller: removeCompanyFromTraveller,
            saveChanges: saveChanges,
            getTravellerById: getTravellerById,
            getTravellers: getTravellers,
            getTravellerRequests: getTravellerRequests,
        };


        return service;

        //Creating the traveller Entity based on the manager
        function createTraveller() {
            managerTraveller = new breeze.EntityManager(serviceName);
            var deferred = $q.defer();

            if (!managerTraveller.metadataStore.hasMetadataFor(serviceName)) {
                managerTraveller.fetchMetadata()
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

                var request = managerTraveller.createEntity('RequestTraveller');

                deferred.resolve(request);
                return deferred.promise;
            }
        }

        //Getting all companies from the database where default company = 1
        function getTravellerCompanies() {
            var query = breeze.EntityQuery
                .from('Company')
                .where('DefaultCompany', '==', true)
                .expand("Address");


            var promise = managerTraveller.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                throw error;
            }
        }

        //Create company Entity and connects it to the traveller
        function addCompanyToTraveller(traveller) {
            traveller.Company = managerTraveller.createEntity('Company');
        }

        //Removes the traveller from the TravelRequest_RequestTravellers and detaches the entity so it isn't created during the saveChange
        function deleteTraveller(request, traveller) {
            request.TravelRequest_RequestTravellers.splice(_.indexOf(request.TravelRequest_RequestTravellers, traveller), 1);
            managerTraveller.detachEntity(traveller);
        }

        //Detaching the traveller Entity so it isn't created during the saveChange
        function detachTraveller(traveller) {
            managerTraveller.detachEntity(traveller);
        }
        
        //Detaching the Company if the company is not a default one.
        function removeCompanyFromTraveller(traveller) {
            if (traveller.Company && !traveller.Company.DefaultCompany) {
                managerTraveller.detachEntity(traveller.Company);
            }
        }

        function handleSaveValidationError(error) {
            var message = 'Not saved due to validation error';
            try {
                var firstErr = error.entityErrors[0];
                message += ': ' + firstErr.errorMessage;
            } catch (e) { }
            return message;
        }

        //Get All travellers
        function getTravellers() {
            var query = breeze.EntityQuery
                .from('RequestTraveller');


            var promise = managerTraveller.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                throw error;
            }
        }

        //Gets traveller from database by ID
        function getTravellerById(id) {
            var query = breeze.EntityQuery
                .from('RequestTraveller')
                .where("Id", "==", id);

            var promise = managerTraveller.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                throw error;
            }
        }

        //Gets travellers travelrequests/itineraries from database by ID
        function getTravellerRequests(id) {
            var query = breeze.EntityQuery
                .from('TravelRequest_RequestTravellers')
                .where("RequestTravellerID", "==", parseInt(id));

            var promise = managerTraveller.executeQuery(query).catch(queryFailed);
            return promise;

            function queryFailed(error) {
                console.log(error.message, 'query failed');
                throw error;
            }
        }

        //Saves entities
        function saveChanges(onSuccess, onFailure) {
            return managerTraveller.saveChanges()
                .then(saveSucceeded)
                .catch(saveFailed);

            function saveSucceeded(result) {
                onSuccess(result);
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
        }

    }
})();