using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.Coupon.Models;

namespace BntWeb.Coupon.ViewModels
{
    public class MemberCouponSimpleViewModel
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
       
        public MemberCouponSimpleViewModel(Models.CouponRelation model)
        {
            Id = model.Id;
            Money = model.Coupon.Money;
            Title = model.Coupon.Title;
            Describe = model.Coupon.Describe;
        }
    }
    //添加优惠券
    public class AddCoupponModel
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 标识
        /// </summary>
        public string Code { get; set; }
        public CouponType CouponType { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public int Money { get; set; }
    }

    public class MyCoupsModel
    {
      /// <summary>
      /// 优惠券Id
      /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public int Money { get; set; }
        /// <summary>
        /// 最低消费 默认0  现金券无限期限制
        /// </summary>
        public int Minimum { get; set; } = 0;
        /// <summary>
        /// 期限  /月  默认0  现金券无限期限制
        /// </summary>
        public int Term { get; set; } = 0;
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        public CouponType CouponType { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 结束时间  现金券无期限限制
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 有效时间
        /// </summary>
        public string ValidTime { get; set; }
    }

}