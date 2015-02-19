(function() {
    'use strict';

    // Define the directive on the module.
    // Inject the dependencies. 
    // Point to the directive definition function.
    angular.module('app').directive('wizardStepSwitch', [wizardStepSwitch]);
    
    function wizardStepSwitch ($window) {
        // Usage:
        // 
        // Creates:
        // 
        var directive = {
            link: link,
            scope: {
                model: '=ngModel',
                label: '@label',
                step:  '@step'
            },
            templateUrl: '/TravelAgency/Content/src/app/wizard/partials/wizardStepSwitch.tpl.html',
            restrict: 'A'
        };
        return directive;

        function link(scope, element, attrs) {
        }
    }

})();