using System;

using BntWeb.MemberBase.Models;

namespace BntWeb.MemberCenter.ViewModels
{
    public class WebAddMemberAddressModels
    {

        public string Address { set; get; }
        
        public string Contacts { set; get; }
        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault { set; get; }
        public string Phone { set; get; }
        /// <summary>
        /// 省级Id
        /// </summary>
        public string Province { set; get; }
        /// <summary>
        /// 市级Id
        /// </summary>
        public string City { set; get; }
        /// <summary>
        /// 区县级Id
        /// </summary>
        public string District { set; get; }
        /// <summary>
        /// 街道/乡镇Id
        /// </summary>
        public string Street { set; get; }
        /// <summary>
        /// 地区名字，每个级别之间用逗号隔开
        /// </summary>
        public string RegionName { get; set; }
        /// <summary>
        /// 邮政编码
        /// </summary>
        public string Postcode { set; get; }
    }

    public class WebEditMemberAddressModels
    {
        public Guid AddressId { set; get; }
        public string Address { set; get; }

        public string Contacts { set; get; }

        public string Phone { set; get; }
        /// <summary>
        /// 省级Id
        /// </summary>
        public string Province { set; get; }
        /// <summary>
        /// 市级Id
        /// </summary>
        public string City { set; get; }
        /// <summary>
        /// 区县级Id
        /// </summary>
        public string District { set; get; } = "";

        /// <summary>
        /// 街道/乡镇Id
        /// </summary>
        public string Street { set; get; } = "";
        /// <summary>
        /// 地区名字，每个级别之间用逗号隔开
        /// </summary>
        public string RegionName { get; set; }
        /// <summary>
        /// 邮政编码
        /// </summary>
        public string Postcode { set; get; }

        public bool IsDefault { get; set; }
    }

    public class WebListAddressModel
    {
        public Guid Id { get; set; }
        public string Address { set; get; }
        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault { set; get; }
        public string Contacts { set; get; }
        public string Phone { set; get; }
        /// <summary>
        /// 地区名字
        /// </summary>
        public string RegionName { get; set; }
        /// <summary>
        /// 邮政编码
        /// </summary>
        public string Postcode { set; get; }

        /// <summary>
        /// 省级Id
        /// </summary>
        public string Province { set; get; }
        /// <summary>
        /// 市级Id
        /// </summary>
        public string City { set; get; }

        /// <summary>
        /// 区县编号
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// 街道/乡镇编号
        /// </summary>
        public string Street { get; set; }
       
      
        
        public WebListAddressModel(MemberAddress model)
        {
            Id = model.Id;
            Address = model.Address;
            IsDefault = model.IsDefault;
            Contacts = model.Contacts;
            Phone = model.Phone;
            RegionName = model.RegionName.Replace(",", "");
            Postcode = model.Postcode;
            Province = model.Province;
            City = model.City;
            District = model.District;
            Street = model.Street;
        }
    }
}
