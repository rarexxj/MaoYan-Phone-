using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using BntWeb.Coupon.Models;
using BntWeb.Coupon.Services;
using BntWeb.Data;
using BntWeb.Data.Services;
using BntWeb.Mvc;
using BntWeb.Security;
using BntWeb.Utility.Extensions;
using BntWeb.Web.Extensions;

namespace BntWeb.Coupon.Controllers
{
    public class CouponController : Controller
    {
  
        private readonly ICurrencyService _currencyService;
        private readonly ICouponService _couponService;

        public CouponController(ICurrencyService currencyService, ICouponService couponService)
        {
            _currencyService = currencyService;
            _couponService = couponService;
        }

        // GET: Coupon
        /// <summary>
        /// 优惠卷列表
        /// </summary>
        /// <returns></returns>
        [AdminAuthorize]
        public ActionResult List()
        {
            return View();
        }
        [AdminAuthorize]
        public ActionResult ListOnPage()
        {
            var result = new DataTableJsonResult();

        //取参数值
        int draw, pageIndex, pageSize, totalCount;
        string sortColumn;
        bool isDesc;
        Request.GetDatatableParameters(out draw, out pageIndex, out pageSize, out sortColumn, out isDesc);
            result.draw = draw;

            //取查询条件
            
            var title = Request.Get("extra_search[Title]");
            var checkTitle = string.IsNullOrWhiteSpace(title);

            var money = Request.Get("extra_search[Money]");
            var checkMoney = string.IsNullOrWhiteSpace(money);

            var minimum = Request.Get("extra_search[Minimum]");
            var checkMinimum = string.IsNullOrWhiteSpace(minimum);

            var couponType = Request.Get("extra_search[CouponType]");
            var checkCouponType = string.IsNullOrWhiteSpace(couponType);
            var couponTypeInt = couponType.To<int>();

            Expression<Func<Models.Coupon, bool>> expression =
                l => (checkTitle || l.Title.ToString().Contains(title)) &&
                     (checkMoney || l.Money.ToString().Contains(money)) &&
                     (checkCouponType || (int)l.CouponType==couponTypeInt) &&
                     (checkMinimum || (l.Minimum.ToString().Contains(minimum)));
            

        Expression<Func<Models.Coupon, object>> orderByExpression;
            //设置排序
            switch (sortColumn)
            {
                case "Code":
                    orderByExpression = act => new { act.Code
                    };
                    break;
                case "Money":
                    orderByExpression = act => new { act.Money
                    };
                    break;
                case "Minimum":
                    orderByExpression = act => new { act.Minimum };
                    break;
                case "Term":
                    orderByExpression = act => new { act.Term };
                    break;
                case "Describe":
                    orderByExpression = act => new { act.Describe };
                    break;
                case "Enabled":
                    orderByExpression = act => new { act.Enabled };
                    break;
                default:
                    orderByExpression = act => new { act.Title };
                    break;
            }

            //分页查询
            var list = _currencyService.GetListPaged(pageIndex, pageSize, expression, out totalCount, new OrderModelField { PropertyName = sortColumn, IsDesc = isDesc });

            result.data = list;
            result.recordsTotal = totalCount;
            result.recordsFiltered = totalCount;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 编辑优惠券
        /// </summary>
        /// <param name="isView"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [AdminAuthorize]
        public ActionResult Edit(bool isView = false, Guid? id = null)
        {
            Models.Coupon coupon = null;
            bool editMode = !isView;
            ViewBag.EditMode = editMode;
            ViewBag.IsView = isView;

            if (id != null && id != Guid.Empty)
                coupon = _currencyService.GetSingleById<Models.Coupon>(id);
            if (coupon == null)
            {
                coupon = new Models.Coupon
                {
                    Id = KeyGenerator.GetGuidKey()
                };
            }
            return View(coupon);
        }
        [AdminAuthorize]
        public ActionResult EditOnPost(Models.Coupon coupon)
        {
            var result = new DataJsonResult();
            Models.Coupon oldCoupon = null;
            if (coupon.Id != Guid.Empty)
                oldCoupon = _currencyService.GetSingleById<Models.Coupon>(coupon.Id);

        

            if (oldCoupon == null)
            {
                var couponModel = new Models.Coupon
                {
                    Id = coupon.Id,
                    Code= KeyGenerator.GenerateRandom(1000, 1).ToString(),
                    Title = coupon.Title,
                    Money = coupon.Money,
                    Minimum = coupon.Minimum,
                    Term = coupon.Term,
                    Describe = coupon.Describe,
                    CouponType = coupon.CouponType,
                    Enabled =true,
                    CreateTime = DateTime.Now


                };
                var ressultAdd = _couponService.CreateCoupon(couponModel);

                if (!ressultAdd)
                {
                    throw new BntWebCoreException("新建失败");

                }

            }
            else
            {

                oldCoupon.Title = coupon.Title;
                oldCoupon.Code = coupon.Code;
                oldCoupon.Money = coupon.Money;
                oldCoupon.Minimum = coupon.Minimum;
                oldCoupon.Term = coupon.Term;
                oldCoupon.Describe = coupon.Describe;
                oldCoupon.CouponType = coupon.CouponType;
                oldCoupon.CreateTime = DateTime.Now;
                var phoneBookUpdate = _couponService.UpdateCoupon(oldCoupon);
                if (!phoneBookUpdate)
                {
                    throw new Exception("更新失败！");
                }

            }
            return Json(result);
        }
        /// <summary>
        ///  删除优惠券
        /// </summary>
        /// <param name="couponId"></param>
        /// <returns></returns>
        [AdminAuthorize]
        public ActionResult Delete(Guid couponId)
        {
            var result = new DataJsonResult();
            var coupon =_currencyService.GetSingleById<Models.Coupon>(couponId);
            if (!_couponService.Delete(coupon))
            {
                result.ErrorMessage = "未知异常，删除失败";
            }

            return Json(result);
        }
        [AdminAuthorize]
        public ActionResult Switch(Guid couponId,string enabled)
        {
            var result = new DataJsonResult();
            var coupon = _couponService.Switch(couponId,enabled);
            if (!coupon)
            {
                result.ErrorMessage = "未知异常，修改优惠券状态失败";
            }

            return Json(result);
        }
    }
}