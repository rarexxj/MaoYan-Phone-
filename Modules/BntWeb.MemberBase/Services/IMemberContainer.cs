﻿/* 
    ======================================================================== 
        File name：        IMemberContainer
        Module:                
        Author：            罗嗣宝
        Create Time：    2016/6/20 9:33:50
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.MemberBase.Models;

namespace BntWeb.MemberBase.Services
{
    public interface IMemberContainer : IDependency
    {
        string UserName { set; get; }
        Member CurrentMember { get; }

        Member GetMember(HttpContextBase httpContext);
    }
}