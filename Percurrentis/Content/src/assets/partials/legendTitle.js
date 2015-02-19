(function() {
    'use strict';

    // Define the directive on the module.
    // Inject the dependencies. 
    // Point to the directive definition function.
    angular.module('components.form').directive('legendTitle', [legendTitle]);
    
    function legendTitle () {
        // Usage:
        // 
        // Creates:
        // 
        var directive = {
            link: link,
            transclude: true,
            scope: {},
            restrict: 'A',
            templateUrl: '/TravelAgency/Content/src/assets/partials/legendTitle.tpl.html'
        };
        return directive;

        function link(scope, element, attrs) {
            scope.title = attrs.legendTitle;
        }
    }

})();