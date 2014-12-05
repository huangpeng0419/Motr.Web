using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Data;
using System.Web.UI;

namespace Motr.Web.Script
{
    public static class JsonTransform
    {
        /// <summary>
        /// 对象转Json字符
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String ToJson(this Object obj)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (obj is DataSet)
                return js.Serialize(Converter(obj as DataSet));
            if (obj is DataTable)
                return js.Serialize(Converter(obj as DataTable));
            if (obj is DataRow)
                return js.Serialize(Converter(obj as DataRow));
            return js.Serialize(obj);
        }
        /// <summary>
        /// Json转对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ToObject<T>(this String json)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize<T>(json);
        }
        private static Dictionary<String, Object> Converter(DataRow row)
        {
            Dictionary<String, Object> dic = new Dictionary<String, Object>();
            if (row == null) return dic;
            foreach (DataColumn column in row.Table.Columns)
                dic.Add(column.ColumnName, row[column.ColumnName]);
            return dic;
        }
        private static Dictionary<String, Object> Converter(DataTable table)
        {
            Dictionary<String, Object> dic = new Dictionary<String, Object>();
            if (table == null) return dic;
            List<Dictionary<String, Object>> rowDic = new List<Dictionary<String, Object>>();
            foreach (DataRow row in table.Rows)
                rowDic.Add(Converter(row));
            dic.Add("Rows", rowDic);
            return dic;
        }
        private static Dictionary<String, Object> Converter(DataSet set)
        {
            Dictionary<String, Object> dic = new Dictionary<String, Object>();
            if (set == null) return dic;
            List<Dictionary<String, Object>> tableDic = new List<Dictionary<String, Object>>();
            foreach (DataTable table in set.Tables)
                tableDic.Add(Converter(table));
            dic.Add("Tables", tableDic);
            return dic;
        }
    }
}
