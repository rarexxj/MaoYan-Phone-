using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using BntWeb.UI.Bundle;

namespace BntWeb.Coupon
{
    public class BundleProvider: IBundleProvider
    {
        public void RegisterBundles(BundleCollection bundles)
        {
            //Js
            bundles.Add(new ScriptBundle("~/js/coupon/list").Include(
                      "~/Modules/BntWeb.Coupon/Content/Scripts/coupon.js"));
            #region Web

            //Js

            bundles.Add(new ScriptBundle("~/js/coupon/webcoupon").Include(
                    "~/Modules/BntWeb.Coupon/Content/Scripts/web.coupon.js"));
            //Js 我的优惠券
            bundles.Add(new ScriptBundle("~/js/coupon/mycoupon").Include(
                 "~/Modules/BntWeb.Coupon/Content/Scripts/web.mycoupon.js"));

            //css 我的优惠券
            bundles.Add(new StyleBundle("~/css/mycoupon").Include
                ("~/Resources/Web/Css/personal.css",
                "~/Resources/Css/order.css"));

            #endregion

        }
    }
}