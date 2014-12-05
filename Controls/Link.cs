using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Motr.Web.Util;

namespace Motr.Web.Controls
{
    public class Link : Control
    {
        public Link()
        {
            Rel = "stylesheet";
            Type = "text/css";
        }
        public String Href { get; set; }
        public String Rel { get; set; }
        public String Type { get; set; }
        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write("<link href=\"{0}\" rel=\"{1}\" type=\"{2}\" {3}/>{4}", ControlHelper.ResolveUrl(Href), Rel, Type, !String.IsNullOrEmpty(ID) ? String.Format("id=\"{0}\"", ID) : String.Empty,Environment.NewLine);
        }
    }
}
