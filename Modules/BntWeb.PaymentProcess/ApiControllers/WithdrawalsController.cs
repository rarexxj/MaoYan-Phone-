
using System.Web.Http;
using BntWeb.Data.Services;
using BntWeb.Logging;
using BntWeb.MemberBase.Models;
using BntWeb.PaymentProcess.Services;
using BntWeb.WebApi.Filters;
using BntWeb.WebApi.Models;

namespace BntWeb.PaymentProcess.ApiControllers
{
    public class WithdrawalsController : BaseApiController
    {
        private readonly ICurrencyService _currencyService;
        private readonly IPaymentService _paymentService;

        public WithdrawalsController(ICurrencyService currencyService, IPaymentService paymentService)
        {
            _currencyService = currencyService;
            _paymentService = paymentService;

            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        /// <summary>
        /// 获取提现支付方式
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [BasicAuthentication]
        public ApiResult GetWithdrawalsPaymentType()
        {
            var result = new ApiResult();
            var oauth = _currencyService.GetSingleByConditon<UserOAuth>(
                    o => o.OAuthType == OAuthType.WeiXin && o.MemberId == AuthorizedUser.Id);

            var wxPayment = _paymentService.LoadPayment("weixin");
            var alipayPayment = _paymentService.LoadPayment("alipay");

            var data = new
            {
                WeiXin = oauth != null && wxPayment != null && wxPayment.Enabled,
                Alipay = alipayPayment != null && alipayPayment.Enabled
            };

            result.SetData(data);

            return result;
        }
    }
}