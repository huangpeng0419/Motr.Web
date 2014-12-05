using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;

namespace Motr.Web.Controls
{
    /// <summary>
    /// 日期选择器
    /// </summary>
    public class DatePicker : WebControl, IPostBackDataHandler
    {
        public DatePicker()
        {
            Option = DatePickerOption.My97DatePicker;
            Wdate = true;
            Lang = "auto";
            Skin = "default";
            DateFmt = "yyyy-MM-dd";
        }
        /// <summary>
        /// 回传数据变化事件
        /// </summary>
        public event EventHandler ValueChange;
        /// <summary>
        /// 日期js路径
        /// </summary>
        public String Url { get; set; }
        /// <summary>
        /// 回传值
        /// </summary>
        public String Value
        {
            get { return ViewState["Value"] + ""; }
            set { ViewState["Value"] = value; }
        }
        #region 日期选择器参数
        /// <summary>
        /// 日期选择器选项
        /// </summary>
        public DatePickerOption Option { get; set; }
        /// <summary>
        /// 是否引用样式Wdate
        /// </summary>
        public Boolean Wdate { get; set; }
        /// <summary>
        /// 语言包
        /// </summary>
        public String Lang { get; set; }
        /// <summary>
        /// 皮肤
        /// </summary>
        public String Skin { get; set; }
        /// <summary>
        /// 日期显示格式
        /// </summary>
        public String DateFmt { get; set; }
        /// <summary>
        /// 制度
        /// </summary>
        public Boolean ReadOnly { get; set; }
        /// <summary>
        /// My97不常用参数
        /// </summary>
        public String My97Params { get; set; }
        #endregion
        /// <summary>
        /// 重写标记
        /// </summary>
        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Input; }
        }
        /// <summary>
        /// 重写添加属性
        /// </summary>
        /// <param name="writer"></param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID);
            writer.AddAttribute(HtmlTextWriterAttribute.Value, Value);
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, GetOnclickJs());
            if (Wdate) writer.AddAttribute(HtmlTextWriterAttribute.Class, "Wdate");
            base.AddAttributesToRender(writer);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            RegisterJs();
            base.OnPreRender(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private String GetOnclickJs()
        {
            StringBuilder builder = new StringBuilder();
            switch (Option)
            {
                case DatePickerOption.My97DatePicker:
                    builder.Append("WdatePicker({");
                    builder.AppendFormat("readOnly:{0},", ReadOnly ? "true" : "false");
                    builder.AppendFormat("lang:'{0}',", Lang);
                    builder.AppendFormat("skin:'{0}',", Skin);
                    builder.AppendFormat("dateFmt:'{0}',", DateFmt);
                    if (My97Params == null)
                        builder.Length--;
                    else
                        builder.Append(My97Params);
                    builder.Append("})");
                    break;
                default:
                    break;
            }
            return builder.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        private void RegisterJs()
        {
            if (this.Url == null) this.Url =VirtualPathUtility.ToAbsolute("~/Scripts/My97DatePicker/WdatePicker.js");
            Page.ClientScript.RegisterClientScriptInclude("DatePicker", this.Url);
        }
        #region IPostBackDataHandler 成员
        /// <summary>
        /// 回传数据
        /// </summary>
        /// <param name="postDataKey"></param>
        /// <param name="postCollection"></param>
        /// <returns></returns>
        public Boolean LoadPostData(String postDataKey, NameValueCollection postCollection)
        {
            if (postCollection[postDataKey] == Value) return false;
            return true;
        }
        /// <summary>
        /// 回传数据变化
        /// </summary>
        public void RaisePostDataChangedEvent()
        {
            if (ValueChange == null) return;
            ValueChange(this, EventArgs.Empty);
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public enum DatePickerOption
    {
        My97DatePicker
    }
}
