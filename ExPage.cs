using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Motr.Web.Script;

namespace Motr.Web
{
    public class ExPage : Page
    { 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        public void ResponseString(String str)
        {
            var res = ExHttp.Response;
            res.Clear();
            res.Write(str);
            res.End();
        }
        #region 输出脚本
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public void Alert(String msg)
        {
            ScriptMgr.Alert(msg);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="url"></param>
        public void AlertAndRedirect(String msg, String url)
        {
            ScriptMgr.AlertAndRedirect(msg, url);
        }
        #endregion
    }
}
