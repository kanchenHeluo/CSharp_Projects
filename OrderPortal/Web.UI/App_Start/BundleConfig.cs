using System.Web;
using System.Web.Optimization;

namespace Web.UI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"
            ));

            bundles.Add(new ScriptBundle("~/bundles/ecitglobal").Include(
                      "~/Scripts/Ecitglobal/Message_Banner.js",
                      "~/Scripts/Ecitglobal/Modal_dialog.js",
                      "~/Scripts/Ecitglobal/Search_Field.js",
                      "~/Scripts/Ecitglobal/Tabs.js"
            ));

            bundles.Add(new StyleBundle("~/Content/css/css").Include(
              
                //ECIT Design Guid -- Start
                      "~/Content/Ecitglobal/Filter_tiles.css",
                      "~/Content/Ecitglobal/Filter_tiles.override.css",
                      "~/Content/Ecitglobal/Form_module.css",
                      "~/Content/Ecitglobal/Form_module.override.css",
                      "~/Content/Ecitglobal/GlobalStyles.css",
                      "~/Content/Ecitglobal/GlobalStyles.override.css",
                      "~/Content/Ecitglobal/GlobalSymbols.css",
                      "~/Content/Ecitglobal/GlobalSymbols.override.css",
                      "~/Content/Ecitglobal/icons.css",
                      "~/Content/Ecitglobal/iconsoverride.css",
                      "~/Content/Ecitglobal/Image_or_Icon_Button.css",
                      "~/Content/Ecitglobal/Message_Banner.css",
                      "~/Content/Ecitglobal/Modal_dialog.css",
                      "~/Content/Ecitglobal/Search_Field.css",
                      "~/Content/Ecitglobal/Tabs.css",
                      "~/Content/Ecitglobal/Error_Summary.css",
                      "~/Content/Ecitglobal/Error_Summary.override.css",
                   
                //Site 
                      "~/Content/Login/login.css",
                      "~/Content/Site/site.css",
                      "~/Content/Site/tile.css",

                //Order
                      "~/Content/Order/autocomplete.css",

                //OrderDashboard override
                      "~/Content/Order/OrderDashboard/OrderDashboard.css"
            ));

            bundles.Add(new StyleBundle("~/Content/css/customizewidgets").Include(
                "~/Content/Order/OrderDashboard/opPanelcontainer.css",
                "~/Content/Order/OrderDashboard/opTable.css",
                "~/Content/Order/OrderDashboard/opPagination.css",
                "~/Content/Order/OrderDashboard/opDropdown.css",
                "~/Content/Order/OrderDashboard/opBusy.css"
            ));

            bundles.Add(new StyleBundle("~/Content/css/kendo").Include(
                "~/Content/kendo/kendo.common.min.css",
                "~/Content/kendo/kendo.metro.min.css",
                "~/Content/kendo/kendo.dataviz.min.css",
                "~/Content/kendo/kendo.dataviz.metro.min.css",
                "~/Content/kendo/kendo.common.override.css"
            ));

            bundles.Add(new ScriptBundle("~/kendo/js").Include(
                "~/Scripts/Kendo/kendo.core.min.js",
                "~/Scripts/Kendo/kendo.calendar.min.js",
                "~/Scripts/Kendo/kendo.popup.min.js",
                "~/Scripts/Kendo/kendo.datepicker.min.js"
            ));

            bundles.Add(new ScriptBundle("~/external/js").Include(
             "~/Scripts/amplify/amplify.core.min.js",
             "~/Scripts/amplify/amplify.store.min.js"
             ));

            bundles.Add(new ScriptBundle("~/common/js").Include(
               "~/Scripts/App/Order/orderApp.js",
                "~/Scripts/App/Common/opCommonModule.js",
                "~/Scripts/App/Common/Services/opNotificationSvc.js",
                "~/Scripts/App/Common/Services/opAjaxSvc.js",
                "~/Scripts/App/Common/Services/localStorageSvc.js",
                "~/Scripts/App/Common/Services/domainDataSvc.js",
                "~/Scripts/App/Common/Services/channelPartnerSvc.js",

                "~/Scripts/App/Common/Directives/opDialog.js",
                "~/Scripts/App/Common/Directives/opSlideIn.js",
                "~/Scripts/App/Common/Directives/autocomplete.js",
                "~/Scripts/App/Common/Directives/opPagination.js",
                "~/Scripts/App/Common/Directives/opDatepicker.js",
                "~/Scripts/App/Common/Directives/opReadOnly.js",
                "~/Scripts/App/Common/Directives/opClickDisabled.js",
                "~/Scripts/App/Common/Directives/opPanelcontainer.js",
                "~/Scripts/App/Common/Directives/opForm.js",
                "~/Scripts/App/Common/Directives/opRequired.js",
                "~/Scripts/App/Common/Directives/opNumber.js",
                "~/Scripts/App/Common/Directives/opDropdown.js",
                "~/Scripts/App/Common/Directives/opFileUploader.js",
                "~/Scripts/App/Common/Directives/opErrorsummary.js",
                "~/Scripts/App/Common/Directives/opBusyPublisher.js",
                "~/Scripts/App/Common/Directives/opBusySubscriber.js",

                "~/Scripts/App/Common/Controllers/choosepartnerCtrl.js",
                "~/Scripts/App/Common/Controllers/userPreferenceCtrl.js"
            ));

            bundles.Add(new ScriptBundle("~/orderApp/js").Include(
                "~/Scripts/App/Order/Services/orderDashboardSvc.js",
                "~/Scripts/App/Order/Services/autocompleteSvc.js",
                "~/Scripts/App/Order/Services/orderSvc.js",

                "~/Scripts/App/Order/Controllers/autocompleteCtrl.js",
                "~/Scripts/App/Order/Controllers/orderDashboardCtrl.js",
                "~/Scripts/App/Order/Controllers/agreementSearchCtrl.js",

                "~/Scripts/App/Order/Controllers/orderEditorCtrl.js",
                "~/Scripts/App/Order/Controllers/orderHeaderCtrl.js",
                "~/Scripts/App/Order/Controllers/orderLineItemCtrl.js",
                "~/Scripts/App/Order/Controllers/orderShipToCtrl.js",
                "~/Scripts/App/Order/Controllers/orderSubmitCtrl.js",
                "~/Scripts/App/Order/Controllers/orderSummaryCtrl.js",
                "~/Scripts/App/Order/Controllers/mockuiCtrl.js",
                "~/Scripts/App/Order/Controllers/batchUploadCtrl.js"
            ));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862

            BundleTable.EnableOptimizations = true;
#if DEBUG
            BundleTable.EnableOptimizations = false;
#endif  
        }
    }
}