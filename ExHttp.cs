using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.Caching;

namespace Motr.Web
{
    /// <summary>
    /// 获取当前Http请求的相关对象
    /// </summary>
    public class ExHttp
    {
        /// <summary>
        /// Page对象
        /// </summary>
        public static Page Page
        {
            get { return (Page)HttpContext.Current.Handler; }
        }
        /// <summary>
        /// Cache对象
        /// </summary>
        public static Cache Cache
        {
            get { return HttpContext.Current.Cache; }
        }
        /// <summary>
        /// Requset对象
        /// </summary>
        public static HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }
        /// <summary>
        /// Response对象
        /// </summary>
        public static HttpResponse Response
        {
            get { return HttpContext.Current.Response; }
        }
        /// <summary>
        /// Server对象
        /// </summary>
        public static HttpServerUtility Server
        {
            get { return HttpContext.Current.Server; }
        }
        /// <summary>
        /// Session对象
        /// </summary>
        public static HttpSessionState Session
        {
            get { return HttpContext.Current.Session; }
        }
    }
}
