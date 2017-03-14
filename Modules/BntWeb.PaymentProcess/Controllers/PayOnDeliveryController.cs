using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BntWeb.Data;
using BntWeb.Data.Services;
using BntWeb.Mvc;
using BntWeb.OrderProcess.Models;
using BntWeb.PaymentProcess.Models;
using BntWeb.PaymentProcess.ViewModels;

namespace BntWeb.PaymentProcess.Controllers
{
    public class PayOnDeliveryController : Controller
    {
        private readonly ICurrencyService _currencyService;

        public PayOnDeliveryController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        /// <summary>
        /// 货到付款提交
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActionResult PayOnDeliveryPost(Guid orderId)
        {
            var result = new DataJsonResult();

            try
            {
                var order = _currencyService.GetSingleById<Order>(orderId);
                if (order == null)
                    throw new Exception("订单不存在");
                if (order.PayStatus == PayStatus.Paid || order.OrderStatus != OrderStatus.PendingPayment)
                    throw new Exception("订单已经支付过或状态非法");
                var payment = _currencyService.GetSingleByConditon<Payment>(
                    p => p.Code.Equals("payondelivery", StringComparison.OrdinalIgnoreCase));
                if (payment == null)
                    throw new Exception("支付方式无效");

                order.PayOnline = false;
                order.PaymentId = payment.Id;
                order.PaymentName = payment.Name;
                order.OrderStatus = OrderStatus.WaitingForDelivery;
                if (_currencyService.Update(order))
                {
                    var orderAction = new OrderAction
                    {
                        Id = KeyGenerator.GetGuidKey(),
                        OrderId = order.Id,
                        Memo = "货到付款-待发货",
                        CreateTime = DateTime.Now,
                        OrderStatus = order.OrderStatus,
                        PayStatus = order.PayStatus,
                        ShippingStatus = order.ShippingStatus,
                        EvaluateStatus = order.EvaluateStatus,
                        UserId = order.MemberId,
                        UserName = order.MemberName
                    };
                    _currencyService.Create(orderAction);
                }
                else
                    throw new Exception("订单状态更新失败");

            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
            }
            return Json(result);
        }

       

    }
}
