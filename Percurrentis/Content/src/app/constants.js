(function () {
    'use strict';

    // Module name is handy for logging
    var moduleId = 'app.constants';

    // Create the module and define its dependencies.
    var constants = angular.module(moduleId, [])
    .config(['$provide', function ($provide) {
        var profile = angular.copy(window.activeProfile);

        if (profile.roles.developer) {
            _.extend(profile.features, {
                'dev': true
            });
        }

        $provide.constant('activeProfile', profile);
    }]);
})();