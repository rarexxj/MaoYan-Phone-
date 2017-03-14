using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using BntWeb.Data;

namespace BntWeb.Coupon.Models
{
    [Table(KeyGenerator.TablePrefix + "Coupon_Use_Records")]
    public class CouponUseRecord
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// 优惠券关联Id
        /// </summary>
        public Guid CouponRelationId { get; set; }

        /// <summary>
        /// 会员id
        /// </summary>
        public string MemberId { get; set; }

        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否使用成功
        /// </summary>
        public bool IsUsed { get; set; }

        /// <summary>
        /// 关联主信息Id
        /// </summary>
        public Guid SourceId { get; set; }

        /// <summary>
        /// 关联模块Key
        /// </summary>
        [MaxLength(50)]
        public string ModuleKey { get; set; }

        /// <summary>
        /// 关联数据类型，可空
        /// </summary>
        [MaxLength(50)]
        public string SourceType { get; set; }
    }


}