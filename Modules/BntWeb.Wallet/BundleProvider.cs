using System.Web.Optimization;
using BntWeb.UI.Bundle;

namespace BntWeb.Wallet
{
    public class BundleProvider : IBundleProvider
    {
        public void RegisterBundles(BundleCollection bundles)
        {
            //Js
            bundles.Add(new ScriptBundle("~/js/admin/Wallet/list").Include(
                      "~/Modules/BntWeb.Wallet/Content/Scripts/crashapply.list.js"));
            bundles.Add(new ScriptBundle("~/js/admin/Wallet/bill/list").Include(
                      "~/Modules/BntWeb.Wallet/Content/Scripts/bill.list.js"));

            //我的积分 css
            bundles.Add(new StyleBundle("~/css/web/integral").Include(
                 "~/Resources/Css/order.css",
                 "~/Resources/Web/Css/personal.css"
              ));
            bundles.Add(new ScriptBundle("~/js/web/myintegral").Include(
                "~/Modules/BntWeb.Wallet/Content/Scripts/web.integral.js"));


        }
    }
}