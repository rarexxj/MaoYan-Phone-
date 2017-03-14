/* 
    ======================================================================== 
        File name：        SystemUser
        Module:                
        Author：            罗嗣宝
        Create Time：    2016/5/11 16:31:21
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BntWeb.Data;
using BntWeb.FileSystems.Media;
using BntWeb.Security.Identity;

namespace BntWeb.MemberBase.Models
{
    [Table(KeyGenerator.TablePrefix + "Members")]
    public class MemberExtension
    {
        [Key]
        [MaxLength(128)]
        public string Id { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [MaxLength(50)]
        public string NickName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public SexType Sex { get; set; }
        /// <summary>
        /// 会员手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        [NotMapped]
        public StorageFile Avatar { get; set; }

        /// <summary>
        /// 会员类型
        /// </summary>
        public MemberType MemberType { get; set; }

        /// <summary>
        /// 省级Id
        /// </summary>
        [MaxLength(10)]
        public string Province { get; set; }

        /// <summary>
        /// 市级Id
        /// </summary>
        [MaxLength(10)]
        public string City { get; set; }

        /// <summary>
        /// 区县级Id
        /// </summary>
        [MaxLength(10)]
        public string District { get; set; }

        /// <summary>
        /// 街道/乡镇Id
        /// </summary>
        [MaxLength(10)]
        public string Street { get; set; }

        /// <summary>
        /// 地区名字，每个级别之间用逗号隔开
        /// </summary>
        [MaxLength(100)]
        public string RegionName { get; set; }

        /// <summary>
        /// 详细门牌地址
        /// </summary>
        [MaxLength(200)]
        public string Address { get; set; }

        [MaxLength(6000)]
        public string Config { get; set; }

        /// <summary>
        /// 渠道Id
        /// </summary>
        public Guid? ChannelId { get; set; }

        /// <summary>
        /// 引荐人Id
        /// </summary>
        public string ReferrerId { get; set; }

        /// <summary>
        /// 所有上级id集合，“,”隔开
        /// </summary>
        public string ParentIds { get; set; }

        /// <summary>
        /// 邀请码
        /// </summary>
        public string InvitationCode { get; set; }
        /// <summary>
        /// qq号
        /// </summary>
        public string Qq { get; set; }
        /// <summary>
        /// 微信
        /// </summary>
        public string Weixin { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 生日类型
        /// </summary>
        public BirthType BirthType { get; set; }
        /// <summary>
        /// 口味偏好
        /// </summary>
        public TastesType TastesType { get; set; }
        /// <summary>
        /// 会员卡号
        /// </summary>
        public string  MemberCard { get; set; }
   
      
        //下面继续添加其他扩展字段，同时要补充到Member实体中
    }
   
    /// <summary>
    /// 会员性别
    /// </summary>
    public enum SexType
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        UnKonw = 0,
        /// <summary>
        /// 未知
        /// </summary>
        [Description("男")]
        Male = 1,
        /// <summary>
        /// 未知
        /// </summary>
        [Description("女")]
        Female = 2
    }
    public class CustomBaseNumber
    {
        private string _chars;

        public CustomBaseNumber(string chars)
        {
            _chars = chars;
        }
        /// <summary>
        /// 会员卡号参数
        /// </summary>
        [NotMapped]
        public int DecBase { get; set; }
        /// <summary>
        /// 会员卡号参数
        /// </summary>
        [NotMapped]
        public string CustomBase
        {
            get
            {
                string value = "";
                int decvalue = DecBase;
                int n = 0;
                if (decvalue == 0) return new string(new char[] { _chars[0] });
                while (decvalue > 0)
                {
                    n = decvalue % _chars.Length;
                    value = _chars[n] + value;
                    decvalue = decvalue / _chars.Length;
                }
                return value;
            }
            set
            {
                int n = 0;
                Func<char, int> getnum = (x) => { for (int i = 0; i < _chars.Length; i++) if (x == _chars[i]) return i; return 0; };
                for (int i = 0; i < value.Length; i++)
                {
                    n += Convert.ToInt32(Math.Pow((double)_chars.Length, (double)(value.Length - i - 1)) * getnum(value[i]));
                }
                DecBase = n;
            }
        }
    }
    /// <summary>
    /// 会员类型
    /// </summary>
    public enum MemberType
    {
        [Description("普通会员")]
        General = 0,

        [Description("合伙人")]
        Partner = 1
    }
    /// <summary>
    /// 生日类型
    /// </summary>
    public enum BirthType
    {/// <summary>
     /// 公历
     /// </summary>
        [Description("公历")]
        Calendar = 0,
        /// <summary>
        /// 农历
        /// </summary>
        [Description("农历")]
        Lunar = 1
    }
    /// <summary>
    /// 口味偏好
    /// </summary>
    public enum TastesType
    {
        /// <summary>
        /// 巧克力蛋糕
        /// </summary>
        [Description("巧克力蛋糕")]
       Chocolate = 0,
        /// <summary>
        /// 奶油蛋糕
        /// </summary>
        [Description("奶油蛋糕")]
        Cream = 1,
        /// <summary>
        /// 水果蛋糕
        /// </summary>
        [Description("水果蛋糕")]
        Fruit = 2,
        /// <summary>
        /// 蛋糕
        /// </summary>
        [Description("蛋糕")]
        Cake = 3

    }
}