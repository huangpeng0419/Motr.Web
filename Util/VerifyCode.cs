using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Motr.Web.Util
{
    public class VerifyCode
    {
        public VerifyCode() : this(CodeType.All, 4) { }
        public VerifyCode(CodeType type) : this(type, 4) { }
        public VerifyCode(CodeType type, Int32 length)
        {
            // 验证码相关
            this.CodeType = type;
            this.CodeLength = length;
            BuilderCode();
            // 图片属性初始值设定
            this.ImageWidth = this.Code.Length * 15;
            this.ImageHeight = 20;
        }
        public CodeType CodeType { get; set; }
        public Int32 CodeLength { get; set; }
        public String Code { get; private set; }
        public Int32 ImageWidth { get; set; }
        public Int32 ImageHeight { get; set; }
        public String GetRandomString(CodeType type)
        {
            String all = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            switch (type)
            {
                case CodeType.Number: return all.Substring(0, 10);
                case CodeType.CapitalLetter: return all.Substring(10, 26);
                case CodeType.LowerCaseLetter: return all.Substring(36, 26);
                case CodeType.LowerCaseLetterAndCapitalLetter: return all.Substring(10);
                case CodeType.NumberAndLowerCaseLetter: return all.Remove(10, 26);
                case CodeType.NumberAndCapitalLetter: return all.Remove(36);
                case CodeType.All:
                default: return all;
            }
        }
        public void BuilderCode()
        {
            String randomString = GetRandomString(this.CodeType);
            Random random = new Random();
            Int32 maxValue = randomString.Length - 1;
            StringBuilder codeBuilder = new StringBuilder();
            for (Int32 i = 0; i < this.CodeLength; i++)
            {
                codeBuilder.Append(randomString[random.Next(maxValue)]);
            }
            this.Code = codeBuilder.ToString();
        }
        public byte[] CreateImage()
        {
            Bitmap image = new Bitmap(this.ImageWidth, this.ImageHeight);
            Graphics g = Graphics.FromImage(image);
            Font f = new Font("Arial", 10, System.Drawing.FontStyle.Bold);
            Brush b = new SolidBrush(Color.Black);
            g.Clear(Color.MediumAquamarine);
            g.DrawString(this.Code, f, b, ((this.ImageWidth - (this.Code.Length * 10) )/ 2), 3);
            g.DrawRectangle(new Pen(Color.FromArgb(90, 87, 46)), 0, 0, this.ImageWidth - 1, this.ImageHeight - 1);
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Jpeg);
            g.Dispose();
            image.Dispose();
            return ms.ToArray();
        }
        public void ResponseImage()
        {
            ExHttp.Response.ClearContent();
            ExHttp.Response.ContentType = "image/Jpeg";
            ExHttp.Response.BinaryWrite(CreateImage());
        }
    }
    public enum CodeType
    {
        Number,
        CapitalLetter,
        LowerCaseLetter,
        LowerCaseLetterAndCapitalLetter,
        NumberAndLowerCaseLetter,
        NumberAndCapitalLetter,
        All,
    }
}
