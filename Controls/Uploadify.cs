using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web;

namespace Motr.Web.Controls
{
    public class Uploadify : Control
    {
        public Uploadify()
        {
            this._BasePath = VirtualPathUtility.ToAbsolute("~/Components/uploadify/");
            this.Multi = true;
        }
        /// <summary>
        /// 默认基路径
        /// </summary>
        private String _BasePath { get; set; }
        /// <summary>
        /// 引用资源
        /// </summary>
        public Boolean ReferSource { get; set; }
        public String CssPath { get; set; }
        public String JQueryPath { get; set; }
        public String UploadifyPath { get; set; }
        public String SWFPath { get; set; }
        public String UploaderPath { get; set; }
        /// <summary>
        /// 多文件选择
        /// </summary>
        public Boolean Multi { get; set; }
        /// <summary>
        /// 其他参数
        /// </summary>
        public String OtherParams { get; set; }
        /// <summary>
        /// 输出
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("<input type=\"file\" id=\"{0}\" name=\"{1}\"/>", this.ClientID, this.UniqueID).AppendLine();
            if (ReferSource)
            {
                this.CssPath = this.CssPath != null ? this.CssPath : String.Format("{0}uploadify.css", this._BasePath);
                this.JQueryPath = this.JQueryPath != null ? this.JQueryPath : String.Format("{0}jquery.min.js", VirtualPathUtility.ToAbsolute("~/Scripts/"));
                this.UploadifyPath = this.UploadifyPath != null ? this.UploadifyPath : String.Format("{0}jquery.uploadify.min.js", this._BasePath);
                this.SWFPath = this.SWFPath != null ? this.SWFPath : String.Format("{0}uploadify.swf", this._BasePath);
                this.UploaderPath = this.UploaderPath != null ? this.UploaderPath : String.Format("{0}uploadify.ashx", this._BasePath);
                builder.AppendFormat("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />", this.CssPath).AppendLine();
                builder.AppendFormat("<script src=\"{0}\" type=\"text/javascript\"></script>", this.JQueryPath).AppendLine();
                builder.AppendFormat("<script src=\"{0}\" type=\"text/javascript\"></script>", this.UploadifyPath).AppendLine();
                builder.AppendFormat(@"
                <script type=""text/javascript"">
                jQuery(function($){{
                       $('#{0}').uploadify({{
                          {1}
                       }});
                }});
                </script>", this.ClientID, this.CustomConfig);
            }
            writer.Write(builder.ToString());
        }
        private String CustomConfig
        {
            get
            {
                StringBuilder options = new StringBuilder();
                options.AppendFormat("'multi':{0},", this.Multi.ToString().ToLower()).AppendLine();
                options.AppendFormat("'swf':'{0}',", this.SWFPath).AppendLine();
                options.AppendFormat("'uploader':'{0}',", this.UploaderPath);
                if (this.OtherParams != null)
                    options.AppendLine(this.OtherParams);
                else
                    options.Length--;
                return options.ToString();
            }
        }
    }
}
