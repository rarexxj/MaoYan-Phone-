﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BntWeb.Advertisement.ApiModel;
using BntWeb.Advertisement.Models;
using BntWeb.ContentMarkup.Models;
using BntWeb.ContentMarkup.Services;
using BntWeb.Data.Services;
using BntWeb.FileSystems.Media;
using BntWeb.Mall.ApiModels;
using BntWeb.Mall.Models;
using BntWeb.Mall.Services;
using BntWeb.WebApi.Filters;
using BntWeb.WebApi.Models;

namespace BntWeb.Mall.ApiControllers
{
    public class GoodsController : BaseApiController
    {
        private readonly IGoodsService _goodsService;
        private readonly ICurrencyService _currencyService;
        private readonly IGoodsCategoryService _goodsCategoryServices;
        private readonly IStorageFileService _storageFileService;
        private readonly IMarkupService _markupService;

        public GoodsController(IGoodsService goodsService, ICurrencyService currencyService, IGoodsCategoryService goodsCategoryServices, IStorageFileService storageFileService, IMarkupService markupService)
        {
            _goodsService = goodsService;
            _currencyService = currencyService;
            _goodsCategoryServices = goodsCategoryServices;
            _storageFileService = storageFileService;
            _markupService = markupService;
        }

        // /Api/v1/Mall/Home
        /// <summary>
        /// 获取采购首页指定分类和指定分类商品
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResult GetMallHomeCategroyGoods()
        {
            var categoryList = _goodsCategoryServices.GetShowIndexCategories();

            var categories = new List<HomeGoodsCategoryModel>();

            var totalCount = 0;
         
            foreach (var item in categoryList)
            {
           
                var category = new HomeGoodsCategoryModel
                {
                    Id = item.Id,
                    Name = item.Name+item.EnglishName,
                    Goods =
                        _goodsService.GetGoodsByCategory(item.Id, null, GoodsSortType.Degault, 1, 4, out totalCount)
                            .Select(g => new ListGoodsModel(g))
                            .ToList()
                };

                categories.Add(category);
            }
            ////推荐
            //var recommendGoods = _goodsService.GetRecommendGoods(1, 4, out totalCount).Select(g => new ListGoodsModel(g)).ToList();
            //一级分类
            var oneLevelCategorys= _currencyService.GetList<GoodsCategory>(me => me.ParentId == Guid.Empty && me.ShowIndex).OrderByDescending(x=>x.Sort).Select(x=>new OneLevelCategoryModel(x)).ToList();
            ////广告位
            //var advertArea =_currencyService.GetSingleByConditon<AdvertArea>(a => a.Key=="01");
            //var adverts = _currencyService.GetList<Advert>(a => a.AreaId.Equals(advertArea.Id)).OrderBy(a => a.Key).Select(x => new AdvertModel(x)).ToList();

            var result = new ApiResult();
            result.SetData(new
            {
                OneLevelCategorys = oneLevelCategorys,
                Categories = categories
                //RecommendGoods = recommendGoods,
                //Adverts= adverts
            });

            return result;
        }


        // /Api/v1/Mall/Goods/Recommend
        /// <summary>
        /// 获取推荐商品
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResult GetRecommendGoods(int type = 2, int pageNo = 1, int limit = 10)
        {
            int totalCount = 0;
            List<Goods> goods;
            if (type == 2)
            {
                goods = _goodsService.GetBestGoods(pageNo, limit, out totalCount);
            }
            else if (type == 3)
            {
                goods = _goodsService.GetHotGoods(pageNo, limit, out totalCount);
            }
            else
                goods = _goodsService.GetRecommendGoods(pageNo, limit, out totalCount);


            var result = new ApiResult();
            var data = new
            {
                TotalCount = totalCount,
                Goods = goods.Select(me => new ListGoodsModel(me)).ToList(),
            };

            result.SetData(data);

            return result;
        }
        
        /// <summary>
        /// 获得分类商品列表
        /// </summary>
        /// <param name="postModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult GetGoods(GoodsPostModel postModel)
        {
            Guid? categoryId = null;
            if (postModel.CategoryId != Guid.Empty)
                categoryId = postModel.CategoryId;

            if (postModel.MinPrice < 0)
                throw new WebApiInnerException("0002", "最低价格不得低于0");

            if (postModel.MinPrice > postModel.MaxPrice)
                throw new WebApiInnerException("0003", "最低价格不得大于最大价格");

            int totalCount = 0;
            object goods;
            if (postModel.Others == null || postModel.Others.Count == 0)
            {
                var list = _goodsService.GetGoodsByCategory(categoryId, postModel.KeyWord, postModel.Brands, postModel.MinPrice, postModel.MaxPrice, postModel.SortType, postModel.PageNo, postModel.Limit, out totalCount);
                goods = list.Select(me => new ListGoodsModel(me)).ToList();
            }
            else
            {
                var list = _goodsService.GetGoodsByCategory(categoryId, postModel.KeyWord, postModel.Brands, postModel.Others, postModel.MinPrice, postModel.MaxPrice, postModel.SortType, postModel.PageNo, postModel.Limit, out totalCount);
                goods = list.Select(me => new ListGoodsViewModel(me)).ToList();
            }

            var result = new ApiResult();
            var data = new
            {
                TotalCount = totalCount,
                Goods = goods
            };

            result.SetData(data);

            return result;
        }

        /// <summary>
        /// 获取筛选条件
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet] 
        public ApiResult GetFilterCriterias(Guid? categoryId)
        {
            if (categoryId == Guid.Empty)
                throw new WebApiInnerException("0001", "无效分类");
            var category = _currencyService.GetSingleById<GoodsCategory>(categoryId);
            if (category == null)
                throw new WebApiInnerException("0001", "无效分类");

            //品牌
            var brands = _currencyService.GetAll<GoodsBrand>().Select(x => new { Id = x.Id, Value = x.Name }).ToList();

            List<FilterCriteriasModel> others = new List<FilterCriteriasModel>();
            //其他属性
            if (category.GoodsTypeId != Guid.Empty)
            {
                var attributes = _currencyService.GetList<GoodsAttribute>(x => x.GoodsTypeId == category.GoodsTypeId).Select(x => new FilterCriteriasModel { Id = x.Id, Name = x.Name, Value = x.Values }).ToList();
                others.AddRange(attributes);
            }

            var result = new ApiResult();
            var data = new
            {
                Brand = brands,
                Others = others
            };
            result.SetData(data);
            return result;
        }

        /// <summary>
        /// 商品详情
        /// </summary>
        /// <param name="goodId"></param>
        /// <returns></returns>
        [HttpGet]
        [BasicAuthentication(Forcible = false)]
        public ApiResult GetGoodsAttrubiteAndSingleGoods(Guid goodId)
        {
            if (goodId == Guid.Empty)
                throw new WebApiInnerException("3011", "商品Id无效");

            var goods = _currencyService.GetSingleById<Goods>(goodId);
            if (goods == null)
                throw new WebApiInnerException("3012", "无此商品");

            var hasCollect = false;
            if (AuthorizedUser != null)
            {
                if (_markupService.MarkupExist(goodId, MallModule.Key, AuthorizedUser.Id, MarkupType.Collect))
                    hasCollect = true;
            }
            //商品主图
            var mainImage = _storageFileService.GetFiles(goodId, MallModule.Key, "MainImage");

            //单品
            var attr = _goodsService.GetGoodsAttributeValues(goodId);
            //商品对应的加价购商品
            var purGood = _goodsService.GeGoodPurchaseGoods(goodId).Select(a=> new PurchaseModel(a)).ToList();
            //list.Select(me => new ListGoodsViewModel(me)).ToList();
            var result = new ApiResult();
            var data = new
            {
                Name = goods.Name,
                OriginalPrice = goods.OriginalPrice,
                Price = goods.ShopPrice,
                Stock = goods.Stock,
                SalesVolume = goods.SalesVolume,
                PaymentAmount = goods.PaymentAmount,
                MainImage = mainImage?.Select(x=>x.Simplified()).ToList(),
                AttributeValues = attr,
                SingleGoogsList = _goodsService.LoadFullGoods(goodId).SingleGoods.Where(x=>x.Stock>0).Select(me => new SingleGoodsModel(me)).ToList(),
                Description = goods.Description,
                HasCollect = hasCollect,
                PurchaseGood= purGood
            };

            result.SetData(data);

            return result;
        }
    }
}
