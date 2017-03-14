using System;
using System.Linq;
using System.Web.Http;
using BntWeb.ContentMarkup.Models;
using BntWeb.ContentMarkup.Services;
using BntWeb.Mall.ApiModels;
using BntWeb.Mall.Services;
using BntWeb.WebApi.Filters;
using BntWeb.WebApi.Models;

namespace BntWeb.Mall.ApiControllers
{
    public class CollectController : BaseApiController
    {
        private readonly IMarkupService _markupService;
        private readonly IGoodsService _goodsService;

        public CollectController(IMarkupService markupService, IGoodsService goodsService)
        {
            _markupService = markupService;
            _goodsService = goodsService;
        }
        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        public ApiResult CreateCollect(Guid goodsId)
        {
            if (goodsId.Equals(Guid.Empty))
                throw new WebApiInnerException("0001", "商品Id不合法");
            if (_markupService.MarkupExist(goodsId, MallModule.Key, AuthorizedUser.Id, MarkupType.Collect))
                throw new WebApiInnerException("0002", "已经收藏过了");

            _markupService.CreateMarkup(goodsId, MallModule.Key, AuthorizedUser.Id, MarkupType.Collect);
            return new ApiResult();
        }
       /// <summary>
       /// 删除收藏
       /// </summary>
       /// <param name="goodsId"></param>
       /// <returns></returns>
        [HttpDelete]
        [BasicAuthentication]
        public ApiResult CancelCollect(Guid goodsId)
        {
            if (goodsId.Equals(Guid.Empty))
                throw new WebApiInnerException("0001", "商品Id不合法");
            if (!_markupService.MarkupExist(goodsId, MallModule.Key, AuthorizedUser.Id, MarkupType.Collect))
                throw new WebApiInnerException("0002", "还没有收藏");

            _markupService.CancelMarkup(goodsId, MallModule.Key, AuthorizedUser.Id, MarkupType.Collect);
            return new ApiResult();
        }
        /// <summary>
        /// 获得收藏列表
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [BasicAuthentication]
        public ApiResult CollectList(int pageNo = 1, int limit = 10)
        {
            int totalCount;
            var list = _goodsService.LoadCollectGoodsByPage(AuthorizedUser.Id, pageNo, limit, out totalCount);

            var result = new ApiResult();
            var data = new
            {
                TotalCount = totalCount,
                Goods = list.Select(item => new CollectListModel(item)).ToList()
            };
            result.SetData(data);
            return result;
        }
    }
}
