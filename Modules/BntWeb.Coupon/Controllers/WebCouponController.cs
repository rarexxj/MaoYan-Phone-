using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;
using BntWeb.Coupon.Models;
using BntWeb.Coupon.Services;
using BntWeb.Coupon.ViewModels;
using BntWeb.Data.Services;
using BntWeb.Environment;
using BntWeb.MemberBase.Services;
using BntWeb.Mvc;
using BntWeb.Security;


namespace BntWeb.Coupon.Controllers
{
    public class WebCouponController : Controller
    {
  
        private readonly ICurrencyService _currencyService;
        private readonly ICouponService _couponService;
        private readonly UrlHelper _urlHelper;
        private readonly IMemberContainer _memberContainer;
        public WebCouponController(IMemberContainer memberContainer, UrlHelper urlHelper, 
            ICurrencyService currencyService, ICouponService couponService)
        {
            _currencyService = currencyService;
            _couponService = couponService;
            _urlHelper = urlHelper;
            _memberContainer = memberContainer;
        }

        // GET: Coupon
        /// <summary>
        ///  Web优惠卷列表
        /// </summary>
        /// <param name="pageNo">起始页</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        [MemberAuthorize]
        public ActionResult WebList(int pageNo=1,int pageSize=9)
        {
            int totalCount;
            Expression<Func<Models.Coupon, bool>> expr = x => x.Enabled==true;
            ViewBag.WebCouponList = _currencyService.GetListPaged<Models.Coupon>(pageNo, pageSize, expr,out totalCount,
                new OrderModelField {PropertyName = "CreateTime", IsDesc = true});
            var routeParas = new RouteValueDictionary{
                    { "area", "Coupon"},
                    { "controller", "WebCoupon"},
                    { "action", "WebList"}
                };
            var returnUrl = HostConstObject.HostUrl + _urlHelper.RouteUrl(routeParas);

            ViewBag.Url = returnUrl+ "?pageNo=[pageNo]";
            //获得总页数
            ViewBag.TotalPage = totalCount % pageSize == 0 ? totalCount / pageSize : totalCount / pageSize + 1;
            ViewBag.CurrentPage = pageNo;
            return View();
        }
        /// <summary>
        /// 领取优惠券
        /// </summary>
        /// <returns></returns>
        [MemberAuthorize]
        public ActionResult GetCoupon(AddCoupponModel addmodel)
        {
            var result = new DataJsonResult();
            //获得当前用户
            var currenuser = _memberContainer.CurrentMember;
            //获得已经领取的优惠券
            var count =
                _currencyService.GetSingleByConditon<CouponRelation>(
                    a => a.CouponId == addmodel.Id && a.MemberId == currenuser.Id && a.Status == CouponStatus.Unused);
            if (count != null)
            {
                result.ErrorMessage = "此优惠券已经领了！";
            }
            else
            {  //领取优惠券

                _couponService.AddMemberCoupon(currenuser.Id, addmodel.Code, addmodel.CouponType);
                result.Data = _currencyService.GetSingleById<Models.Coupon>(addmodel.Id);
            }
           
            return Json(result);

        }
        /// <summary>
        /// 分页我的优惠券(所有状态)
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [MemberAuthorize]
        public ActionResult MyCoupon(int pageNo = 1, int pageSize = 9,CouponStatus status= CouponStatus.Unused)
        {
            //当前用户
            var member = _memberContainer.CurrentMember;
            //我的优惠券列表
                int totalCount;
                var coupon = _couponService.GetCoupons(member.Id, status,out totalCount, pageNo, pageSize);
            if (coupon.Count != 0)
            {
                ViewBag.Myoupon = coupon;
                var routeParas = new RouteValueDictionary{
                    { "area", "Coupon"},
                    { "controller", "WebCoupon"},
                    { "action", "MyCoupon"}
                };
                var returnUrl = HostConstObject.HostUrl + _urlHelper.RouteUrl(routeParas);

                ViewBag.Url = returnUrl + "?pageNo=[pageNo]";
                //获得总页数
                ViewBag.TotalPage = totalCount % pageSize == 0 ? totalCount / pageSize : totalCount / pageSize + 1;
                ViewBag.CurrentPage = pageNo;
            }
            return View();
        }
    }
}