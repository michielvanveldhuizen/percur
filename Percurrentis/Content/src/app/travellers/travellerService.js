(function () {
    'use strict';

    var serviceId = 'travellerService';

    angular.module('app').factory(serviceId, ['$q', 'breeze',
        travellerService]);

    function travellerService($q, breeze) {
        var serviceName = 'breeze/TravelRequest';

        //console.log(breeze);
        
        var managerTraveller = new breeze.EntityManager(serviceName);

        //console.log(manager);

        //To use a function first declare it here 
        var service = {
            createTraveller: createTraveller,
            getTravellerCompanies: getTravellerCompanies,
            addCompanyToTraveller: addCompanyToTraveller,
            deleteTraveller: deleteTraveller,
            removeCompanyFromTraveller: removeCompanyFromTraveller,
            saveChanges: saveChanges,
        };


        return service;

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


                /*managerTraveller.executeQuery(query).then(function (q) {
                    request.CustomerOrProspect.Address.AddressType =
                        _.first(q.results);*/

                    deferred.resolve(request);
               /* }, function (error) {
                    console.log('Error while creating travel ' +
                        'request (fetching address types)', error);
                    deferred.reject(error);
                });*/

                return deferred.promise;
            }
        }

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

        function addCompanyToTraveller(traveller) {
            traveller.Company = managerTraveller.createEntity('Company');
        }

        function deleteTraveller(request, traveller) {
            request.TravelRequest_RequestTravellers.splice(_.indexOf(request.TravelRequest_RequestTravellers, traveller), 1);
            managerTraveller.detachEntity(traveller);
        }

        function detachTraveller(traveller) {
            managerTraveller.detachEntity(traveller);
        }

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