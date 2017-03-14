using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.Security.Permissions;

namespace BntWeb.Coupon
{
    public class Permissions: IPermissionProvider
    {
        private static readonly string CategoryKey = CouponModule.DisplayName;

        public const string ManageCouponKey = "BntWeb-Coupon-ManageCoupon";
        public static readonly Permission ManageCoupon = new Permission { Description = "优惠券管理", Name = ManageCouponKey, Category = CategoryKey };

        public int Position => CouponModule.Position;

        public string Category => CategoryKey;

        public IEnumerable<Permission> GetPermissions()
        {
            return new[]
            {
                ManageCoupon
            };
        }
    }
}