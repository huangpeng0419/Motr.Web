using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Motr.Web.Util
{
    public class ExportMgr
    {
        /// <summary>
        /// 导出数据公用方法
        /// </summary>
        /// <param name="fileName">文件名(带后缀,如:***.txt|***.doc)</param>
        /// <param name="exportContent">导出文本</param>
        /// <param name="contentType">导出文本类型(如:text/plain|application/vnd.ms-excel)</param>
        /// <param name="encoding">字符集编码(如:UTF-8|GB2312)</param>
        public static void ExportDoc(String fileName, String exportContent, String contentType, String encoding)
        {
            HttpResponse Response = ExHttp.Response;
            Response.ClearHeaders();
            Response.ClearContent();
            Response.ContentType = contentType;
            Response.Charset = encoding;
            Response.ContentEncoding = Encoding.GetEncoding(encoding);
            fileName = HttpUtility.UrlEncode(fileName, Encoding.GetEncoding(encoding));
            Response.AddHeader("content-disposition", String.Format("attachment;filename=\"{0}\"", fileName));
            Response.Write(exportContent);
            Response.End();
        }

        /// <summary>
        /// 导出Excel文件
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="fileName">文件名</param>
        public static void ExportExcel(System.Web.UI.Control control, String fileName)
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            control.RenderControl(htw);
            ExportExcel(sw.ToString(), fileName);
        }

        /// <summary>
        /// 导出Excel文件
        /// </summary>
        /// <param name="exportContent">导出文本</param>
        /// <param name="fileName">文件名</param>
        public static void ExportExcel(String exportContent, String fileName)
        {
            exportContent = String.Format(@"
            <html>
              <head>
              <meta http-equiv=Content-Type content=""text/html;charset=utf-8"">
              <style type=""text/css"">
                td{vnd.ms-excel.numberformat:@;}
              </style>
              </head>
                 <body>{0}</body>
            </html>",exportContent);
            ExportDoc(fileName + ".xls", exportContent, "application/vnd.ms-excel", "UTF-8");
        }
    }
}
