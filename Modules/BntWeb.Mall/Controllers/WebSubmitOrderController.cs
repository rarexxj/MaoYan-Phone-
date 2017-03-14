using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using BntWeb.Carousel.Services;
using BntWeb.Config.Models;
using BntWeb.Core.SystemSettings.Models;
using BntWeb.Coupon.Models;
using BntWeb.Coupon.Services;
using BntWeb.Data.Services;
using BntWeb.FileSystems.Media;
using BntWeb.Logging;
using BntWeb.Logistics.Services;
using BntWeb.Mall.ApiModels;
using BntWeb.Mall.Models;
using BntWeb.Mall.Services;
using BntWeb.MemberBase.Models;
using BntWeb.MemberBase.Services;
using BntWeb.Mvc;
using BntWeb.OrderProcess.Models;
using BntWeb.OrderProcess.Services;
using BntWeb.Security;
using BntWeb.Services;
using BntWeb.Utility.Extensions;
using BntWeb.Wallet.Models;
using BntWeb.Wallet.Services;
using BntWeb.WebApi.Models;


namespace BntWeb.Mall.Controllers
{
    public class WebSubmitOrderController : Controller
    {
        private readonly ICurrencyService _currencyService;
        private readonly IGoodsService _goodsService;
        private readonly IGoodsCategoryService _goodsCategoryService;
        private readonly IStorageFileService _storageFileService;
        private readonly IConfigService _configService;
        private const string MainImage = "MainImage";
        private readonly ICarouselService _carouselService;
        private readonly IMemberContainer _memberContainer;
        private readonly IWalletService _walletService;
        private readonly ICouponService _couponService;
        private readonly IOrderService _orderService;
        private readonly IShippingAreaService _shippingAreaService;
        public WebSubmitOrderController(ICurrencyService currencyService, IGoodsService goodsService,
            IGoodsCategoryService goodsCategoryService, IStorageFileService storageFileService,
             IConfigService configService,ICarouselService carouselService, 
             IMemberContainer memberContainer, IWalletService walletService,
             ICouponService couponService, IOrderService orderService,
             IShippingAreaService shippingAreaService)
        {
            _currencyService = currencyService;
            _goodsService = goodsService;
            _goodsCategoryService = goodsCategoryService;
            _storageFileService = storageFileService;
            _configService = configService;
            _carouselService = carouselService;
            _memberContainer = memberContainer;
            _walletService = walletService;
            _couponService = couponService;
            _orderService = orderService;
            _shippingAreaService = shippingAreaService;
            Logger = NullLogger.Instance;

        }

        public ILogger Logger { get; set; }
     
        [MemberAuthorize]
        public ActionResult ConfirmOrderList(string myCartsId = null, string purIds = null, string optIds = null,string singleIds=null)
        {

            //获得当前用户
            var currentMember = _memberContainer.CurrentMember;
            //获得当前用户的默认地址
            ViewBag.DefaultAddress = _currencyService.GetSingleByConditon<MemberAddress>(a => a.MemberId == currentMember.Id && a.IsDefault);
            //将前台的json数组转换为LIst

            //从立即购买跳过来的
            if (!string.IsNullOrWhiteSpace(singleIds))
            {
           
                var nowGoodObj = singleIds.DeserializeJsonToList<SingGoodsModel>();
                var singleGoods = new SingleGoods();
                if (nowGoodObj != null)
                {
                    singleGoods = _goodsService.LoadFullSingleGoods(nowGoodObj[0].id.ToGuid());
                

                if (singleGoods == null)
                    throw new Exception("无该单品信息");
                    //单品主图
                    singleGoods.Image = _storageFileService.GetFiles(singleGoods.Id, MallModule.Instance.InnerKey, singleGoods.Goods.Id.ToString()).FirstOrDefault()?.Simplified(); ;
                    //单品数量
                    singleGoods.Goods.Quantity = nowGoodObj[0].quantity;
                //单品
                ViewBag.Goods = singleGoods;
                }
                //加价购商品
                if (!string.IsNullOrWhiteSpace(purIds))
                {
                    var purGoodObj= purIds.DeserializeJsonToList<PurGoodsModel>();
                    var purchaseGoods = _currencyService.GetSingleById<Goods>(purGoodObj[0].id.ToGuid());
                    purchaseGoods.MainImage= _storageFileService.GetFiles(purchaseGoods.Id, MallModule.Instance.InnerKey,"MainImage").FirstOrDefault()?.Simplified();
                   
                    //加价购商品
                    ViewBag.PurchaseGoods = purchaseGoods;
                }
            }
            //从购物车过来
          if (!string.IsNullOrWhiteSpace(myCartsId))
           {
                var nowGoodObj = myCartsId.DeserializeJsonToList<CartsGoodsModel>();
               var cartGood = new List<Cart>();
               foreach (var item in nowGoodObj)
               {
                   var cart = _currencyService.GetSingleById<Cart>(item.id.ToGuid());
                   cart.Quantity = item.quantity;
                    cartGood.Add(cart);
               }
                //购物车商品  前台循环用 

                ViewBag.MyCarts = cartGood.Select(me => new ListCartModel(me)).ToList();
             
                //自选商品
                if (!string.IsNullOrWhiteSpace(optIds))
                {
                    var opts = optIds.DeserializeJsonToList<OptGoodsModel>();
                    ViewBag.OptList = opts;
                   
                }
            }
            //获得自选商品
            var optionalGoods = _goodsService.GetOptionalGoods();
            ViewBag.OptionalGoods = optionalGoods;
         
            //选择其他收货地址
            ViewBag.OtherAddress = _currencyService.GetList<MemberAddress>(a => a.MemberId == currentMember.Id && !a.IsDefault);

            //获得我的可用积分
            ViewBag.MyIntenal = _walletService.GetWalletByMemberId(currentMember.Id, WalletType.Integral);
            //获得我的可使用的优惠券个数
            ViewBag.MyCouponCount = _currencyService.GetList<CouponRelation>(a => a.MemberId == currentMember.Id && a.Status == CouponStatus.Unused).Count;

          
           
            return View();
        }
        #region

        #endregion
        /// <summary>
        /// 我的可用优惠券
        /// </summary>
        /// <returns></returns>
        [MemberAuthorize]
        public ActionResult MyCouponsList()
        {
            var result = new DataJsonResult();
          
            var currenceUser = _memberContainer.CurrentMember;
            var coupon= _couponService.GetMyCouponList(currenceUser.Id, CouponStatus.Unused);
            result.Data = coupon;

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 订单提交
        /// </summary>
        /// <param name="submitOrder"></param>
        /// <returns></returns>
        [MemberAuthorize]
        public ActionResult Submit(SubmitOrderModel submitOrder)
        {
            var result = new DataJsonResult();
            var currencemember = _memberContainer.CurrentMember;

            //获得收货地址
            var myAddress = _currencyService.GetSingleById<MemberAddress>(submitOrder.AddressId);
            var coupon =new Coupon.Models.Coupon();
            if (submitOrder.CouponId != Guid.Empty)
                coupon= _currencyService.GetSingleById<Coupon.Models.Coupon>(submitOrder.CouponId);
        
            #region 订单信息
         
            if (string.IsNullOrWhiteSpace(submitOrder.NowBuyGoods)&& string.IsNullOrWhiteSpace(submitOrder.CartGoods))
            {
                throw new Exception("订单中没有商品！");
            }
           
            var order = new Order
            {
                
                MemberId = currencemember.Id,
                MemberName = currencemember.UserName,
                OrderStatus = OrderStatus.PendingPayment,
                ShippingStatus = ShippingStatus.Unshipped,
                PayStatus = PayStatus.Unpaid,
                EvaluateStatus = EvaluateStatus.NotEvaluated,
                Consignee = myAddress.Contacts,
                Province= myAddress?.Province,
                City= myAddress?.City,
                District= myAddress?.District,
                PCDS = myAddress?.RegionName,
                Address = myAddress?.Address,
                Tel = myAddress?.Phone,
                BestTime = submitOrder.BestTime,
                Memo = submitOrder.Memo,
                Integral =submitOrder.Integral.To<int>(),
                CouponMoney=coupon.Money,
                NeedShipping = true,
                PayOnline = true,
                CreateTime = DateTime.Now,
                ModuleKey = MallModule.Key,
                ModuleName = MallModule.DisplayName,
                SourceType = "Mall"
            };
            #endregion
            //加载商品
            #region 加载商品(立即购买和购物车商品)

            //立即购买商品
            var orderGoods = new List<OrderGoods>();
            if (!string.IsNullOrWhiteSpace(submitOrder.NowBuyGoods))
            {
                var buyNow = submitOrder.NowBuyGoods.DeserializeJsonToList<NowBuyGoodsModel>();
                foreach (var item in buyNow)
                {
                   //判断有没有加价购商品
                  //单品Id（item.id）为空说明是加价购商品
                    if (item.id== null || item.id.ToGuid()==Guid.Empty)
                    {
                       
                        var isPurGood = _currencyService.GetSingleById<Goods>(item.goodid.ToGuid());
                        isPurGood.PurImage = _storageFileService.GetFiles(isPurGood.Id, MallModule.Key, "MainImage").FirstOrDefault()?.Simplified();
                        orderGoods.Add(new OrderGoods
                        {
                            GoodsId = isPurGood.Id,
                            GoodsNo = isPurGood.GoodsNo,
                            GoodsName = isPurGood.Name,
                            Unit ="个",
                            Price = isPurGood.ShopPrice,
                            Quantity = item.quantity,
                            IsReal = true,
                            GoodsImage = isPurGood.PurImage,
                            GoodType= GoodType.PurchasePrice,
                            FreeShipping = isPurGood.FreeShipping
                        });
                    }
                    else
                    {//单品
                        var singleGoods = _goodsService.LoadFullSingleGoods(item.id.ToGuid());
                        if (singleGoods?.Goods == null || singleGoods.Goods.Status != GoodsStatus.InSale)
                            throw new WebApiInnerException("0002", "存在非法或者失效的商品");
                        if (singleGoods.Stock < item.quantity)
                            throw new WebApiInnerException("0003", $"【{singleGoods.Goods.Name}】库存不足");
                        var goodsImage = _storageFileService.GetFiles(singleGoods.Id, MallModule.Instance.InnerKey, singleGoods.Goods.Id.ToString()).FirstOrDefault()?.Simplified() ??
                               _storageFileService.GetFiles(singleGoods.GoodsId, MallModule.Key, "MainImage").FirstOrDefault()?.Simplified();

                        orderGoods.Add(new OrderGoods
                        {
                            GoodsId = singleGoods.Goods.Id,
                            GoodsNo = singleGoods.Goods.GoodsNo,
                            GoodsName = singleGoods.Goods.Name,
                            SingleGoodsId = singleGoods.Id,
                            SingleGoodsNo = singleGoods.SingleGoodsNo,
                            GoodsAttribute = string.Join(",", singleGoods.Attributes.Select(a => a.AttributeValue).ToArray()),
                            Unit = singleGoods.Unit,
                            Price = singleGoods.Price,
                            Quantity = item.quantity,
                            IsReal = true,
                            GoodsImage = goodsImage,
                            FreeShipping = singleGoods.Goods.FreeShipping
                        });
                    }
                }
            }
            #endregion
         
            #region 自选商品
          
            //将前台的json数组转换为LIst
            var optList = submitOrder.OptGoods.DeserializeJsonToList<OptGoodsModel>();
            if (optList != null)
            {//将自选商品加到订单商品里
                foreach (var item in optList)
                {
                    var optGood = _currencyService.GetSingleById<Goods>(item.id.ToGuid());
                    if (optGood == null)
                        throw new Exception("不存在此自选商品！");
                    var goodsImage = _storageFileService.GetFiles(optGood.Id, MallModule.Instance.InnerKey, optGood.Id.ToString()).FirstOrDefault()?.Simplified() ??
                               _storageFileService.GetFiles(optGood.Id, MallModule.Key, "MainImage").FirstOrDefault()?.Simplified();
                    orderGoods.Add(new OrderGoods
                    {
                        GoodsId = optGood.Id,
                        GoodsNo = optGood.GoodsNo,
                        GoodsName = optGood.Name,
                        Unit ="个",
                        Price = optGood.ShopPrice,
                        Quantity =item.quantity,
                        GoodsAttribute=submitOrder.OptionalMemo,
                        IsReal = true,
                        GoodsImage = goodsImage,
                        GoodType = GoodType.Optional,
                        FreeShipping = optGood.FreeShipping
                    });
                }
            }
            #endregion
            //查省和市的id
            //var distinct = _currencyService.GetSingleByConditon<District>(a => a.FullName == myAddress.City);
            //计算商品总价
            order.GoodsAmount = orderGoods.Sum(g => g.Price * g.Quantity);
            //计算物流费用
            order.ShippingFee = orderGoods.Any(g => g.FreeShipping) ? 0 : _shippingAreaService.GetAreaFreight(myAddress?.Province, myAddress?.City);
            //计算订单总价
            order.OrderAmount = order.GoodsAmount + order.ShippingFee;
            //事务控制
            using (TransactionScope scope = new TransactionScope())
            {
                //优惠券抵用
                var couponrelation =
                    _currencyService.GetSingleByConditon<CouponRelation>(
                        a => a.MemberId == currencemember.Id && a.CouponId == coupon.Id);
                if (couponrelation != null)
                {
                    couponrelation.Status = CouponStatus.Used;
                    //优惠券使用记录
                    var couponUse = new CouponUseRecord();
                    couponUse.MemberId = currencemember.Id;
                    couponUse.ModuleKey = MallModule.Key;
                    couponUse.SourceId = order.Id;
                    couponUse.CouponRelationId = couponrelation.Id;
                    _couponService.AddCouponUseRecord(couponUse); 
                    //更新优惠券关系表的 状态
                    _currencyService.Update<CouponRelation>(couponrelation);
                }

                //计算积分折抵
                if (order.Integral > 0)
                {
                    var systemConfig = _configService.Get<SystemConfig>();
                    var integralWallet = _walletService.GetWalletByMemberId(order.MemberId,
                        Wallet.Models.WalletType.Integral);
                    if (integralWallet == null || integralWallet.Available < order.Integral)
                        throw new WebApiInnerException("0003", "可用积分不足");
                   
                    else
                    {
                        var money = order.GoodsAmount - coupon.Money + order.ShippingFee;
                        if (money <= 0)
                        {
                            order.PayFee = 0;
                        }
                        else
                        {
                            if (money < order.Integral)
                            {
                                order.Integral = (int) money;
                                string error;
                                _walletService.Draw(order.MemberId, Wallet.Models.WalletType.Integral, order.Integral,
                                    "抵扣订单", out error);
                                if (string.IsNullOrWhiteSpace(error))
                                {
                                    order.IntegralMoney = (decimal)order.Integral / 100 * systemConfig.DiscountRate ;
                                }
                            }
                            else
                            {
                                //计算剩余积分总共可以抵扣多少钱
                                var maxDiscountMoney = (decimal) integralWallet.Available/100*systemConfig.DiscountRate;
                                if (maxDiscountMoney >= order.GoodsAmount)
                                {
                                    order.Integral = (int) (order.GoodsAmount/systemConfig.DiscountRate*100);
                                }
                                string error;
                                _walletService.Draw(order.MemberId, Wallet.Models.WalletType.Integral, order.Integral,
                                    "抵扣订单", out error);
                                if (string.IsNullOrWhiteSpace(error))
                                {
                                    order.IntegralMoney = (decimal) order.Integral/100*systemConfig.DiscountRate;
                                }
                            }
                            
                        }

                    }
                }
                
                    //计算需要支付的费用
                    order.PayFee = order.GoodsAmount - order.IntegralMoney - coupon.Money + order.ShippingFee;
               
              
                if (order.PayFee == 0)
                {
                    order.OrderStatus = OrderStatus.WaitingForDelivery;
                    order.PayStatus = PayStatus.Paid;
                    order.PayTime = DateTime.Now;
                }
                else
                {
                    order.UnpayFee = order.PayFee;
                }
                _orderService.SubmitOrder(order, orderGoods);

                //清理购物车相关数据
                if (!string.IsNullOrWhiteSpace(submitOrder.CartGoods))
                {
                    var mycarts = submitOrder.CartGoods.DeserializeJsonToList<CartsGoodsModel>();
                
                foreach (var item in mycarts)

                {
                    var isOther = _currencyService.GetSingleById<Goods>(item.id.ToGuid());
                    if (isOther != null)
                    {
                        _currencyService.DeleteByConditon<Cart>(
                            c => c.MemberId.Equals(currencemember.Id) && c.SingleGoodsId.ToString().Equals(item.id));
                    }
                    else
                    {
                            _currencyService.DeleteByConditon<Cart>(
                                c => c.MemberId.Equals(currencemember.Id) && c.GoodsId.ToString().Equals(item.id));
                        }
                }
                }
                //提交
                scope.Complete();
            }
            result.Data = order.Id;
            return Json(result);
        }
        /// <summary>
        /// 积分兑换
        /// </summary>
        /// <param name="goodId"></param>
        /// <returns></returns>
        [MemberAuthorize]
        public ActionResult ExchangeOrder(Guid goodId)
        {
            //获得当前用户 
            var currentMember = _memberContainer.CurrentMember;
            //获得当前用户的默认地址
            ViewBag.DefaultAddress = _currencyService.GetSingleByConditon<MemberAddress>(a => a.MemberId == currentMember.Id && a.IsDefault);
            //获得我的可用积分
            ViewBag.MyIntenal = _walletService.GetWalletByMemberId(currentMember.Id, WalletType.Integral);
            //选择其他收货地址
            ViewBag.OtherAddress = _currencyService.GetList<MemberAddress>(a => a.MemberId == currentMember.Id && !a.IsDefault);
            var exchangegood = _currencyService.GetList<Goods>(a=>a.Id== goodId).Select(a => new ListGoodsModel(a)).ToList();
            ViewBag.ExchangeGood = exchangegood;
            return View();
        
        }
        /// <summary>
        /// 积分换购提交订单
        /// </summary>
        /// <param name="submitOrder"></param>
        /// <returns></returns>
        public ActionResult ConfirmExchange(SubmitOrderModel submitOrder)
        {
            var result = new DataJsonResult();
            var currencemember = _memberContainer.CurrentMember;

            //获得收货地址
            var myAddress = _currencyService.GetSingleById<MemberAddress>(submitOrder.AddressId);
            #region 订单信息

            if (submitOrder.GoodId==Guid.Empty)
            {
                throw new Exception("订单中没有商品！");
            }

            var order = new Order
            {
                MemberId = currencemember.Id,
                MemberName = currencemember.UserName,
                OrderStatus = OrderStatus.PendingPayment,
                ShippingStatus = ShippingStatus.Unshipped,
                PayStatus = PayStatus.Unpaid,
                EvaluateStatus = EvaluateStatus.NotEvaluated,
                Consignee = submitOrder.Consignee,
                Province = myAddress?.Province,
                City = myAddress?.City,
                District = myAddress?.District,
                PCDS = myAddress?.RegionName,
                Address = myAddress?.Address,
                Tel = myAddress?.Phone,
                BestTime = submitOrder.BestTime,
                Memo = submitOrder.Memo,
                Integral = submitOrder.Integral.To<int>(),
                NeedShipping = true,
                PayOnline = true,
                CreateTime = DateTime.Now,
                ModuleKey = MallModule.Key,
                ModuleName = MallModule.DisplayName,
                Type= OrderType.Exchange,
                SourceType = "Mall"
            };
            #endregion
         
            #region 积分兑换商品
            var orderGoods = new List<OrderGoods>();
           
                    //判断积分换购商品
                    var integralGood = _currencyService.GetSingleById<Goods>(submitOrder.GoodId);
                    if (integralGood != null)
                    {
                    integralGood.PurImage = _storageFileService.GetFiles(integralGood.Id, MallModule.Key, "MainImage").FirstOrDefault()?.Simplified();
                        orderGoods.Add(new OrderGoods
                        {
                            GoodsId = integralGood.Id,
                            GoodsNo = integralGood.GoodsNo,
                            GoodsName = integralGood.Name,
                            Unit = "个",
                            Price = integralGood.ExchangeIntegral,
                            Quantity =submitOrder.Quality.To<int>(),
                            IsReal = true,
                            GoodsImage = integralGood.PurImage,
                            GoodType=GoodType.IntegralExchange,
                            FreeShipping = integralGood.FreeShipping
                        });
                }
           
            #endregion
            //查省和市的id
            var distinct = _currencyService.GetSingleByConditon<District>(a => a.FullName == myAddress.City);
            //计算商品总价
            order.GoodsAmount = orderGoods.Sum(g => g.Price * g.Quantity);
            //计算物流费用
            order.ShippingFee = orderGoods.Any(g => g.FreeShipping) ? 0 : _shippingAreaService.GetAreaFreight(distinct.ParentId, distinct.Id);
            //计算订单总价
            order.OrderAmount = order.GoodsAmount + order.ShippingFee;
            //事务控制
            using (TransactionScope scope = new TransactionScope())
            {
                //计算积分折抵
                if (order.Integral > 0)
                {
                    var integralWallet = _walletService.GetWalletByMemberId(order.MemberId, Wallet.Models.WalletType.Integral);
                    if (integralWallet == null || integralWallet.Available < order.Integral)
                        throw new WebApiInnerException("0003", "可用积分不足");
                    else
                    {
                        var systemConfig = _configService.Get<SystemConfig>();
                        //计算剩余积分总共可以抵扣多少钱
                        var maxDiscountMoney = (decimal)integralWallet.Available / 100 * systemConfig.DiscountRate;
                        if (maxDiscountMoney >= order.GoodsAmount)
                        {
                            order.Integral = (int)(order.GoodsAmount / systemConfig.DiscountRate * 100);
                        }
                        string error;
                        _walletService.Draw(order.MemberId, Wallet.Models.WalletType.Integral, order.Integral, "积分兑换", out error);
                        if (string.IsNullOrWhiteSpace(error))
                        {
                            order.IntegralMoney = (decimal)order.Integral / 100 * systemConfig.DiscountRate;
                        }
                    }
                }
                //计算需要支付的费用
                order.PayFee = order.GoodsAmount+ order.ShippingFee - order.IntegralMoney;
              
                    order.OrderStatus = OrderStatus.WaitingForDelivery;
                    order.PayStatus = PayStatus.Paid;
                    order.PayTime = DateTime.Now;
               
                _orderService.SubmitOrder(order, orderGoods);
                //提交
                scope.Complete();
            }
            result.Data = order.Id;
            return Json(result);
        }
       
    }
}