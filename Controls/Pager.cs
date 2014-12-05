using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Motr.Web.Controls
{
    public delegate void PageChangedEventHandler(Object sender, PagerEventArgs args);
    public class PagerEventArgs : EventArgs
    {
        public Int32 PageIndex { get; set; }
        public PagerEventArgs(Int32 pageIndex)
        {
            this.PageIndex = pageIndex;
        }
    }
    public class Pager : Control, IPostBackEventHandler
    {
        private Int32 CurrentPageCount { get; set; }
        public Int32 FixedPageCount { get; set; }
        public Int32 CurrentPageIndex { get; set; }
        public Int32 PageSize { get; set; }
        public Int32 RecordCount { get; set; }
        public event PageChangedEventHandler PageIndexChanged = null;
        public Pager()
        {
            this.CurrentPageIndex = 1;
            this.PageSize = 10;
            this.FixedPageCount = 10;
        }
        protected override void Render(HtmlTextWriter writer)
        {
            if (this.RecordCount == 0) return;
            this.CurrentPageCount = (int)Math.Ceiling((double)this.RecordCount / (double)this.PageSize);
            if (this.CurrentPageCount == 1) return;
            writer.Write(Build());
        }
        private String Build()
        {
            var s = new StringBuilder();
            s.Append("<div class=\"pager\">");
            //<![CDATA[ header
            if (this.FixedPageCount <= this.CurrentPageCount && this.FixedPageCount / 2 <= this.CurrentPageIndex)
                s.AppendFormat("<a class=\"frist\" href=\"{0}\">首页</a>", this.GetPostBackClientHyperlink(1));
            if (this.CurrentPageIndex > 1)
                s.AppendFormat("<a class=\"previous\" href=\"{0}\">&lt;上一页</a>", this.GetPostBackClientHyperlink(this.CurrentPageIndex - 1));
            //]]>
            //<![CDATA[ body
            int start, end;
            GetStartEnd(out start, out end);
            for (int i = start; i <= end; i++)
            {
                if (i == this.CurrentPageIndex)
                    s.AppendFormat("<b>{0}</b>", i.ToString());
                else
                    s.AppendFormat("<a href=\"{0}\">{1}</a>", this.GetPostBackClientHyperlink(i), i.ToString());
            }
            //]]>
            //<![CDATA[ footer
            if (this.CurrentPageIndex != this.CurrentPageCount)
                s.AppendFormat("<a class=\"next\" href=\"{0}\">下一页&gt;</a>", this.GetPostBackClientHyperlink(this.CurrentPageIndex + 1));
            if (this.CurrentPageCount - this.CurrentPageIndex >= (this.FixedPageCount / 2))
                s.AppendFormat("<a class=\"last\" href=\"{0}\">尾页</a>", this.GetPostBackClientHyperlink(this.CurrentPageCount));
            //]]>
            s.Append("</div>");
            return s.ToString();
        }

        private void GetStartEnd(out int start, out int end)
        {
            start = this.CurrentPageIndex;
            end = this.FixedPageCount;
            if (this.CurrentPageCount < this.FixedPageCount)
            {
                start = 1;
                end = this.CurrentPageCount;
            }
            else
            {
                var remain = this.CurrentPageCount - this.CurrentPageIndex;
                if (remain >= this.FixedPageCount - 1)
                {
                    int i = this.FixedPageCount / 2;
                    if (start <= i)
                        start = 1;
                    else
                    {
                        start = this.CurrentPageIndex - i+1;
                        end = end + this.CurrentPageIndex - i;
                    }
                }
                else
                {
                    start = start - (this.FixedPageCount - 1 - remain);
                    end = this.CurrentPageIndex + remain;
                }
            }
        }
        private string GetPostBackClientHyperlink(int index)
        {
            return this.Page.ClientScript.GetPostBackClientHyperlink(this, index.ToString());
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            if (this.PageIndexChanged == null) return;
            this.PageIndexChanged(this, new PagerEventArgs(int.Parse(eventArgument)));
        }
    }
}
