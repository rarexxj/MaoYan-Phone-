using System;
using System.Linq.Expressions;
using System.Transactions;
using System.Web.Mvc;
using BntWeb.Data;
using BntWeb.Data.Services;
using BntWeb.Logging;
using BntWeb.Logistics.Models;
using BntWeb.OrderProcess.Models;
using BntWeb.Mvc;
using BntWeb.OrderProcess.Services;
using BntWeb.Security;
using BntWeb.Utility.Extensions;
using BntWeb.Web.Extensions;

namespace BntWeb.OrderProcess.Controllers
{
    public class AdminController : Controller
    {
        private readonly ICurrencyService _currencyService;
        private readonly IOrderService _orderService;
        private readonly IUserContainer _userContainer;

        public AdminController(ICurrencyService currencyService, IOrderService orderService, IUserContainer userContainer)
        {
            _currencyService = currencyService;
            _orderService = orderService;
            _userContainer = userContainer;

            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewOrderKey })]
        public ActionResult List()
        {
            return View();
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewOrderKey })]
        public ActionResult ListOnPage()
        {
            var result = new DataTableJsonResult();

            //取参数值
            int draw, pageIndex, pageSize, totalCount;
            string sortColumn;
            bool isDesc;
            Request.GetDatatableParameters(out draw, out pageIndex, out pageSize, out sortColumn, out isDesc);
            result.draw = draw;

            //取查询条件
            var orderNo = Request.Get("extra_search[OrderNo]");
            var checkOrderNo = string.IsNullOrWhiteSpace(orderNo);

            var consignee = Request.Get("extra_search[Consignee]");
            var checkConsignee = string.IsNullOrWhiteSpace(consignee);
            var phone = Request.Get("extra_search[Phone]");
            var checkPhone = string.IsNullOrWhiteSpace(phone);
            
            var orderStatus = Request.Get("extra_search[OrderStatus]");
            var checkOrderStatus = string.IsNullOrWhiteSpace(orderStatus);
            var orderStatusInt = orderStatus.To<int>();

            var refundStatus = Request.Get("extra_search[RefundStatus]");
            var checkRefundStatus = string.IsNullOrWhiteSpace(refundStatus);
            var refundStatusInt = refundStatus.To<int>();

            var payStatus = Request.Get("extra_search[PayStatus]");
            var checkPayStatus = string.IsNullOrWhiteSpace(payStatus);
            var payStatusInt = payStatus.To<int>();

            var shippingStatus = Request.Get("extra_search[ShippingStatus]");
            var checkShippingStatus = string.IsNullOrWhiteSpace(shippingStatus);
            var shippingStatusInt = shippingStatus.To<int>();

            var createTimeBegin = Request.Get("extra_search[CreateTimeBegin]");
            var checkCreateTimeBegin = string.IsNullOrWhiteSpace(createTimeBegin);
            var createTimeBeginTime = createTimeBegin.To<DateTime>();

            var createTimeEnd = Request.Get("extra_search[CreateTimeEnd]");
            var checkCreateTimeEnd = string.IsNullOrWhiteSpace(createTimeEnd);
            var createTimeEndTime = createTimeEnd.To<DateTime>();

            Expression<Func<Order, bool>> expression =
                l => (checkOrderNo || l.OrderNo.Contains(orderNo)) &&
                     (checkConsignee || l.Consignee.Contains(consignee)) &&
                     (checkPhone || l.MemberName.Contains(phone)) &&
                     l.OrderStatus != OrderStatus.Deleted &&
                     (checkOrderStatus || (int)l.OrderStatus == orderStatusInt) &&
                     (checkRefundStatus || (int)l.RefundStatus == refundStatusInt) &&
                     (checkPayStatus || (int)l.PayStatus == payStatusInt) &&
                     (checkShippingStatus || (int)l.ShippingStatus == shippingStatusInt) &&
                     (checkCreateTimeBegin || l.CreateTime >= createTimeBeginTime) &&
                     (checkCreateTimeEnd || l.CreateTime <= createTimeEndTime);


            //分页查询
            var list = _currencyService.GetListPaged<Order>(pageIndex, pageSize, expression, out totalCount, new OrderModelField { PropertyName = sortColumn, IsDesc = isDesc });

            result.data = list;
            result.recordsTotal = totalCount;
            result.recordsFiltered = totalCount;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewOrderKey })]
        public ActionResult Detail(Guid orderId)
        {
            var order = _orderService.Load(orderId);

            ViewBag.Shippings = _currencyService.GetList<Shipping>(s => s.Status == Logistics.Models.ShippingStatus.Enabled).ToJson();

            return View(order);
        }

        /// <summary>
        /// 关闭订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="memo"></param>
        /// <returns></returns>
        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageOrderKey })]
        public ActionResult Close(Guid orderId, string memo)
        {
            var result = new DataJsonResult();
            if (!_orderService.ChangeOrderStatus(orderId, OrderStatus.Closed, null, null, null, memo))
            {
                result.ErrorCode = "关闭订单出现异常错误";
            }
            return Json(result);
        }

        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="shippingId"></param>
        /// <param name="shippingName"></param>
        /// <param name="shippingCode"></param>
        /// <param name="shippingNo"></param>
        /// <returns></returns>
        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageOrderKey })]
        public ActionResult Delivery(Guid orderId, Guid shippingId, string shippingName, string shippingCode, string shippingNo)
        {
            var result = new DataJsonResult();
            var order = _orderService.Load(orderId);
            if (order.OrderStatus != OrderStatus.WaitingForDelivery)
            {
                result.ErrorMessage = "订单状态不合法";
            }
            else
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (_orderService.SetShippingInfo(orderId, shippingId, shippingName, shippingCode, shippingNo))
                        if (_orderService.ChangeOrderStatus(orderId, OrderStatus.WaitingForReceiving, null, Models.ShippingStatus.Shipped, null, "订单发货"))
                            //提交
                            scope.Complete();
                }

                //删除订单发货提醒记录
                _currencyService.DeleteByConditon<OrderDeliveryReminder>(x => x.OrderId == orderId);
            }

            return Json(result);
        }

        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="shippingId"></param>
        /// <param name="shippingName"></param>
        /// <param name="shippingCode"></param>
        /// <param name="shippingNo"></param>
        /// <returns></returns>
        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageOrderKey })]
        public ActionResult ChangeShipping(Guid orderId, Guid shippingId, string shippingName, string shippingCode, string shippingNo)
        {
            var result = new DataJsonResult();
            var order = _orderService.Load(orderId);
            if (order.OrderStatus != OrderStatus.WaitingForReceiving)
            {
                result.ErrorMessage = "订单状态不合法";
            }
            else
            {
                if (!_orderService.SetShippingInfo(orderId, shippingId, shippingName, shippingCode, shippingNo))
                {
                    result.ErrorMessage = "物流信息更新失败";
                }
            }

            return Json(result);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageOrderKey })]
        public ActionResult ChangePrice(Guid orderId, Guid orderGoodsId, decimal goodsPrice)
        {
            var result = new DataJsonResult();
            try
            {
                if (!_orderService.ChangePrice(orderId, orderGoodsId, goodsPrice))
                {
                    result.ErrorMessage = "异常错误，修改失败";
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
            }


            return Json(result);
        }

        /// <summary>
        /// 平台确认收货
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageOrderKey })]
        public ActionResult PayAndComplate(Guid orderId)
        {
            var result = new DataJsonResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var order = _currencyService.GetSingleById<Order>(orderId);

                    if (order.OrderStatus != OrderStatus.WaitingForReceiving)
                        throw new BntWebCoreException("订单状态不合法，无法确认收货");

                    order.PayStatus = PayStatus.Paid;
                    order.PayTime = DateTime.Now;
                    order.UnpayFee = 0;
                    if (!_currencyService.Update(order))
                        throw new Exception("收货失败");

                    var orderAction = new OrderAction
                    {
                        Id = KeyGenerator.GetGuidKey(),
                        OrderId = order.Id,
                        Memo = "收货确认-已付款",
                        CreateTime = DateTime.Now,
                        OrderStatus = OrderStatus.Completed,
                        PayStatus = PayStatus.Paid,
                        ShippingStatus = order.ShippingStatus,
                        EvaluateStatus = order.EvaluateStatus,
                        UserId = _userContainer.CurrentUser.Id,
                        UserName = _userContainer.CurrentUser.UserName
                    };
                    _currencyService.Create(orderAction);

                    if (!_orderService.ChangeOrderStatus(orderId, OrderStatus.Completed, null, null, null, "平台确认收货"))
                    {
                        throw new Exception("确认收货失败，可能订单状态已经变更");
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
            }
            return Json(result);
        }
        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageOrderKey })]
        public ActionResult Delete(Guid orderId)
        {
            var result = new DataJsonResult();
            if (orderId.Equals(Guid.Empty))
                throw new Exception("订单Id为空");

            var order = _currencyService.GetSingleById<Order>(orderId);
           
            if (order.OrderStatus != OrderStatus.Closed && order.OrderStatus != OrderStatus.Completed)
                throw new Exception("只能删除关闭或完成的订单");

            order.OrderStatus = OrderStatus.Deleted;
            if (!_currencyService.Update(order))
            {
                throw new Exception("删除失败，可能订单已经被删除");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        } 

    }
}