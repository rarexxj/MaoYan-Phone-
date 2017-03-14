using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using BntWeb.Data;
using BntWeb.Data.Services;
using BntWeb.Environment;
using BntWeb.FileSystems.Media;
using BntWeb.Logging;
using BntWeb.Logistics.Models;
using BntWeb.MemberBase.Services;
using BntWeb.Mvc;
using BntWeb.OrderProcess.Models;
using BntWeb.OrderProcess.Services;
using BntWeb.Security;
using BntWeb.Utility.Extensions;
using BntWeb.Validation;


namespace BntWeb.OrderProcess.Controllers
{
    public class WebOrderController : Controller
    {
        private readonly ICurrencyService _currencyService;
        private readonly IMemberContainer _memberContainer;
        private readonly IOrderService _orderService;
        private readonly IStorageFileService _storageFileService;

        private readonly UrlHelper _urlHelper;

        public WebOrderController(IStorageFileService storageFileService,
            IUserContainer userContainer, IMemberContainer memberContainer,
            ICurrencyService currencyService, IOrderService orderService, UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
            _storageFileService = storageFileService;
            _currencyService = currencyService;
            _orderService = orderService;
            _memberContainer = memberContainer;
            Logger = NullLogger.Instance;
        }
        public ILogger Logger { get; set; }
        // GET: WebOrder
        //获得我的订单：所有，待付款，待收货，待评价
        /// <summary>
        /// 分页加载订单列表
        /// </summary>
        /// <param name="status"> 订单状态</param>
        /// <param name="keywords">关键词（商品名称或订单号）</param>
        /// <param name="evaluateStatus">订单评价状态</param>
        /// <param name="refundStatus">订单退款状态</param>
        /// <param name="payStatus">付款状态</param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [MemberAuthorize]
        public ActionResult WebOrderList(OrderStatus? status = null, string keywords = "", EvaluateStatus? evaluateStatus = null,
            OrderRefundStatus? refundStatus = null, PayStatus? payStatus = null, int pageNo = 1, int pageSize = 10)
        {
            if(status != null)
                ViewBag.Status = (int)status;
            else if(refundStatus != null)
                ViewBag.Status = 4;
            else if(evaluateStatus != null)
                ViewBag.Status = 3;
            else
                ViewBag.Status = -1;

            //获得当前用户
            int totalCount = 0;
            var currenceMember = _memberContainer.CurrentMember;
            //我的订单列表
            var myOrders = _orderService.LoadByPage(currenceMember.Id, out totalCount, status, payStatus, null, evaluateStatus, refundStatus, keywords, pageNo, pageSize, status, refundStatus);
            ViewBag.MyOrders = myOrders;

            var orderState = _orderService.StateOrderCount(currenceMember.Id);
            ViewBag.OrderState = orderState;

            var routeParas = new RouteValueDictionary{
                    { "area", "OrderProcess"},
                    { "controller", "WebOrder"},
                    { "action", "WebOrderList"}
                };
            var returnUrl = HostConstObject.HostUrl + _urlHelper.RouteUrl(routeParas);

            ViewBag.Url = returnUrl + "?pageNo=[pageNo]";
            //获得总页数
            ViewBag.TotalPage = totalCount % pageSize == 0 ? totalCount / pageSize : totalCount / pageSize + 1;
            ViewBag.CurrentPage = pageNo;

            return View();
        }
        

        //删除订单
        [MemberAuthorize]
        public ActionResult WebDeleteOrder(Guid orderId)
        {
            var result = new DataJsonResult();
            try
            {
                var currentUser = _memberContainer.CurrentMember;
                Argument.ThrowIfNullOrEmpty(orderId.ToString(), "订单Id");

                var order = _currencyService.GetSingleById<Order>(orderId);
                if (!order.MemberId.Equals(currentUser.Id))
                    throw new BntWebCoreException("只能删除自己的订单");
                if (order.OrderStatus != OrderStatus.Closed && order.OrderStatus != OrderStatus.Completed)
                    throw new BntWebCoreException("只能删除关闭或完成的订单");

                order.OrderStatus = OrderStatus.Deleted;
                if (!_currencyService.Update(order))
                {
                    throw new BntWebCoreException("删除失败，可能订单已经被删除");
                }

            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        //取消订单
        [MemberAuthorize]
        public ActionResult WebCancelOrder(Guid orderId)
        {
            var result = new DataJsonResult();
            try
            {
                var currentUser = _memberContainer.CurrentMember;
                Argument.ThrowIfNullOrEmpty(orderId.ToString(), "订单Id");

                var order = _currencyService.GetSingleById<Order>(orderId);
                if (!order.MemberId.Equals(currentUser.Id))
                    throw new Exception("只能处理自己的订单");

                if (order.OrderStatus != OrderStatus.PendingPayment)
                    throw new Exception("订单状态不合法，请联系商家取消订单");

                if (!_orderService.ChangeOrderStatus(orderId, OrderStatus.Closed, null, null, null, "客户取消订单"))
                {
                    throw new Exception("取消订单失败，可能订单状态已经变更");

                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
            }

            return Json(result);

        }
        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [MemberAuthorize]
        public ActionResult WebCompleteOrder(Guid orderId)
        {
            var result = new DataJsonResult();
            var currentUser = _memberContainer.CurrentMember;
            Argument.ThrowIfNullOrEmpty(orderId.ToString(), "订单Id");

            var order = _currencyService.GetSingleById<Order>(orderId);
            if (!order.MemberId.Equals(currentUser.Id))
                throw new BntWebCoreException("只能处理自己的订单");

            if (order.OrderStatus != OrderStatus.WaitingForReceiving)
                throw new BntWebCoreException("订单状态不合法，无法确认收货");

            if (!_orderService.ChangeOrderStatus(orderId, OrderStatus.Completed, null, null, null, "客户确认收货"))
            {
                throw new BntWebCoreException("确认收货失败，可能订单状态已经变更");
            }
            return Json(result);

        }
        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [MemberAuthorize]
        public ActionResult OrderDetails(Guid orderId)
        {
            //获得订单详情
            var order = _orderService.Load(orderId);
            //物流信息
            ViewBag.Shippings = _currencyService.GetList<Shipping>(s => s.Status == Logistics.Models.ShippingStatus.Enabled).ToJson();

            foreach (var item in order.OrderGoods)
            {
                item.GoodsImage1 =
                    _storageFileService.GetFiles(item.Id, OrderProcessModule.Key, "MainImage")
                        .FirstOrDefault()?.Simplified();
            }
            return View(order);
        }
        /// <summary>
        /// 提醒发货
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [MemberAuthorize]
        public JsonResult WebRemind(Guid orderId)
        {
            var result = new DataJsonResult();
            try
            {
                if (orderId.Equals(Guid.Empty))
                    throw new Exception("订单Id不合法");

                var order = _currencyService.GetSingleById<Order>(orderId);
                if (order == null)
                    throw new Exception("订单不存在");
                if (order.OrderStatus != OrderStatus.WaitingForDelivery)
                    throw new Exception("订单不是待发货状态，不能提醒发货");
                if (!order.MemberId.Equals(_memberContainer.CurrentMember.Id))
                    throw new Exception("只能对自己的订单提醒发货");
                if (!_orderService.CheckTodayCanRemind(orderId, _memberContainer.CurrentMember.Id))
                    throw new Exception("一天只能提醒发货一次");
                var model = new OrderDeliveryReminder()
                {
                    Id = KeyGenerator.GetGuidKey(),
                    OrderId = orderId,
                    OrderNo = order.OrderNo,
                    MemberId = _memberContainer.CurrentMember.Id,
                    MemberName = _memberContainer.CurrentMember.UserName,
                    CreateTime = DateTime.Now
                };
                if (!_currencyService.Create(model))
                {
                    throw new Exception("创建发货提醒失败，内部错误");
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 订单付款方式
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>

        public ActionResult PayType(Guid orderId)
        {
            //获得订单详情
            var order = _orderService.Load(orderId);
            if (order.OrderStatus != OrderStatus.PendingPayment)
            {
                return RedirectToAction("WebOrderList");
            }
            return View(order);
        }

        
    }
}