
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using BntWeb.ContentMarkup.Models;
using BntWeb.ContentMarkup.Services;
using BntWeb.Environment;
using BntWeb.FileSystems.Media;
using BntWeb.Mall.ApiModels;
using BntWeb.Mall.Services;
using BntWeb.MemberBase.Services;
using BntWeb.Mvc;
using BntWeb.Security;
using BntWeb.Web.Extensions;
using BntWeb.WebApi.Models;

namespace BntWeb.Mall.Controllers
{
    public class WebBrowseController : Controller
    {
        private readonly IMarkupService _markupService;
        private readonly IGoodsService _goodsService;
        private readonly IMemberContainer _memberContainer;
        private readonly UrlHelper _urlHelper;
        private readonly IStorageFileService _storageFileService;
        public WebBrowseController(IMemberContainer memberContainer, IMarkupService markupService, UrlHelper urlHelper, IGoodsService goodsService, IStorageFileService storageFileService)
        {
            _memberContainer = memberContainer;
            _markupService = markupService;
            _urlHelper = urlHelper;
            _goodsService = goodsService;
            _storageFileService = storageFileService;
        }
        // GET: WebBrowse
        /// <summary>
        /// 获得我浏览的商品列表
        /// </summary>
        /// <returns></returns>
        [MemberAuthorize]
        public ActionResult WebBrowseList(int pageNo = 1, int pageSize = 8)
        {
            int totalCount;
            var list = _goodsService.LoadBrowseGoodsByPage(_memberContainer.CurrentMember.Id, pageNo, pageSize, out totalCount);
            ViewBag.List = list;
            var routeParas = new RouteValueDictionary{
                    { "area", "Mall"},
                    { "controller", "WebBrowse"},
                    { "action", "WebBrowseList"}
                };
            var returnUrl = HostConstObject.HostUrl + _urlHelper.RouteUrl(routeParas);

            ViewBag.Url = returnUrl + "?pageNo=[pageNo]";
            //获得总页数
            ViewBag.TotalPage = totalCount % pageSize == 0 ? totalCount / pageSize : totalCount / pageSize + 1;
            ViewBag.CurrentPage = pageNo;
            ViewBag.memberid = _memberContainer.CurrentMember.Id;
            return View();
        }
    
        /// <summary>
        /// 清空浏览历史
        /// </summary>
        /// <returns></returns>
        [MemberAuthorize]
        public ActionResult WebClearRecord()
        {
            var result = new DataTableJsonResult();
            var currentUser = _memberContainer.CurrentMember;
            _markupService.ClearMarkup(MallModule.Key, currentUser.Id, MarkupType.Browse);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        //我的收藏
        [MemberAuthorize]
        public ActionResult WebCollectionList(int pageNo = 1, int pageSize = 10)
        {
            int totalCount;

            _memberContainer.UserName = HttpContext.User.Identity.Name;
            var currentMember = _memberContainer.CurrentMember;
            var list = _goodsService.LoadCollectGoodsByPage(currentMember.Id, pageNo, pageSize, out totalCount);
            foreach (var item in list)
            {
                var mainImage = _storageFileService.GetFiles(item.Id, MallModule.Key, "MainImage").FirstOrDefault();
                item.MainImage = mainImage?.Simplified();
            }

            ViewBag.memberid = currentMember.Id;

            ViewBag.List = list;
            var routeParas = new RouteValueDictionary{
                    { "area", MallModule.Area},
                    { "controller", "WebBrowse"},
                    { "action", "WebCollectionList"}
                };
            
            var returnUrl = HostConstObject.HostUrl + _urlHelper.RouteUrl(routeParas);

            ViewBag.Url = returnUrl + "?pageNo=[pageNo]";
            //获得总页数
            ViewBag.TotalPage = totalCount % pageSize == 0 ? totalCount / pageSize : totalCount / pageSize + 1;
            ViewBag.CurrentPage = pageNo;
            return View();
        }

        //添加收藏
        [MemberAuthorize]
        public JsonResult WebCollectionAdd(Guid goodsId)
        {
            var code = "200";
            var msg = "";
            try
            {
                var memberId = _memberContainer.CurrentMember.Id;

                var result = _goodsService.AddGoodsCollection(goodsId, memberId);
                code = result ? "200" : "0";

            }
            catch (Exception ex)
            {
                code = "0";
                msg = ex.Message;

            }

            return Json(new { code = code, msg = msg }, JsonRequestBehavior.AllowGet);


        }

        //删除收藏
        [MemberAuthorize]
        public JsonResult WebCollectionDel(Guid goodsid, string memberid)
        {
            var code = "200";
            var msg = "";
            try
            {
                var memberId = _memberContainer.CurrentMember.Id;
                if (memberId != memberid)
                {
                    throw new Exception("用户信息异常");
                }
                var result = _goodsService.DelGoodsCollection(goodsid, memberid);
                code = result ? "200" : "0";

            }
            catch (Exception ex)
            {
                code = "0";
                msg = ex.Message;

            }

            return Json(new { code = code, msg = msg }, JsonRequestBehavior.AllowGet);

        }

    }
}