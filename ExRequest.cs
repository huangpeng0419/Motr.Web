using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections.Specialized;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

namespace Motr.Web
{
    public static class ExRequest
    {
        /// <summary>
        /// 获得查询字符串的值并转换为Int值
        /// </summary>
        /// <param name="key">名称</param>
        /// <param name="defaultVal">转换失败的默认值</param>
        /// <returns>值</returns>
        public static Int32 QueryInt(this HttpRequest req, String key, Int32 defaultVal)
        {
            String strVal = req.QueryString[key];
            Int32 Int32Val;
            if (Int32.TryParse(strVal, out Int32Val))
                return Int32Val;
            return defaultVal;
        }
        /// <summary>
        /// 获得查询字符串的值并转换为Int值(转换失败的默认值为 0)
        /// </summary>
        /// <param name="key">名称</param>
        /// <returns>值</returns>
        public static Int32 QueryInt(this HttpRequest req, String key)
        {
            return req.QueryInt(key, 0);
        }
        /// <summary>
        /// 当前页面名称
        /// </summary>
        public static String PageName(this HttpRequest req)
        {
            String absolutePath = req.Url.AbsolutePath;
            try { return absolutePath.Substring(absolutePath.LastIndexOf("/") + 1).ToLower(); }
            catch { return String.Empty; }
        }
        /// <summary>
        /// 基本路径
        /// </summary>
        public static String BaseUrl(this HttpRequest req)
        {
            if (req.ApplicationPath != "/")
                return "http://" + req.ServerVariables["HTTP_HOST"];
            return "http://" + req.ServerVariables["HTTP_HOST"] + req.ApplicationPath;
        }
        /// <summary>
        /// 当前页面客户端的ClientIP
        /// </summary>
        public static String ClientIP(this HttpRequest req)
        {
            String result = req.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (String.IsNullOrEmpty(result))
                result = req.ServerVariables["REMOTE_ADDR"];
            if (String.IsNullOrEmpty(result))
                result = req.UserHostAddress;
            if (String.IsNullOrEmpty(result) || !Regex.IsMatch(result, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$"))
                return "127.0.0.1";
            return result;
        }
        /// <summary>
        /// 判断是否来自搜索引擎链接
        /// </summary>
        /// <returns>是否来自搜索引擎链接</returns>
        public static Boolean IsSearchEnginesGet(this HttpRequest req)
        {
            if (ExHttp.Request.UrlReferrer == null) return false;
            String[] SearchEngine = { "google", "yahoo", "msn", "baidu", "sogou", "sohu", "sina", "163", "lycos", "tom", "yisou", "iask", "soso", "gougou", "zhongsou" };
            String tmpReferrer = req.UrlReferrer.ToString().ToLower();
            for (Int32 i = 0; i < SearchEngine.Length; i++)
            {
                if (tmpReferrer.IndexOf(SearchEngine[i]) >= 0) return true;
            }
            return false;
        }
        /// <summary>
        /// 保存用户上传的文件
        /// </summary>
        /// <param name="path">保存路径</param>
        public static void SaveRequestFile(this HttpRequest req, String path)
        {
            if (req.Files.Count <= 0) return;
            req.Files[0].SaveAs(path);
        }
    }
}