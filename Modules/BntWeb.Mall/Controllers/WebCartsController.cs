using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BntWeb.Data.Services;
using BntWeb.FileSystems.Media;
using BntWeb.Logging;
using BntWeb.Mall.Models;
using BntWeb.Mall.Services;
using BntWeb.Mall.ViewModels;
using BntWeb.MemberBase.Services;
using BntWeb.Mvc;
using BntWeb.Security;
using BntWeb.Utility.Extensions;


namespace BntWeb.Mall.Controllers
{
    public class WebCartsController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ICurrencyService _currencyService;
        private readonly IGoodsService _goodsService;
        private readonly IStorageFileService _storageFileService;
        private readonly IMemberContainer _memberContainer;
        public WebCartsController(IUserContainer userContainer, ICartService cartService, 
            ICurrencyService currencyService, IGoodsService goodsService,
            IStorageFileService storageFileService, IMemberContainer memberContainer)
        {
            _cartService = cartService;
            _currencyService = currencyService;
            _goodsService = goodsService;
            _storageFileService = storageFileService;
            _memberContainer = memberContainer;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }
        /// <summary>
        /// 获取购物车列表
        /// </summary>
        /// <returns></returns>
  [MemberAuthorize]
        public ActionResult WebCartsList()
        {
            //获得当前用户
            var currentMember = _memberContainer.CurrentMember;
            //购物车列表
            var myCarts= _cartService.GetList(currentMember.Id);
            //商品主图
            if (myCarts != null)
            {
                foreach (var item in myCarts)
                {
                    if (item.SingleGoodsId != Guid.Empty)
                    {
                        bool isInvalid = _goodsService.CheckSingleGoodsIsInvalid(item.SingleGoodsId, item.Price);
                        item.Status = isInvalid ? CartStatus.Invalid : CartStatus.Normal;
                    }
                  
                 
                    item.MainImage = 
                        _storageFileService.GetFiles(item.GoodsId, MallModule.Key, "MainImage").
                        FirstOrDefault()?
                        .Simplified();
                    item.TotalMoney = item.Price*item.Quantity;
                }
            }
            ViewBag.MyCarts = myCarts;
            //自选商品
            var optionalGoods =_goodsService.GetOptionalGoods();
            ViewBag.OptionalGoods = optionalGoods;
            return View();
        }
      
        /// <summary>
        /// 添加购物车
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>
     [MemberAuthorize]
        public ActionResult WebAddCart(WebCreateCartModel editModel)
        {
            var result = new DataJsonResult();
            var currentMember = _memberContainer.CurrentMember;
            if (editModel.GoodsId == null || editModel.GoodsId == Guid.Empty)
                throw new Exception("缺少商品Id参数");
            if (editModel.SingleGoodsId == null || editModel.SingleGoodsId == Guid.Empty)
                throw new Exception("缺少单品Id参数");
            if (editModel.Quantity < 1)
                throw new Exception("数量错误");

            var goods = _currencyService.GetSingleById<Goods>(editModel.GoodsId);
            if (goods == null)
                throw new Exception("无该商品信息");

            var singleGoods = _goodsService.LoadFullSingleGoods(editModel.SingleGoodsId);
            if (singleGoods == null)
                throw new Exception("无该单品信息");
            
            var model = _currencyService.GetSingleByConditon<Cart>(me => me.MemberId == currentMember.Id && me.GoodsId == editModel.GoodsId && me.SingleGoodsId == editModel.SingleGoodsId);
            if (model == null)
            {
                model = new Cart
                {
                    GoodsId = editModel.GoodsId.ToString().ToGuid(),
                    SingleGoodsId = editModel.SingleGoodsId.ToString().ToGuid(),
                    Quantity = editModel.Quantity,
                    MemberId = currentMember.Id,
                    GoodsAttribute = string.Join(",", singleGoods.Attributes.Select(me => me.AttributeValue).ToList()),
                    SingleGoodsNo = singleGoods.SingleGoodsNo,
                    Price = singleGoods.Price,
                    Unit = singleGoods.Unit,
                    Status = CartStatus.Normal,
                    GoodsName = goods.Name,
                    GoodsNo = goods.GoodsNo,
                    FreeShipping = goods.FreeShipping
                };

                model = _cartService.Create(model);

                if (model.Id == Guid.Empty)
                    throw new BntWebCoreException("添加失败,内部执出错");
            }
            else
            {
                model.Quantity = model.Quantity + editModel.Quantity;
                _currencyService.Update(model);
            }
            if (editModel.Flag == "1")
            {
                var purchaseGoods = _currencyService.GetSingleById<Goods>(editModel.PurchaseId);
                var purchasseModel= _currencyService.GetSingleByConditon<Cart>(me => me.MemberId == currentMember.Id && me.GoodsId == editModel.PurchaseId);
                if (purchasseModel == null)
                {
                    model = new Cart
                    {
                        GoodsId = editModel.PurchaseId.ToString().ToGuid(),
                        Quantity =1,
                        MemberId = currentMember.Id,
                        Price = purchaseGoods.ShopPrice,
                        Unit ="个",
                        Status = CartStatus.Normal,
                        GoodsName = purchaseGoods.Name,
                        GoodsNo = purchaseGoods.GoodsNo,
                        FreeShipping = purchaseGoods.FreeShipping
                    };

                    model = _cartService.Create(model);

                    if (model.Id == Guid.Empty)
                        throw new BntWebCoreException("添加失败,内部执出错");
                }
                else
                {
                    model.Quantity = model.Quantity + editModel.Quantity;
                    _currencyService.Update(model);
                }
            }

            return Json(result);
        }
        /// <summary>
        /// 删除购物车(单个或多个删除)
        /// </summary>
        /// <param name="cartIds"></param>
    
        public ActionResult DeleteCart(string cartIds)
        {
            var result = new DataJsonResult();
         
            if (string.IsNullOrWhiteSpace(cartIds))
                throw new Exception("无效Id");
            var arrayList = SplitStringWithComma(cartIds);
            if (arrayList.Length != 0)
            {
                for(int i=0;i<arrayList.Length;i++)
                {
                    var cart = _currencyService.GetSingleById<Cart>(arrayList[i].ToGuid());
                    if (cart == null)
                        throw new Exception("购物车无该商品");
                    var id = arrayList[i].ToString();
                    if (_currencyService.DeleteByConditon<Cart>(me => me.Id.ToString() == id) < 1)
                        throw new Exception("内部删除错误");

                }

            }
               
            return Json(result);
        }
        //分割以逗号拼接的字符串
        private static string[] SplitStringWithComma(string splitStr)
        {
            var newstr = string.Empty;
            List<string> sList = new List<string>();

            bool isSplice = false;
            string[] array = splitStr.Split(new char[] { ',' });
            foreach (var str in array)
            {
                if (!string.IsNullOrEmpty(str) && str.IndexOf('"') > -1)
                {
                    var firstchar = str.Substring(0, 1);
                    var lastchar = string.Empty;
                    if (str.Length > 0)
                    {
                        lastchar = str.Substring(str.Length - 1, 1);
                    }
                    if (firstchar.Equals("\"") && !lastchar.Equals("\""))
                    {
                        isSplice = true;
                    }
                    if (lastchar.Equals("\""))
                    {
                        if (!isSplice)
                            newstr += str;
                        else
                            newstr = newstr + "," + str;

                        isSplice = false;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(newstr))
                        newstr += str;
                }

                if (isSplice)
                {
                    //添加因拆分时丢失的逗号  
                    if (string.IsNullOrEmpty(newstr))
                        newstr += str;
                    else
                        newstr = newstr + "," + str;
                }
                else
                {
                    /* sList.Add(newstr.Replace("\"", "").Trim());*///去除字符中的双引号和首尾空格
                    sList.Add(newstr.Replace("\"", "").Trim());
                    newstr = string.Empty;
                }
            }
            return sList.ToArray();
        }
        /// <summary>
        ///  编辑单个购物车
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>

        public ActionResult WebEditCart(WebEditCartModel editModel)
        {
            var result = new DataJsonResult();
            if (editModel.CartId == Guid.Empty)
                throw new BntWebCoreException("无效Id");
            if (editModel.Quantity < 1)
                throw new BntWebCoreException("无效数量");

            var cart = _currencyService.GetSingleById<Cart>(editModel.CartId);
            if (cart == null)
                throw new BntWebCoreException("购物车无该商品");

            cart.Quantity = editModel.Quantity;

            if (!_currencyService.Update(cart))
                throw new BntWebCoreException("内部保存错误");

            return Json(result);
        }
        /// <summary>
        /// 清除购物车无效商品
        /// </summary>
        /// <returns></returns>
        [MemberAuthorize]
        public ActionResult WebClearCart()
        {
            var result = new DataJsonResult();
            var currentMember = _memberContainer.CurrentMember;
            _cartService.DeleteMemberCartInvalidGoods(currentMember.Id);
            return Json(result);
        }
    }
}