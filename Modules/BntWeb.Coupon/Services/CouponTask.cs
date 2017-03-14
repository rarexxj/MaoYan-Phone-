using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.Logging;
using BntWeb.Task;

namespace BntWeb.Coupon.Services
{
    public class CouponTask: IBackgroundTask
    {
        private readonly ICouponService _couponService;

        public CouponTask(ICouponService couponService)
        {
            _couponService = couponService;

            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        public void Sweep()
        {
            //修改超过时间未使用的优惠券状态为已过期
            var count=_couponService.ProcessTimeOutCoupon();
            if (count > 0)
                Logger.Warning("取消超时未使用优惠券{0}条", count);
        }
    }
}