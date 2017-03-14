using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using BntWeb.Data;

namespace BntWeb.Coupon.Models
{
    [Table(KeyGenerator.TablePrefix + "Coupon_Relations")]
    public class CouponRelation
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// 优惠券Id
        /// </summary>
        public Guid CouponId { get; set; }
        /// <summary>
        /// 优惠码
        /// </summary>
        public string CodeNo { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 结束时间  现金券无期限限制
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 会员id
        /// </summary>
        public string MemberId { get; set; }

        public CouponStatus Status { get; set; }

        [ForeignKey("CouponId")]
        public virtual Coupon Coupon { get; set; }
    }

    /// <summary>
    /// 优惠券状态
    /// </summary>
    public enum CouponStatus
    {
        /// <summary>
        /// 未使用
        /// </summary>
        [Description("可使用")]
        Unused = 0,
        /// <summary>
        /// 已冻结
        /// </summary>
        [Description("已使用")]
        Used = 1,
        /// <summary>
        /// 已使用
        /// </summary>
        [Description("已失效")]
        Expired = 2,
     
    }
}