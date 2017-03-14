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

using System.ComponentModel;

namespace BntWeb.MemberCenter.ViewModels
{
    public class WebSmsRequestModel
    {
        public string PhoneNumber { get; set; }

        public RequestSmsType RequestType { get; set; }
    }
    public enum RequestSmsType
    {
        [Description("注册")]
        Register = 0,

        [Description("找回密码")]
        FindPassword = 1,

        [Description("修改手机")]
        ChangePhoneNumber = 2,

        [Description("验证码登录")]
        Login = 3,

        [Description("绑定手机")]
        BoundPhoneNumber = 4
    }
}