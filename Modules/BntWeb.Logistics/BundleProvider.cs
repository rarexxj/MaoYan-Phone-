using System.Web.Optimization;
using BntWeb.UI.Bundle;

namespace BntWeb.Logistics
{
    public class BundleProvider : IBundleProvider
    {
        public void RegisterBundles(BundleCollection bundles)
        {
            //Js
            bundles.Add(new ScriptBundle("~/js/admin/logistics/shipping/list").Include(
                      "~/Modules/BntWeb.Logistics/Content/Scripts/shipping.list.js"));

            bundles.Add(new ScriptBundle("~/js/admin/logistics/shippingarea/list").Include(
                      "~/Modules/BntWeb.Logistics/Content/Scripts/shippingarea.list.js"));

            bundles.Add(new ScriptBundle("~/js/admin/logistics/shippingarea/edit").Include(
                     "~/Modules/BntWeb.Logistics/Content/Scripts/shippingarea.edit.js"));

        }
    }
}