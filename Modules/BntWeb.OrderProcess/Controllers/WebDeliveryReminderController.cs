using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BntWeb.Data;
using BntWeb.Data.Services;
using BntWeb.Logging;
using BntWeb.Mvc;
using BntWeb.OrderProcess.Models;
using BntWeb.OrderProcess.Services;
using BntWeb.Security;
using BntWeb.Validation;
using BntWeb.WebApi.Models;

namespace BntWeb.OrderProcess.Controllers
{
    public class WebDeliveryReminderController : Controller
    {
        private readonly ICurrencyService _currencyService;
        private readonly IUserContainer _userContainer;
        private readonly IOrderService _orderService;

        public WebDeliveryReminderController(IUserContainer userContainer, ICurrencyService currencyService, IOrderService orderService)
        {
            _currencyService = currencyService;
            _userContainer = userContainer;
            _orderService = orderService;

            Logger = NullLogger.Instance;
        }
        public ILogger Logger { get; set; }
        // GET: WebDeliveryReminder
        /// <summary>
        /// 提醒发货
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActionResult Remind(Guid orderId)
        {
            var result = new DataTableJsonResult();
            var currentUser = _userContainer.CurrentUser;
            Argument.ThrowIfNullOrEmpty(orderId.ToString(), "订单Id");
            var order = _currencyService.GetSingleById<Order>(orderId);
            Argument.ThrowIfNullOrEmpty(order.ToString(), "订单不存在");
          
            if (order.OrderStatus != OrderStatus.WaitingForDelivery)
                throw new BntWebCoreException("订单不是待发货状态，不能提醒发货");
            if (!order.MemberId.Equals(currentUser.Id))
                throw new BntWebCoreException("只能对自己的订单提醒发货");

            if (!_orderService.CheckTodayCanRemind(orderId, currentUser.Id))
                throw new BntWebCoreException("一天只能提醒发货一次");
           
            var model = new OrderDeliveryReminder()
            {
                Id = KeyGenerator.GetGuidKey(),
                OrderId = orderId,
                OrderNo = order.OrderNo,
                MemberId = currentUser.Id,
                MemberName = currentUser.UserName,
                CreateTime = DateTime.Now
            };

            if (!_currencyService.Create(model))
            {
                throw new BntWebCoreException("创建发货提醒失败，内部错误");
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}