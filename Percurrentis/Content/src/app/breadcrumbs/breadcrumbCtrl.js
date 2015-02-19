(function () {
    'use strict';

    var controllerId = 'breadcrumbCtrl';

    angular.module('app').controller(controllerId,
        ['$scope', 'breadcrumbs',
        function($scope, breadcrumbs) {

        $scope.breadcrumbs = breadcrumbs;
    }])
})();
