using System.Web;
using System.Web.Optimization;

namespace com.cpp.calypso.web
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                     "~/Scripts/jquery.blockUI*",
                     "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                    "~/Scripts/jquery.unobtrusive*",
                    "~/Scripts/jquery.validate*"));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));


            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/ Scripts/popper.min.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-datepicker.js",
                      "~/Scripts/locales/bootstrap-datepicker.es.min.js",
                      "~/Scripts/respond.js"));


            //Script Vendor 
            bundles.Add(new ScriptBundle("~/bundles/vendor").Include(
                    "~/Scripts/locales/moment.min.js",
                    "~/Scripts/toastr.js",

                    "~/Scripts/sweetalert/dist/sweetalert.min.js",
                    //abp
                    "~/Scripts/Abp/Framework/scripts/abp.js",
                    "~/Scripts/Abp/Framework/scripts/libs/abp.jquery.js",
                    "~/Scripts/Abp/Framework/scripts/libs/abp.toastr.js",
                    "~/Scripts/Abp/Framework/scripts/libs/abp.blockUI.js",
                    "~/Scripts/Abp/Framework/scripts/libs/abp.sweet-alert.js"
                    ));



            //Agregar javascript App de la plataforma
            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                            "~/Scripts/app/app.js"
                ));



            bundles.Add(new StyleBundle("~/Content/bundle_css").Include(

                      "~/Content/bootstrap.css",
                      // "~/Content/ShardsReact/shards.min.css",
                      "~/Content/font-awesome.css",
                      "~/Content/simple-line-icons.min.css",
                      "~/Content/bootstrap-datepicker3.css",
                      "~/Content/toastr.min.css",

                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/bundle_css_react").Include(
                   "~/Content/css/omega/theme.css",
                               "~/Content/css/react-bt/react-bootstrap-table-all.min.css",
                   "~/Content/css/primereact.min.css",
                   "~/Content/css/primeicons.css",
                   "~/Content/css/block-ui.css",
                   "~/Content/css/time-picker.css"));

            bundles.Add(new StyleBundle("~/Content/bundle_css_react_primereact_v2").Include(
                  "~/Content/css/nova-light/theme.css",
                  "~/Content/css/primereact-v2/primereact.min.css",
                              "~/Content/css/react-bt/react-bootstrap-table-all.min.css",
                  "~/Content/css/primeicons.css",
                   "~/Content/css/block-ui.css",
                     "~/Content/fonts/roboto-v15-latin-regular.eot",
                       "~/Content/fonts/roboto-v15-latin-regular.svg",
                            "~/Content/fonts/roboto-v15-latin-regular.ttf",
                                     "~/Content/fonts/roboto-v15-latin-regular.woff2",
                   "~/Content/css/time-picker.css"));


            bundles.Add(new StyleBundle("~/Content/bundle_css_react_primereact_v3").Include(
                  "~/Content/css/primereact-v3/nova-light/theme.css",
                  "~/Content/css/react-bt/react-bootstrap-table-all.min.css",
                  "~/Content/css/primereact-v3/primereact.min.css",
                      "~/Content/css/primereact-v3/primereact.css",
                  "~/Content/css/primeicons.css",
                   "~/Content/css/block-ui.css",
                   "~/Content/css/time-picker.css",

                   "~/Content/react-quill/quill.bubble.css",

                   "~/Content/react-quill/quill.core.css",
                   "~/Content/react-quill/quill.snow.css",
                     "~/Content/fonts/roboto-v15-latin-regular.eot",
                       "~/Content/fonts/roboto-v15-latin-regular.svg",
                            "~/Content/fonts/roboto-v15-latin-regular.ttf",
                                     "~/Content/fonts/roboto-v15-latin-regular.woff2"
                   ));
            bundles.Add(new StyleBundle("~/Content/primereact_").Include(
                    "~/Content/css/primereact_/nova/theme.css",
                    "~/Content/css/primereact_/primeicons.css",
                    "~/Content/css/primereact_/primereact.min.css",
                    "~/Content/css/primereact_/primereact.css",
                    "~/Content/css/primereact_/flags.css",
                    "~/Content/css/primereact_/fonts/primeicons.eot",
                    "~/Content/css/primereact_/fonts/primeicons.svg",
                    "~/Content/css/primereact_/fonts/primeicons.ttf",
                    "~/Content/css/primereact_/fonts/primeicons.woff",
                                "~/Content/css/react-bt/react-bootstrap-table-all.min.css",
                      "~/Content/fonts/roboto-v15-latin-regular.eot",
                       "~/Content/fonts/roboto-v15-latin-regular.svg",
                            "~/Content/fonts/roboto-v15-latin-regular.ttf",
                                     "~/Content/fonts/roboto-v15-latin-regular.woff2",
                    
                    "~/Content/css/block-ui.css",
                   "~/Content/css/time-picker.css",
                   "~/Content/react-quill/quill.bubble.css",
                   "~/Content/react-quill/quill.core.css",
                   "~/Content/react-quill/quill.snow.css"
                   ));



        }
    }
}
