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
    public class SpecialGoodsController : Controller
    {
        private readonly ICurrencyService _currencyService;
        private readonly IGoodsService _goodsService;
        private readonly IGoodsCategoryService _goodsCategoryService;
        private readonly IStorageFileService _storageFileService;
        private readonly IConfigService _configService;
        private const string MainImage = "MainImage";
      

        public SpecialGoodsController(ICurrencyService currencyService, IGoodsService goodsService, IGoodsCategoryService goodsCategoryService, IStorageFileService storageFileService, IConfigService configService)
        {
            _currencyService = currencyService;
            _goodsService = goodsService;
            _goodsCategoryService = goodsCategoryService;
            _storageFileService = storageFileService;
            _configService = configService;

            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        /// <summary>
        /// 加价购商品，积分换购商品，自选商品
        /// </summary>
        /// <returns></returns>
        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageGoodsKey })]
        public ActionResult SpecialList()
        {
            return View();
        }
        /// <summary>
        /// 分页加载特殊商品
        /// </summary>
        /// <returns></returns>
        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageGoodsKey })]
        public ActionResult SpecialOnpage()
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

            var specialType = Request.Get("extra_search[SpecialType]");
            var checkSpecialType = string.IsNullOrWhiteSpace(specialType);

            Expression<Func<Goods, bool>> expression =
                l => (checkName || l.Name.ToString().Contains(name)) &&
                     (checkGoodsNo || l.GoodsNo.Contains(goodsNo)) &&
                     (checkStatus || ((int)l.Status).ToString().Equals(status)) &&
                      (checkSpecialType || ((int)l.SpecialType).ToString().Equals(specialType)) &&
                      (l.SpecialType != SpecialType.General) &&
                     l.Status != GoodsStatus.Delete;

            //分页查询
            var list = _currencyService.GetListPaged<Goods>(pageIndex, pageSize, expression, out totalCount, new OrderModelField { PropertyName = sortColumn, IsDesc = isDesc });

            result.data = list;
            result.recordsTotal = totalCount;
            result.recordsFiltered = totalCount;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageGoodsKey })]
        public ActionResult SpecialEdit(Guid? id = null)
        {
            Goods goods = null;
            if (id != null && id != Guid.Empty)
            {
                goods = _currencyService.GetSingleByConditon<Goods>(a => a.Id == id && a.SpecialType != SpecialType.General && a.Status != GoodsStatus.Delete);
                //问号加上为了排除其为空
                goods.MainImage = _storageFileService.GetFiles(goods.Id, MallModule.Key, "MainImage").FirstOrDefault()?.Simplified();
            }
            if (goods == null)
            {
                goods = new Goods
                {
                    Id = KeyGenerator.GetGuidKey()
                };
            }
            return View(goods);
        }
     
        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageGoodsKey })]
        [System.Web.Mvc.ValidateInput(false)]
        public ActionResult SpecialEditOnPost(SpecialGoodsViewModel postGoods)
        {
            var result = new DataJsonResult();
            var goods = _currencyService.GetSingleById<Goods>(postGoods.Id);
            var isNew = false;
            if (goods == null)
            {
                goods = new Goods
                {
                    Id = postGoods.Id,
                    Name = postGoods.Name,
                    ExchangeIntegral = postGoods.ExchangeIntegral,
                    ShopPrice = postGoods.ShopPrice,
                    Stock = postGoods.Stock,
                    GoodsNo = postGoods.GoodsNo,
                    Description = postGoods.Description,
                    FreeShipping = postGoods.FreeShipping,
                    Status = GoodsStatus.NotInSale,
                    CreateTime = DateTime.Now,
                    SpecialType = postGoods.SpecialType
                };
                goods.LastUpdateTime = goods.CreateTime;
                isNew = true;
            }
            else
            {
                goods.Name = postGoods.Name;
                goods.ExchangeIntegral = postGoods.ExchangeIntegral;
                goods.ShopPrice = postGoods.ShopPrice;
                goods.Stock = postGoods.Stock;
                goods.GoodsNo = postGoods.GoodsNo;
                goods.FreeShipping = postGoods.FreeShipping;
                goods.Description = postGoods.Description;
                goods.LastUpdateTime = DateTime.Now;
                goods.SpecialType = postGoods.SpecialType;
            }
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