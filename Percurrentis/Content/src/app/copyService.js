(function () {
    'use strict';

    var serviceId = 'copyService';

    angular.module('app').factory(serviceId, ['$q', 'breeze',
        copyService]);

    function copyService($q, breeze) {

        var service = {
            createEntity: createEntity,
            copyAccommodation: copyAccommodation
        };

        return service;
        
        function createEntity(entityType)
        {
            console.log(entityType);
            var serviceName = 'breeze/TravelRequest';
            var manager = new breeze.EntityManager(serviceName);

            var deferred = $q.defer();

            if (!manager.metadataStore.hasMetadataFor(serviceName)) {
                manager.fetchMetadata()
                .then(function () {

                    deferred.resolve(makeEntity(entityType));
                }, function (error) {
                    console.log('Error while fetching metadata', error);
                    deferred.reject(error);
                });
            } else {
                deferred.resolve(makeEntity(entityType));
            }

            return deferred.promise;

            function makeEntity(entityType)
            {
                var request = manager.createEntity(entityType);
                request.Address = manager.createEntity('Address');
                return request;
            }
        }

        function copyAccommodation(source, destination) {
            destination.Address.AddressName = source.Address.AddressName;
            destination.Address.Street = source.Address.Street;
            destination.Address.PostalCode = source.Address.PostalCode;
            destination.Address.City = source.Address.City;
            destination.Address.StateProvince = source.Address.StateProvince;
            destination.Address.CountryRegionID = source.Address.CountryRegionID;
            destination.ParentID = source.Id;

            //destination.TravelRequestID = null;
            destination.TravelProposalID = source.TravelProposalID;

            destination.CheckInDate = source.CheckInDate;
            destination.CheckOutDate = source.CheckOutDate;

            destination.Cost = source.Cost;
            destination.CostSecondary = source.CostSecondary;
            destination.SecondaryCurrency = source.SecondaryCurrency;

            return destination;
        }
    }
})();