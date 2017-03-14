using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using BntWeb.Config.Models;
using BntWeb.Data;
using BntWeb.Data.Services;
using BntWeb.FileSystems.Media;
using BntWeb.Logging;
using BntWeb.Mall.Models;
using BntWeb.Mall.Services;
using BntWeb.Mall.ViewModels;
using BntWeb.Mvc;
using BntWeb.Security;
using BntWeb.Services;
using BntWeb.Utility.Extensions;
using BntWeb.Web.Extensions;

namespace BntWeb.Mall.Controllers
{
    public class GoodsController : Controller
    {
        private readonly ICurrencyService _currencyService;
        private readonly IGoodsService _goodsService;
        private readonly IGoodsCategoryService _goodsCategoryService;
        private readonly IStorageFileService _storageFileService;
        private readonly IConfigService _configService;
        private const string MainImage = "MainImage";
      

        public GoodsController(ICurrencyService currencyService, IGoodsService goodsService, IGoodsCategoryService goodsCategoryService, IStorageFileService storageFileService, IConfigService configService)
        {
            _currencyService = currencyService;
            _goodsService = goodsService;
            _goodsCategoryService = goodsCategoryService;
            _storageFileService = storageFileService;
            _configService = configService;

            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageGoodsKey })]

        public ActionResult List()
        {
            ViewBag.CategoryList = _goodsCategoryService.GetCategories();

            return View();
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageGoodsKey })]
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
            var name = Request.Get("extra_search[Name]");
            var checkName = string.IsNullOrWhiteSpace(name);

            var goodsNo = Request.Get("extra_search[GoodsNo]");
            var checkGoodsNo = string.IsNullOrWhiteSpace(goodsNo);

            var status = Request.Get("extra_search[Status]");
            var checkStatus = string.IsNullOrWhiteSpace(status);

            var goodsCategory = Request.Get("extra_search[GoodsCategory]");
            var checkGoodsCategory = string.IsNullOrWhiteSpace(goodsCategory);



            Expression<Func<Goods, bool>> expression =
                l => (checkName || l.Name.ToString().Contains(name)) &&
                     (checkGoodsNo || l.GoodsNo.Contains(goodsNo)) && (l.SpecialType == SpecialType.General) &&
                      (checkGoodsCategory || (l.CategoryId).ToString().Equals(goodsCategory))&&
                     (checkStatus || ((int)l.Status).ToString().Equals(status)) && l.Status != GoodsStatus.Delete;


            //分页查询
            var list = _currencyService.GetListPaged<Goods>(pageIndex, pageSize, expression, out totalCount, new OrderModelField { PropertyName = sortColumn, IsDesc = isDesc });

            result.data = list;
            result.recordsTotal = totalCount;
            result.recordsFiltered = totalCount;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
      
        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageGoodsKey })]
        public ActionResult Edit(Guid? id = null)
        {
            //商品分类
            var categories = _goodsService.GetCategories();
            if (categories == null || categories.Count == 0)
                throw new BntWebCoreException("商品分类异常");
            ViewBag.CategoriesJson = categories.Select(me => new { id = me.Id, pId = me.ParentId, name = me.Name }).ToList().ToJson();
            ViewBag.GoodsType = _currencyService.GetList<GoodsType>(t => t.Enabled);
            ViewBag.GoodsBrand = _currencyService.GetAll<GoodsBrand>().OrderByDescending(b => b.Sort).ToList();
            var systemConfig = _configService.Get<SystemConfig>();
            ViewBag.MaxLevel = systemConfig.MaxLevel;

            Goods goods = null;
            var selectedAttrs = new List<SelectedAttrViewModel>();
            if (id != null && id != Guid.Empty)
            {
                goods = _goodsService.LoadFullGoods(id.Value);
                if (goods != null)
                {
                    //获取扩展属性
                    var categoryRelations = _goodsCategoryService.GetCategoryRelations(goods.Id);
                    var categoryIds = string.Join(",", categoryRelations.Select(me => me.CategoryId).ToList());
                    var categoryNames = string.Join(",", categoryRelations.Select(me => me.GoodsCategory.Name).ToList());
                    ViewBag.ExtendCategoryIds = categoryIds;
                    ViewBag.ExtendCategoryNames = categoryNames;
                    ViewBag.Commissions = _currencyService.GetList<GoodsCommission>(c => c.GoodsId.Equals(goods.Id),
                        new OrderModelField { PropertyName = "Level", IsDesc = false });

                    //拼接已经选中的属性数据
                    foreach (var singleGoods in goods.SingleGoods)
                    {
                        foreach (var attribute in singleGoods.Attributes)
                        {
                            var attr = selectedAttrs.FirstOrDefault(a => a.Id.Equals(attribute.AttributeId));
                            if (attr == null)
                            {
                                selectedAttrs.Add(new SelectedAttrViewModel
                                {
                                    Id = attribute.AttributeId,
                                    Vals = new List<string> { attribute.AttributeValue }
                                });
                            }
                            else
                            {
                                if (!attr.Vals.Contains(attribute.AttributeValue))
                                {
                                    attr.Vals.Add(attribute.AttributeValue);
                                }
                            }
                        }
                    }
                }
            }

            if (goods == null)
            {
                goods = new Goods
                {
                    Id = KeyGenerator.GetGuidKey()
                };
            }

            ViewBag.CategoryName = categories.Find(me => me.Id == goods.CategoryId) != null
                ? categories.Find(me => me.Id == goods.CategoryId).Name
                : "";
            ViewBag.CurrentSingleGoods = goods.SingleGoods.ToJson();
            ViewBag.CurrentAttrs = selectedAttrs.ToJson();
            //获取所有的加价购商品
            ViewBag.OptGoods = _currencyService.GetList<Goods>(a => a.SpecialType == SpecialType.PurchasePrice && a.Status != GoodsStatus.Delete);
            return View(goods);
        }

        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageGoodsKey })]
        [System.Web.Mvc.ValidateInput(false)]
        public ActionResult EditOnPost(GoodsViewModel postGoods)
        {
            var result = new DataJsonResult();
            postGoods.SingleGoods = postGoods.SingleGoodsJson.DeserializeJsonToList<SingleGoodsViewModel>();
            var goods = _currencyService.GetSingleById<Goods>(postGoods.Id);
            var isNew = false;
            if (goods == null)
            {
                goods = new Goods
                {
                    Id = postGoods.Id,
                    CategoryId = postGoods.CategoryId,
                    BrandId = postGoods.BrandId,
                    Name = postGoods.Name,
                    OriginalPrice = postGoods.OriginalPrice,
                    GoodsType = postGoods.GoodsType,
                    GoodsNo = postGoods.GoodsNo,
                    Description = postGoods.Description,
                    Status = GoodsStatus.NotInSale,
                    CreateTime = DateTime.Now,
                    SingleGoods = new List<SingleGoods>(),
                    FreeShipping = postGoods.FreeShipping,
                    IsHot = postGoods.IsHot,
                    IsNew = postGoods.IsNew,
                    RelationOpt = postGoods.RelationOpt,
                    IsBest = postGoods.IsBest
                };
                goods.LastUpdateTime = goods.CreateTime;
                isNew = true;
            }
            else
            {
                goods.CategoryId = postGoods.CategoryId;
                goods.BrandId = postGoods.BrandId;
                goods.Name = postGoods.Name;
                goods.OriginalPrice = postGoods.OriginalPrice;
                goods.GoodsNo = postGoods.GoodsNo;
                goods.GoodsType = postGoods.GoodsType;
                goods.Description = postGoods.Description;
                goods.SingleGoods = new List<SingleGoods>();
                goods.LastUpdateTime = DateTime.Now;
                goods.FreeShipping = postGoods.FreeShipping;
                goods.IsHot = postGoods.IsHot;
                goods.IsNew = postGoods.IsNew;
                goods.RelationOpt = postGoods.RelationOpt;
                goods.IsBest = postGoods.IsBest;
            }

            var sort = 1;
            foreach (var postSingleGoods in postGoods.SingleGoods)
            {
                if (postSingleGoods.Stock > 0)
                {
                    var singleGoods = new SingleGoods
                    {
                        Id = postSingleGoods.Id,
                        CreateTime = goods.CreateTime,
                        Stock = postSingleGoods.Stock,
                        Unit = postSingleGoods.Unit,
                        Price = postSingleGoods.Price,
                        GoodsId = goods.Id,
                        Sort = sort++,
                        Attributes = new List<SingleGoodsAttribute>(),
                    };

                    foreach (var attr in postSingleGoods.Attrs)
                    {
                        singleGoods.Attributes.Add(new SingleGoodsAttribute
                        {
                            Id = KeyGenerator.GetGuidKey(),
                            AttributeId = attr.Id,
                            AttributeValue = attr.Val.Trim(),
                            SingleGoodsId = singleGoods.Id
                        });
                    }
                    singleGoods.Image = new StorageFile { Id = postSingleGoods.Image?.Id ?? Guid.Empty }.Simplified();
                    goods.SingleGoods.Add(singleGoods);
                }
            }

            goods.ShopPrice = goods.SingleGoods.Min(g => g.Price);
            goods.Stock = goods.SingleGoods.Sum(g => g.Stock);

            result.Success = _goodsService.SaveGoods(goods, isNew);
            //处理主图图片关联
            _storageFileService.ReplaceFile(goods.Id, MallModule.Key, MallModule.DisplayName, postGoods.MainImage, MainImage);



            return Json(result);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageGoodsKey })]
        public ActionResult Delete(Guid goodsId)
        {
            var result = new DataJsonResult();
            if (!_goodsService.RecoveryGoods(goodsId))
            {
                result.ErrorMessage = "未知异常，删除失败";
            }

            return Json(result);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageGoodsKey })]
        public ActionResult BatchDelete(List<Guid> goodsIds)
        {
            var result = new DataJsonResult();
            if (!_goodsService.BatchRecoveryGoods(goodsIds))
            {
                result.ErrorMessage = "未知异常，删除失败";
            }

            return Json(result);
        }


        public ActionResult GetGoodsAttribute(Guid goodsTypeId)
        {
            var result = new DataJsonResult();

            result.Data = _currencyService.GetList<GoodsAttribute>(a => a.GoodsTypeId.Equals(goodsTypeId));

            return Json(result);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageGoodsKey })]
        public ActionResult InSale(Guid id)
        {
            var result = new DataJsonResult();
            if (_goodsService.SetGoodsStatus(id, GoodsStatus.InSale))
                result.Success = true;
            else
            {
                result.Success = false;
                result.ErrorMessage = "上架失败";
            }
            return Json(result);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageGoodsKey })]
        public ActionResult NotInSale(Guid id)
        {
            var result = new DataJsonResult();
            if (_goodsService.SetGoodsStatus(id, GoodsStatus.NotInSale))
                result.Success = true;
            else
            {
                result.Success = false;
                result.ErrorMessage = "下架失败";
            }
            return Json(result);
        }
    }
}