using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BntWeb.Environment;
using BntWeb.Logging;
using BntWeb.MemberBase.Services;
using BntWeb.Wallet.Services;
using BntWeb.Mvc;
using BntWeb.Security;
using BntWeb.SystemMessage.Models;
using BntWeb.SystemMessage.Services;
using BntWeb.Utility.Extensions;
using BntWeb.Wallet.Models;
using BntWeb.Wallet.ViewModel;
using BntWeb.Web.Extensions;
using BntWeb.WebApi.Models;

namespace BntWeb.Wallet.Controllers
{
    public class WebIntegralController : Controller
    {
        private readonly IWalletService _walletService;
        private readonly ISystemMessageService _systemMessageService;
        private readonly IMemberContainer _memberContainer;
        private readonly UrlHelper _urlHelper;
        public WebIntegralController(UrlHelper urlHelper, IMemberContainer memberContainer, IWalletService walletService, ISystemMessageService systemMessageService)
        {
            _walletService = walletService;
            _systemMessageService = systemMessageService;
            _memberContainer = memberContainer;
            _urlHelper = urlHelper;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }
        /// <summary>
        ///  分页获得我的积分
        /// </summary>
        /// <param name="walletType"></param>
        /// <param name="billType"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [MemberAuthorize]
        public ActionResult MyIntegral(BillType? billType=null, WalletType walletType = WalletType.Integral, int pageNo = 1, int pageSize = 9)
        {

            var currencemember = _memberContainer.CurrentMember;
            //获得当前可用积分
            ViewBag.MyIntenal = _walletService.GetWalletByMemberId(currencemember.Id, WalletType.Integral);
            int totalCount;
            ViewBag.ListWalletBill = _walletService.GetWalletBillByMemberId(currencemember.Id, pageNo, pageSize, out totalCount, walletType, billType);

            var routeParas = new RouteValueDictionary{
                    { "area", "Wallet"},
                    { "controller", "WebIntegral"},
                    { "action", "MyIntegral"}
                };
            var returnUrl = HostConstObject.HostUrl + _urlHelper.RouteUrl(routeParas);

            ViewBag.Url = returnUrl + "?pageNo=[pageNo]";
            //获得总页数
            ViewBag.TotalPage = totalCount % pageSize == 0 ? totalCount / pageSize : totalCount / pageSize + 1;
            ViewBag.CurrentPage = pageNo;
            return View();
       
    }
     

    }
}