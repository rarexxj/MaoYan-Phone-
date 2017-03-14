/* 
    ======================================================================== 
        File name：        Routes
        Module:                
        Author：            罗嗣宝
        Create Time：    2016/4/29 8:47:08
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BntWeb.Environment.Configuration;
using BntWeb.Mall;
using BntWeb.Mvc.Routes;

namespace BntWeb.Home
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
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "",
                        new RouteValueDictionary
                        {
                            {"area", "Home"},
                            {"controller", "Home"},
                            {"action", "Index"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", "Home"}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "Head",
                        new RouteValueDictionary
                        {
                            {"area", "Home"},
                            {"controller", "Home"},
                            {"action", "Head"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", "Home"}
                        },
                        new MvcRouteHandler())
                },


                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "Footer",
                        new RouteValueDictionary
                        {
                            {"area", "Home"},
                            {"controller", "Home"},
                            {"action", "Footer"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", "Home"}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "Float",
                        new RouteValueDictionary
                        {
                            {"area", "Home"},
                            {"controller", "Home"},
                            {"action", "Float"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", "Home"}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "PagePartial",
                        new RouteValueDictionary
                        {
                            {"area", "Home"},
                            {"controller", "Home"},
                            {"action", "PagePartial"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", "Home"}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor
                {
                    Priority = 0,
                    Route = new Route(
                        "Personal",
                        new RouteValueDictionary
                        {
                            {"area", "Home"},
                            {"controller", "Home"},
                            {"action", "Personal"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary
                        {
                            {"area", "Home"}
                        },
                        new MvcRouteHandler())
                }

            };
        }

    }
}