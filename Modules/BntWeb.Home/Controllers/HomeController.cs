
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BntWeb.Advertisement;
using BntWeb.Advertisement.ApiModel;
using BntWeb.Advertisement.Models;
using BntWeb.Carousel.ApiModel;
using BntWeb.Carousel.Services;
using BntWeb.Config.Models;
using BntWeb.Data.Services;
using BntWeb.FileSystems.Media;
using BntWeb.Mall.ApiModels;
using BntWeb.Mall.Models;
using BntWeb.Mall.Services;
using BntWeb.MemberBase.Services;
using BntWeb.MemberCenter;
using BntWeb.Security;
using BntWeb.Services;
using BntWeb.Utility.Extensions;

namespace BntWeb.Home.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGoodsService _goodsService;
        private readonly ICurrencyService _currencyService;
        private readonly IGoodsCategoryService _goodsCategoryServices;
        private readonly IStorageFileService _storageFileService;
        private readonly IConfigService _configService;
        private readonly ICarouselService _carouselService;
        private readonly IMemberContainer _memberContainer;
        private readonly IMemberService _memberService;
        public HomeController(ICarouselService carouselService, IGoodsService goodsService, 
            ICurrencyService currencyService, IGoodsCategoryService goodsCategoryServices, 
            IStorageFileService storageFileService, IConfigService configService,
            IMemberContainer memberContainer, IMemberService memberService)
        {
            _configService = configService;
            _memberService = memberService;
            _carouselService = carouselService;
            _goodsService = goodsService;
            _currencyService = currencyService;
            _goodsCategoryServices = goodsCategoryServices;
            _storageFileService = storageFileService;
            _memberContainer = memberContainer;
        }

        /// <summary>
        /// 首页列表
        /// </summary>
        /// <returns></returns>
        // GET: WebHomePage

        public ActionResult Index()
        {

            //首页轮播图
            ViewBag.Carousel = _carouselService.LoadItemsByGroupKey("01").Select(c => new CarouseModel(c)).ToList();

            var categoryList = _goodsCategoryServices.GetShowIndexCategories();
            var systemConfig = _configService.Get<SystemConfig>();
            ViewBag.Type1Id = systemConfig.Homeone;
            ViewBag.Type2Id = systemConfig.Hometwo;
            ViewBag.Type3Id = systemConfig.Homethree;
            var categories = new List<HomeGoodsCategoryModel>();
            var totalCount = 0;
            foreach (var item in categoryList)
            {
                if (item.Id == systemConfig.Homeone)
                {
                    var category = new HomeGoodsCategoryModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                       EnglishName=item.EnglishName,
                        Goods = _goodsService.GetHotGoodsByCategory(item.Id, null, GoodsSortType.Degault, 1, 8, out totalCount)
                                                   .Select(g => new ListGoodsModel(g))
                                                   .ToList()
                    };

                    categories.Add(category);
                }
                else if (item.Id == systemConfig.Hometwo)
                {
                    var category = new HomeGoodsCategoryModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        EnglishName = item.EnglishName,
                        Goods = _goodsService.GetHotGoodsByCategory(item.Id, null, GoodsSortType.Degault, 1, 6, out totalCount)
                                                   .Select(g => new ListGoodsModel(g))
                                                   .ToList()
                    };

                    categories.Add(category);
                }
                else if (item.Id == systemConfig.Homethree)
                {
                    var category = new HomeGoodsCategoryModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        EnglishName = item.EnglishName,
                        Goods = _goodsService.GetHotGoodsByCategory(item.Id, null, GoodsSortType.Degault, 1, 4, out totalCount).Where(h=>h.IsHot)
                                                   .Select(g => new ListGoodsModel(g))
                                                   .ToList()
                    };

                    categories.Add(category);
                }
            }
            //首页分类蛋糕
         
            List<SimplifiedStorageFile> img1 = new List<SimplifiedStorageFile>();
            List<SimplifiedStorageFile> img2 = new List<SimplifiedStorageFile>();
            List<SimplifiedStorageFile> img3 = new List<SimplifiedStorageFile>();
            List<SimplifiedStorageFile> img4 = new List<SimplifiedStorageFile>();
            List<SimplifiedStorageFile> img5 = new List<SimplifiedStorageFile>();
            ViewBag.CategoryList = categories;
            //广告位01 的区域及其广告
            var advertArea = _currencyService.GetSingleByConditon<AdvertArea>(a => a.Key == "01");
            var advertOne = _currencyService.GetSingleByConditon<Advert>(a => a.AreaId.Equals(advertArea.Id) && a.Key == "01");
            //广告图1
            if (advertOne != null)
            {
                var img = _storageFileService.GetFiles(advertOne.Id, AdvertisementModule.Key, "AdvertImage").FirstOrDefault()?.Simplified();
                img1.Add(img);
            int index = advertOne.ShotUrl.IndexOf("http", StringComparison.Ordinal);
                if (index > -1)
                {
                    ViewBag.AdvertIndex1 = 1;
                }
            }
            ViewBag.AdvertImg1 = img1;

            //广告位21 和22 两个广告
            var advertTwo = _currencyService.GetSingleByConditon<Advert>(a => a.AreaId.Equals(advertArea.Id) && a.Key == "21");
            //广告图21
            if (advertTwo != null)
            {
                var img = _storageFileService.GetFiles(advertTwo.Id, AdvertisementModule.Key, "AdvertImage").FirstOrDefault()?.Simplified();
                img2.Add(img);
                int index = advertTwo.ShotUrl.IndexOf("http", StringComparison.Ordinal);
                if (index > -1)
                {
                    ViewBag.AdvertIndex2 = 1;
                }
            }
            ViewBag.AdvertImg2 = img2;
            var advertTwoSecond = _currencyService.GetSingleByConditon<Advert>(a => a.AreaId.Equals(advertArea.Id) && a.Key == "22");
            //广告图22
            if (advertTwoSecond != null)
            {
                var img = _storageFileService.GetFiles(advertTwoSecond.Id, AdvertisementModule.Key, "AdvertImage").FirstOrDefault()?.Simplified();
                img3.Add(img);
                int index = advertTwoSecond.ShotUrl.IndexOf("http", StringComparison.Ordinal);
                if (index > -1)
                {
                    ViewBag.AdvertIndex3 = 1;
                }
            }
            ViewBag.AdvertImg3 = img3;
            //广告位31 32两个广告位

            //广告图31
            var advertThree = _currencyService.GetSingleByConditon<Advert>(a => a.AreaId.Equals(advertArea.Id) && a.Key == "31");
            if (advertThree != null)
            {
                var img = _storageFileService.GetFiles(advertThree.Id, AdvertisementModule.Key, "AdvertImage").FirstOrDefault()?.Simplified();
                img4.Add(img);
                int index = advertThree.ShotUrl.IndexOf("http", StringComparison.Ordinal);
                if (index > -1)
                {
                    ViewBag.AdvertIndex4 = 1;
                }
            }
            ViewBag.AdvertImg4 = img4;
            //广告图32
            var advertThreeSecond = _currencyService.GetSingleByConditon<Advert>(a => a.AreaId.Equals(advertArea.Id) && a.Key == "32");

            if (advertThreeSecond != null)
            {
                var img = _storageFileService.GetFiles(advertThreeSecond.Id, AdvertisementModule.Key, "AdvertImage").FirstOrDefault()?.Simplified();
                img5.Add(img);
                int index = advertThreeSecond.ShotUrl.IndexOf("http", StringComparison.Ordinal);
                if (index > -1)
                {
                    ViewBag.AdvertIndex5 = 1;
                }
            }
            ViewBag.AdvertImg5 = img5;
            ViewBag.AdvertOne = advertOne;
            ViewBag.AdvertTwo = advertTwo;
            ViewBag.AdvertTwoSecond = advertTwoSecond;
            ViewBag.AdvertThreeSecond = advertThreeSecond;
            ViewBag.AdvertThree = advertThree;

            return View();
        }
        /// <summary>
        /// 首页头部
        /// </summary>
        /// <returns></returns>
        [MemberAuthorize(Forcible = false)]
        public PartialViewResult Head()
        {
            if (HttpContext.User?.Identity != null)
            {
                _memberContainer.UserName = HttpContext.User.Identity.Name;
            }
            var currentUser = _memberContainer.CurrentMember;
            ViewBag.CurrentUser = currentUser;
            ViewBag.Catagoery = _currencyService.GetList<GoodsCategory>(me=>me.Name!= "鲜花专区");
            return PartialView();
        }
        /// <summary>
        /// 首页底部
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Footer()
        {
          
            return PartialView();
        }
        /// <summary>
        /// 首页右边浮动框
        /// </summary>
        /// <returns></returns>
        public ActionResult Float()
        {
            return View();
        }
        /// <summary>
        /// 个人中心
        /// </summary>
        /// <returns></returns>
        [MemberAuthorize]
        public ActionResult Personal()
        {
            _memberContainer.UserName = HttpContext.User.Identity.Name;
            var currentMember = _memberContainer.CurrentMember;
            var personalModel = _memberService.FindMemberById(currentMember.Id);
            ViewBag.WebAvatarFile =
                 _storageFileService.GetFiles(currentMember.Id.ToGuid(), MemberCenterModule.Key, "Avatar")
                     .FirstOrDefault();
            return View(personalModel);
        }
        //分页
        public ActionResult PagePartial(string url = "", int totalPage = 0, int currentPage = 1)
        {
            ViewBag.Url = url;
            ViewBag.TotalPage = totalPage;
            ViewBag.CurrentPage = currentPage;
            return PartialView("_PagePartial");
        }
    }
}