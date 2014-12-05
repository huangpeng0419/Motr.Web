using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Motr.Web.Util
{
    public class ControlHelper
    {
        /// <summary>
        /// 获取绝对路径
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static String ResolveUrl(String url)
        {
            if (String.IsNullOrEmpty(url)) return url;
            if (url.StartsWith("~")) return VirtualPathUtility.ToAbsolute(url);
            return url;
        }
    }
}
