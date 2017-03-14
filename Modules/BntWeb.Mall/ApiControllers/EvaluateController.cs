using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BntWeb.Data.Services;
using BntWeb.Evaluate.Services;
using BntWeb.FileSystems.Media;
using BntWeb.Mall.ApiModels;
using BntWeb.Mall.Models;
using BntWeb.Mall.Services;
using BntWeb.WebApi.Models;

namespace BntWeb.Mall.ApiControllers
{
    public class EvaluateController : BaseApiController
    {
        private readonly IEvaluateService _evaluateService;
        private readonly ICurrencyService _currencyService;
        private readonly IStorageFileService _storageFileService;
        private readonly IGoodsService _goodsService;

        public EvaluateController(IGoodsService goodsService,IEvaluateService evaluateService, ICurrencyService currencyService, IStorageFileService storageFileService)
        {
            _goodsService = goodsService;
            _evaluateService = evaluateService;
            _currencyService = currencyService;
            _storageFileService = storageFileService;
        }

        /// <summary>
        /// 获取商品评价列表分页
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="pageNo"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult GetEvaluatesList(Guid goodsId, int pageNo = 1, int limit = 10)
        {
            int totalCount=0;
            var goods =
                _currencyService.GetSingleByConditon<Goods>(a => a.Id == goodsId && a.SpecialType == SpecialType.General);
            var evaluates = new List<Evaluate.Models.Evaluate>();
            if (goods == null)
            {
                evaluates = _goodsService.GetGoodsEvalListByPage(goodsId, pageNo, limit, out totalCount);
            }
            else
            {
                 evaluates = _goodsService.GetGoodsEvaluatesListByPage(goodsId, pageNo, limit, out totalCount); 
            }
           
            ApiResult result = new ApiResult();
            var data = new
            {
                TotalCount=totalCount,
                Evaluates= evaluates.Select(x=>new GoodsEvaluateModel(x))
            };
            result.SetData(data);
            return result;
        }

        /// <summary>
        /// 获取商品最新九评价用户头像
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult GetNew9EvaluatesMember(Guid goodsId)
        {
            var evaluates = _goodsService.GetNew9Evaluates(goodsId);
            ApiResult result = new ApiResult();
            result.SetData(evaluates.Select(x=>new EvaluateMemberAvatarModel(x)));
            return result;
        }
    }
}
