(function () {
    'use strict';

    var serviceId = 'entityManagerFactory';

    angular.module('app').factory(serviceId, ['breeze', entityManagerFactory]);

    function entityManagerFactory(breeze) {
        var serviceRoot = window.location.protocol + '//' + window.location.host + '/';
        var serviceName = serviceRoot + 'breeze/';

        var service = {
            getManager: function (endpoint) {
                return new breeze.EntityManager(serviceName + endpoint);
            },
            serviceName: serviceName
        };

        return service;
    }
})();