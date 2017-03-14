/* 
    ======================================================================== 
        File name：        AdminMenu
        Module:                
        Author：            罗嗣宝
        Create Time：    2016/6/16 11:52:13
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.UI.Navigation;

namespace BntWeb.Coupon
{
    public class AdminMenu : INavigationProvider
    {
        
        //public Localizer T { get; set; }
        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(CouponModule.Key, CouponModule.DisplayName, CouponModule.Position.ToString(), BuildMenu, new List<string> { "icon-shopping-cart" });
        }

        private void BuildMenu(NavigationItemBuilder menu)
        {
            menu.Add(CouponModule.Key + "-CouponList", "优惠券", "10",
                item => item
                    .Action("List", "Coupon", new { area = CouponModule.Area })
                );
          
        }
    }
}