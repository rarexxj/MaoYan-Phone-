using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BntWeb.ContentMarkup.Models;
using BntWeb.ContentMarkup.Services;
using BntWeb.Data.Services;
using BntWeb.Mall.ApiModels;
using BntWeb.Mall.Services;
using BntWeb.Mvc;
using BntWeb.Security;
using BntWeb.Web.Extensions;


namespace BntWeb.Mall.Controllers
{
    public class WebCollectController : Controller
    {
    
            private readonly IMarkupService _markupService;
            private readonly IGoodsService _goodsService;
        private readonly IUserContainer _userContainer;
        public WebCollectController(IUserContainer userContainer, IMarkupService markupService, IGoodsService goodsService)
            {
                _markupService = markupService;
                _goodsService = goodsService;
               _userContainer = userContainer;
        }
            // GET: WebCollect
            /// <summary>
            /// 获得我收藏的商品列表
            /// </summary>
            /// <returns></returns>
            public ActionResult WebCollectList()
        {
            return View();
        }
        /// <summary>
        /// 获得我收藏的商品列表
        /// </summary>
        /// <returns></returns>
        [AdminAuthorize]
        public ActionResult WebCollectOnPage()
        {
            var result = new DataTableJsonResult();
            //取参数值
            int draw, pageIndex, pageSize, totalCount;
            string sortColumn;
            bool isDesc;
            Request.GetDatatableParameters(out draw, out pageIndex, out pageSize, out sortColumn, out isDesc);
            result.draw = draw;
            var currentUser = _userContainer.CurrentUser;
            result.data = _goodsService.LoadCollectGoodsByPage(currentUser.Id, pageIndex, pageSize, out totalCount).Select(item => new CollectListModel(item)).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 收藏商品
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public ActionResult WebCreateCollect(Guid goodsId)
        {
            var result = new DataTableJsonResult();
            var currentUser = _userContainer.CurrentUser;
            if (goodsId.Equals(Guid.Empty))
                throw new BntWebCoreException("商品Id不合法");
            if (_markupService.MarkupExist(goodsId, MallModule.Key, currentUser.Id, MarkupType.Collect))
                throw new BntWebCoreException("已经收藏过了");

            _markupService.CreateMarkup(goodsId, MallModule.Key, currentUser.Id, MarkupType.Collect);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 取消收藏商品
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public ActionResult WebCancelCollect(Guid goodsId)
        {
            var result = new DataTableJsonResult();
            var currentUser = _userContainer.CurrentUser;
            if (goodsId.Equals(Guid.Empty))
                throw new BntWebCoreException("商品Id不合法");
            if (!_markupService.MarkupExist(goodsId, MallModule.Key, currentUser.Id, MarkupType.Collect))
                throw new BntWebCoreException("还没有收藏");

            _markupService.CancelMarkup(goodsId, MallModule.Key, currentUser.Id, MarkupType.Collect);
         
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}