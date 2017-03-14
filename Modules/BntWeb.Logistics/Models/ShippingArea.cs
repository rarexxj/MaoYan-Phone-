using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using BntWeb.Data;

namespace BntWeb.Logistics.Models
{
    [Table(KeyGenerator.TablePrefix + "Shippings_Areas")]
    public class ShippingArea
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 配送区域的名字
        /// </summary>
        [MaxLength(120)]
        public string Name { get; set; }

        /// <summary>
        /// 区域名字 多个,隔开
        /// </summary>
        [MaxLength(200)]
        public string AreaNames { get; set; }

        /// <summary>
        /// 费用
        /// </summary>
        public decimal Freight { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 物流状态
        /// </summary>
        public ShippingAreaStatus Status { get; set; }

        /// <summary>
        /// 排序，从大到小
        /// </summary>
        public int Sort { get; set; }
    }

    public enum ShippingAreaStatus
    {
        /// <summary>
        /// 已删除
        /// </summary>
        [Description("已删除")]
        Delete = -1,

        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 1

    }
}