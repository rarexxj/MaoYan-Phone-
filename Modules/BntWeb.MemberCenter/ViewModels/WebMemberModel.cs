/* 
    ======================================================================== 
        File name：        SwitchUserViewModel
        Module:                
        Author：            罗嗣宝
        Create Time：    2016/5/18 8:39:36
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/

using System;
using BntWeb.MemberBase.Models;

namespace BntWeb.MemberCenter.ViewModels
{
    public class WebMemberModel
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 短信验证码
        /// </summary>
        public string SmsVerifyCode { get; set; }
        /// <summary>
        /// 图片Key
        /// </summary>
        public string ImageVerifyKey { get; set; }
        /// <summary>
        /// 图片验证码
        /// </summary>
        public string ImageVerifyCode { get; set; }
    }
    public class WebMemberLoginModel
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

        public string MobileDevice { get; set; }

   
    }
    public class WebLoginWithSmsModel
    {
        public string DPhoneNumber { get; set; }

        public string SmsVerifyCode { get; set; }

        public string MobileDevice { get; set; }

    
    }
    public class WebResetPasswordModel
    {
        public string PhoneNumber { get; set; }

        public string Password { get; set; }

        public string SmsVerifyCode { get; set; }
    }


    public class WebChangePasswordModel
    {
        public string Id { get; set; }
        public string Password { get; set; }

        public string NewPassword { get; set; }
    }

    public class WebModifyMemberModel
    {
        /// <summary>
        /// 用户名（昵称）
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public SexType Sex { get; set; }
        /// <summary>
        /// Qq号
        /// </summary>
        public string Qq { get; set; }
        /// <summary>
        /// 微信
        /// </summary>
        public string Weixin { get; set; }
        /// <summary>
        /// 生日类型
        /// </summary>
        public BirthType BirthType { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 口味偏好
        /// </summary>
        public TastesType TastesType { get; set; }
        public Guid AvatarId { get; set; }
    }

    public class WebChangePhoneNumberModel
    {
        public string Id { get; set; }
        public string PhoneNumber { get; set; }

        public string SmsVerifyCode { get; set; }

        public string NewPhoneNumber { get; set; }

        public string NewSmsVerifyCode { get; set; }
    }

    public class WebBoundPhoneNumberModel
    {
        public string PhoneNumber { get; set; }

        public string SmsVerifyCode { get; set; }

        public string Password { get; set; }

    }
}