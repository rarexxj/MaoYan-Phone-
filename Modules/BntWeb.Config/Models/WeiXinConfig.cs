/* 
    ======================================================================== 
        File name：        WeiXinConfig
        Module:                
        Author：            罗嗣宝
        Create Time：    2016/7/27 13:42:49
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/

using System;
using System.Collections.Generic;
using BntWeb.FileSystems.Media;

namespace BntWeb.Config.Models
{
    public class WeiXinConfig
    {
        public string AppId
        {
            get; set;
        }

        public string AppSecret
        {
            get; set;
        }

        public string MchId
        {
            get; set;
        }

        public string Key
        {
            get; set;
        }

    }
}