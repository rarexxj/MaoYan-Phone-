using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BntWeb.Data;
namespace BntWeb.Coupon.Models
{
    [Table(KeyGenerator.TablePrefix + "View_Coupons")]
    public class CouponView
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


    }
    
}