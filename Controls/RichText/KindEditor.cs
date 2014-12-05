using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;

namespace Motr.Web.Controls
{
    public class KindEditor : Control
    {
        public KindEditor()
        {
            this._BasePath = VirtualPathUtility.ToAbsolute("~/Components/kindeditor/");
            this.Width = new Unit(100, UnitType.Percentage);
        }
        /// <summary>
        /// 默认基路径
        /// </summary>
        private String _BasePath { get; set; }
        /// <summary>
        /// 引用资源
        /// </summary>
        public Boolean ReferSource { get; set; }
        /// <summary>
        /// js库路径(为空时默认/Components/kindeditor/kindeditor-min.js)
        /// </summary>
        public String SrcJs { get; set; }
        /// <summary>
        /// 主题(为空时默认js全局配置)
        /// </summary>
        public String ThemeType { get; set; }
        /// <summary>
        /// 语言包(为空时默认js全局配置)
        /// </summary>
        public String LangType { get; set; }
        /// <summary>
        /// 允许拖动调整的方向0 不允许 1 只允许高度 2高度、宽度都可
        /// </summary>
        public String ResizeType { get; set; }
        /// <summary>
        /// 工具选项
        /// </summary>
        public String Items { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        public Unit Width { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        public String Height { get; set; }
        /// <summary>
        /// Css路径
        /// </summary>
        public String CssPath { get; set; }
        /// <summary>
        /// 上传文件处理程序
        /// </summary>
        public String UploadJson { get; set; }
        /// <summary>
        /// 文件管理处理程序
        /// </summary>
        public String FileManagerJson { get; set; }
        /// <summary>
        /// 允许从远程站点选择
        /// </summary>
        public Boolean AllowFileManager { get; set; }
        /// <summary>
        /// 未提供其他参数选项时使用
        /// </summary>
        public String OtherOptions { get; set; }
        /// <summary>
        /// 文本
        /// </summary>
        public String Text { get; set; }
        protected override void OnInit(EventArgs e)
        {
            this.Text = HttpContext.Current.Request.Form[this.UniqueID];
            base.OnInit(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("<textarea id=\"{0}\" name=\"{1}\">{2}</textarea>", this.ClientID, this.UniqueID, this.Text).AppendLine();
            if (ReferSource) builder.AppendFormat("<script src=\"{0}\" type=\"text/javascript\"></script>", this.GetSrcJs());
            builder.AppendFormat(@"
            <script type=""text/javascript"">
               KindEditor.ready(function(K){{
                   window.{0} = K.create('#{0}',{{
                      {1}
                   }});
               }});
            </script>
            ", this.ClientID, this.CustomConfig);
            writer.Write(builder.ToString());
        }
        /// <summary>
        /// 自定义配置
        /// </summary>
        /// <returns></returns>
        private String CustomConfig
        {
            get
            {
                StringBuilder options = new StringBuilder();
                options.AppendFormat("'width':'{0}',", this.Width);
                if (this.Height != null)
                    options.AppendFormat("'height':'{0}',", this.Height);
                if (this.ThemeType != null)
                    options.AppendFormat("'themeType':'{0}',", this.ThemeType);
                if (this.LangType != null)
                    options.AppendFormat("'langType':'{0}',", this.LangType);
                if (this.ResizeType != null)
                    options.AppendFormat("'resizeType':'{0}',", this.ResizeType);
                if (this.Items != null)
                    options.AppendFormat("'items':{0},", this.CustomItems);
                if (this.CssPath != null)
                    options.AppendFormat("'cssPath':'{0}',", this.CssPath);
                if (this.UploadJson != null)
                    options.AppendFormat("'uploadJson':'{0}',", this.UploadJson);
                if (this.FileManagerJson != null)
                    options.AppendFormat("'fileManagerJson':'{0}',", this.FileManagerJson);
                if (this.AllowFileManager)
                    options.AppendFormat("'allowFileManager':true,", this.CssPath);
                if (this.OtherOptions != null)
                    options.AppendLine(this.OtherOptions);
                else
                    options.Length--;
                return options.ToString();
            }
        }
        /// <summary>
        /// 自定义工具选项
        /// </summary>
        private String CustomItems
        {
            get
            {
                switch (this.Items)
                {
                    case "Simple": return "['fontname', 'fontsize', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline','|', 'plainpaste', 'removeformat','justifyleft', 'justifycenter', 'justifyright','|', 'emoticons', 'table','hr', 'image', 'link','insertfile','preview','prInt32','fullscreen']";
                    default: return this.Items;
                }
            }
        }
        /// <summary>
        /// 获取js库路径
        /// </summary>
        /// <returns></returns>
        private String GetSrcJs()
        {
            if (this.SrcJs != null) return this.SrcJs;
            return String.Format("{0}kindeditor-min.js", _BasePath);
        }
    }
}
