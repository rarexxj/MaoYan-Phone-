using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using BntWeb.Core.SystemSettings.Services;
using BntWeb.MemberBase.Services;
using BntWeb.MemberCenter.ApiModels;
using BntWeb.Security.Identity;
using BntWeb.Services;
using BntWeb.Validation;
using BntWeb.WebApi.Models;

namespace BntWeb.MemberCenter.ApiControllers
{
    public class SmsController : BaseApiController
    {
        private readonly ISmsService _smsService;
        private readonly IDefaultCaptchaService _defaultCaptchaService;
        private readonly IMemberService _memberService;

        public SmsController(ISmsService smsService, IDefaultCaptchaService defaultCaptchaService, IMemberService memberService)
        {
            _smsService = smsService;
            _defaultCaptchaService = defaultCaptchaService;
            _memberService = memberService;
        }

        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ApiResult SendCode([FromBody]SmsRequestModel request)
        {
            Argument.ThrowIfNullOrEmpty(request.PhoneNumber, "手机号码");

            var result = new ApiResult();
            var type = request.RequestType;
            if (!Enum.IsDefined(typeof(SmsRequestType), type))
                throw new WebApiInnerException("0001", "请求类型参数无效");

            if (request.RequestType == SmsRequestType.FindPassword)
            {
                //验证手机号是否存在
                var user = _memberService.FindUserByPhone(request.PhoneNumber);
                if (user == null)
                    throw new WebApiInnerException("0003", "此手机号未注册");
            }

            //会员注册、找回密码短信、修改密码  验证图形验证码
        
                if (request.RequestType == SmsRequestType.Register || request.RequestType == SmsRequestType.FindPassword)
            {
                Argument.ThrowIfNullOrEmpty(request.ImageVerifyCode, "图形验证码");
                Argument.ThrowIfNullOrEmpty(request.ImageVerifyKey, "图形验证码");

                if (!_defaultCaptchaService.CaptchaVerifyCodeWithKey(request.ImageVerifyKey, request.ImageVerifyCode, false))
                    throw new WebApiInnerException("0002", "图形验证码验证失败");
            }
            var smsContent= _smsService.SendCode(request.PhoneNumber, MemberCenterModule.Instance, type.ToString());
            if (string.IsNullOrWhiteSpace(smsContent.ErrorMessage))
            {

                //result.SetData(new
                //{
                //    smsvalue = smsContent.KeyValues.Values
                //});
                var data = new
                {
                    smsContent.Key,
                    Code = smsContent.KeyValues["Code"]
                };
                //将短信验证码加密
                result.SetData(data, true);
            }
            else
            {
                result.msg = smsContent.ErrorMessage;
            }

       
            return result;
        }

        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ApiResult SendCodeWeixin([FromBody]SmsRequestModel request)
        {
            Argument.ThrowIfNullOrEmpty(request.PhoneNumber, "手机号码");

            var result = new ApiResult();
            var type = request.RequestType;
            if (!Enum.IsDefined(typeof(SmsRequestType), type))
                throw new WebApiInnerException("0001", "请求类型参数无效");

            if (request.RequestType == SmsRequestType.FindPassword)
            {
                //验证手机号是否存在
                var user = _memberService.FindUserByPhone(request.PhoneNumber);
                if (user == null)
                    throw new WebApiInnerException("0003", "此手机号未注册");
            }

            var smsContent = _smsService.SendCode(request.PhoneNumber, MemberCenterModule.Instance, type.ToString());
            if (string.IsNullOrWhiteSpace(smsContent.ErrorMessage))
            {

                //result.SetData(new
                //{
                //    smsvalue = smsContent.KeyValues.Values
                //});
                var data = new
                {
                    smsContent.Key,
                    Code = smsContent.KeyValues["Code"]
                };
                //将短信验证码加密
                result.SetData(data, true);
            }
            else
            {
                result.msg = smsContent.ErrorMessage;
            }


            return result;
        }
    }
}