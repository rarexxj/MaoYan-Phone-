/* 
    ======================================================================== 
        File name：        AlipayConfig
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
    public class AlipayConfig
    {
        public string SellerId
        {
            get; set;
        }

        public string AccountName
        {
            get; set;
        }

        public string Partner
        {
            get; set;
        }

        public string MD5Key
        {
            get; set;
        }

    }
}