using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using BntWeb.Mvc.Routes;
using BntWeb.WebApi.Routes;

namespace BntWeb.Coupon
{
    public class HttpRoutes : IHttpRouteProvider
    {
        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[] {
                            new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/Coupon",
                                                        Defaults = new
                                                        {
                                                            area = CouponModule.Area,
                                                            controller = "Coupon",
                                                            action = "GetCoupons"
                                                        }
                                                    },
                               new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/CouponList",
                                                        Defaults = new
                                                        {
                                                            area = CouponModule.Area,
                                                            controller = "Coupon",
                                                            action = "CouponList"
                                                        }
                                                    },
                                   new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/ReceiveCoupon/{memberId}",
                                                        Defaults = new
                                                        {
                                                            area = CouponModule.Area,
                                                            controller = "Coupon",
                                                            action = "ReceiveCoupon"
                                                        }
                                                    },
                               
                            new HttpRouteDescriptor {
                                                        Priority = 0,
                                                        RouteTemplate = "Api/v1/Coupon/Withdrawal",
                                                        Defaults = new
                                                        {
                                                            area = CouponModule.Area,
                                                            controller = "Coupon",
                                                            action = "GetWithdrawalCoupons"
                                                        }
                                                    }
                         };
        }

    }
}
