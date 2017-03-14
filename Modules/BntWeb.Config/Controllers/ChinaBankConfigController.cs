using System.Web.Mvc;
using BntWeb.Config.Models;
using BntWeb.Mvc;
using BntWeb.Security;
using BntWeb.Services;

namespace BntWeb.Config.Controllers
{
    public class ChinaBankConfigController : Controller
    {
        private readonly IConfigService _configService;

        public ChinaBankConfigController(IConfigService configService)
        {
            _configService = configService;
        }


        [AdminAuthorize(PermissionsArray = new[] { Permissions.SystemConfigKey })]
        public ActionResult Config()
        {
            var config = _configService.Get<ChinaBankConfig>();
            return View(config);
        }

        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.SystemConfigKey })]
        public ActionResult SaveConfig(ChinaBankConfig configViewModel)
        {
            var result = new DataJsonResult();
            if (!_configService.Save(configViewModel))
            {
                result.ErrorMessage = "异常错误，配置文件保存失败";
            }
            return Json(result);
        }
    }
}
