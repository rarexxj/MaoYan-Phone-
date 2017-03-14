using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BntWeb.Data.Services;
using BntWeb.Evaluate;
using BntWeb.Evaluate.Services;
using BntWeb.FileSystems.Media;
using BntWeb.MemberBase.Services;
using BntWeb.Mvc;
using BntWeb.OrderProcess.Models;
using BntWeb.OrderProcess.Services;
using BntWeb.OrderProcess.ViewModels;
using BntWeb.Security;
using BntWeb.Utility.Extensions;
using BntWeb.Validation;
using BntWeb.Wallet.Services;

namespace BntWeb.OrderProcess.Controllers
{
    public class WebEvaluateController : Controller
    {
        private readonly IEvaluateService _evaluateService;
        private readonly ICurrencyService _currencyService;
        private readonly IOrderService _orderService;
        private readonly IStorageFileService _storageFileService;
        private readonly IMemberContainer _memberContainer;
        private readonly IWalletService _walletService;
        private readonly IMemberService _memberService;
        public WebEvaluateController(IMemberContainer memberContainer, IEvaluateService evaluateService,
            ICurrencyService currencyService, IOrderService orderService,
            IStorageFileService storageFileService, IWalletService walletService, IMemberService memberService)
        {
            _walletService = walletService;
            _evaluateService = evaluateService;
            _currencyService = currencyService;
            _orderService = orderService;
            _storageFileService = storageFileService;
            _memberContainer = memberContainer;
            _memberService = memberService;
        }
        // GET: WebEvaluate
        /// <summary>
        /// 订单评价
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActionResult WebEvaluateList(Guid orderId)
        {
            var order = _orderService.Load(orderId);
         
            return View(order);
        }

        // GET: WebRefund
       /// <summary>
       /// 提交评价
       /// </summary>
       /// <param name="orderId"></param>
       /// <param name="evaluates"></param>
       /// <returns></returns>
       [MemberAuthorize]
        public ActionResult WebSubmitEvaluate(Guid orderId, WebEvaluateListModel evaluates)
        {
            var orderList = _orderService.Load(orderId);
            var result = new DataJsonResult();
            var currentMember = _memberContainer.CurrentMember;
          
            if (orderId.Equals(Guid.Empty))
                throw new Exception("订单Id不合法");
         
            var order = _currencyService.GetSingleById<Order>(orderId);
            if (order == null)
                throw new Exception("订单不存在");
            if (order.OrderStatus != OrderStatus.Completed)
                throw new Exception("订单未完成，不可以评价");
            if (!order.MemberId.Equals(currentMember.Id))
                throw new Exception("只能对自己的订单进行评价");
            //图片id 
            var imageIds = new List<Guid>();
            if (!string.IsNullOrWhiteSpace(evaluates.EvaluateImageIds))
           {
                var fileIds = evaluates.EvaluateImageIds.DeserializeJsonToList<ImageObj>();
               if (fileIds != null)
               {
                   foreach (var item in fileIds)
                   {
                       imageIds.Add(item.id.ToGuid());
                   }
               }
           }

           if (orderList != null)
           {
            List<Evaluate.Models.Evaluate> evaluateList = new List<Evaluate.Models.Evaluate>();
            foreach (var evaluateInfo in orderList.OrderGoods)
            {
                if (evaluates.GoodTasteScore > 5 || evaluates.GoodTasteScore < 0)
                    throw new Exception("商品评分不能大于5且不能小于0");
                Argument.ThrowIfNullOrEmpty(evaluates.Content, "评价内容");
                var goods = new OrderGoods();
                if (evaluateInfo.GoodType == GoodType.General)
                {
                    goods =
                        _currencyService.GetSingleByConditon<OrderGoods>(
                            x => x.OrderId == orderId && x.SingleGoodsId == evaluateInfo.SingleGoodsId);
                    if (goods == null)
                        throw new Exception("单品不存在");
                }
                else
                    {
                        goods =
                              _currencyService.GetSingleByConditon<OrderGoods>(
                                  x => x.OrderId == orderId);

                    }
             
                var refund = _currencyService.Count<OrderRefund>(
                        x => x.OrderId == orderId && x.SingleGoodsId == evaluateInfo.SingleGoodsId && x.RefundStatus == RefundStatus.Completed && x.ReviewResult == ReviewResult.Passed);
                if (refund > 0)
                    throw new Exception("单品已退款不能评价");
                var model = new Evaluate.Models.Evaluate()
                {
                    GoodTasteScore = evaluates.GoodTasteScore,
                    FreshMaterialScore=evaluates.FreshMaterialScore,
                    LogisticsScore = evaluates.LogisticsScore,
                    DesMatchScore= evaluates.DesMatchScore,
                    Content = evaluates.Content,
                    SourceId = goods.Id,
                    ExtentsionId = evaluateInfo.SingleGoodsId,
                    GoodId= goods.Id,
                    SourceType = "Order",
                    MemberId = currentMember.Id,
                    MemberName = currentMember.UserName,
                    ModuleKey = OrderProcessModule.Key,
                    ModuleName = OrderProcessModule.DisplayName,
                    IsAnonymity = false,
                    FilesId =imageIds
                };

                evaluateList.Add(model);
            }
            
            var orderGoods = _currencyService.Count<OrderGoods>(x => x.OrderId == orderId);
            var orderRefunds = _currencyService.Count<OrderRefund>(
                        x => x.OrderId == orderId && x.RefundStatus == RefundStatus.Completed && x.ReviewResult == ReviewResult.Passed);
            if (orderGoods - orderRefunds != evaluateList.Count)
                throw new BntWebCoreException("订单需评价商品数与提交评价数不符");
               string error;
            _evaluateService.CreateOrderEvaluates(evaluateList, order.Id);
                //提交评价 加五个积分
                _walletService.Deposit(currentMember.Id, Wallet.Models.WalletType.Integral, 5, "订单评价", out error);

              
            }
          
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        ///查看评价（评价详情）
        /// </summary>
        /// <returns></returns>
        [MemberAuthorize]
        public ActionResult LookEvaluate(Guid orderId)
        {
            var currentMember = _memberContainer.CurrentMember;
            //获得我的评价详情
            var evaluateList = _orderService.LoadOrderEvaluateList(orderId, currentMember.Id);
            if (evaluateList.Count != 0) { 
            foreach (var evaluate in evaluateList)
            {
              
                  var goods = new OrderGoods();
                if (evaluate.SingleGoodsId != Guid.Empty)
                {
                    goods =
                        _currencyService.GetSingleByConditon<OrderGoods>(
                            x => x.OrderId == orderId && x.SingleGoodsId == evaluate.SingleGoodsId);
                    if (goods != null)
                    {
                        var goodsImage = _storageFileService.GetFiles(goods.Id, OrderProcessModule.Key, "GoodsImage").FirstOrDefault();
                        evaluate.MainImage = goodsImage?.Simplified();
                    }
                }
                else
                {
                    goods =
                          _currencyService.GetSingleByConditon<OrderGoods>(
                              x => x.OrderId == orderId && x.GoodsId == evaluate.GoodsId);
                    if (goods != null)
                    {
                        var goodsImage = _storageFileService.GetFiles(goods.Id, OrderProcessModule.Key, "GoodsImage").FirstOrDefault();
                        evaluate.MainImage = goodsImage?.Simplified();
                    }
                }
               
            }
           
            ViewBag.EvaluteImages = _storageFileService.GetFiles(evaluateList[0].Id, EvaluateModule.Key, "Evaluate");
            }
            //获得会员头像  
            ViewBag.MemberAvatar =
                 _storageFileService.GetFiles(currentMember.Id.ToGuid(), "BntWeb-MemberCenter", "Avatar")
                     .FirstOrDefault(); ;
        
           
            ViewBag.EvaluateList = evaluateList;
            return View();
        }
 
        /// <summary>
        /// 删除评价
        /// </summary>
        /// <param name="evaluateId"></param>
        /// <returns></returns>
        public ActionResult Delete(string evaluateId)
        {
            var result = new DataJsonResult();

            _evaluateService.DeleteEvaluate(evaluateId.ToGuid());

            return Json(result);
        }
      
    }
}