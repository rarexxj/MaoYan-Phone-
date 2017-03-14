/* 
    ======================================================================== 
        File name：		BundleProvider
        Module:			
        Author：		        罗嗣宝
        Create Time：		2016/6/17 15:21:48
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System.Web.Optimization;
using BntWeb.UI.Bundle;

namespace BntWeb.Mall
{
    public class BundleProvider : IBundleProvider
    {
        public void RegisterBundles(BundleCollection bundles)
        {
            //editable
            bundles.Add(new StyleBundle("~/css/admin/editable").Include(
                      "~/Resources/Admin/Css/jquery.gritter.css",
                      "~/Resources/Admin/Css/bootstrap-editable.css"));
            bundles.Add(new ScriptBundle("~/js/admin/editable").Include(
                      "~/Resources/Admin/Scripts/jquery.gritter.min.js",
                      "~/Resources/Admin/Scripts/x-editable/bootstrap-editable.min.js",
                      "~/Resources/Admin/Scripts/x-editable/ace-editable.min.js"));


            //Js
            bundles.Add(new ScriptBundle("~/js/admin/mall/goodstype/list").Include(
                      "~/Modules/BntWeb.Mall/Content/Scripts/goodstype.list.js"));
            bundles.Add(new ScriptBundle("~/js/admin/mall/specials/list").Include(
                    "~/Modules/BntWeb.Mall/Content/Scripts/specials.list.js"));
           
            bundles.Add(new ScriptBundle("~/js/admin/mall/goodsbrand/list").Include(
                      "~/Modules/BntWeb.Mall/Content/Scripts/goodsbrand.list.js"));

            bundles.Add(new ScriptBundle("~/js/admin/mall/attribute/list").Include(
                      "~/Modules/BntWeb.Mall/Content/Scripts/attribute.list.js"));

            bundles.Add(new ScriptBundle("~/js/admin/mall/goods/list").Include(
                      "~/Modules/BntWeb.Mall/Content/Scripts/goods.list.js"));

            bundles.Add(new ScriptBundle("~/js/admin/mall/goodscategories/list").Include(
                      "~/Modules/BntWeb.Mall/Content/Scripts/goodscategory.list.js"));

            bundles.Add(new ScriptBundle("~/js/admin/mall/goodscategories/edit").Include(
                      "~/Modules/BntWeb.Mall/Content/Scripts/goodscategory.edit.js"));

            bundles.Add(new ScriptBundle("~/js/admin/mall/goods/edit").Include(
                      "~/Modules/BntWeb.Mall/Content/Scripts/goods.edit.js",
                      "~/Modules/BntWeb.Mall/Content/Scripts/goods.edit.category.js"));

            bundles.Add(new ScriptBundle("~/js/admin/mall/goodsshortage/list").Include(
                     "~/Modules/BntWeb.Mall/Content/Scripts/goodsshortage.list.js"));

            bundles.Add(new ScriptBundle("~/js/admin/mall/goodsrecycle/list").Include(
                   "~/Modules/BntWeb.Mall/Content/Scripts/goodsrecycle.list.js"));

            #region web  js
            //商品详情或确认订单的头部引用的Js
            bundles.Add(new ScriptBundle("~/js/goodDetails").Include(
                                "~/Resources/Web/js/magnifier.js",
                                "~/Resources/Web/js/imgshow.js",
                                "~/Resources/Web/js/pageGroup.js",
                                "~/Resources/Web/Scripts/alertAndverify.js"
                               ));
            //商品详情的Js
            bundles.Add(new ScriptBundle("~/js/good/details").Include(
                "~/Modules/BntWeb.Mall/Content/Scripts/web.gooddetails.js"));
           //购物车 列表 js
            bundles.Add(new ScriptBundle("~/js/cartList").Include(
                "~/Modules/BntWeb.Mall/Content/Scripts/web.cartslist.js"));
            //兑换css  
            bundles.Add(new StyleBundle("~/css/web/exchange").Include(
                     "~/Resources/Web/Css/personal.css",
                     "~/Resources/Css/order.css"));
            //兑换商品js
            bundles.Add(new ScriptBundle("~/js/exchangeGood").Include(
               "~/Modules/BntWeb.Mall/Content/Scripts/web.exchange.js"));
           
            //购物车 列表 js
            bundles.Add(new ScriptBundle("~/js/exchange").Include(
                "~/Modules/BntWeb.Mall/Content/Scripts/web.exchorder.js"));

            //确认订单 js
            bundles.Add(new ScriptBundle("~/js/confirmorder").Include(
               "~/Modules/BntWeb.Mall/Content/Scripts/web.confirm.order.js"));


            //timepick
            bundles.Add(new ScriptBundle("~/js/date").Include(
                                "~/Resources/Web/js/jquery.datetimepicker.full.js"
                               ));
            bundles.Add(new StyleBundle("~/css/admin/dd").Include(
                      "~/Resources/Css/jquery.datetimepicker.css "
                   ));
            //浏览历史
            bundles.Add(new StyleBundle("~/css/browsing").Include(
                    "~/Resources/Web/Css/public.css ", 
                    "~/Resources/Web/Css/personal.css ", 
                    "~/Resources/Css/order.css"
                 ));
            #endregion
        }
    }
}