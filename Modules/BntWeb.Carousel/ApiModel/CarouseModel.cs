using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using Autofac;
using BntWeb.Carousel.Models;
using BntWeb.Environment;
using BntWeb.Environment.Configuration;
using BntWeb.FileSystems.Media;

namespace BntWeb.Carousel.ApiModel
{
    public class CarouseModel
    {
        /// <summary>
        /// 内容标题
        /// </summary>
        public string SourceTitle { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 内容查看地址
        /// </summary>
        public string HrefUrl { get; set; }

        /// <summary>
        /// 短地址
        /// </summary>
        public string ShotUrl { get; set; }

        /// <summary>
        /// 轮播图
        /// </summary>
        public SimplifiedStorageFile CoverImage { get; set; }

        public CarouseModel(CarouselItem model)
        {
            SourceTitle = model.SourceTitle;
            Summary = model.Summary;
            ShotUrl = model.ShotUrl;
            CoverImage = model.CoverImage.Simplified();
            if (!string.IsNullOrWhiteSpace(model.ShotUrl))
            {
                var arr = model.ShotUrl.Split('|');
                if (arr.Length == 2)
                {
                    var appConfigurationAccessor = HostConstObject.Container.Resolve<IAppConfigurationAccessor>();
                    var hostUrl = appConfigurationAccessor.GetConfiguration("HostUrl");
                    switch (arr[0].ToLower())
                    {
                        case "goods":
                            HrefUrl = hostUrl + "/Product/Info/" + arr[1];
                            break;
                        case "article":
                            HrefUrl = hostUrl + "/College/Detail/" + arr[1];
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}