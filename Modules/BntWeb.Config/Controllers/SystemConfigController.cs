using System;
using System.Linq;
using System.Web.Mvc;
using BntWeb.Config.Models;
using BntWeb.Data.Services;
using BntWeb.FileSystems.Media;
using BntWeb.Mvc;
using BntWeb.Security;
using BntWeb.Services;

namespace BntWeb.Config.Controllers
{
    public class SystemConfigController : Controller
    {
        private readonly IConfigService _configService;
        private readonly IStorageFileService _storageFileService;
        private readonly ICurrencyService _currencyService;
        public SystemConfigController(ICurrencyService currencyService, IConfigService configService, IStorageFileService storageFileService)
        {
            _configService = configService;
            _storageFileService = storageFileService;
            _currencyService = currencyService;
        }


        [AdminAuthorize(PermissionsArray = new[] { Permissions.SystemConfigKey })]
        public ActionResult Config()
        {
            var category= _currencyService.GetList<GoodCategory>(a=>a.ShowIndex);
            if(category!=null)
            ViewBag.Catwgory = category;
            var config = _configService.Get<SystemConfig>();
            return View(config);
        }

        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.SystemConfigKey })]
        public ActionResult SaveConfig(SystemConfigViewModel configViewModel)
        {
            var result = new DataJsonResult();
            var config = new SystemConfig();

            config.StockWarning = configViewModel.StockWarning;
            config.CrashApplyOutTime = configViewModel.CrashApplyOutTime;
            config.ReduceStock = configViewModel.ReduceStock;
            config.RecommendIntegral = configViewModel.RecommendIntegral;
            config.ConsumptionIntegral = configViewModel.ConsumptionIntegral;
            config.DiscountRate = configViewModel.DiscountRate;
            config.MaxLevel = 3;//configViewModel.MaxLevel;
            config.Homeone = configViewModel.Homeone;
            config.Hometwo = configViewModel.Hometwo;
            config.Homethree = configViewModel.Homethree;

            var i = 1;
            foreach (var rate in configViewModel.Rates)
            {
                config.Rates.Add(rate);
            }

            //水印
            config.WaterMark.WaterMarkType = configViewModel.WaterMarkType;
            config.WaterMark.Opacity = configViewModel.Opacity;
            config.WaterMark.Position = configViewModel.Position;
            config.WaterMark.WaterMarkText = configViewModel.WaterMarkText;
          
            if (!configViewModel.WaterMarkImage.Equals(Guid.Empty) && _storageFileService.ReplaceFile(Guid.Empty, ConfigModule.Instance.InnerKey, ConfigModule.Instance.InnerDisplayName, configViewModel.WaterMarkImage, "WaterMarkImage"))
            {
                config.WaterMark.MarkImage = _storageFileService.GetFiles(Guid.Empty, ConfigModule.Instance.InnerKey, "WaterMarkImage").FirstOrDefault();
            }

            if (!_configService.Save(config))
            {
                result.ErrorMessage = "异常错误，配置文件保存失败";
            }
            return Json(result);
        }
    }
}