(function() {
    'use strict';

    // Define the directive on the module.
    // Inject the dependencies. 
    // Point to the directive definition function.
    angular.module('components.form').directive('address', [address]);
    
    function address () {
        // Usage:
        // 
        // Creates:
        // 
        var directive = {
            link: link,
            scope: {
                model: '=ngModel'
            },
            transclude: true,
            templateUrl: '/TravelAgency/Content/src/components/form/address.html',
            restrict: 'AE'
        };

        return directive;

        function link(scope, element, attrs) {
        }
    }

})();