using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BntWeb.Environment.Configuration;
using BntWeb.Mvc.Routes;

namespace BntWeb.Coupon
{
    public class Routes: IRouteProvider
    {
        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[] {
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "Coupon/{action}",
                                                         new RouteValueDictionary {
                                                                                      { "area", CouponModule.Area},
                                                                                      { "controller", "Coupon"},
                                                                                      { "action", "List"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", CouponModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                              new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         "CouponList",
                                                         new RouteValueDictionary {
                                                                                      { "area", CouponModule.Area},
                                                                                      { "controller", "WebCoupon"},
                                                                                      { "action", "WebList"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", CouponModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                                  new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         "GetCoupon",
                                                         new RouteValueDictionary {
                                                                                      { "area", CouponModule.Area},
                                                                                      { "controller", "WebCoupon"},
                                                                                      { "action", "GetCoupon"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", CouponModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                                        new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         "MyCoupons",
                                                         new RouteValueDictionary {
                                                                                      { "area", CouponModule.Area},
                                                                                      { "controller", "WebCoupon"},
                                                                                      { "action", "MyCoupon"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", CouponModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 }

                                        
                         };
        }
    }
}