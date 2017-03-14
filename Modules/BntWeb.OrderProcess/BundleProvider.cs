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

namespace BntWeb.OrderProcess
{
    public class BundleProvider : IBundleProvider
    {
        public void RegisterBundles(BundleCollection bundles)
        {
            //Js
            bundles.Add(new ScriptBundle("~/js/admin/order/list").Include(
                      "~/Modules/BntWeb.OrderProcess/Content/Scripts/order.list.js"));
            bundles.Add(new ScriptBundle("~/js/admin/order/detail").Include(
                      "~/Modules/BntWeb.OrderProcess/Content/Scripts/order.detail.js"));
            bundles.Add(new ScriptBundle("~/js/admin/order/refund/list").Include(
                      "~/Modules/BntWeb.OrderProcess/Content/Scripts/orderrefund.list.js"));
            bundles.Add(new ScriptBundle("~/js/admin/order/refund/detail").Include(
                      "~/Modules/BntWeb.OrderProcess/Content/Scripts/orderrefund.detail.js"));
            bundles.Add(new ScriptBundle("~/js/admin/order/evaluate/detail").Include(
                     "~/Modules/BntWeb.OrderProcess/Content/Scripts/orderevaluate.detail.js"));
            bundles.Add(new ScriptBundle("~/js/admin/order/reminder/list").Include(
                     "~/Modules/BntWeb.OrderProcess/Content/Scripts/orderreminder.list.js"));


            //Web Js
            //退款
            bundles.Add(new ScriptBundle("~/js/refund/refundall").Include(
                "~/Modules/BntWeb.OrderProcess/Content/Scripts/web.refund.js"));

            //付款方式公共的 js 
            bundles.Add(new ScriptBundle("~/js/pay/payType").Include(
              "~/Resources/Web/js/pageGroup.js", "~/Resources/Web/Scripts/alertAndverify.js"));
            //付款方式js
            bundles.Add(new ScriptBundle("~/js/webPayType").Include
                ("~/Modules/BntWeb.OrderProcess/Content/Scripts/web.paytype.js"));
            //上传凭证
            bundles.Add(new ScriptBundle("~/js/web/uploadify").Include(
                  "~/Resources/Web/js/update/jquery.uploadify.js"));
        
            //Web Css 退款类型
            bundles.Add(new StyleBundle("~/css/refund/refundtype").Include(
            "~/Resources/Css/order.css", "~/Resources/Web/Css/personal.css"));
            //退款 订单详情
            bundles.Add(new StyleBundle("~/css/refund/allrefund").Include(
                "~/Resources/Web/Css/personal.css",
                 "~/Resources/Css/order_info.css",
                "~/Resources/Css/order.css"
               ));
            //订单列表 css
            //我的积分 css
            bundles.Add(new StyleBundle("~/css/web/orderlist").Include(
                 "~/Resources/Css/order.css",
                 "~/Resources/Web/Css/personal.css"
              ));
            //订单列表 js
            bundles.Add(new ScriptBundle("~/js/orderlist").Include
             ("~/Modules/BntWeb.OrderProcess/Content/Scripts/web.orderlist.js"));
            //订单评价页
            bundles.Add(new ScriptBundle("~/js/web/evaluateWeb").Include(
                "~/Modules/BntWeb.OrderProcess/Content/Scripts/web.evaluate.js"));
            //头部CSs 订单评价

            //上传评价图片 js
            bundles.Add(new ScriptBundle("~/js/web/newuploadify").Include(
                "~/Resources/Web/js/myupload/plupload.full.min.js"));


        }
    }
}