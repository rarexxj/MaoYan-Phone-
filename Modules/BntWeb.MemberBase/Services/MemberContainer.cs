using System.Web;
using Autofac;
using BntWeb.Environment;
using BntWeb.MemberBase.Models;
using BntWeb.Security.Identity;

namespace BntWeb.MemberBase.Services
{
    public class MemberContainer : IMemberContainer
    {
        public string UserName { set; get; }
        public Member CurrentMember
        {
            get
            {
                var userManager = HostConstObject.Container.Resolve<DefaultUserManager>();
                if (HttpContext.Current.User != null && HttpContext.Current.User.Identity != null)
                {
                    UserName = HttpContext.Current.User.Identity.Name;
                }

                var user = userManager.FindByNameAsync(UserName)?.Result;
                if (user == null || user.UserType != Security.Identity.UserType.Member) return null;
                var memberService = HostConstObject.Container.Resolve<IMemberService>();
                if (user.UserType != UserType.Member)
                    return null;
                var member = memberService.FindMember(user);
                return member;
            }
        }

        public Member GetMember(HttpContextBase httpContext)
        {
            var userManager = HostConstObject.Container.Resolve<DefaultUserManager>();
            if (httpContext.User != null && httpContext.User.Identity != null)
            {
                UserName = httpContext.User.Identity.Name;
            }

            var user = userManager.FindByNameAsync(UserName)?.Result;
            if (user == null || user.UserType != Security.Identity.UserType.Member) return null;
            var memberService = HostConstObject.Container.Resolve<IMemberService>();
            if (user.UserType != UserType.Member)
                return null;
            var member = memberService.FindMember(user);
            return member;
        }
    }
}