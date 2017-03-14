using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.Environment;

namespace BntWeb.Coupon
{
    public class CouponModule:IBntWebModule
    {
        public static readonly CouponModule Instance = new CouponModule();

        public string InnerKey => "BntWeb-Coupon";
        public static string Key => Instance.InnerKey;
        public string InnerDisplayName => "优惠券管理";
        public static string DisplayName => Instance.InnerDisplayName;
        public string InnerArea => "Coupon";
        public static string Area => Instance.InnerArea;
        public int InnerPosition => 9100;
        public static int Position => Instance.InnerPosition;
    }
}