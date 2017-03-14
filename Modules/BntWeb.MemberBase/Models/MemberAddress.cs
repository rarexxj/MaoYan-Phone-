using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BntWeb.Data;

namespace BntWeb.MemberBase.Models
{
    [Table(KeyGenerator.TablePrefix + "Member_Addresses")]
    public class MemberAddress
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(128)]
        public string MemberId { set; get; }
        /// <summary>
        /// 省级Id
        /// </summary>
        [MaxLength(20)]
        public string Province { set; get; }
        /// <summary>
        /// 市级Id
        /// </summary>
        [MaxLength(20)]
        public string City { set; get; }
        /// <summary>
        /// 区县级Id
        /// </summary>
        [MaxLength(20)]
        public string District { set; get; }
        /// <summary>
        /// 街道/乡镇Id
        /// </summary>
        [MaxLength(20)]
        public string Street { set; get; }
        [MaxLength(200)]
        public string Address { set; get; }
        public bool IsDefault { set; get; }
        /// <summary>
        /// 联系人
        /// </summary>
        [MaxLength(50)]
        public string Contacts { set; get; }
        [MaxLength(20)]
        public string Phone { set; get; }
        /// <summary>
        /// 邮编
        /// </summary>
        [MaxLength(6)]
        public string Postcode { set; get; }
        /// <summary>
        /// 三级省市区联动的Id
        /// </summary>
        public string DistinctId { get; set; }
        
        /// <summary>
        /// 地区名字，每个级别之间用逗号隔开
        /// </summary>
        [MaxLength(100)]
        public string RegionName { get; set; }
    }
}
