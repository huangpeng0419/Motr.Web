using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Motr.Web
{
    /// <summary>
    /// 扩展Cookie操作
    /// </summary>
    public class ExCookie
    {
        /// <summary>
        /// 保存一个Cookie到客户端（临时性）
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        public static void Add(String name, String value)
        {
            Add(name, value, TimeSpan.Zero);
        }

        /// <summary>
        /// 保存一个Cookie到客户端（持久性）
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        /// <param name="ts">指定一个Cookie过期的日期和时间</param>
        public static void Add(String name, String value, TimeSpan ts)
        {
            HttpCookie cookie = new HttpCookie(name, value);
            if (ts != TimeSpan.Zero)
                cookie.Expires = DateTime.Now.Add(ts);
            ExHttp.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 获取一个Cookie值
        /// </summary>
        /// <param name="name">名称</param>
        public static String GetValue(String name)
        {
            HttpCookie cookie = ExHttp.Request.Cookies[name];
            if (cookie == null) return String.Empty;
            return cookie.Value;
        }

        /// <summary>
        /// 清除一个Cookie
        /// </summary>
        /// <param name="name">名称</param>
        public static void Clear(String name)
        {
            HttpCookie cookie = ExHttp.Request.Cookies[name];
            if (cookie == null) return;
            cookie.Expires = DateTime.Now.AddDays(-1);
            ExHttp.Response.Cookies.Add(cookie);
        }
    }
}
