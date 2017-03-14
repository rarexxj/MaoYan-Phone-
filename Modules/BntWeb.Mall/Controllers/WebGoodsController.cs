using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BntWeb.Carousel.ApiModel;
using BntWeb.Carousel.Services;
using BntWeb.Config.Models;
using BntWeb.ContentMarkup.Models;
using BntWeb.ContentMarkup.Services;
using BntWeb.Data;
using BntWeb.Data.Services;
using BntWeb.Environment;
using BntWeb.FileSystems.Media;
using BntWeb.Logging;
using BntWeb.Mall.ApiModels;
using BntWeb.Mall.Models;
using BntWeb.Mall.Services;
using BntWeb.Mall.ViewModels;
using BntWeb.MemberBase.Services;
using BntWeb.Security;
using BntWeb.Wallet.Models;
using BntWeb.Wallet.Services;


namespace BntWeb.Mall.Controllers
{
    public class WebGoodsController : Controller
    {
        private readonly ICurrencyService _currencyService;
        private readonly IGoodsService _goodsService;
        private readonly IGoodsCategoryService _goodsCategoryService;
        private readonly IStorageFileService _storageFileService;
        private readonly ICarouselService _carouselService;
        private readonly IMemberContainer _memberContainer;
        private readonly IWalletService _walletService;
        private readonly IMarkupService _markupService;
        private readonly UrlHelper _urlHelper;
        public WebGoodsController(IMarkupService markupService, ICurrencyService currencyService, IGoodsService goodsService,
            IGoodsCategoryService goodsCategoryService, IStorageFileService storageFileService,
            ICarouselService carouselService,
             IMemberContainer memberContainer, IWalletService walletService,
             UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
            _currencyService = currencyService;
            _goodsService = goodsService;
            _goodsCategoryService = goodsCategoryService;
            _storageFileService = storageFileService;
            _carouselService = carouselService;
            Logger = NullLogger.Instance;
            _walletService = walletService;
            _memberContainer = memberContainer;
            _markupService = markupService;
        }

        public ILogger Logger { get; set; }

        //获得某类商品
        public ActionResult GoodsList(Guid? categoryId, int pageNo = 1, int pageSize = 10)
        {
            int totalCount;
            var goodsList = (categoryId != Guid.Empty && categoryId != null) ? _currencyService.GetList<Goods>(a => a.CategoryId == categoryId && a.Status == GoodsStatus.InSale)
                : _currencyService.GetList<Goods>(a => a.SpecialType == SpecialType.Flower && a.Status == GoodsStatus.InSale);

            if (categoryId != null)
            {
                ViewBag.CategoryName = _goodsCategoryService.GetCategoryById(categoryId.Value)?.Name;
            }
            else
            {
                ViewBag.CategoryName = "鲜花专区";
            }



            //分类轮播图
            ViewBag.Carousel = _carouselService.LoadItemsByGroupKey("04").Select(c => new CarouseModel(c)).ToList();
            ViewBag.GoodsList = goodsList;
            foreach (var item in goodsList)
            {
                var mainImage = _storageFileService.GetFiles(item.Id, MallModule.Key, "MainImage").FirstOrDefault();
                item.MainImage = mainImage?.Simplified();

            }
            return View();
        }
        /// <summary>
        /// 商品详情
        /// </summary>
        /// <param name="goodId"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>

        public ActionResult GoodsDetails(Guid goodId, int pageNo = 1, int pageSize = 9)
        {
            if (string.IsNullOrWhiteSpace(goodId.ToString()))
                throw new Exception("商品Id为空");
            //加载所有商品
            var allGoods = _goodsService.LoadFullGoods(goodId);
            var mainImages = _storageFileService.GetFiles(goodId, MallModule.Key, "MainImage").Select(me => me.Simplified()).ToList();
            ViewBag.MainImages = mainImages;

            var routeParass = new RouteValueDictionary{
                    { "area", "Mall"},
                    { "controller", "WebGoods"},
                    { "action", "NotGood"}
                };
            var returnUrls = HostConstObject.HostUrl + _urlHelper.RouteUrl(routeParass);

            if (allGoods == null)
                return Redirect(returnUrls);
            //获得商品分类名称
            if (allGoods.CategoryId != Guid.Empty)
            {
                ViewBag.CategoryName = _currencyService.GetSingleById<GoodsCategory>(allGoods.CategoryId).Name;
            }
            else
            {
                ViewBag.CategoryName = "鲜花专区";
            }


            //加载所有的品牌图片
            List<SimplifiedStorageFile> brandGoogImages = new List<SimplifiedStorageFile>();
            var goodsBrand = _currencyService.GetAll<GoodsBrand>();
            if (goodsBrand != null)
            {
                foreach (var item in goodsBrand)
                {
                    var img = _storageFileService.GetFiles(item.Id, MallModule.Key, "Logo").FirstOrDefault();
                    if (img != null)
                        brandGoogImages.Add(img.Simplified());
                }

            }
            ViewBag.BrandImages = brandGoogImages;
            //加价购商品
            //选择该商品对应的加价购商品
            var otionalIds = _currencyService.GetSingleByConditon<Goods>(me => me.Id == goodId).RelationOpt;
            var purchaseGoods = _currencyService.GetList<Goods>(a => otionalIds.Contains(a.Id.ToString())).ToList();

            if (purchaseGoods.Count != 0)
            {
                foreach (var item in purchaseGoods)
                {
                    item.PurImage = _storageFileService.GetFiles(item.Id, MallModule.Key, "MainImage").FirstOrDefault()?.Simplified();

                }

            }
            ViewBag.PurchaseGoods = purchaseGoods;
            //热门推荐商品
            var hotGoods = _currencyService.GetList<Goods>(a => a.IsHot && a.Status == GoodsStatus.InSale);
            if (hotGoods != null)
            {
                foreach (var item in hotGoods)
                {
                    item.MainImage = _storageFileService.GetFiles(item.Id, MallModule.Key, "MainImage").FirstOrDefault()?.Simplified();

                }

            }
            ViewBag.HotGoods = hotGoods;
            ////添加浏览记录
         
            var member = _memberContainer.GetMember(HttpContext);
            if (member != null)
            {
                if (_markupService.MarkupExist(goodId, MallModule.Key, member.Id, MarkupType.Browse))
                    _markupService.CancelMarkup(goodId, MallModule.Key, member.Id, MarkupType.Browse);
                _markupService.CreateMarkup(goodId, MallModule.Key, member.Id, MarkupType.Browse);

             
            }

            //获取商品收藏数量
            ViewBag.collectNums = _goodsService.GetCollectNumsByGoodsId(goodId);
            ViewBag.goodsId = goodId;
            //分页获得商品评价
            int totalCount;
            var evaluateList = _goodsService.GetGoodsEvaluatesListByPage(goodId, pageNo, pageSize, out totalCount);
            var evaluate = evaluateList.Select(x => new GoodsEvaluateModel(x)).ToList();
            ViewBag.EvaluateList = evaluate;
            var routeParas = new RouteValueDictionary{
                    { "area", "Wallet"},
                    { "controller", "WebGoods"},
                    { "action", "GoodsDetails"}
                };
            var returnUrl = HostConstObject.HostUrl + _urlHelper.RouteUrl(routeParas);

            ViewBag.Url = returnUrl + "?pageNo=[pageNo]";
            //获得总页数
            ViewBag.TotalPage = totalCount % pageSize == 0 ? totalCount / pageSize : totalCount / pageSize + 1;
            ViewBag.CurrentPage = pageNo;
            return View(allGoods);
        }

        public ActionResult HotGoods()
        {
            var hotGoods = _currencyService.GetList<Goods>(a => a.IsHot && a.Status == GoodsStatus.InSale);
            //分类轮播图
            ViewBag.Carousel = _carouselService.LoadItemsByGroupKey("03").Select(c => new CarouseModel(c)).ToList();
            foreach (var item in hotGoods)
            {
                var mainImage = _storageFileService.GetFiles(item.Id, MallModule.Key, "MainImage").FirstOrDefault();
                item.MainImage = mainImage?.Simplified();

            }
            ViewBag.HotGoods = hotGoods;
            return View();
        }
        //积分兑换商品
        [MemberAuthorize]
        public ActionResult Exchange(int pageNo = 1, int pageSize = 9)
        {
            //获得当前用户
            var currentMember = _memberContainer.CurrentMember;

            //获得当前可用积分
            ViewBag.MyIntenal = _walletService.GetWalletByMemberId(currentMember.Id, WalletType.Integral);

            int totalCount;
            Expression<Func<Goods, bool>> expr = x => x.SpecialType == SpecialType.IntegralExchange
            && x.Status == GoodsStatus.InSale;
            ViewBag.Exchange = _currencyService.GetListPaged<Goods>(pageNo, pageSize, expr, out totalCount,
                new OrderModelField { PropertyName = "CreateTime", IsDesc = true }).Select(me => new ExchangeModel(me)).ToList();
            var routeParas = new RouteValueDictionary{
                    { "area", "Mall"},
                    { "controller", "WebGoods"},
                    { "action", "Exchange"}
                };
            var returnUrl = HostConstObject.HostUrl + _urlHelper.RouteUrl(routeParas);

            ViewBag.Url = returnUrl + "?pageNo=[pageNo]";
            //获得总页数
            ViewBag.TotalPage = totalCount % pageSize == 0 ? totalCount / pageSize : totalCount / pageSize + 1;
            ViewBag.CurrentPage = pageNo;

            return View();
        }

        public ViewResult NotGood()
        {
            return View();
        }
    }
}