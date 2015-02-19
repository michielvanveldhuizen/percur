(function() {
    'use strict';

    // Define the directive on the module.
    // Inject the dependencies. 
    // Point to the directive definition function.
    angular.module('components.form').directive('addressForm', [addressForm]);
    
    function addressForm () {
        // Usage:
        // 
        // Creates:
        // 
        var directive = {
            link: link,
            scope: {
                model: '=ngModel',
                countries: '='
            },
            templateUrl: '/TravelAgency/Content/src/components/form/addressForm.html',
            restrict: 'AE'
        };

        return directive;

        function link(scope, element, attrs) {
        }
    }

})();