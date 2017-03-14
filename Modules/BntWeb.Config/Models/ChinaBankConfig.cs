using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BntWeb.Config.Models
{
    public class ChinaBankConfig
    {
        /// <summary>
        /// 商务方案
        /// </summary>
        public string MchId
        {
            get; set;
        }
        /// <summary>
        /// MD5密钥
        /// </summary>
        public string MD5Key
        {
            get; set;
        }
    }
}
