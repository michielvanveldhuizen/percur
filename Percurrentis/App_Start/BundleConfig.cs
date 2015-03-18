using System.Web.Optimization;

namespace Percurrentis
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Modernizr (browser feature detection library) &
            // Respond.js (Enable responsive CSS code on browsers that don't support it, eg IE8)
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Content/vendor/ProUI/js/vendor/modernizr-2.7.1-respond-1.4.2.min.js"));

            // Vendor JavaScript
            bundles.Add(new ScriptBundle("~/bundles/vendor/js").Include(
                "~/Content/vendor/angular/angular.js",
                "~/Content/vendor/angular/angular-animate.js",
                "~/Content/vendor/angular/angular-resource.js",
                "~/Content/vendor/angular/angular-cookies.js",
                "~/Content/vendor/angular/angular-sanitize.js",
                "~/Content/vendor/angular/angular-route.js",
                "~/Content/vendor/angular-chosen/chosen.js",
                "~/Content/vendor/angular-ui/ui-bootstrap-0.10.0.js",
                "~/Content/vendor/underscore/underscore-min.js"));

            // Breeze
            bundles.Add(new ScriptBundle("~/bundles/scripts/breeze").Include(
                "~/Scripts/breeze.debug.js",
                "~/Scripts/breeze.angular.js"));

            // Application JavaScript
            bundles.Add(new ScriptBundle("~/bundles/src/js").Include(
                "~/Content/src/app/app.js",
                "~/Content/src/app/constants.js",
                "~/Content/src/app/breadcrumbs/breadcrumbs.js",
                "~/Content/src/app/breadcrumbs/breadcrumbCtrl.js",
                "~/Content/src/app/modal/modal.js",
                "~/Content/src/app/travelRequest/requestCtrl.js",
                "~/Content/src/app/travelRequest/requestApprovalCtrl.js",
                "~/Content/src/app/insurance/insuranceCtrl.js",
                "~/Content/src/app/travellers/travellersCtrl.js",
                "~/Content/src/app/wizard/requestCtrl.js",
                "~/Content/src/app/wizard/travelRequestService.js",
                "~/Content/src/app/wizard/partials/wizardStepSwitch.js",
                "~/Content/src/app/itinerary/itineraryCtrl.js",
                "~/Content/src/app/proposalWizard/proposalCtrl.js",
                "~/Content/src/app/current/currentCtrl.js",
                "~/Content/src/app/wizard/partials/formtypes/requestTraveller.js",
                "~/Content/src/assets/partials/legendTitle.js",
                "~/Content/src/assets/partials/formControl.js",
                "~/Content/src/assets/partials/formControlStatic.js",
                "~/Content/src/components/breeze.directives.validation.js",
                "~/Content/src/components/entityManagerFactory.js",
                "~/Content/src/components/featureInterceptor.js",
                "~/Content/src/components/notificationWidget.js",
                "~/Content/src/components/rcSubmit.js",
                "~/Content/src/components/wizard/wizard.js",
                "~/Content/src/components/form/address.js",
                "~/Content/src/components/form/addressForm.js"));
        }
    }
}