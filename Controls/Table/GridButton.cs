using Motr.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Motr.Web.Controls
{
    public sealed class GridButton
    {
        public String Text { get; set; }
        public String Class { get; set; }
        /// <summary>
        /// 指定跳转Url
        /// </summary>
        public String LinkUrl { get; set; }
        /// <summary>
        /// 是否可见
        /// </summary>
        public Boolean Visible { get; set; }
        /// <summary>
        /// 点击事件
        /// </summary>
        public String OnClientClick { get; set; }
        /// <summary>
        /// 需回发
        /// </summary>
        public Boolean NeedPostBack { get; set; }
        public GridButton()
        {
            this.Visible = true;
        }
        
    }
}
