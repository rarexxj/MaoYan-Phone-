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

using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using BntWeb.Environment.Configuration;
using BntWeb.Mvc.Routes;

namespace BntWeb.MemberCenter
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
            return new[] {
                #region Admin
             
                new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         "WeiXin/Login/{action}",
                                                         new RouteValueDictionary {
                                                                                      { "area", MemberCenterModule.Area},
                                                                                      { "controller", "WeiXin"},
                                                                                      { "action", "LoginOAuth"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", MemberCenterModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Members/Page",
                                                         new RouteValueDictionary {
                                                                                      { "area", MemberCenterModule.Area},
                                                                                      { "controller", "Admin"},
                                                                                      { "action", "ListOnPage"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", MemberCenterModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Members/{action}",
                                                         new RouteValueDictionary {
                                                                                      { "area", MemberCenterModule.Area},
                                                                                      { "controller", "Admin"},
                                                                                      { "action", "List"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", MemberCenterModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                                #endregion
                                   #region Web
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         "Login",
                                                         new RouteValueDictionary {
                                                                                      { "area", MemberCenterModule.Area},
                                                                                      { "controller", "WebMember"},
                                                                                      { "action", "WebLogin"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", MemberCenterModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                               new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         HostConstConfig.AdminDirectory + "/Member/LoginWithSms",
                                                         new RouteValueDictionary {
                                                                                      { "area", MemberCenterModule.Area},
                                                                                      { "controller", "WebMember"},
                                                                                      { "action", "LoginWithSms"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", MemberCenterModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                                  new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         "LogOff",
                                                         new RouteValueDictionary {
                                                                                      { "area", MemberCenterModule.Area},
                                                                                      { "controller", "WebMember"},
                                                                                      { "action", "LogOff"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", MemberCenterModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            
                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         "WebRegister",
                                                         new RouteValueDictionary {
                                                                                      { "area", MemberCenterModule.Area},
                                                                                      { "controller", "WebMember"},
                                                                                      { "action", "WebRegister"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", MemberCenterModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                                                          new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         "WebPersonal",
                                                         new RouteValueDictionary {
                                                                                      { "area", MemberCenterModule.Area},
                                                                                      { "controller", "WebMember"},
                                                                                      { "action", "WebPersonal"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", MemberCenterModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                                                             new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         "ChangePhoneNumber",
                                                         new RouteValueDictionary {
                                                                                      { "area", MemberCenterModule.Area},
                                                                                      { "controller", "WebMember"},
                                                                                      { "action", "ChangePhoneNumber"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", MemberCenterModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                                                                  new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         "WebResetPassword",
                                                         new RouteValueDictionary {
                                                                                      { "area", MemberCenterModule.Area},
                                                                                      { "controller", "WebMember"},
                                                                                      { "action", "ForgetPassword"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", MemberCenterModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                                                                       new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         "PhoneIsExist",
                                                         new RouteValueDictionary {
                                                                                      { "area", MemberCenterModule.Area},
                                                                                      { "controller", "WebMember"},
                                                                                      { "action", "PhoneIsExist"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", MemberCenterModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },

                                                                       new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         "ImageVerify",
                                                         new RouteValueDictionary {
                                                                                      { "area", MemberCenterModule.Area},
                                                                                      { "controller", "WebMember"},
                                                                                      { "action", "ImageVerify"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", MemberCenterModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                                                
                              new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         "Member/{action}",
                                                         new RouteValueDictionary {
                                                                                      { "area", MemberCenterModule.Area},
                                                                                      { "controller", "WebMember"},
                                                                                      { "action", "LogOn"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", MemberCenterModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 }
                              ,  new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         "MemberAddressList",
                                                         new RouteValueDictionary {
                                                                                      { "area", MemberCenterModule.Area},
                                                                                      { "controller", "WebMemberAddress"},
                                                                                      { "action", "WebMemberAddressList"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", MemberCenterModule.Area}
                                                                                  },
                                                         new MvcRouteHandler()) 
                                                 },
                                   new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         "WebCreateAddress",
                                                         new RouteValueDictionary {
                                                                                      { "area", MemberCenterModule.Area},
                                                                                      { "controller", "WebMemberAddress"},
                                                                                      { "action", "WebCreateAddress"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", MemberCenterModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                                    new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         "WebDeleteAddress",
                                                         new RouteValueDictionary {
                                                                                      { "area", MemberCenterModule.Area},
                                                                                      { "controller", "WebMemberAddress"},
                                                                                      { "action", "WebDeleteAddress"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", MemberCenterModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                                      new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         "WebEditAddress",
                                                         new RouteValueDictionary {
                                                                                      { "area", MemberCenterModule.Area},
                                                                                      { "controller", "WebMemberAddress"},
                                                                                      { "action", "WebEditAddress"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", MemberCenterModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 }
                                      ,
                                      new RouteDescriptor {
                                                     Priority = 0,
                                                     Route = new Route(
                                                         "WebSetDefaultAddress",
                                                         new RouteValueDictionary {
                                                                                      { "area", MemberCenterModule.Area},
                                                                                      { "controller", "WebMemberAddress"},
                                                                                      { "action", "SetDefaultAddress"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", MemberCenterModule.Area}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 }
                           
                            #endregion
                         };
        }

    }
}