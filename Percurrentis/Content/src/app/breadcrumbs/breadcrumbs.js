(function () {
    'use strict';

    // Module name is handy for logging
    var id = 'services.breadcrumbs';

    // Create the module and define its dependencies.
    var breadcrumbs = angular.module(id, []);

    breadcrumbs.factory('breadcrumbs', ['$rootScope', '$location', '$route', function ($rootScope, $location, $route) {

        var breadcrumbs = [];
        var breadcrumbsService = {};

        //we want to update breadcrumbs only when a route is actually changed
        //as $location.path() will get updated immediately (even if route change fails!)
        $rootScope.$on('$routeChangeSuccess', function (event, current) {
            $rootScope.title = $route.current.data.title;
            if ($route.current.data && $route.current.data.breadcrumbs) {
                breadcrumbs = $route.current.data.breadcrumbs;
                _.each(breadcrumbs, function (val) {
                    if (typeof val.name !== typeof undefined) {
                        if (val.name == "Request Detail") {
                            needFooterCheck = true;
                        }
                    }
                    if (!val.path || val.path == '') {
                        val.path = 'javascript:void(0)';
                    }
                });
                
                return;
            }

            var pathElements = $location.path().split('/'), result = [], i;
            var breadcrumbPath = function (index) {
                return '#/' + (pathElements.slice(0, index + 1)).join('/');
            };

            // Add a "Home" link
            result.push({ name: "Home", path: breadcrumbPath(0) });
            
            pathElements.shift();
            if (pathElements[0] !== "") {
                for (i = 0; i < pathElements.length; i++) {
                    result.push({ name: pathElements[i], path: breadcrumbPath(i) });
                }
            }

            breadcrumbs = result;
            
        });

        breadcrumbsService.all = function () {
            return breadcrumbs;
        };

        breadcrumbsService.first = function () {
            return breadcrumbs[0] || {};
        };

        return breadcrumbsService;
    }]);

})();
