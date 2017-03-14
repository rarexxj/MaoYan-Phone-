using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BntWeb.Data;
namespace BntWeb.Coupon.Models
{
    [Table(KeyGenerator.TablePrefix + "Coupons")]
    public class Coupon
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// 标识
        /// </summary>
        public string Code { get; set; }
     
        /// <summary>
        /// 金额
        /// </summary>
        public int Money { get; set; }
        /// <summary>
        /// 最低消费 默认0  现金券无限期限制
        /// </summary>
        public int Minimum { get; set; }=0;
        /// <summary>
        /// 期限  /月  默认0  现金券无限期限制
        /// </summary>
        public int Term { get; set; } = 0;
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }
        /// <summary>
        /// 优惠券类型
        /// </summary>
        public CouponType CouponType { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
/// <summary>
/// 创建时间
/// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        [NotMapped]
        public string ValidTime { get; set; }
    }

    /// <summary>
    /// 优惠券类型
    /// </summary>
    public enum CouponType
    {
        /// <summary>
        /// 满减
        /// </summary>
        [Description("满减")]
        FullCut =0,
        /// <summary>
        /// 立减
        /// </summary>
        [Description("立减")]
        Minus = 1
    }
}