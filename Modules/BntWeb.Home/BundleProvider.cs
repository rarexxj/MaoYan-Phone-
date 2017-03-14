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

namespace BntWeb.Home
{
    public class BundleProvider : IBundleProvider
    {
        public void RegisterBundles(BundleCollection bundles)
        {
            //css
            bundles.Add(new StyleBundle("~/css/public").Include(
                      "~/Resources/Web/Css/public.css"));
            bundles.Add(new StyleBundle("~/css/main").Include("~/Resources/Css/main.css"));

            //Js
            bundles.Add(new ScriptBundle("~/js/homepage").Include(
                      "~/Resources/Web/Scripts/jquery-1.9.0.min.js",
                      "~/Resources/Web/Scripts/alertAndverify.js",
                      "~/Resources/Web/Scripts/jquery.SuperSlide.2.1.1.js",
                      "~/Resources/Common/Vendors/layer/layer.js",
                      "~/Resources/Web/Scripts/main.js"));

            //js 分页
            bundles.Add(new ScriptBundle("~/js/page").Include(
                     "~/Resources/Web/Scripts/pageGroup.js"));

        }
    }
}