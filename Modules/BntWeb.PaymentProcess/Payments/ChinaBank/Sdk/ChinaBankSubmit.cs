/* 
    ======================================================================== 
        File name：		ChinaBankSubmit
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/12/9 11:48:14
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BntWeb.PaymentProcess.Payments.ChinaBank.Sdk
{
    public class ChinaBankSubmit
    {
        #region 字段
        //支付网关
        private string GATEWAY_NEW = "https://tmapi.jdpay.com/PayGate";
        //商户号
        private readonly string v_mid;
        //编码格式
        private readonly string _inputCharset= "UTF-8";
        //商户的MD5私钥
        private readonly string key;
        
        #endregion
        
        public ChinaBankSubmit() { }

        /// <summary>
        /// 建立请求，以表单HTML形式构造（默认）
        /// </summary>
        /// <param name="sParaTemp">请求参数数组</param>
        /// <param name="strMethod">提交方式。两个值可选：post、get</param>
        /// <param name="strButtonValue">确认按钮显示文字</param>
        /// <returns>提交表单HTML文本</returns>
        public string BuildRequest(SortedDictionary<string, string> sParaTemp, string strMethod, string strButtonValue)
        {
            ////待请求参数数组
            //Dictionary<string, string> dicPara = new Dictionary<string, string>();
            //dicPara = sParaTemp;

            StringBuilder sbHtml = new StringBuilder();

            sbHtml.Append("<form id='chinabanksubmit' name='chinabanksubmit' action='" + GATEWAY_NEW + "?encoding=" + _inputCharset + "' method='" + strMethod.ToLower().Trim() + "'>");

            foreach (KeyValuePair<string, string> temp in sParaTemp)
            {
                sbHtml.Append("<input type='hidden' name='" + temp.Key + "' value='" + temp.Value + "'/>");
            }

            //submit按钮控件请不要含有name属性
            sbHtml.Append("<input type='submit' value='" + strButtonValue + "' style='display:none;'></form>");

            sbHtml.Append("<script>document.forms['chinabanksubmit'].submit();</script>");

            return sbHtml.ToString();
        }
    }
}