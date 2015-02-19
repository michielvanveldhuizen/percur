(function () {
    'use strict';
    // Module name is handy for logging
    var id = 'components.featureInterceptor';
    console.log('hierrr');
    // Create the module and define its dependencies.
    var featureInterceptor = angular.module(id, [])
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.responseInterceptors.push([
            '$q', '$templateCache', 'activeProfile',
            function ($q, $templateCache, activeProfile) {
                // Keep track which HTML templates have already been modified.
                var modifiedTemplates = {};

                // Tests if there are any keep/omit attributes.
                var HAS_FLAGS_EXP = /data-(keep|omit)/;

                // Tests if the requested url is a html page.
                var IS_HTML_PAGE = /\.html$|\.html\?/i;

                return function (promise) {
                    return promise.then(function (response) {
                        var url = response.config.url,
                            responseData = response.data;

                        if (!modifiedTemplates[url] && IS_HTML_PAGE.test(url) &&
                            HAS_FLAGS_EXP.test(responseData)) {

                            // Create a DOM fragment from the response HTML.
                            var template = $('<div>').append(responseData);

                            // Find and parse the keep/omit attributes in the view.
                            template.find('[data-keep],[data-omit]').each(function () {
                                var element = $(this),
                                    data = element.data(),
                                    keep = data.keep,
                                    features = keep || data.omit || '';

                                // Check if the user has all of the specified features.
                                var hasFeature = _.all(features.split(','), function (feature) {
                                    return activeProfile.features[feature];
                                });

                                if (features.length &&
                                    ((keep && !hasFeature) || (!keep && hasFeature))) {
                                    element.remove();
                                }
                            });
                            // Set the modified template.
                            response.data = template.html();

                            // Replace the template in the template cache, and mark the
                            // template as modified.
                            $templateCache.put(url, response.data);
                            modifiedTemplates[url] = true;
                        }

                        return response;
                    },

                    // Reject the response in case of an error.
                    function (response) {
                        return $q.reject(response);
                });
            };
        }]);
    }]);
})();