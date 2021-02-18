using System.Web;
using System.Web.Optimization;

namespace Literary_Arts
{
    public class BundleConfig
    {
        // 如需統合的詳細資訊，請瀏覽 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // 準備好可進行生產時，請使用 https://modernizr.com 的建置工具，只挑選您需要的測試。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            //樣式表
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/css/aos.css",
                "~/Content/css/header.css",
                  "~/Content/css/index.css"));

            //js
            bundles.Add(new ScriptBundle("~/Content/js").Include(
                    "~/Content/js/aos.js",
                     "~/Content/js/index.js",
                    "~/Scripts/bootstrap.js",
                     "~/Content/js/active.js",
                     "~/Scripts/Literary_Arts/common.js"
                      ));


        }
    }
}
