using Motr.Web.Script;
using Motr.Web.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Motr.Web.Controls
{
    public class Grid : Control, IPostBackEventHandler
    {
        private String _DataKeyField { get; set; }
        public Boolean AutoGenerateColumns { get; set; }
        public Object DataSource { get; set; }
        public List<GridHead> GridHeadList { get; set; }
        public GridButton ButtonView { get; set; }
        public GridButton ButtonEdit { get; set; }
        public GridButton ButtonDelete { get; set; }
        public String SelectedValue { get { return ExHttp.Request.Form[String.Format("{0}_Item", this.ClientID)]; } }
        //<![[CDATA table related
        #region
        public Int32 CellPadding { get; set; }
        public Unit Width { get; set; }
        public String TableClass { get; set; }
        public String HeadClass { get; set; }
        public Boolean IsShowCheckBox { get; set; }
        #endregion
        //]]>
        //<![[CDATA pager related
        #region
        public Boolean ShowPager { get; set; }
        public Pager Pager { get; set; }
        public Int32 CurrentPageIndex
        {
            get { return this.Pager.CurrentPageIndex; }
            set { this.Pager.CurrentPageIndex = value; }
        }
        public Int32 PageSize
        {
            get { return this.Pager.PageSize; }
            set { this.Pager.PageSize = value; }
        }
        public Int32 RecordCount
        {
            get { return this.Pager.RecordCount; }
            set { this.Pager.RecordCount = value; }
        }
        #endregion
        //]]>
        //<![[CDATA Event
        public event DeleteEventHandler DeleteRow = null;
        //]]>
        public Grid()
        {
            this.Width = new Unit(100, UnitType.Percentage);
            this.TableClass = "grid";
            this.ShowPager = true;
            this.Pager = new Pager();
            this.Controls.Add(this.Pager);
            this.ButtonView = new GridButton() { Text = "查看" };
            this.ButtonEdit = new GridButton() { Text = "编辑" };
            this.ButtonDelete = new GridButton() { Text = "删除", NeedPostBack = true };
        }
        protected override void Render(HtmlTextWriter writer)
        {
            if (this.AutoGenerateColumns == false)
            {
                var h = GridHeadList.Find(o => o.IsDataKeyField == true);
                if (h != null) this._DataKeyField = h.ColumnName;
                else throw new Exception("请指定DataKeyFied");
            }
            if (this.IsShowCheckBox) this.GridHeadList.Insert(0, new GridHead() { HeadType = GridHeadType.CheckBox, ColumnWidth = "30px" });
            if (this.ButtonView.Visible) this.GridHeadList.Add(new GridHead() { HeadType = GridHeadType.View, ColumnWidth = "30px" });
            if (this.ButtonEdit.Visible) this.GridHeadList.Add(new GridHead() { HeadType = GridHeadType.Edit, ColumnWidth = "30px" });
            if (this.ButtonDelete.Visible) this.GridHeadList.Add(new GridHead() { HeadType = GridHeadType.Delete, ColumnWidth = "30px" });
            writer.Write(BuildTable());
        }
        private String BuildTable()
        {
            StringBuilder html = new StringBuilder();
            html.Append("<table");
            html.AppendFormat(" id=\"{0}\"", this.ClientID);
            html.AppendFormat(" style=\"width:{0}", this.Width);
            html.Append("\"");
            if (this.CellPadding > 0) html.AppendFormat(" cellpadding=\"{0}\"", this.CellPadding);
            if (this.TableClass != null) html.AppendFormat(" class=\"{0}\"", this.TableClass);
            html.AppendLine(">");
            BuildHeader(html);
            BuildBody(html);
            BuildFooter(html);
            html.AppendLine("</table>");
            BuildScript(html);
            return html.ToString();
        }
        private void BuildHeader(StringBuilder html)
        {
            html.AppendFormat("<thead id=\"{0}_THead\">", this.ClientID).AppendLine();
            html.Append("<tr");
            if (this.HeadClass != null) html.AppendFormat(" class=\"{0}\"", this.HeadClass);
            html.Append(">");
            GridHeadList.ForEach(o =>
            {
                if (o.ColumnWidth != null && o.ColumnWidth == "0") return;
                html.Append("<td");
                if (o.ColumnWidth != null) html.AppendFormat(" style=\"width:{0}\"", o.ColumnWidth);
                html.Append(">");
                switch (o.HeadType)
                {
                    case GridHeadType.None: html.Append(o.ColumnName); break;
                    case GridHeadType.CheckBox: html.AppendFormat("<input type=\"checkbox\" id=\"{0}_ChkAll\" onclick=\"selectedAll(this)\">", this.ClientID); break;
                    case GridHeadType.View: html.Append(this.ButtonView.Text); break;
                    case GridHeadType.Edit: html.Append(this.ButtonEdit.Text); break;
                    case GridHeadType.Delete: html.Append(this.ButtonDelete.Text); break;
                }
                html.Append("</td>");
            });
            html.AppendLine("</tr>");
            html.AppendLine("</thead>");
        }
        private void BuildBody(StringBuilder html)
        {
            html.AppendFormat("<tbody id=\"{0}_TBody\">", this.ClientID);
            if (this.DataSource is IList)
                BuildBodyByList(html);
            else if (this.DataSource is DataTable)
                BuildBodyByDataTable(html);
            html.AppendLine("</tbody>");
        }
        private void BuildBodyByList(StringBuilder html)
        {
            var list = (this.DataSource as IList);
            if (list == null || list.Count == 0) return;
            PropertyInfo dataKeyFieldPropertyInfo = CheckColumnMatch(list[0].GetType().GetProperties(), null) as PropertyInfo;
            Object cellText = null;
            foreach (var item in list)
            {
                var plist = item.GetType().GetProperties();
                html.AppendLine("<tr>");
                this.GridHeadList.ForEach(h =>
                {
                    if (h.ColumnWidth != null && h.ColumnWidth == "0") return;
                    html.Append("<td>");
                    switch (h.HeadType)
                    {
                        case GridHeadType.None:
                            cellText = h.Property.GetValue(item, null);
                            if (h.HtmlEncode && cellText != null) cellText = cellText.ToString().Replace("<", "&lt;").Replace(">", "&gt;");
                            html.Append(h.Cell == null ? cellText : h.Cell(cellText, item));
                            break;
                        case GridHeadType.CheckBox: html.AppendFormat("<input type=\"checkbox\" name=\"{0}_Item\" value=\"{1}\"/>", this.ClientID, dataKeyFieldPropertyInfo.GetValue(item, null)); break;
                        case GridHeadType.View: html.Append(BuildButtonHtml(GridHeadType.View, this.ButtonView, dataKeyFieldPropertyInfo.GetValue(item, null))); break;
                        case GridHeadType.Edit: html.Append(BuildButtonHtml(GridHeadType.Edit, this.ButtonEdit, dataKeyFieldPropertyInfo.GetValue(item, null))); break;
                        case GridHeadType.Delete: html.Append(BuildButtonHtml(GridHeadType.Delete, this.ButtonDelete, dataKeyFieldPropertyInfo.GetValue(item, null))); break;
                    }
                    html.Append("</td>");
                });
                html.AppendLine("</tr>");
            }
        }
        private object CheckColumnMatch(PropertyInfo[] propInfoArray, DataColumnCollection dataColumnColl)
        {
            PropertyInfo dataKeyFieldPropertyInfo = null; // 数据键属性对象
            string dateKeyFieldString = null;
            this.GridHeadList.ForEach(h =>
            {
                try
                {
                    if (h.HeadType == GridHeadType.Delete
                        || h.HeadType == GridHeadType.Edit
                        || h.HeadType == GridHeadType.View
                        || h.HeadType == GridHeadType.CheckBox) return;
                    if (propInfoArray != null)
                        h.Property = propInfoArray.First(o => o.Name.Equals(h.FieldName, StringComparison.OrdinalIgnoreCase));
                    if (dataColumnColl != null && dataColumnColl[h.FieldName] == null)
                        throw new Exception(String.Format("不包含{0}", h.FieldName)); ;
                    if (h.IsDataKeyField)
                    {
                        if (propInfoArray != null)
                            dataKeyFieldPropertyInfo = h.Property;
                        if (dataColumnColl != null)
                            dateKeyFieldString = h.FieldName;
                    }
                }
                catch { throw new Exception(String.Format("不包含{0}", h.FieldName)); }
            });
            if (dateKeyFieldString != null) return dateKeyFieldString;
            return dataKeyFieldPropertyInfo;
        }

        private String BuildButtonHtml(GridHeadType gridHeadType, GridButton gridButton, Object dateKeyValue)
        {
            String hrefValue = "href=\"javascript:void(0)\"";
            String onClick = String.Empty;
            if (gridButton.NeedPostBack)
            {
                onClick = ExHttp.Page.ClientScript.GetPostBackEventReference(this, String.Format("{0}_{1}", gridHeadType.ToString(), dateKeyValue));
                if (gridHeadType == GridHeadType.Delete)
                    onClick = string.Format("if(confirm('确定删除？')){{ {0};return true; }}else {{ return false; }}", onClick);
            }
            else if (gridButton.LinkUrl != null)
                hrefValue = ControlHelper.ResolveUrl(gridButton.LinkUrl);
            else
            {
                if (gridButton.OnClientClick == null) gridButton.OnClientClick = "opt('{0}','{1}',this)";
                onClick = String.Format(gridButton.OnClientClick, gridHeadType.ToString(), dateKeyValue);
            }
            return String.Format("<a {0}{1}>{2}</a>", hrefValue, onClick != String.Empty ? String.Format(" onclick=\"{0}\"", onClick) : String.Empty, gridButton.Text);
        }


        private void BuildBodyByDataTable(StringBuilder html)
        {
            var table = this.DataSource as DataTable;
            if (table == null || table.Rows.Count == 0) return;
            string dateKeyFieldString = CheckColumnMatch(null, table.Columns).ToString();
            Object cellText = null;
            foreach (DataRow row in table.Rows)
            {
                var plist = row.GetType().GetProperties();
                html.AppendLine("<tr>");
                this.GridHeadList.ForEach(h =>
                {
                    if (h.ColumnWidth != null && h.ColumnWidth == "0") return;
                    html.Append("<td>");
                    switch (h.HeadType)
                    {
                        case GridHeadType.None:
                            cellText = row[h.FieldName];
                            if (h.HtmlEncode && cellText != null) cellText = cellText.ToString().Replace("<", "&lt;").Replace(">", "&gt;");
                            html.Append(h.Cell == null ? cellText : h.Cell(cellText, row));
                            break;
                        case GridHeadType.CheckBox: html.AppendFormat("<input type=\"checkbox\" name=\"{0}_Item\" value=\"{1}\"/>", this.ClientID, row[dateKeyFieldString]); break;
                        case GridHeadType.View: html.Append(BuildButtonHtml(GridHeadType.View, this.ButtonView, row[dateKeyFieldString])); break;
                        case GridHeadType.Edit: html.Append(BuildButtonHtml(GridHeadType.Edit, this.ButtonEdit, row[dateKeyFieldString])); break;
                        case GridHeadType.Delete: html.Append(BuildButtonHtml(GridHeadType.Delete, this.ButtonDelete, row[dateKeyFieldString])); break;
                    }
                    html.Append("</td>");
                });
                html.AppendLine("</tr>");
            }
        }
        private void BuildFooter(StringBuilder html)
        {
            if (this.ShowPager == false) return;
            var pagerHtml = GetPagerHtml();
            if (String.IsNullOrEmpty(pagerHtml)) return;
            html.AppendLine("<tfoot><tr>");
            html.AppendFormat("<td colspan=\"{0}\">", this.GridHeadList.Where(o => o.ColumnWidth != "0").Count());
            html.AppendLine(pagerHtml);
            html.AppendLine("</td></tr></tfoot>");
        }

        private String GetPagerHtml()
        {
            using (var sw = new StringWriter())
            {
                var htw = new HtmlTextWriter(sw);
                this.Pager.RenderControl(htw);
                return sw.ToString();
            }
        }
        private void BuildScript(StringBuilder html)
        {
            if (this.IsShowCheckBox == false) return;
            html.AppendLine(@"
            <script type=""text/javascript"">
                function selectedAll(o) {
                    var tableEle = o.parentNode.parentNode.parentNode.parentNode;
                    var iptList = tableEle.getElementsByTagName(""tbody"")[0].getElementsByTagName(""input"");
                    var itemName = tableEle.id + ""_Item"";
                    for (var i = 0 ; i < iptList.length ; i++) {
                        if (iptList[i].name != itemName) continue;
                        iptList[i].checked = o.checked;
                    }
                }
           </script>");
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            var arr = eventArgument.Split('_');
            var args = new GridCommandEventArgs()
            {
                CommandType = arr[0],
                DateKeyValue = arr[1]
            };
            if (args.CommandType == GridHeadType.Delete.ToString() && this.DeleteRow != null)
                this.DeleteRow(this, args);
        }
    }
}
