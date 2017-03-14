using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using BntWeb.Coupon.ApiModels;
using BntWeb.Coupon.Models;
using BntWeb.Coupon.Services;
using BntWeb.Data.Services;
using BntWeb.FileSystems.Media;
using BntWeb.Validation;
using BntWeb.WebApi.Filters;
using BntWeb.WebApi.Models;

namespace BntWeb.Coupon.ApiControllers
{
    public class CouponController : BaseApiController
    {
        private readonly ICouponService _couponService;
        private readonly ICurrencyService _currencyService;

        public CouponController(ICouponService couponService, ICurrencyService currencyService)
        {
            _couponService = couponService;
            _currencyService = currencyService;
        }

        /// <summary>
        /// 获取会员优惠券列表 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [BasicAuthentication]
        public ApiResult GetCoupons(bool isAvailable = true, int pageNo = 1, int limit = 10)
        {
            int totalCount = 0;

            var coupons = _couponService.GetMemberCoupons(AuthorizedUser.Id, isAvailable, pageNo, limit, out totalCount);
            var result = new ApiResult();
            var data = new
            {
                TotalCount = totalCount,
                Coupons = coupons.Select(me => new CouponListModel(me)).ToList(),
            };

            result.SetData(data);

            return result;
        }

        /// <summary>
        /// 优惠券列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResult CouponList()
        {
            var result = new ApiResult();
         
            var list = _currencyService.GetList<Models.Coupon>(a=>a.Enabled).OrderByDescending(a=>a.CreateTime);
            result.SetData(list);
            return result;
        }
        /// <summary>
        /// 获取会员可提现现金券列表 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [BasicAuthentication]
        public ApiResult GetWithdrawalCoupons()
        {
            var coupons = _couponService.GetMemberWithdrawalCoupons(AuthorizedUser.Id);
            var result = new ApiResult();
            var data = new
            {
                Coupons = coupons.Select(me => new WithdrawalCouponListModel(me)).ToList()
            };

            result.SetData(data);

            return result;
        }
        /// <summary>
        /// 领取优惠券
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        public ApiResult ReceiveCoupon(string memberId, [FromBody]ReceiveCouponModel model)
        {
            Argument.ThrowIfNullOrEmpty(model.Code, "优惠券标识");
            Argument.ThrowIfNullOrEmpty(model.Type.ToString(), "优惠券类型");
            var result = new ApiResult();
            var co = _couponService.AddMemberCoupon(memberId, model.Code,model.Type);
            if(!co)
                throw new WebApiInnerException("0001", "领取优惠券失败！");
            return result;
        }


    }
}
