
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BntWeb.Config.Models;
using BntWeb.Coupon.Models;
using BntWeb.Coupon.Services;
using BntWeb.Data.Services;
using BntWeb.FileSystems.Media;
using BntWeb.Logistics.Services;
using BntWeb.Mall.ApiModels;
using BntWeb.Mall.Models;
using BntWeb.Mall.Services;
using BntWeb.MemberBase.Models;
using BntWeb.Services;


using BntWeb.Wallet.Services;
using BntWeb.WebApi.Filters;
using BntWeb.WebApi.Models;

namespace BntWeb.Mall.ApiControllers
{
    public class OrderCalculationController : BaseApiController
    {
        private readonly IGoodsService _goodsService;
        private readonly ICurrencyService _currencyService;
        private readonly IShippingAreaService _shippingAreaService;
        private readonly IWalletService _walletService;
        private readonly IConfigService _configService;
        private readonly ICouponService _couponService;
        private readonly IStorageFileService _storageFileService;

        public OrderCalculationController(ICouponService couponService, IGoodsService goodsService,
            ICurrencyService currencyService,
            IShippingAreaService shippingAreaService, IWalletService walletService, IConfigService configService,
            IStorageFileService storageFileService)
        {
            _goodsService = goodsService;
            _currencyService = currencyService;
            _shippingAreaService = shippingAreaService;
            _walletService = walletService;
            _configService = configService;
            _couponService = couponService;
            _storageFileService = storageFileService;
        }

        /// <summary>
        /// 订单计算
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        public ApiResult OrderCalculation([FromBody] OrderCalculationModel calculationModel)
        {

            //计算商品总价
            var goodsAmount = 0M;
            var isFreeShipping = false;

            int goodsNum = 0;
            Goods purchaseGoods = null;
            List<OrderCalculationGoodsModel> goods = new List<OrderCalculationGoodsModel>();
            //购物车
            if (calculationModel.IsFromCart)
            {
                if (calculationModel.CartIds == null || calculationModel.CartIds.Count < 1)
                    throw new WebApiInnerException("0001", "没有选择商品");

                foreach (var item in calculationModel.CartIds)
                {
                    var cartInfo = _currencyService.GetSingleById<Cart>(item);
                    if (cartInfo == null || cartInfo.MemberId != AuthorizedUser.Id)
                        throw new WebApiInnerException("0002", "存在非法商品");
                    if (cartInfo.SingleGoodsId != Guid.Empty)
                    {


                        var singleGoods = _goodsService.LoadFullSingleGoods(cartInfo.SingleGoodsId);
                        if (singleGoods?.Goods == null || singleGoods.Goods.Status != GoodsStatus.InSale)
                            throw new WebApiInnerException("0003", "存在非法或者失效的商品");
                        if (singleGoods.Stock < cartInfo.Quantity)
                            throw new WebApiInnerException("0004", $"【{singleGoods.Goods.Name}】库存不足");

                        goodsAmount += singleGoods.Price*cartInfo.Quantity;
                        if (singleGoods.Goods.FreeShipping)
                        {
                            isFreeShipping = true;
                        }
                        goodsNum += cartInfo.Quantity;
                        goods.Add(new OrderCalculationGoodsModel(cartInfo));
                    }
                    else
                    {
                        //加价购商品  
                        purchaseGoods = _currencyService.GetSingleById<Goods>(cartInfo.GoodsId);
                        purchaseGoods.MainImage =
                            _storageFileService.GetFiles(purchaseGoods.Id, MallModule.Instance.InnerKey, "MainImage")
                                .FirstOrDefault()?
                                .Simplified();
                        if (purchaseGoods.FreeShipping)
                        {
                            isFreeShipping = true;
                        }
                        goodsAmount += purchaseGoods.ShopPrice;
                    }
                }

            }
            //立即购买
            else
            {
                //单品
                if (calculationModel.SingleGoods == null || calculationModel.SingleGoods.Count < 1)
                    throw new WebApiInnerException("0001", "没有选择商品");

                foreach (var item in calculationModel.SingleGoods)
                {
                    var singleGoods = _goodsService.LoadFullSingleGoods(item.SingleGoodsId);
                    if (singleGoods?.Goods == null || singleGoods.Goods.Status != GoodsStatus.InSale)
                        throw new WebApiInnerException("0003", "存在非法或者失效的商品");
                    if (singleGoods.Stock < item.Quantity)
                        throw new WebApiInnerException("0004", $"【{singleGoods.Goods.Name}】库存不足");

                    goodsAmount += singleGoods.Price*item.Quantity;
                    if (singleGoods.Goods.FreeShipping)
                    {
                        isFreeShipping = true;
                    }
                    goodsNum += item.Quantity;
                    goods.Add(new OrderCalculationGoodsModel(singleGoods, item.Quantity));
                }

                //加价购商品
                if (calculationModel.PurchaseId != null && calculationModel.PurchaseId != Guid.Empty)
                {

                    purchaseGoods = _currencyService.GetSingleById<Goods>(calculationModel.PurchaseId);
                    purchaseGoods.MainImage =
                        _storageFileService.GetFiles(purchaseGoods.Id, MallModule.Instance.InnerKey, "MainImage")
                            .FirstOrDefault()?
                            .Simplified();
                    if (purchaseGoods.FreeShipping)
                    {
                        isFreeShipping = true;
                    }
                    goodsAmount += purchaseGoods.ShopPrice;
                }
            }

                //获取收货地址
                MemberAddress address;
                if (calculationModel.AddressId == null)
                {
                    address = _currencyService.GetList<MemberAddress>(me => me.MemberId == AuthorizedUser.Id)
                        .OrderByDescending(x => x.IsDefault)
                        .FirstOrDefault();
                }
                else
                {
                    address = _currencyService.GetSingleById<MemberAddress>(calculationModel.AddressId);
                }

                //计算物流费用
                var shippingFee = address == null || isFreeShipping
                    ? 0
                    : _shippingAreaService.GetAreaFreight(address.Province, address.City);

                //可用积分
                var integralWallet = _walletService.GetWalletByMemberId(AuthorizedUser.Id,
                    Wallet.Models.WalletType.Integral);
                //我的优惠券
                var coupon = _couponService.GetMyCouponList(AuthorizedUser.Id, CouponStatus.Unused);
                var systemConfig = _configService.Get<SystemConfig>();
                var result = new ApiResult();

            var sa = new object();
            if (purchaseGoods != null)
            {
                sa = new
                {
                    PurId = purchaseGoods.Id,
                    PurName = purchaseGoods.Name,
                    PurQuanlity = 1,
                    PurPrice = purchaseGoods.ShopPrice,
                    PurMainImage = purchaseGoods.MainImage

                };
            }
                var data = new
                {
                    Goods = new
                    {
                        TotalQuantity = goodsNum,
                        List = goods
                    },
                    PurchaseGood = sa
                    ,
                    Addresses = address == null
                        ? null
                        : new
                        {
                            Id = address.Id,
                            Contacts = address.Contacts,
                            Phone = address.Phone,
                            RegionName = address.RegionName?.Replace(",", ""),
                            Address = address.Address,
                            Postcode = address.Postcode,
                            Province = address.Province,
                            City = address.City,
                            District = address.District,
                            Street = address.Street,
                            IsDefault = address.IsDefault
                        },
                    Coupon = coupon,
                    GoodsAmount = goodsAmount,
                    ShippingFee = shippingFee,
                    AvailableIntegral = integralWallet?.Available ?? 0,
                    IntegralDiscountRate = systemConfig.DiscountRate
                };
                result.SetData(data);
                return result;
            }
        }
    }

