/* 
    ======================================================================== 
        File name：		Routes
        Module:			
        Author：		罗嗣宝
        Create Time：		2016/7/7 9:13:28
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using BntWeb.Environment.Configuration;
using BntWeb.Mvc.Routes;


namespace BntWeb.OrderProcess
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
                #region

                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        HostConstConfig.AdminDirectory + "/Orders/Page",
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area},
                            {"controller", "Admin"},
                            {"action", "ListOnPage"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        HostConstConfig.AdminDirectory + "/Orders/{action}",
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area},
                            {"controller", "Admin"},
                            {"action", "List"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        HostConstConfig.AdminDirectory + "/OrdersRefund/Page",
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area},
                            {"controller", "Refund"},
                            {"action", "ListOnPage"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        HostConstConfig.AdminDirectory + "/OrdersRefund/{action}",
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area},
                            {"controller", "Refund"},
                            {"action", "List"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        HostConstConfig.AdminDirectory + "/OrdersEvaluates/{action}",
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area},
                            {"controller", "Evaluate"},
                            {"action", "Detail"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        HostConstConfig.AdminDirectory + "/OrdersReminders/{action}",
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area},
                            {"controller", "DeliveryReminder"},
                            {"action", "List"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area}
                        },
                        new MvcRouteHandler())
                },

                #endregion
                #region Web
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "RefundType",
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area},
                            {"controller", "WebRefund"},
                            {"action", "RefundType"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "AllRefund",
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area},
                            {"controller", "WebRefund"},
                            {"action", "AllRefund"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area}
                        },
                        new MvcRouteHandler())
                },
                  new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "AllRefund",
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area},
                            {"controller", "WebRefund"},
                            {"action", "WebRevokeApply"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area}
                        },
                        new MvcRouteHandler())
                },
                
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "WebApplyRefund",
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area},
                            {"controller", "WebRefund"},
                            {"action", "WebApplyRefund"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "Pay",
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area},
                            {"controller", "WebOrder"},
                            {"action", "PayType"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "OrderDetail/{orderId}",
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area},
                            {"controller", "WebOrder"},
                            {"action", "OrderDetails"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "OrderDelete/{orderId}",
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area},
                            {"controller", "WebOrder"},
                            {"action", "WebDeleteOrder"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "OrderCancel/{orderId}",
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area},
                            {"controller", "WebOrder"},
                            {"action", "WebCancelOrder"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "myOrder",
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area},
                            {"controller", "WebOrder"},
                            {"action", "WebOrderList"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "Remind/{orderId}",
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area},
                            {"controller", "WebOrder"},
                            {"action", "WebRemind"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "WebEvaluate",
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area},
                            {"controller", "WebEvaluate"},
                            {"action", "WebEvaluateList"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area}
                        },
                        new MvcRouteHandler())
                }
                ,
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "WebSubmitEvaluate",
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area},
                            {"controller", "WebEvaluate"},
                            {"action", "WebSubmitEvaluate"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area}
                        },
                        new MvcRouteHandler())
                },
                    new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "LookEvaluate",
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area},
                            {"controller", "WebEvaluate"},
                            {"action", "LookEvaluate"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area}
                        },
                        new MvcRouteHandler())
                },
   new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "Delete",
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area},
                            {"controller", "WebEvaluate"},
                            {"action", "Delete"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area}
                        },
                        new MvcRouteHandler())
                }
                ,
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "WebApplyInfo",
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area},
                            {"controller", "WebRefund"},
                            {"action", "WebApplyInfo"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "Order/Complete",
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area},
                            {"controller", "WebOrder"},
                            {"action", "WebCompleteOrder"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", OrderProcessModule.Area}
                        },
                        new MvcRouteHandler())
                }
                
                                        
                #endregion
            };

        }
    }
}