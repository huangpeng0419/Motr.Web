using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Motr.Web.Controls
{
    public class GridHead
    {
        public CellEventHandler Cell { get; set; }
        public GridHeadType HeadType { get; set; }
        public String FieldName { get; set; }
        public String ColumnName { get; set; }
        /// <summary>
        /// 值为零时表示此列不显示
        /// </summary>
        public String ColumnWidth { get; set; }
        public Boolean IsDataKeyField { get; set; }
        public PropertyInfo Property { get; set; }
        /// <summary>
        /// Html编码
        /// </summary>
        public Boolean HtmlEncode { get; set; }
        public GridHead() {
            this.HeadType = GridHeadType.None;
          
        }
        public GridHead(String field, String columnName) : this(field, columnName,null, false) { }
        public GridHead(String field, String columnName, Boolean isDataKeyField) : this(field, columnName, null, isDataKeyField) { }
        public GridHead(String field, String columnName, String columnWidth) : this(field, columnName, columnWidth, false) { }
        public GridHead(String field, String columnName, String columnWidth, Boolean isDataKeyField)
        {
            this.FieldName = field;
            this.ColumnName = columnName;
            this.ColumnWidth = columnWidth;
            this.IsDataKeyField = isDataKeyField;
        }
    }
    public enum GridHeadType
    {
        None,
        CheckBox,
        View,
        Edit,
       Delete
    }
}
