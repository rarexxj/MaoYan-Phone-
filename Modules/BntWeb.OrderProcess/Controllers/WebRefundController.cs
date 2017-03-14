using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BntWeb.Data;
using BntWeb.Data.Services;
using BntWeb.FileSystems.Media;
using BntWeb.MemberBase.Services;
using BntWeb.Mvc;
using BntWeb.OrderProcess.Models;
using BntWeb.OrderProcess.Services;
using BntWeb.OrderProcess.ViewModels;
using BntWeb.Security;
using BntWeb.Validation;

namespace BntWeb.OrderProcess.Controllers
{
   
    public class WebRefundController : Controller
    {
        private readonly ICurrencyService _currencyService;
        private readonly IRefundService _refundService;
        private readonly IStorageFileService _storageFileService;
        private readonly IOrderService _orderService;
        private readonly IMemberContainer _memberContainer;
        public WebRefundController(IMemberContainer memberContainer, ICurrencyService currencyService, 
            IRefundService refundService, IStorageFileService storageFileService,
            IOrderService orderService)
        {
            _memberContainer = memberContainer;
            _currencyService = currencyService;
            _refundService = refundService;
            _storageFileService = storageFileService;
            _orderService = orderService;
        }

      
        public ActionResult RefundType(Guid orderId)
        {
            if (orderId != Guid.Empty)
                //获得订单详情
                ViewBag.OrderDetails = _orderService.Load(orderId);
            return View();
        }
        #region 修改前
        ///// <summary>
        ///// 退款类型
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult RefundType(Guid orderId, Guid goodsOrderId,decimal money)
        //{
        //    if(orderId!=Guid.Empty)
        //        //获得订单详情
        //    ViewBag.OrderDetails = _orderService.Load(orderId);


        //    ViewBag.goodsOrderId = goodsOrderId;
        //    ViewBag.money = money;

        //    return View();
        //}
        /// <summary>
        /// 退款类型
        /// </summary>
        /// <returns></returns>
        /// <summary>
        ///  申请退款
        /// </summary>
        /// <param name="goodsOrderId"></param>
        /// <param name="orderId"></param>
        /// <param name="reMoney"></param>
        /// <param name="refundType"></param>
        /// <param name="applyId"></param>
        /// <returns></returns>
        //public ActionResult AllRefund(Guid goodsOrderId, Guid orderId,decimal reMoney ,RefundType refundType,Guid applyId)
        //{
        //    if (applyId == Guid.Empty)  //添加退款申请
        //    {
        //        if (orderId != Guid.Empty)
        //            //获得订单详情
        //            ViewBag.OrderDetails = _orderService.Load(orderId);
        //        //退款类型：0是仅退款，1退款并退货；2仅换货
        //        if (refundType == Models.RefundType.OnlyRefund)
        //            ViewBag.RefundName = "仅退款";
        //        if (refundType == Models.RefundType.RefundAndReturn)
        //            ViewBag.RefundName = "退款并退货";
        //        if (refundType == Models.RefundType.OnlyReturn)
        //            ViewBag.RefundName = "仅换货";

        //        ViewBag.RefundType = refundType;
        //        ViewBag.goodsorderId = goodsOrderId;
        //        ViewBag.reMoney = reMoney;

        //        return View();
        //    }
        //    else  //修改退款申请
        //    {

        //        var model = _currencyService.GetSingleById<OrderRefund>(applyId);
        //        if (model == null)
        //            throw new BntWebCoreException("退款申请不存在");

        //        ViewBag.OrderDetails = _orderService.Load(model.OrderId);
        //        //退款类型：0是仅退款，1退款并退货；2仅换货
        //        if (model.RefundType == Models.RefundType.OnlyRefund)
        //            ViewBag.RefundName = "仅退款";
        //        if (model.RefundType == Models.RefundType.RefundAndReturn)
        //            ViewBag.RefundName = "退款并退货";
        //        if (model.RefundType == Models.RefundType.OnlyReturn)
        //            ViewBag.RefundName = "仅换货";

        //        //根据订单号 和退款单品ID 获取退款的订单商品信息
        //        var orderGoods =
        //            _currencyService.GetSingleByConditon<OrderGoods>(
        //                x =>
        //                    x.OrderId == model.OrderId &&
        //                    (x.SingleGoodsId == model.SingleGoodsId || x.GoodsId == model.SingleGoodsId));

        //        ViewBag.RefundType = model.RefundType;
        //        ViewBag.goodsorderId = orderGoods.Id;
        //        ViewBag.reMoney = model.RefundAmount;
        //        return View();

        //    }
        //}

        // GET: WebRefund
        #endregion
        public ActionResult AllRefund( Guid orderId,RefundType refundType, Guid applyId)
        {
            if (applyId == Guid.Empty)  //添加退款申请
            {
                if (orderId != Guid.Empty)
                    //获得订单详情
                    ViewBag.OrderDetails = _orderService.Load(orderId);
                //退款类型：0是仅退款，1退款并退货；2仅换货
                if (refundType == Models.RefundType.OnlyRefund)
                    ViewBag.RefundName = "仅退款";
                if (refundType == Models.RefundType.RefundAndReturn)
                    ViewBag.RefundName = "退款并退货";
                if (refundType == Models.RefundType.OnlyReturn)
                    ViewBag.RefundName = "仅换货";
                ViewBag.RefundType = refundType;
                return View();
            }
            else  //修改退款申请
            {

                var model = _currencyService.GetSingleById<OrderRefund>(applyId);
                if (model == null)
                    throw new BntWebCoreException("退款申请不存在");

                ViewBag.OrderDetails = _orderService.Load(model.OrderId);
                //退款类型：0是仅退款，1退款并退货；2仅换货
                if (model.RefundType == Models.RefundType.OnlyRefund)
                    ViewBag.RefundName = "仅退款";
                if (model.RefundType == Models.RefundType.RefundAndReturn)
                    ViewBag.RefundName = "退款并退货";
                if (model.RefundType == Models.RefundType.OnlyReturn)
                    ViewBag.RefundName = "仅换货";

                //根据订单号 和退款单品ID 获取退款的订单商品信息
                var orderGoods =
                    _currencyService.GetSingleByConditon<OrderGoods>(
                        x =>
                            x.OrderId == model.OrderId);
                ViewBag.RefundType = model.RefundType;
                ViewBag.goodsorderId = orderGoods.Id;
                ViewBag.reMoney = model.RefundAmount;
                return View();

            }
        }


        /// <summary>
        /// 申请退款form表单提交 从订单详情里过来
        /// </summary>
        /// <param name="apply"></param>
        /// <returns></returns>
        [MemberAuthorize]
        public ActionResult WebApplyRefund(WebRefundModel apply)
        {
            var result = new DataJsonResult();
            try
            {
                var currentMember = _memberContainer.CurrentMember;
                Argument.ThrowIfNullOrEmpty(apply.Reason, "退款原因");
                if (apply.RefundAmount <= 0)
                    throw new BntWebCoreException("退款金额不能小于等于0");

                if (apply.OrderId.Equals(Guid.Empty))
                    throw new BntWebCoreException("订单Id不合法");

                var order = _currencyService.GetSingleById<Order>(apply.OrderId);
                Argument.ThrowIfNullOrEmpty(order.ToString(), "订单不存在");


                if (order.OrderStatus == OrderStatus.WaitingForDelivery && apply.RefundType == Models.RefundType.RefundAndReturn)
                    throw new Exception("订单未发货，不能申请退款并退货");

                if (order.OrderStatus != OrderStatus.WaitingForDelivery && order.OrderStatus != OrderStatus.WaitingForReceiving)
                    throw new Exception("当前订单状态不能申请退款");
                if (!order.MemberId.Equals(currentMember.Id))
                    throw new Exception("只能对自己的订单申请退款");

                var singleGoods = _currencyService.GetList<OrderGoods>(x => x.OrderId == apply.OrderId);
                if (singleGoods == null)
                    throw new Exception("商品不存在");
                var maxRefundAmount = order.GoodsAmount - order.CouponMoney;

                if (apply.RefundAmount > maxRefundAmount)
                    throw new BntWebCoreException("退款金额不能大于单品实付金额");

                foreach (var item in singleGoods)
                {
                    if (item.RefundStatus != OrderRefundStatus.NoRefund)
                        throw new Exception("已申请退款不可重复申请");
                    var model = new OrderRefund()
                    {
                        Id = KeyGenerator.GetGuidKey(),
                        OrderId = apply.OrderId,
                        SingleGoodsId = item.SingleGoodsId == Guid.Empty ? item.GoodsId : item.SingleGoodsId,  //单品Id 如果订单商品表的单品Id为Guid.Empty 就取商品ID
                        RefundType = apply.RefundType,
                        RefundAmount = apply.RefundAmount,
                        Reason = apply.Reason

                    };

                    _refundService.CreateOrderRefund(model);
                    //退款上传凭证（图片）
                    if (apply.RefundImageIds != null)
                    {
                        _storageFileService.ReplaceFile(model.Id, OrderProcessModule.Key, OrderProcessModule.DisplayName, apply.RefundImageIds,
                            "RefundImages");
                    }
                }
            }
            //更改订单状态
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
            }
            return Json(result);
        }
        /// <summary>
        /// 修改退款申请
        /// </summary>
        /// <param name="apply"></param>
        /// <returns></returns>
        [MemberAuthorize]
        public ActionResult WebUpdateApply(RefundApplyUpdateModel apply)
        {
            var result = new DataTableJsonResult();

            Argument.ThrowIfNullOrEmpty(apply.Reason, "退款原因");
            Argument.ThrowIfNullOrEmpty(apply.Reason, "退款原因");

            if (apply.RefundAmount <= 0)
                throw new BntWebCoreException("退款金额不能小于等于0");

            if (apply.Id.Equals(Guid.Empty))
                throw new BntWebCoreException("退款申请Id不合法");

            var oldApply = _currencyService.GetSingleById<OrderRefund>(apply.Id);
            if (oldApply == null)
                throw new BntWebCoreException("退款申请不存在");
            if (oldApply.RefundStatus == RefundStatus.Completed)
                throw new BntWebCoreException("退款已完成不得修改");
            var order = _currencyService.GetSingleById<Order>(oldApply.OrderId);
            if (order == null)
                throw new BntWebCoreException("订单不存在");
            if (order.OrderStatus == OrderStatus.WaitingForDelivery && apply.RefundType == Models.RefundType.RefundAndReturn)
                throw new BntWebCoreException("订单未发货，不能申请退款并退货");
            if (order.OrderStatus != OrderStatus.WaitingForDelivery && order.OrderStatus != OrderStatus.WaitingForReceiving)
                throw new BntWebCoreException("当前订单状态不能申请退款");

            var singleGoods = _currencyService.GetSingleByConditon<OrderGoods>(x => x.OrderId == oldApply.OrderId && x.SingleGoodsId == oldApply.SingleGoodsId);
            if (singleGoods == null)
                throw new BntWebCoreException("商品不存在");

            var maxRefundAmount = singleGoods.Price * singleGoods.Quantity;
            if (order.IntegralMoney > 0)
            {
                maxRefundAmount += order.IntegralMoney * (singleGoods.Price * singleGoods.Quantity / order.GoodsAmount);
            }
            if (apply.RefundAmount > maxRefundAmount)
                throw new BntWebCoreException("退款金额不能大于单品实付金额");

            oldApply.RefundType = apply.RefundType;
            oldApply.RefundAmount = apply.RefundAmount;
            oldApply.Reason = apply.Reason;

            _currencyService.Update<OrderRefund>(oldApply);
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 撤销退款申请
        /// </summary>
        /// <param name="applyId"></param>
        /// <returns></returns>
        [MemberAuthorize]
        public ActionResult WebRevokeApply(Guid applyId)
        {
            var result = new DataJsonResult();

            try
            {
                if (applyId.Equals(Guid.Empty))
                    throw new BntWebCoreException("退款申请Id不合法");

                var refundApply = _currencyService.GetSingleById<OrderRefund>(applyId);
                if (refundApply == null)
                    throw new BntWebCoreException("退款申请不存在");

                if (refundApply.RefundStatus != RefundStatus.Applying)
                    throw new BntWebCoreException("退款申请已处理不得撤销");

                _refundService.RevokeOrderRefund(refundApply);
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;

            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取订单商品退款详情
        /// </summary>
        /// <param name="orderId">订单id</param>
     
        /// <returns></returns>
        [MemberAuthorize]
        public ActionResult WebApplyInfo(Guid orderId)
        {
            var apply =
                 _currencyService.GetList<OrderRefund>(
                     x => x.OrderId == orderId).OrderByDescending(x => x.CreateTime).Select(o => new RefundApplyInfoModel(o)).FirstOrDefault();

            var orderGoods = _currencyService.GetList<OrderGoods>(x => x.OrderId == orderId);
     
            if (apply != null)
            {
                var refundProof = _storageFileService.GetFiles(apply.Id, OrderProcessModule.Key, "RefundImages");
                ViewBag.refundProof = refundProof;
            }
         
            ViewBag.apply = apply;
            ViewBag.orderGoods = orderGoods;
            ViewBag.OrderDetails = _orderService.Load(orderId);

            return View();

        }
          
      
    }
}