using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Motr.Web.Util;

namespace Motr.Web.Controls
{
    public class Script : Control
    {
        public const String DefaultType = "text/javascript";
        public String Src { get; set; }
        public String Type { get; set; }
        public String Charset { get; set; }
        public Boolean IsDefer { get; set; }
        protected override void Render(HtmlTextWriter writer)
        {
            StringBuilder sb = new StringBuilder("<script");
            if (!String.IsNullOrEmpty(Src)) sb.AppendFormat(" src=\"{0}\"", ControlHelper.ResolveUrl(Src));
            sb.AppendFormat(" type=\"{0}\"", String.IsNullOrEmpty(Type) ? DefaultType : Type);
            if (IsDefer) sb.AppendFormat(" defer=\"defer\"");
            if (!String.IsNullOrEmpty(Charset)) sb.AppendFormat(" charset=\"{0}\"", Charset);
            if (!String.IsNullOrEmpty(ID)) sb.AppendFormat(" id=\"{0}\"", ID);
            sb.AppendFormat("></script>{0}",Environment.NewLine);
            writer.Write(sb.ToString());
        }
    }
}
