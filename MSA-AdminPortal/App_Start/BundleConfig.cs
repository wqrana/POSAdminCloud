using System.Web;
using System.Web.Optimization;

namespace MSA_AdminPortal
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
       
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;
            bundles.IgnoreList.Clear(); // required
            // Code removed for clarity.
            
            BundleTable.EnableOptimizations = false;

            bundles.Add(new ScriptBundle("~/content/metronic/plugins/core/js/ie9").Include(
                       "~/content/themes/assets/global/plugins/respond.min.js",
                       "~/content/themes/assets/global/plugins/excanvas.min.js"
                       ));


            //core scripts
            bundles.Add(new ScriptBundle("~/content/metronic/plugins/core/js").Include(
                //"~/content/themes/assets/global/plugins/jquery.min.js",
                //"~/content/themes/assets/global/plugins/jquery-migrate.min.js",
                //"~/content/themes/assets/global/plugins/jquery-ui/jquery-ui.min.js",
                            "~/content/themes/assets/global/plugins/bootstrap/js/bootstrap.min.js",
                            "~/content/themes/assets/global/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js",
                            "~/content/themes/assets/global/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                //"~/content/themes/assets/global/plugins/jquery.blockui.min.js",
                            "~/content/themes/assets/global/plugins/jquery.cokie.min.js",
                            "~/content/themes/assets/global/plugins/uniform/jquery.uniform.min.js"
                            , "~/content/themes/assets/global/plugins/bootstrap-switch/js/bootstrap-switch.min.js"
                            , "~/content/themes/assets/global/plugins/bootstrap-toastr/toastr.min.js"
                            , "~/content/themes/assets/global/plugins/select2/select2.js"
                      ));



            //page level scripts
            bundles.Add(new ScriptBundle("~/content/metronic/scripts/js").Include(
                "~/content/themes/assets/global/scripts/metronic.js",
                "~/content/themes/assets/admin/layout/scripts/layout.js",
                "~/content/themes/assets/admin/layout/scripts/quick-sidebar.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/common/js").Include(
               "~/scripts/common.js"
               ));


            // GLOBAL MANDATORY STYLES
            bundles.Add(new StyleBundle("~/content/metronic/mandatory/css").Include(
                "~/content/themes/assets/global/plugins/font-awesome/css/font-awesome.min.css"
                , "~/content/themes/assets/global/plugins/simple-line-icons/simple-line-icons.min.css"
                , "~/content/themes/assets/global/plugins/bootstrap-switch/css/bootstrap-switch.min.css"
                , "~/content/themes/assets/global/plugins/bootstrap/css/bootstrap.min.css"
                 , "~/content/themes/assets/global/plugins/uniform/css/uniform.default.css"
                 //, "~/content/themes/assets/global/css/style.css"
                  //, "~/Content/themes/assets/global/plugins/bootstrap-switch/static/stylesheets/bootstrap-switch-metro.css"
                  //, "~/Content/themes/assets/global/plugins/bootstrap-switch/static/stylesheets/bootstrap-switch.css"
                , "~/Content/themes/assets/global/plugins/bootstrap-toastr/toastr.min.css"
               , "~/Content/themes/assets/global/plugins/select2/select2.css"

                ));


            // PAGE LEVEL PLUGIN STYLES
            bundles.Add(new StyleBundle("~/content/metronic/plugin/css").Include(
                "~/Content/themes/assets/global/plugins/bootstrap-datepicker/css/datepicker.css"
            ));


            // THEME STYLES
            bundles.Add(new StyleBundle("~/content/metronic/theme/css").Include(
                    "~/content/themes/assets/global/css/components.css"
                    , "~/content/themes/assets/global/css/plugins.css"
                    , "~/content/themes/assets/admin/layout/css/layout.css"
                    , "~/content/themes/assets/admin/layout/css/themes/default.css"
                    , "~/content/themes/assets/admin/layout/css/custom.css"
                    ));

            // custom STYLES
            bundles.Add(new StyleBundle("~/content/custom/css").Include(
                "~/content/custom.css"
                ));
            // 

            bundles.Add(new StyleBundle("~/Content/themes/datatables/css").Include(
                "~/Content/themes/assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.css",
                "~/Content/themes/assets/global/plugins/datatables/plugins/responsive/dataTables.responsive.css"
                ));
            // 

            bundles.Add(new ScriptBundle("~/Content/themes/datatables/js").Include(
                "~/Content/themes/assets/global/plugins/datatables/media/js/jquery.dataTables.js",
                "~/Content/themes/assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js",
                "~/Content/themes/assets/admin/pages/scripts/table-ajax.js",
                "~/Scripts/jquery.dataTables.columnFilter.js",
                "~/Content/themes/assets/global/plugins/datatables/plugins/responsive/dataTables.responsive.min.js",
                "~/Content/themes/assets/global/plugins/datatables/plugins/responsive/responsive.bootstrap.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/SaveTimeZone").Include(
                "~/Scripts/SaveTimeZone.js"
                ));

        }
    }
}