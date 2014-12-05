using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Reflection;
using System.Web.UI;
using Motr.Web.Controls;

namespace Motr.Web
{
    public static class ExForm
    {
        /// <summary>
        /// 根据NameValueCollection对象的键值对构建对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="req"></param>
        /// <returns></returns>
        public static T GetEntity<T>(this NameValueCollection coll)
        {
            return coll.GetEntity<T>(String.Empty);
        }
        /// <summary>
        /// Form提交数据转特定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="perfix"></param>
        /// <returns></returns>
        public static T GetEntity<T>(this NameValueCollection coll, String perfix)
        {
            T t = Activator.CreateInstance<T>();
            String val = null;
            foreach (PropertyInfo p in t.GetType().GetProperties())
            {
                try
                {
                    if (p.CanWrite == false) continue; 
                    val = coll[perfix + p.Name];
                    if (val == null) continue;
                    if (p.PropertyType.IsEnum)
                        p.SetValue(t, Enum.Parse(p.PropertyType, val), null);
                    else if (p.PropertyType.IsGenericType)
                    {
                        if (p.PropertyType.IsValueType)
                            p.SetValue(t, String.IsNullOrEmpty(val) ? null : Convert.ChangeType(val, Nullable.GetUnderlyingType(p.PropertyType)), null);
                    }
                    else
                        p.SetValue(t, String.IsNullOrEmpty(val) ? null : Convert.ChangeType( val,p.PropertyType), null);
                }
                catch
                {
                    throw new Exception(String.Format("Convert error:{0}({1})", p.Name, p.PropertyType.ToString()));
                }
            }
            return t;
        }
        /// <summary>
        /// 绑定Form数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container"></param>
        /// <param name="t"></param>
        public static void BindForm<T>(this Control container, T t)
        {
            container.BindForm<T>(t, "yyyy-MM-dd");
        }
        /// <summary>
        /// 绑定Form数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container"></param>
        /// <param name="t"></param>
        /// <param name="dateFormat"></param>
        public static void BindForm<T>(this Control container, T t, String dateFormat)
        {
            foreach (PropertyInfo p in t.GetType().GetProperties())
            {
                try
                {
                    var c = container.FindControl(p.Name);
                    if (c == null) continue;
                    Object obj = p.GetValue(t, null);
                    if (obj == null) continue;
                    var val = obj.ToString();
                    if (p.PropertyType == typeof(DateTime)) val = DateTime.Parse(val).ToString(dateFormat);
                    if (c is TextBox)
                        ((TextBox)c).Text = val;
                    else if (c is ListControl)
                    {
                        ListControl lc = c as ListControl;
                        lc.ClearSelection();
                        ListItem li = lc.Items.FindByValue(val);
                        if (li != null) li.Selected = true;
                    }
                    else if (c is HiddenField)
                        ((HiddenField)c).Value = val;
                    else if (c is HtmlInputControl)
                        ((HtmlInputControl)c).Value = val;
                    else if (c is Label)
                        ((Label)c).Text = val;
                    else if (c is Literal)
                        ((Literal)c).Text = val;
                    else if (c is HtmlSelect)
                        ((HtmlSelect)c).Value = val;
                    else if (c is LinkButton)
                        ((LinkButton)c).Text = val;
                    else if (c is HtmlTextArea)
                        ((HtmlTextArea)c).Value = val;
                    else if (c is HtmlInputCheckBox)
                        ((HtmlInputCheckBox)c).Checked = Boolean.Parse(val);
                    else if (c is KindEditor)
                        ((KindEditor)c).Text = val;
                }
                catch
                {
                    throw new Exception(String.Format("Convert error:{0}({1})", p.Name, p.PropertyType.ToString()));
                }
            }
        }
    }
}
