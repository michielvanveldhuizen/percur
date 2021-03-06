﻿(function () {
    'use strict';

    // Create module namespaces
    angular.module('components.form', []);

    // Module name is handy for logging
    var id = 'app';
    
    // Create the module and define its dependencies.
    var app = angular.module('app', [
        // Angular modules 
        'ngCookies',
        'ngResource',
        'ngSanitize',
        'ngRoute',
        'ngAnimate',

        // Custom modules 
        'app.constants',
        'components.wizard',
        'components.form',
        'services.breadcrumbs',

        // 3rd Party Modules
        'components.loadingWidget',
        'components.rcSubmit',
        'components.featureInterceptor',
        'ui.bootstrap',
        'localytics.directives',
        'breeze.angular',
        'xeditable',
        'ui.bootstrap',
        'breeze.directives'
    ])
    .config(['$routeProvider', 'activeProfile', 'breezeProvider', function ($routeProvider, activeProfile, breezeProvider) {
        var base = "/TravelAgency/Content/src/app"
        
        $routeProvider
            .when('/',
                {
                templateUrl: base + '/home/home.tpl.html',
                data: {
                    title: "Home",
                    icon: "gi-home"
                }
            })
            .when('/Request/', {
                templateUrl: base + '/travelRequest/list.tpl.html',
                data: {
                    title: 'Travel Requests',
                    icon: 'gi-show_lines',
                    breadcrumbs: [{
                        name: "Home",
                        path: "/TravelAgency/#/"
                    },
                    {
                        name: "Travel Requests",
                        path: "/TravelAgency/#/Request"
                    }]
                }
            })
            .when('/Insurance/',
            {
                templateUrl: base + '/insurance/list.tpl.html',
                data: {
                    title: "Insurance",
                    icon: "gi-notes_2",
                    breadcrumbs: [{
                        name: "Home",
                        path: "/TravelAgency/#/"
                    },
                    {
                        name: "Insurance",
                        path: "/TravelAgency/#/Insurance/"
                    }]
                }
            })
            .when('/Itinerary/',
            {
                templateUrl: base + '/itinerary/list.tpl.html',
                data: {
                    title: "Itinerary List",
                    icon: "gi-log_book",
                    breadcrumbs: [{
                        name: "Home",
                        path: "/TravelAgency/#/"
                    },
                    {
                        name: "Itinerary",
                        path: "/TravelAgency/#/Itinerary/"
                    }]
                }
            })
            .when('/Itinerary/:Hash',
            {
                templateUrl: base + '/itinerary/itinerary.tpl.html',
                data: {
                    title: "Itinerary",
                    icon: "gi-log_book",
                    breadcrumbs: [{
                        name: "Home",
                        path: "/TravelAgency/#/"
                    },
                    {
                        name: "Itinerary",
                        path: "/TravelAgency/#/Itinerary/"
                    }]
                }
            })
            .when('/Proposal/',
            {
                templateUrl: base + '/proposal/list.tpl.html',
                data: {
                    title: "Proposals",
                    icon: "gi-log_book",
                    breadcrumbs: [{
                        name: "Home",
                        path: "/TravelAgency/#/"
                    },
                    {
                        name: "Proposals",
                        path: "/TravelAgency/#/ProposalWizard/"
                    }]
                }
            })
            .when('/Proposal/:Id',
            {
                templateUrl: base + '/proposal/detail.tpl.html',
                data: {
                    title: "Proposals",
                    icon: "gi-log_book",
                    breadcrumbs: [{
                        name: "Home",
                        path: "/TravelAgency/#/"
                    },
                    {
                        name: "Proposal Detail",
                        path: "/TravelAgency/#/Proposal/"
                    }]
                }
            })
            .when('/ProposalWizard/',
            {
                templateUrl: base + '/proposalWizard/index.tpl.html',
                data: {
                    title: "ProposalWizard",
                    icon: "gi-log_book",
                    breadcrumbs: [{
                        name: "Home",
                        path: "/TravelAgency/#/"
                    },
                    {
                        name: "ProposalWizard",
                        path: "/TravelAgency/#/ProposalWizard/"
                    }]
                }
            })
            .when('/ProposalWizard/:Hash',
            {
                templateUrl: base + '/proposalWizard/index.tpl.html',
                data: {
                    title: "ProposalWizard",
                    icon: "gi-log_book",
                    breadcrumbs: [{
                        name: "Home",
                        path: "/TravelAgency/#/"
                    },
                    {
                        name: "ProposalWizard",
                        path: "/TravelAgency/#/ProposalWizard/"
                    }]
                }
            })
            .when('/Current/',
            {
                templateUrl: base + '/current/travels.tpl.html',
                data: {
                    title: "Current Travels",
                    icon: "gi-globe",
                    breadcrumbs: [{
                        name: "Home",
                        path: "/TravelAgency/#/"
                    },
                    {
                        name: "Current Travels",
                        path: "/TravelAgency/#/Current/"
                    }]
                }
            })
            .when('/Request/New', {
                templateUrl: base + '/wizard/request.tpl.html',
                data: {
                    title: "Request",
                    icon: "gi-airplane",
                    breadcrumbs: [{
                        name: "Home",
                        path: "/TravelAgency/#/"
                    },
                    {
                        name: "File Travel Request",
                        path: "/TravelAgency/#/Request"
                    }]
                }
            })
            .when('/Request/Cancelled', {
                templateUrl: base + '/wizard/cancelled.tpl.html',
                data: {
                    title: "Request Cancelled",
                    icon: "gi-ban",
                    breadcrumbs: [{
                        name: "Home",
                        path: "/TravelAgency#/"
                    },
                    {
                        name: "File Travel Request",
                        path: "/TravelAgency/#/Request/New"
                    },
                    {
                        name: "Request Cancelled",
                        path: "#/Request/Cancelled"
                    }]
                }
            })
            .when('/Request/Submitted', {
                templateUrl: base + '/wizard/submitted.tpl.html',
                data: {
                    title: "Request Submitted",
                    icon: "gi-ok",
                    breadcrumbs: [{
                        name: "Home",
                        path: "/TravelAgency/#/"
                    },
                    {
                        name: "File Travel Request",
                        path: "/TravelAgency/#/Request/New"
                    },
                    {
                        name: "Request Submitted",
                        path: "/TravelAgency/#/Request/Submitted"
                    }]
                }
            })
            .when('/Request/:Hash', {
                templateUrl: base + '/travelRequest/detail.tpl.html',
                data: {
                    title: 'Travel Request',
                    icon: 'gi-airplane',
                    breadcrumbs: [{
                        name: "Home",
                        path: "/TravelAgency/#/"
                    },
                    {
                        name: "Travel Requests",
                        path: "/TravelAgency/#/Request"
                    },
                    {
                        name: "Request Detail"
                    }]
                }
            })
            .when('/Travellers/', {
                templateUrl: base + '/travellers/list.tpl.html',
                data: {
                    title: 'Travellers List',
                    icon: 'gi-parents',
                    breadcrumbs: [{
                        name: "Home",
                        path: "/TravelAgency/#/"
                    },
                    {
                        name: "Travellers List",
                        path: "/TravelAgency/#/Travellers"
                    },
                    ]
                }
            })
            .when('/Travellers/New', {
                templateUrl: base + '/travellers/add.tpl.html',
                data: {
                    title: 'Add New Traveller',
                    icon: 'gi-parents',
                    breadcrumbs: [{
                        name: "Home",
                        path: "/TravelAgency/#/"
                    },
                    {
                        name: "Travellers List",
                        path: "/TravelAgency/#/Travellers"
                    },
                    {
                        name: "Add New Traveller",
                    },
                    ]
                }
            })
            .when('/Travellers/Edit/:id', {
                templateUrl: base + '/travellers/add.tpl.html',
                data: {
                    title: 'Edit Traveller',
                    icon: 'gi-parents',
                    breadcrumbs: [{
                        name: "Home",
                        path: "/TravelAgency/#/"
                    },
                    {
                        name: "Travellers List",
                        path: "/TravelAgency/#/Travellers"
                    },
                    {
                        name: "Edit Traveller",
                    },
                    ]
                }
            })
            .when('/Travellers/:Id', {
                templateUrl: base + '/travellers/detail.tpl.html',
                data: {
                    title: 'Traveller Details',
                    icon: 'gi-parents',
                    breadcrumbs: [{
                        name: "Home",
                        path: "/TravelAgency/#/"
                    },
                    {
                        name: "Travellers List",
                        path: "/TravelAgency/#/Travellers"
                    },
                    {
                        name: "Traveller Details",
                    },
                    ]
                }
            })
            .when('/Calendar/', { // Route page
                templateUrl: base + '/calendar/calendar.tpl.html', // the template location
                data: {
                    title: 'Calendar', // title
                    icon: 'gi-calendar', //icon
                    breadcrumbs: [{ //breadcrumbs 
                        name: "Home", // first breadcrumb
                        path: "/TravelAgency/#/" // first breadcrumb path
                    },
                    {
                        name: "Calendar", // second breadcrumb, (this one has no path so links to nothing)
                    },
                    ]
                }
            })
            .otherwise({
                redirectTo: '/'
            })
    }]);

    app.filter('toArray', function () {
        return function (obj) {
            var result = [];
            angular.forEach(obj, function (val, key) {
                result.push(val);
            });
            return result;
        };
    });

    // Execute bootstrapping code and any dependencies.
    app.run(function(editableOptions, editableThemes) 
    {
        // bootstrap3 theme. Can be also 'bs2', 'default'
        editableOptions.theme = 'bs3';
        editableThemes.bs3.inputClass = 'input-sm';
        editableThemes.bs3.buttonsClass = 'btn-sm';
    },
    ['$q', '$rootScope', '$route', 'requestNotificationChannel', 'breeze',
        function ($q, $rootScope, $route, requestNotificationChannel) {
            $rootScope.$on('$routeChangeSuccess', function () {
                if (!$route.current.data) return;

                $rootScope.pageTitle   = $route.current.data.title || 'Home';
                $rootScope.pageIcon    = $route.current.data.icon || 'gi-file';
            });

            // display loading indicator on http request start
            requestNotificationChannel.onRequestStarted($rootScope/*, NProgress.start*/);

            // hide loading indicator on http request done
            requestNotificationChannel.onRequestEnded($rootScope/*, NProgress.done*/);

            breeze.Validator.messageTemplates["required"] = 'This field is required.';

        }]);
})();