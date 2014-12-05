using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;

namespace Motr.Web.Controls
{
    public class CKEditor : Control
    {
        public CKEditor()
        {
            this._BasePath = VirtualPathUtility.ToAbsolute("~/Components/ckeditor/");
            this.ToolBar = "Simple";
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
        /// js库路径(为空时默认/Components/ckeditor/ckeditor-min.js)
        /// </summary>
        public String SrcJs { get; set; }
        /// <summary>
        /// loadway
        /// </summary>
        public Boolean IsInLine { get; set; }
        /// <summary>
        /// 语言包
        /// </summary>
        public String Language { get; set; }
        /// <summary>
        /// 皮肤颜色(例：#14B8C4)
        /// </summary>
        public String UIColor { get; set; }
        /// <summary>
        /// 工具栏
        /// </summary>
        public String ToolBar { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        public Unit Width { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        public Unit Height { get; set; }
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
               window.{0}=CKEDITOR.{2}('{0}',{{{1}}});
            </script>
            ", this.ClientID, this.CustomConfig, this.IsInLine ? "inline" : "replace");
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
                if (this.Language != null)
                    options.AppendFormat("'language':'{0}',", this.Language);
                if (this.UIColor != null)
                    options.AppendFormat("'uiColor':'{0}',", this.UIColor);
                if (!this.Width.IsEmpty)
                    options.AppendFormat("'width':'{0}',", this.Width);
                if (!this.Height.IsEmpty)
                    options.AppendFormat("'height':'{0}',", this.Height);
                if(this.ToolBar!=null)
                    options.AppendFormat("'toolbar':{0},", this.CustomToolBar);
                if (this.OtherOptions != null)
                    options.AppendLine(this.OtherOptions);
                else if (options.Length > 0)
                    options.Length--;
                return options.ToString();
            }
        }
        /// <summary>
        ///  自定义工具栏
        /// </summary>
        private String CustomToolBar
        {
            get
            {
                switch (ToolBar)
                {
                    case "Simple": return @"
			        [
					   [ 'Bold', 'Italic', '-', 'NumberedList', 'BulletedList', '-', 'Link', 'Unlink' ],
					   [ 'FontSize', 'TextColor', 'BGColor' ],
                       [ 'Maximize']
				    ]";
                    default: return this.ToolBar;
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
            return String.Format("{0}ckeditor.js", _BasePath);
        }
    }
}