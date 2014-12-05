using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;

namespace Motr.Web.Script
{
    /// <summary>
    /// 客户端脚本
    /// </summary>
    public class ScriptMgr
    {
        /// <summary>
        /// 客户端脚本提示
        /// </summary>
        /// <param name="message">要弹出的内容</param>
        public static void Alert(String message)
        {
            RegisterBottom(String.Format("alert(\"{0}\");", EncodeScriptText(message)));
        }

        /// <summary>
        /// 基于某个控件弹出选择提示信息
        /// </summary>
        /// <param name="contorl">控制</param>
        /// <param name="message">内容</param>
        public static void Confirm(System.Web.UI.Control control, String message)
        {
            String script = String.Format("return confirm(\"{0}\");", EncodeScriptText(message));

            if (control is WebControl)
                (control as WebControl).Attributes.Add("onclick", script);
            else if (control is HtmlControl)
                (control as HtmlControl).Attributes.Add("onclick", script);
        }

        /// <summary>
        /// 显示客户端消息并整个页面重定向某个URL
        /// </summary>
        /// <param name="message">要弹出的消息</param>
        /// <param name="url">重定向的URL</param>
        public static void ShowAndTopRedirect(String message, String url)
        {
            AlertAndRedirect("top", message, url);
        }

        /// <summary>
        /// 显示客户端消息并当前页面重定向某个URL
        /// </summary>
        /// <param name="message">要弹出的消息</param>
        /// <param name="url">重定向的URL</param>
        public static void AlertAndRedirect(String message, String url)
        {
            AlertAndRedirect("window", message, url);
        }

        /// <summary>
        /// 显示客户端消息并页面重定向某个URL
        /// </summary>
        /// <param name="page">当前页或整个页面</param>
        /// <param name="message">要弹出的消息</param>
        /// <param name="url">重定向的URL</param>
        private static void AlertAndRedirect(String page, String message, String url)
        {
            WriteJavascript(String.Format("alert(\"{0}\");{1}.location.href=\"{2}\";", EncodeScriptText(message), page, EncodeScriptText(url)));
        }
        /// <summary>
        /// 输出脚本
        /// </summary>
        /// <param name="script"></param>
        public static void WriteJavascript(String script)
        {
            var res = ExHttp.Response;
            res.Clear();
            res.Write(String.Format("<script type='text/javascript'>{0}</script>", script));
            res.End();
        }
        /// <summary>
        /// 注册一段脚本到页面顶部
        /// </summary>
        /// <param name="script">要注册的脚本</param>
        public static void RegisterTop(String script)
        {
            ExHttp.Page.ClientScript.RegisterClientScriptBlock(ExHttp.Page.GetType(), "", script, true);
        }

        /// <summary>
        /// 注册一段脚本到页面底部
        /// </summary>
        /// <param name="script">要注册的脚本</param>
        public static void RegisterBottom(String script)
        {
            ExHttp.Page.ClientScript.RegisterStartupScript(ExHttp.Page.GetType(), "", script, true);
        }
        /// <summary>
        /// 对客户端脚本进行编码
        /// </summary>
        /// <param name="text">要编码的内容</param>
        public static String EncodeScriptText(String text)
        {
            return text.Replace(@"\", @"\\").Replace("\"", "\\\"").Replace("\n", @"\n").Replace("\t", @"\t").Replace("\a", @"\a").Replace("\b", @"\b");
        }
    }
}
