(function () {
    'use strict';

    angular.module('components.form').directive('formControlStatic', [formControlStatic]);

    function formControlStatic() {
        // Usage:
        //   <div data-form-control data-label="My test control">
        //     <!--Input element, hints and other elements here-->
        //   </div>
        // Creates:
        //   A two-column form group with a label on the left hand side
        //   and your content on the right hand side.
        var directive = {
            link: link,
            transclude: true,
            scope: {
                label: '@'
            },
            templateUrl: '/TravelAgency/Content/src/assets/partials/formControlStatic.tpl.html',
            restrict: 'A'
        };
        return directive;

        function link(scope, element, attrs) {
        }
    }

})();