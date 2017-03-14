/* 
    ======================================================================== 
        File name：        BundleProvider
        Module:                
        Author：            罗嗣宝
        Create Time：    2016/5/25 16:45:54
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/

using System.Web.Optimization;
using BntWeb.UI.Bundle;

namespace BntWeb.MemberCenter
{
    public class BundleProvider : IBundleProvider
    {
        public void RegisterBundles(BundleCollection bundles)
        {
            //Js
            bundles.Add(new ScriptBundle("~/js/admin/members/list").Include(
                      "~/Modules/BntWeb.MemberCenter/Content/Scripts/member.list.js"));
            #region Web
            bundles.Add(new ScriptBundle("~/js/web/login").Include(
                   "~/Modules/BntWeb.MemberCenter/Content/Scripts/web.login.js"));
            bundles.Add(new ScriptBundle("~/js/web/register").Include(
                     "~/Modules/BntWeb.MemberCenter/Content/Scripts/web.register.js"));
            bundles.Add(new ScriptBundle("~/js/web/forget").Include(
                     "~/Modules/BntWeb.MemberCenter/Content/Scripts/web.forget.js"));
            bundles.Add(new ScriptBundle("~/js/web/phonechange").Include(
                   "~/Modules/BntWeb.MemberCenter/Content/Scripts/web.changephone.js"));
            bundles.Add(new ScriptBundle("~/js/web/addressList").Include(
                "~/Modules/BntWeb.MemberCenter/Content/Scripts/web.address.js"));

            bundles.Add(new StyleBundle("~/css/web/reg").Include(
                   "~/Resources/Css/reg.css"));

            bundles.Add(new StyleBundle("~/css/web/forget").Include(
                 "~/Resources/Css/forget.css"));

            bundles.Add(new StyleBundle("~/css/web/personal").Include(
                 "~/Resources/Web/Css/personal.css"));

            bundles.Add(new StyleBundle("~/css/web/address").Include(
               "~/Resources/Css/consignee.css"));
         
            bundles.Add(new ScriptBundle("~/js/web/wdatePicker").Include(
                "~/Resources/Web/js/My97DatePicker/WdatePicker.js"));

            bundles.Add(new ScriptBundle("~/js/web/uploadify").Include(
               "~/Resources/Web/js/update/jquery.uploadify.js"));
        
            bundles.Add(new ScriptBundle("~/js/web/personal").Include(
                    "~/Modules/BntWeb.MemberCenter/Content/Scripts/web.personal.js"));

            bundles.Add(new ScriptBundle("~/js/web/forget").Include(
                  "~/Modules/BntWeb.MemberCenter/Content/Scripts/web.forget.js"));

        
            #endregion
        }
    }
}