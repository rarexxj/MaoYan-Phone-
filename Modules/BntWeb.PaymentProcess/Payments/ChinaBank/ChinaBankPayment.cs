/* 
    ======================================================================== 
        File name：		ChinaBankPayment
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/12/9 11:37:10
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Transactions;
using System.Web;
using BntWeb.Config.Models;
using BntWeb.Data.Services;
using BntWeb.Logging;
using BntWeb.OrderProcess.Models;
using BntWeb.OrderProcess.Services;
using BntWeb.PaymentProcess.Models;
using BntWeb.PaymentProcess.Payments.ChinaBank.Sdk;
using BntWeb.PaymentProcess.Services;
using BntWeb.Services;
using BntWeb.Wallet.Services;

namespace BntWeb.PaymentProcess.Payments.ChinaBank
{
    public class ChinaBankPayment : IPaymentDispatcher
    {
        private readonly IPaymentService _paymentService;
        private readonly ICurrencyService _currencyService;
        private readonly IOrderService _orderService;
        private readonly IWalletService _walletService;
        private readonly IConfigService _configService;

        public ChinaBankPayment(IPaymentService paymentService, ICurrencyService currencyService, IOrderService orderService, IWalletService walletService, IConfigService configService)
        {
            _paymentService = paymentService;
            _currencyService = currencyService;
            _orderService = orderService;
            _walletService = walletService;
            _configService = configService;

            Logger = NullLogger.Instance;
        }
        public ILogger Logger { get; set; }

        public string SyncReturn(HttpRequestBase request)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 网银在线支付异步回调处理订单状态
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string AsyncReturn(HttpRequestBase request)
        {
            var status_msg = "";
            var payment = _paymentService.LoadPayment(PaymentType.ChinaBank.ToString());
            var configs = _configService.Get<ChinaBankConfig>();

            var key = configs.MD5Key;

            var v_oid = request["v_oid"];
            var v_pstatus = request["v_pstatus"];
            var v_md5str = request["v_md5str"];
            var v_amount = request["v_amount"];
            var v_moneytype = request["v_moneytype"];

            var v_pstring = request["v_pstring"];//支付结果信息
            var v_pmode = request["v_pmode"];//支付银行，例如工商银行
            var remark1 = request["remark1"];
            var remark2 = request["remark2"];

            string str = v_oid + v_pstatus + v_amount + v_moneytype + key;
            str = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "md5").ToUpper();

            Logger.Warning("正在异步操作商户订单号：" + v_oid);
            Logger.Warning($"返回支付信息：[v_pstatus:{v_pstatus}][v_pstring:{v_pstring}][v_pmode:{v_pmode}][]" );
            if (str == v_md5str)
            {
                //前面成功
                status_msg = "ok";
                //20（表示支付成功）30（表示支付失败）
                if (v_pstatus.Equals("20"))
                {
                    //支付成功
                    //商户系统的逻辑处理（例如判断金额，判断支付状态，更新订单状态等等）.......
                    PayLog payLog =
                        _currencyService.GetSingleByConditon<PayLog>(o => o.TransactionNo == v_oid);
                    if (payLog != null)
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            Order order = _currencyService.GetSingleByConditon<Order>(o => o.Id == payLog.OrderId);

                            if (order != null && order.OrderStatus == OrderStatus.PendingPayment &&
                                order.PayStatus != PayStatus.Paid)
                            {

                                payLog.PayTime = DateTime.Now;
                                payLog.LogStatus = LogStatus.Paid;
                                _currencyService.Update(payLog);

                                order.PayTime = DateTime.Now;
                                order.PaymentId = payment.Id;
                                order.PaymentName = payment.Name;
                                _currencyService.Update(order);

                                if (order.SourceType.Equals("Recharge"))
                                {
                                    string error;
                                    _walletService.Deposit(order.MemberId, Wallet.Models.WalletType.Cash, order.PayFee,
                                        "账户充值" + order.OrderNo, out error);

                                    _orderService.ChangeOrderStatus(order.Id, OrderStatus.Completed, PayStatus.Paid,
                                        null, null, "网银在线充值");
                                }
                                else
                                {
                                    _orderService.ChangeOrderStatus(order.Id, OrderStatus.WaitingForDelivery,
                                        PayStatus.Paid,
                                        null, null, "网银在线支付");
                                }
                                scope.Complete();
                            }
                            else if (order != null && order.OrderStatus == OrderStatus.WaitingForDelivery)
                            {
                                status_msg = "ok";
                            }
                            else
                            {
                                status_msg = "error";
                            }
                        }
                    }

                }
                else
                {
                    status_msg = "error";
                }
            }
            else
            {
                status_msg = "error";
            }

            return status_msg;
        }

        public string GetSignInfo(string subject, string body, string notifyUrl, PayLog payLog, Payment payment,
            Dictionary<string, string> param = null)
        {
            throw new NotImplementedException();
        }

        public string H5Pay(string subject, string body, string notifyUrl, string returnUrl, PayLog payLog, Payment payment,
            Dictionary<string, string> param = null)
        {
            throw new NotImplementedException();
        }

        public string WebPay(string subject, string body, string notifyUrl, string returnUrl, PayLog payLog, Payment payment,
            Dictionary<string, string> param = null)
        {
            if (param == null)
            {
                return null;
            }
            if (body.Length > 512)
                body = body.Substring(0, 500) + "...";
            var chinabankConfig = _configService.Get<ChinaBankConfig>();
            ////////////////////////////////////////////////////////////////////////////////////////////////
            //把请求参数打包成数组
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            //必要的交易信息
            sParaTemp.Add("v_amount", payLog.OrderAmount.ToString());// 订单金额
            sParaTemp.Add("v_moneytype", "CNY");// 币种
            sParaTemp.Add("v_oid", payLog.TransactionNo);// 订单号
            sParaTemp.Add("v_mid", chinabankConfig.MchId);// 商户号
            sParaTemp.Add("v_url", returnUrl);// 消费者完成购物后页面返回的商户页面，URL参数是以http://开头的完整URL地址：同步通知地址
            sParaTemp.Add("payment_type", "1");
            sParaTemp.Add("remark2", $"[url:={notifyUrl}]");//支付成功后异步回调地址；必须要有[url:=]格式。
            sParaTemp.Add("remark1", payLog.TransactionNo); //自定义值，
            //收货地址[选填]
            sParaTemp.Add("v_rcvname", param["v_rcvname"]??"");// 收货人
            sParaTemp.Add("v_rcvaddr", param["v_rcvaddr"] ?? "");// 收货地址
            sParaTemp.Add("v_rcvtel", param["v_rcvtel"] ?? ""); // 收货人电话
            sParaTemp.Add("v_rcvpost", param["v_rcvpost"] ?? "");// 收货人邮编
            sParaTemp.Add("v_rcvemail", param["v_rcvemail"] ?? "");// 收货人邮件
            sParaTemp.Add("v_rcvmobile", param["v_rcvmobile"] ?? "");// 收货人手机号

            //订货人信息
            sParaTemp.Add("v_ordername", param["v_ordername"] ?? "");// 订货人姓名
            sParaTemp.Add("v_orderaddr", param["v_orderaddr"] ?? "");// 订货人地址
            sParaTemp.Add("v_ordertel", param["v_ordertel"] ?? "");// 订货人电话
            sParaTemp.Add("v_orderpost", param["v_orderpost"] ?? "");// 订货人邮编
            sParaTemp.Add("v_orderemail", param["v_orderemail"] ?? "");// 订货人邮件
            sParaTemp.Add("v_ordermobile", param["v_ordermobile"] ?? "");// 订货人手机号

            //签名数据
            var signText = sParaTemp["v_amount"] + sParaTemp["v_moneytype"] + sParaTemp["v_oid"] + sParaTemp["v_mid"] + sParaTemp["v_url"] + chinabankConfig.MD5Key; // 拼凑加密串
            var v_md5info  = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(signText, "md5").ToUpper();
            //签名参数
            sParaTemp.Add("v_md5info", v_md5info);
            
            //建立请求
            return new ChinaBankSubmit().BuildRequest(sParaTemp, "post", "确认");
        }
    }
}