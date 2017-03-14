using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using BntWeb.Caching;
using BntWeb.Core.SystemSettings.Services;
using BntWeb.Data;
using BntWeb.Data.Services;
using BntWeb.FileSystems.Media;
using BntWeb.MemberBase.Models;
using BntWeb.MemberBase.Services;
using BntWeb.MemberCenter.ApiModels;
using BntWeb.MemberCenter.ViewModels;
using BntWeb.Mvc;
using BntWeb.Security;
using BntWeb.Security.Identity;
using BntWeb.Services;
using BntWeb.Utility.Extensions;
using BntWeb.Validation;
using BntWeb.Wallet.Services;
using BntWeb.WebApi.Models;
using Microsoft.AspNet.Identity;


namespace BntWeb.MemberCenter.Controllers
{
    public class WebMemberController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly ISmsService _smsService;
        private readonly IUserContainer _userContainer;
        private readonly DefaultUserManager _userManager;
        private readonly ISignals _signals;
        private readonly ICurrencyService _currencyService;
        private readonly IDefaultCaptchaService _defaultCaptchaService;
        private readonly IMemberContainer _memberContainer;
        private readonly IStorageFileService _storageFileService;
        private readonly IWalletService _walletService;

        public WebMemberController(IMemberContainer memberContainer, IDefaultCaptchaService defaultCaptchaService,
            ICurrencyService currencyService,
            ISignals signals, DefaultUserManager userManager,
            IMemberService memberService, ISmsService smsService,
            IUserContainer userContainer, 
            IStorageFileService storageFileService, IWalletService walletService)
        {
            _walletService = walletService;
            _currencyService = currencyService;
            _memberService = memberService;
            _smsService = smsService;
            _userContainer = userContainer;
            _userManager = userManager;
            _signals = signals;
            _defaultCaptchaService = defaultCaptchaService;
            _memberContainer = memberContainer;
            _storageFileService = storageFileService;
        }


        /// <summary>
        /// 前台会员注册
        /// </summary>
        /// WebMemberModel registerModel
        /// <returns></returns>
        public ActionResult WebRegister()
        {

            return View();

        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Index", "Home", new {area = "Home"});
        }



        /// <summary>
        /// 注册的的异步回调
        /// </summary>
        /// <param name="postModel"></param>
        /// <returns></returns>
        public async Task<ActionResult> SaveRegister(WebMemberModel postModel)
        {
            var result = new DataJsonResult();
            try
            {

                if (string.IsNullOrWhiteSpace(postModel.PhoneNumber))
                    throw new Exception("手机号码不能为空");
                if (string.IsNullOrWhiteSpace(postModel.Password))
                    throw new Exception("密码不能为空");
                if (string.IsNullOrWhiteSpace(postModel.SmsVerifyCode))
                    throw new Exception("短信验证码不能为空");
                if (string.IsNullOrWhiteSpace(postModel.ImageVerifyCode))
                    throw new Exception("图形验证码不能为空");

                var user = _memberService.FindUserByNameOrPhone(postModel.PhoneNumber);
                if (user != null) throw new Exception("此手机号已经注册");

                if (
                    !_defaultCaptchaService.CaptchaVerifyCodeWithKey(postModel.ImageVerifyKey, postModel.ImageVerifyCode,
                        false))
                    throw new Exception("图形验证码验证失败");

                if (
                    !_smsService.VerifyCode(postModel.PhoneNumber, postModel.SmsVerifyCode, MemberCenterModule.Instance,
                        SmsRequestType.Register.ToString()))
                    throw new Exception("短信验证码验证失败");

                //手动清除图形验证码缓存
                _defaultCaptchaService.Clear(postModel.ImageVerifyKey);

                var token = KeyGenerator.GetGuidKey().ToString();
                //生成会员卡号
                CustomBaseNumber cbn = new CustomBaseNumber("0123456789abcdefghjklmnpqrstuvwxyz");
                //取出最大的会员卡号
                var max =(_memberService.GetMaxMemberCard().To<int>() + 1).ToString();
                cbn.CustomBase = _memberService.GetMaxMemberCard()==""?"1" : max;
                var newMember = new Member
                {
                    UserName = postModel.PhoneNumber,
                    PhoneNumber = postModel.PhoneNumber,
                    //MemberCard = "5k" + cbn.CustomBase.PadLeft(7, '0'),
                    MemberCard = postModel.PhoneNumber,
                    LockoutEnabled = false,
                    Sex = SexType.UnKonw,
                    NickName = KeyGenerator.GenerateRandom(8),
                    Birthday = DateTime.Parse("1949-10-1"),
                    CreateTime = DateTime.Now,
                    DynamicToken = token

                };

                using (TransactionScope scope = new TransactionScope())
                {
                    var identityResult = _memberService.CreateMember(newMember, postModel.Password);

                    if (!identityResult.Succeeded)
                    {
                        throw new Exception(identityResult.Errors.FirstOrDefault());
                    }
                    else
                    {
                        string error;
                        var createdMember = _memberService.FindMemberById(newMember.Id);
                        //注册成功 加5个积分
                        _walletService.Deposit(newMember.Id, Wallet.Models.WalletType.Integral,5, "注册加五个积分", out error);
                        result.Data = createdMember.Simplified();
                        //提交
                        scope.Complete();
                    }
                }

                var newUser = _userManager.Find(newMember.UserName, postModel.Password);


                //登录日志
                _currencyService.Create(new UserLoginLog()
                {
                    Id = KeyGenerator.GetGuidKey(),
                    UserId = newUser.Id,
                    UserName = newUser.UserName,
                    Ip = GetClientIP(Request),
                    CreateTime = DateTime.Now
                });
                var signInManager = new DefaultSignInManager(_userManager, Request.GetOwinContext().Authentication);
                await signInManager.SignInAsync(newUser, true, true);
                //更新缓存
                _signals.Trigger(_userContainer.UserChangedSignalName);
                _signals.Trigger($"member_{newUser.Id}_changed");
                result.Data = _memberService.FindMember(newUser).Simplified();

            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
            }
            return Json(result);
        }

       

 


    /// <summary>  
        /// 获取客户端Ip  
        /// </summary>  
        /// <returns></returns>  
        private string GetClientIP(HttpRequestBase request)
        {
            string clientIP = "";
            if (request != null)
            {
                clientIP = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(clientIP) || (clientIP.ToLower() == "unknown"))
                {
                    clientIP = request.ServerVariables["HTTP_X_REAL_IP"];
                    if (string.IsNullOrEmpty(clientIP))
                    {
                        clientIP = request.ServerVariables["REMOTE_ADDR"];
                    }
                }
                else
                {
                    clientIP = clientIP.Split(',')[0];
                }
            }
            return clientIP;
        }

        /// <summary>
        /// 登录（密码或短信验证码）
        /// </summary>
        /// <returns></returns>
        public ActionResult WebLogin()
        {
            //清除缓存
            HttpContext.GetOwinContext().Authentication.SignOut();
            return View();
        }


        /// <summary>
        /// 登录用户登录(手机号密码)
        /// </summary>
        /// <param name="postModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> LogOn(WebMemberLoginModel postModel)
        {
            var result = new DataJsonResult();
            try
            {
                //根据用户名登录，再根据手机号判断，（前提是手机号与用户名不能互用）
                var user = _userManager.Find(postModel.PhoneNumber, postModel.Password) ??
                           _userManager.Users.FirstOrDefault(u => u.PhoneNumber == postModel.PhoneNumber);
                if (user == null)
                    result.ErrorMessage = "用户名或密码错误";
                else
                {
                    if (_userManager.Find(user.UserName, postModel.Password) == null)
                    {
                        result.ErrorMessage = "用户名或密码错误";
                    }
                    else if (user.UserType == UserType.Member)
                    {
                        if (user.LockoutEnabled)
                            result.ErrorMessage = "此用户已经禁止登录";
                        else
                        {
                            var signInManager = new DefaultSignInManager(_userManager,
                                Request.GetOwinContext().Authentication);

                            var token = KeyGenerator.GetGuidKey().ToString();
                            user.DynamicToken = token;
                            user.MobileDevice = postModel.MobileDevice;
                          
                            _userManager.Update(user);

                            //登录日志
                            _currencyService.Create(new UserLoginLog()
                            {
                                Id = KeyGenerator.GetGuidKey(),
                                UserId = user.Id,
                                UserName = user.UserName,
                                CreateTime = DateTime.Now
                            });


                            await signInManager.SignInAsync(user, true, false);
                            //更新缓存,dddddfd
                            _signals.Trigger(_userContainer.UserChangedSignalName);
                            _signals.Trigger($"member_{user.Id}_changed");
                            result.Data = _memberService.FindMember(user).Simplified();
                        }
                    }
                    else
                    {
                        result.ErrorMessage = "用户不存在或密码错误";
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
            }
            return Json(result);
        }

        /// <summary>
        /// 动态码登录（短信验证码）
        /// </summary>
        /// <param name="loginWithSms"></param>
        /// <returns></returns>
        ///  [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> LoginWithSms(WebLoginWithSmsModel loginWithSms)

        {
            var result = new DataJsonResult();

            if (string.IsNullOrWhiteSpace(loginWithSms.DPhoneNumber))
                throw new Exception("手机号码不能为空");
            if (string.IsNullOrWhiteSpace(loginWithSms.SmsVerifyCode))
                throw new Exception("短信验证码");


            if (
                !_smsService.VerifyCode(loginWithSms.DPhoneNumber, loginWithSms.SmsVerifyCode,
                    MemberCenterModule.Instance, SmsRequestType.Login.ToString()))
                throw new WebApiInnerException("0001", "短信验证码验证失败");
            try
            {
                var user = _userManager.FindByName(loginWithSms.DPhoneNumber);
                if (user == null)
                {
                    result.ErrorMessage = "用户名或密码错误";
                }
                else
                {
                    if (user.UserType == UserType.Member)
                    {
                        if (user.LockoutEnabled)
                            result.ErrorMessage = "此用户已经禁止登录";
                        else
                        {
                            var signInManager = new DefaultSignInManager(_userManager,
                                Request.GetOwinContext().Authentication);

                            var token = KeyGenerator.GetGuidKey().ToString();
                            user.DynamicToken = token;
                            user.MobileDevice = loginWithSms.MobileDevice;

                            var needClearUser =
                                _userManager.Users.Where(
                                    u =>
                                        u.MobileDevice.Equals(loginWithSms.MobileDevice,
                                            StringComparison.OrdinalIgnoreCase) &&
                                        !u.Id.Equals(user.Id, StringComparison.OrdinalIgnoreCase)).ToArray();
                            foreach (var item in needClearUser)
                            {
                                item.MobileDevice = "";
                                _userManager.Update(item);
                            }
                            _userManager.Update(user);

                            //登录日志
                            _currencyService.Create(new UserLoginLog()
                            {
                                Id = KeyGenerator.GetGuidKey(),
                                UserId = user.Id,
                                UserName = user.UserName,
                                CreateTime = DateTime.Now
                            });


                            await signInManager.SignInAsync(user, true, false);
                            //更新缓存,dddddfd
                            _signals.Trigger(_userContainer.UserChangedSignalName);
                            _signals.Trigger($"member_{user.Id}_changed");
                            result.Data = _memberService.FindMember(user).Simplified();
                        }
                    }
                    else
                    {
                        result.ErrorMessage = "用户不存在或密码错误";
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
            }
            return Json(result);

        }

        //编辑个人信息
        [MemberAuthorize]
        public ActionResult WebPersonal()
        {
            var currentMember = _memberContainer.CurrentMember;
            ViewBag.PhoneNumber = currentMember.PhoneNumber;
            var personalModel = _memberService.FindMemberById(currentMember.Id);
            ViewBag.WebAvatarFile =
                _storageFileService.GetFiles(currentMember.Id.ToGuid(), MemberCenterModule.Key, "Avatar")
                    .FirstOrDefault();
            ViewBag.MemberId = currentMember.Id;
            return View(personalModel);
        }

        /// <summary>
        /// 编辑个人信息
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        [MemberAuthorize]
        public ActionResult UpdatePersonal(WebModifyMemberModel member)
        {
            var result = new DataJsonResult();
            var currentMember = _memberContainer.CurrentMember;
            ViewBag.PhoneNumber = currentMember.PhoneNumber;
            var oldMember = _memberService.FindMemberById(currentMember.Id);
            if (oldMember == null)
                throw new Exception("用户信息不存在！");

            if (!string.IsNullOrWhiteSpace(member.NickName))
                oldMember.NickName = member.NickName;

            if (!string.IsNullOrWhiteSpace(member.NickName))
                oldMember.RealName = member.RealName;

            oldMember.Sex = member.Sex == SexType.Male ? SexType.Male : SexType.Female;
            if (!string.IsNullOrWhiteSpace(member.Qq))
                oldMember.Qq = member.Qq;

            if (!string.IsNullOrWhiteSpace(member.Weixin))
                oldMember.Weixin = member.Weixin;

            if (!string.IsNullOrWhiteSpace(member.BirthType.ToString()))
                oldMember.BirthType = member.BirthType == BirthType.Calendar ? BirthType.Calendar : BirthType.Lunar;

            if (!string.IsNullOrWhiteSpace(member.Birthday.ToString()))
                oldMember.Birthday = member.Birthday;

            if (!string.IsNullOrWhiteSpace(member.TastesType.ToString()))
            {
                if (member.TastesType == TastesType.Chocolate)
                {
                    oldMember.TastesType = TastesType.Chocolate;
                }
                else if (member.TastesType == TastesType.Cream)
                {
                    oldMember.TastesType = TastesType.Cream;
                }
                else if (member.TastesType == TastesType.Fruit)
                {
                    oldMember.TastesType = TastesType.Fruit;
                }
                else
                {
                    oldMember.TastesType = TastesType.Cake;
                }
            }
            StorageFile storageFile = new StorageFile();
            if (member.AvatarId != Guid.Empty)
            {
               
                if (_storageFileService.ReplaceFile(currentMember.Id.ToGuid(), MemberCenterModule.Key, MemberCenterModule.DisplayName, member.AvatarId, "Avatar"))
                {
                    //更新缓存
                    _signals.Trigger(_userContainer.UserChangedSignalName);
                    _signals.Trigger($"member_{currentMember.Id}_changed");
                    storageFile =
                        _storageFileService.GetFiles(currentMember.Id.ToGuid(), MemberCenterModule.Key, "")
                            .FirstOrDefault();
                    if (storageFile != null)
                        result.Data = storageFile.Id;
                }
                else
                {
                    throw new Exception("头像上传失败");
                }
            }
            var identityResult = _memberService.UpdateMember(oldMember);
            if (!identityResult.Succeeded)
            {
                throw new Exception("更新失败:" + identityResult.Errors.FirstOrDefault());
            }


            return Json(result);
        }




        /// <summary>
        /// 找回/重置密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ForgetPassword()
        {
            return View();
        }
        [MemberAuthorize(Forcible = false)]
        public async Task<ActionResult> ResetPasswordOnpost(WebResetPasswordModel passwordReset)
        {
            Argument.ThrowIfNullOrEmpty(passwordReset.PhoneNumber, "手机号码");
            Argument.ThrowIfNullOrEmpty(passwordReset.Password, "密码");
            Argument.ThrowIfNullOrEmpty(passwordReset.SmsVerifyCode, "短信验证码");

            if (
                !_smsService.VerifyCode(passwordReset.PhoneNumber, passwordReset.SmsVerifyCode,
                    MemberCenterModule.Instance, RequestSmsType.FindPassword.ToString()))
                throw new WebApiInnerException("0002", "短信验证码验证失败");

            var result = new DataJsonResult();

            User user = _memberService.FindUserByName(passwordReset.PhoneNumber);
            if (user == null) throw new WebApiInnerException("0001", "此手机号未注册");
            var signInManager = new DefaultSignInManager(_userManager, Request.GetOwinContext().Authentication);
            await signInManager.SignInAsync(user, true, false);
            //更新缓存,dddddfd
            _signals.Trigger(_userContainer.UserChangedSignalName);
            _signals.Trigger($"member_{user.Id}_changed");
            _memberService.ResetPassword(user, passwordReset.Password);

            return Json(result);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult PasswordChange()
        {
          
            return View();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult PasswordChangeOnPost(WebChangePasswordModel changePassword)
        {
            Argument.ThrowIfNullOrEmpty(changePassword.Password, "旧密码");
            Argument.ThrowIfNullOrEmpty(changePassword.NewPassword, "新密码");
            var currentMember = _memberContainer.CurrentMember;
          
            var result = new DataJsonResult();

            var identityResult =
                _userManager.ChangePasswordAsync(currentMember.Id, changePassword.Password, changePassword.NewPassword)
                    .Result;
            if (!identityResult.Succeeded)
                throw new WebApiInnerException("0002", identityResult.Errors.FirstOrDefault());

            return Json(result);
        }

        /// <summary>
        /// 修改手机号码
        /// </summary>
        /// <returns></returns>
        [MemberAuthorize]
        public ActionResult ChangePhoneNumber()
        {
            var currentMember = _memberContainer.CurrentMember;
            ViewBag.PhoneNumber = currentMember.PhoneNumber;
            return View();
        }

        /// <summary>
        /// 修改手机号码
        /// </summary>
        /// <param name="changePhone"></param>
        /// <returns></returns>
 
        public async Task<JsonResult> WebChangePhoneNumberOnPost(WebChangePhoneNumberModel changePhone)
        {
            Argument.ThrowIfNullOrEmpty(changePhone.PhoneNumber, "旧手机");
            Argument.ThrowIfNullOrEmpty(changePhone.SmsVerifyCode, "旧手机验证码");
            Argument.ThrowIfNullOrEmpty(changePhone.NewPhoneNumber, "新手机");
            Argument.ThrowIfNullOrEmpty(changePhone.NewSmsVerifyCode, "新手机验证码");

            //获得当前用户
            var currentMember = _memberContainer.CurrentMember;
            if (
                !_smsService.VerifyCode(changePhone.PhoneNumber, changePhone.SmsVerifyCode, MemberCenterModule.Instance,
                    RequestSmsType.ChangePhoneNumber.ToString()))
                throw new WebApiInnerException("0002", "旧手机短信验证码验证失败");

            if (
                !_smsService.VerifyCode(changePhone.NewPhoneNumber, changePhone.NewSmsVerifyCode,
                    MemberCenterModule.Instance, RequestSmsType.BoundPhoneNumber.ToString()))
                throw new WebApiInnerException("0003", "新手机短信验证码验证失败");

            //判断是否已经存在此手机号
            var testUser = _userContainer.FindUser(changePhone.NewPhoneNumber);
            if (testUser != null)
            {
                throw new WebApiInnerException("0004", "新手机已经注册，无法更改");
            }
            var user = _userManager.FindById(currentMember.Id);

            user.UserName = changePhone.NewPhoneNumber;
            user.PhoneNumber = changePhone.NewPhoneNumber;

            var identityResult = _userManager.Update(user);
            if (identityResult.Succeeded)
            {
                var signInManager = new DefaultSignInManager(_userManager, Request.GetOwinContext().Authentication);
                await signInManager.SignInAsync(user, true, false);
                //更新缓存
                _signals.Trigger(_userContainer.UserChangedSignalName);
                _signals.Trigger($"member_{currentMember.Id}_changed");
            }
            else
            {
                throw new WebApiInnerException("0005", "手机号修改失败");
            }
            var result = new DataJsonResult();
            return Json(result);
        }
        
        /// <summary>
        /// 绑定新手机
        /// </summary>
        /// <returns></returns>

        public ActionResult WebBoundPhoneOnPost(WebBoundPhoneNumberModel boundPhone)
        {
            var result = new DataJsonResult();
            Argument.ThrowIfNullOrEmpty(boundPhone.PhoneNumber, "手机号");
            Argument.ThrowIfNullOrEmpty(boundPhone.SmsVerifyCode, "手机验证码");
            Argument.ThrowIfNullOrEmpty(boundPhone.Password, "密码");

            if (
                !_smsService.VerifyCode(boundPhone.PhoneNumber, boundPhone.SmsVerifyCode, MemberCenterModule.Instance,
                    RequestSmsType.BoundPhoneNumber.ToString()))
                throw new WebApiInnerException("0001", "手机短信验证码验证失败");
            //获得当前用户
            var currentUser = _userContainer.CurrentUser;
            var user = _userManager.Find(currentUser.UserName, boundPhone.Password);
            if (user != null && user.UserType == UserType.Member)
            {
                if (user.LockoutEnabled)
                    throw new WebApiInnerException("0002", "此用户已经禁止登录");

                //判断是否已经存在此手机号
                var testUser = _userContainer.FindUser(boundPhone.PhoneNumber);
                if (testUser != null)
                {
                    throw new WebApiInnerException("0004", "手机号已经注册，无法绑定");
                }

                user.UserName = boundPhone.PhoneNumber;
                user.PhoneNumber = boundPhone.PhoneNumber;


                var oldMember = _memberService.FindMemberById(currentUser.Id);
                if (oldMember == null)
                    throw new WebApiInnerException("0005", "会员信息不存在");
                var midentityResult = _memberService.UpdateMember(oldMember);
                if (!midentityResult.Succeeded)
                {
                    throw new WebApiInnerException("0006", "更新失败:" + midentityResult.Errors.FirstOrDefault());
                }
            }
            else
            {
                throw new WebApiInnerException("0003", "密码不正确");
            }

            var identityResult = _userManager.Update(user);
            if (identityResult.Succeeded)
            {
                //更新缓存
                _signals.Trigger(_userContainer.UserChangedSignalName);
                _signals.Trigger($"member_{currentUser.Id}_changed");
            }
            else
            {
                throw new WebApiInnerException("0007", "手机号绑定失败");
            }
            return Json(result);
        }

        public ActionResult PhoneIsExist(string phone)
        {
            var result = new DataJsonResult();
        
            var user = _currencyService.GetSingleByConditon<Member>(a => a.PhoneNumber == phone);
            if (user == null)
                throw new Exception("该用户不存在");
            return Json(result);
        }
        /// <summary>
        /// 判断图形验证码是否正确
        /// </summary>
        /// <param name="imageVerifyCode"></param>
        /// <param name="imageVerifyKey"></param>
        /// <returns></returns>
        public ActionResult ImageVerify(string imageVerifyKey ,string imageVerifyCode)
        {
            var result = new DataJsonResult();
            if (string.IsNullOrWhiteSpace(imageVerifyCode))
                throw new Exception("图形验证码为空");
          
                if (string.IsNullOrWhiteSpace(imageVerifyKey))
                    throw new Exception("图形验证码为空");
            if (!_defaultCaptchaService.CaptchaVerifyCodeWithKey(imageVerifyKey, imageVerifyCode, false))
            {
                result.ErrorMessage = "图形验证码验证失败";
            }
            else
            {
                result.Data = 1;
            }
            return Json(result);
        }
      
    }
 

  
}