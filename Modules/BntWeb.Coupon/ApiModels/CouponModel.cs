using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.Coupon.Models;

namespace BntWeb.Coupon.ApiModels
{
    public class CouponListModel
    {
        /// <summary>
        /// 金额
        /// </summary>
        public int Money { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public string ValidTime { get; set; }
        /// <summary>
        /// 优惠码
        /// </summary>
        public string CodeNo { get; set; }

        public CouponListModel(Models.CouponRelation model)
        {
            Money = model.Coupon.Money;
            Title = model.Coupon.Title;
            Describe = model.Coupon.Describe;
            CodeNo = model.CodeNo;
            if (model.Coupon.CouponType == Models.CouponType.Minus)
            {
                ValidTime = "长期";
            }
            else
            {
                var beginTime = model.BeginTime.ToString("yyyy-MM-dd");
                var endTime = Convert.ToDateTime(model.EndTime).ToString("yyyy-MM-dd");
                ValidTime = $"{beginTime} 至 {endTime}";
            }
        }
    }

    public class WithdrawalCouponListModel
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public int Money { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }

        public WithdrawalCouponListModel(Models.CouponRelation model)
        {
            Id = model.Id;
            Money = model.Coupon.Money;
            Title = model.Coupon.Title;
            Describe = model.Coupon.Describe;

        }
    }
    public class ReceiveCouponModel
    {
    
        /// <summary>
        /// 金额
        /// </summary>
        public int Money { get; set; }
        /// <summary>
        /// 标识
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 优惠券类型
        /// </summary>
        public CouponType Type { get; set; }

     
    }
}