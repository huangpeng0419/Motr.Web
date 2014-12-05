using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Motr.Web
{

    public static class ExResponse
    {
        /// <summary>
        /// 设置页面不被缓存
        /// </summary>
        public static void SetPageNoCache(this HttpResponse response)
        {
            response.Buffer = true;
            response.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
            response.Expires = 0;
            response.CacheControl = "no-cache";
            response.AddHeader("Pragma", "No-Cache"); 
        }
        public static void DownloadTxt(this HttpResponse response,String contentType,String txt,String fileName)
        {
            response.Clear();
            response.ContentType = contentType;
            response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
            response.Write(txt);
            response.End();
        }
        public static void DownloadTxt(this HttpResponse response, String txt, String fileName)
        {
            response.DownloadTxt("application/octet-stream", txt, fileName);
        }
    }
}
