/* 
    ======================================================================== 
        File name：		Routes
        Module:			
        Author：            罗嗣宝
        Create Time：    2016/6/29 13:11:44
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using BntWeb.Environment.Configuration;
using BntWeb.Mvc.Routes;


namespace BntWeb.Mall
{
    public class Routes : IRouteProvider
    {
        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[]
            {
                #region Admin

                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        HostConstConfig.AdminDirectory + "/Mall/GoodsRecycle/Page",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "GoodsRecycle"},
                            {"action", "ListOnPage"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        HostConstConfig.AdminDirectory + "/Mall/GoodsRecycle/{action}",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "GoodsRecycle"},
                            {"action", "List"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        HostConstConfig.AdminDirectory + "/Mall/GoodsShortage/Page",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "GoodsShortage"},
                            {"action", "ListOnPage"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        HostConstConfig.AdminDirectory + "/Mall/GoodsShortage/{action}",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "GoodsShortage"},
                            {"action", "List"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        HostConstConfig.AdminDirectory + "/Mall/GoodsBrand/Page",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "GoodsBrand"},
                            {"action", "ListOnPage"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        HostConstConfig.AdminDirectory + "/Mall/GoodsBrand/{action}",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "GoodsBrand"},
                            {"action", "List"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        HostConstConfig.AdminDirectory + "/Mall/GoodsType/Page",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "GoodsType"},
                            {"action", "ListOnPage"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        HostConstConfig.AdminDirectory + "/Mall/GoodsType/Attribute/Page",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "Attribute"},
                            {"action", "ListOnPage"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        HostConstConfig.AdminDirectory + "/Mall/GoodsType/Attribute/{action}",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "Attribute"},
                            {"action", "List"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        HostConstConfig.AdminDirectory + "/Mall/GoodsType/{action}",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "GoodsType"},
                            {"action", "List"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        HostConstConfig.AdminDirectory + "/Mall/Goods/Page",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "Goods"},
                            {"action", "ListOnPage"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                },
                   
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        HostConstConfig.AdminDirectory + "/Mall/Goods/{action}",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "Goods"},
                            {"action", "List"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                },
                 new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        HostConstConfig.AdminDirectory + "/Mall/Special/SpecialPage",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "SpecialGoods"},
                            {"action", "SpecialOnpage"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                },
                   new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        HostConstConfig.AdminDirectory + "/Mall/Special/{action}",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "SpecialGoods"},
                            {"action", "SpecialList"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        HostConstConfig.AdminDirectory + "/Mall/GoodsCategory/{action}",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "GoodsCategory"},
                            {"action", "List"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                },

                #endregion 
                #region Web
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "goodsList",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "WebGoods"},
                            {"action", "GoodsList"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                },
                  new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "NotGood",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "WebGoods"},
                            {"action", "NotGood"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                },
                 
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "goodDetails/{goodId}",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "WebGoods"},
                            {"action", "GoodsDetails"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())

                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "HotGoods",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "WebGoods"},
                            {"action", "HotGoods"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "Exchange",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "WebGoods"},
                            {"action", "Exchange"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                },

                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "WebEditAddress",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "WebMemberAddress"},
                            {"action", "WebEditAddress"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "WebAddCart",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "WebCarts"},
                            {"action", "WebAddCart"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "WebCartsList",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "WebCarts"},
                            {"action", "WebCartsList"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                },

                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "DeleteCart",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "WebCarts"},
                            {"action", "DeleteCart"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "WebClearCart",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "WebCarts"},
                            {"action", "WebClearCart"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "MyCouponsList",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "WebSubmitOrder"},
                            {"action", "MyCouponsList"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "MyCouponsList",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "WebSubmitOrder"},
                            {"action", "MyCouponsList"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "ConfirmExchange",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "WebSubmitOrder"},
                            {"action", "ConfirmExchange"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "ConfirmOrderList",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "WebSubmitOrder"},
                            {"action", "ConfirmOrderList"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "Submit",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "WebSubmitOrder"},
                            {"action", "Submit"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                }
                ,
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "ExchangeOrder",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "WebSubmitOrder"},
                            {"action", "ExchangeOrder"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                }
                ,
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "WebBrowseList",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "WebBrowse"},
                            {"action", "WebBrowseList"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                }

                ,
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "WebBrowseClear",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "WebBrowse"},
                            {"action", "WebClearRecord"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                }

                ,
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "WebCollectionList",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "WebBrowse"},
                            {"action", "WebCollectionList"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                }

                ,
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "WebCollectionAdd",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "WebBrowse"},
                            {"action", "WebCollectionAdd"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                }

                ,
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "WebCollectionDel",
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area},
                            {"controller", "WebBrowse"},
                            {"action", "WebCollectionDel"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", MallModule.Area}
                        },
                        new MvcRouteHandler())
                }





                           
                #endregion
            };
        }
    }
}