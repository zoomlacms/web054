
using Newtonsoft.Json;
using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using ZoomLa.BLL.AdSystem;
using ZoomLa.BLL.Helper;
using ZoomLa.Common;
using ZoomLa.SQLDAL;

namespace ZoomLaCMS.LOGO
{
    public partial class createimg : System.Web.UI.Page
    {
        //1,DrawString画出的字体,边框颜色与字体颜色不同
        //答:g.Clear(Color.White); 使用与背景一致的颜色,透明色会有异色描边的情况
        B_Logo_Icon iconBll = new B_Logo_Icon();
        //-----------------------------
        public string Action { get { return DataConvert.CStr(Request.QueryString["action"]); } }
        protected void Page_Load(object sender, EventArgs e)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            switch (Action)
            {
                case "text":
                    {
                        FontPack model = new FontPack().FromJson(Request["model"]);
                        if (string.IsNullOrEmpty(model.text)) { throw new Exception("文字内容不能为空"); }
                        int bmpLen = 0;
                        int maxWidth = 0;//用于list.aspx,不可超过原有的最大宽度
                        maxWidth = model.direction == "vertical" ? 0:DataConvert.CLng(Request["width"]);
                        int size = GetFontSize(model.text, maxWidth, ref bmpLen);//自动计算出文字大小
                        //根据字体,进行一定程度的缩放0.8-1(特殊要求,不可超过原宽度)
                        switch (model.family)
                        {
                            case "architech":
                                size = (int)(size * 0.83);
                                break;
                            case "harlow solid italic":
                                size = (int)(size * 0.90);
                                break;
                        }
                        //"Aileron_Light|BPdotsDiamond|Bauhaus|balloon|architech|belshaw|bpreplay|noodle_script|harlow_solid_italic|arial|arial_narrow|Arial_Rounded_MT_bold"
                        //------------------------------------
                        //根据方向生成不同的宽度的图片
                        Font mFont = new Font(model.family, size, FontStyle.Regular);
                        Bitmap empty = null;
                        //有的字体未定义小写字母
                        switch (model.direction)
                        {
                            case "vertical":
                                {
                                    model.text = model.text.ToUpper();//垂直下强制大写
                                    empty = new Bitmap((int)(size * 1.5), (int)(bmpLen * 1.3));
                                    Graphics g = Graphics.FromImage(empty);
                                    g.Clear(Color.FromArgb(0));
                                    //每个字为一行
                                    char[] chars = model.text.ToCharArray();
                                    SolidBrush brush = new SolidBrush(model.ColorHx16toRGB(model.color));//Color.FromArgb(100,255,255,255)
                                    int lastY = 0;//最近一次的Y轴定位
                                    for (int i = 0; i < chars.Length; i++)
                                    {
                                        int x = 0;
                                        string text = chars[i] + "";
                                        //空格不用占过多宽
                                        if (text == " ") { lastY += 8; continue; }
                                        else if (i == 0) { }
                                        else
                                        {
                                            lastY = lastY + (int)(model.size * 2.5);
                                        }
                                        //Char.IsLower(chars[i]) ||
                                        if ( text == "I") { x = model.size / 2; }

                                        g.DrawString(text, mFont, brush, new Point(x, lastY));
                                    }
                                }
                                break;
                            default://
                                {
                                    //水平下做自动居中处理(即图片宽度不变,前后留空)
                                    int bmpWidth = bmpLen;
                                    if (maxWidth > 0 && maxWidth > bmpLen) { bmpWidth = maxWidth; }
                                    int bmpStart = (int)((bmpWidth - bmpLen) / 2);
                                    //throw new Exception(bmpStart + "|" + bmpWidth + "|" + bmpLen);

                                    empty = new Bitmap(bmpWidth, (int)(size * 1.5));
                                    Graphics g = Graphics.FromImage(empty);
                                    //g.Clear(model.ColorHx16toRGB(model.bkcolor));//清除画面，填充背景
                                    g.Clear(Color.FromArgb(0));
                                    g.DrawString(model.text, mFont,
                                       new SolidBrush(model.ColorHx16toRGB(model.color)),//Color.FromArgb(100,255,255,255)
                                       new Point(bmpStart, 0));
                                }
                                break;
                        }

                        empty.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    }
                    break;
                case "logo":
                default:
                    {
                        //int LogoID = DataConvert.CLng(Request.QueryString["LogoID"]);
                        //string svgPath = Server.MapPath(iconBll.PlugDir + "assets/icons/" + LogoID + ".svg");
                        //SvgDocument svg = SvgDocument.Open(svgPath);
                        //Bitmap bmp = svg.Draw();
                        //System.Drawing.Image empty = ImgHelper.ReadImgToMS(iconBll.PlugDir + "assets/empty.png");
                        //Graphics g = Graphics.FromImage(empty);
                        ////SizeF textSize = g.MeasureString(CompName, new Font(""));
                        //g.DrawImage(bmp, new Point() { X = 95, Y = 40 });//400,400,商标也固定大小
                        //                                                 //添加文字
                        //Font mFont = new Font(SelFont("m"), GetFontSize(CompName), FontStyle.Regular);
                        //Font sFont = new Font(SelFont("s"), (GetFontSize(SubTitle) / 2), FontStyle.Regular);
                        //g.DrawString(CompName, mFont,
                        //    new SolidBrush(Color.FromArgb(100, 0, 0, 0)),
                        //    GetPosition(empty, g.MeasureString(CompName, mFont), 265));
                        //g.DrawString(SubTitle, sFont,
                        //    new SolidBrush(Color.FromArgb(100, 102, 102, 102)),
                        //    GetPosition(empty, g.MeasureString(SubTitle, sFont), 305 + (int)mFont.Size));
                        //empty.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    }
                    break;
            }

            //------------
            Response.Cache.SetNoStore();
            Response.ClearContent();
            Response.ContentType = "image/gif";
            Response.BinaryWrite(ms.ToArray());
        }
        private Point GetPosition(Image image, SizeF textSize, int posy)
        {
            int x = (image.Width - (int)textSize.Width) / 2 + 5;
            return new Point(x, posy);
        }
        //根据字符数计算出Size,最多十个字符
        /// <summary>
        /// 生成文字图片
        /// </summary>
        /// <param name="text">需要生成的文字</param>
        /// <param name="maxWidth">最大宽度,仅用于水平字体</param>
        /// <param name="mapLen">计算后的图片宽</param>
        private int GetFontSize(string text,int maxWidth, ref int mapLen)//400
        {
            //////六空格==三英文==两汉字 (1,2,3),以空格作为基准单位,每个英文占两空格,每个汉字占三空格位
            int size = 25; mapLen = 0;
            maxWidth = maxWidth < 1 ? 5000000 : maxWidth;
            //if (text.Length <= 4) { size = 45; }
            //else if (text.Length <= 6) { size = 35; }
            //else { size = 25; }
            foreach (char t in text.ToCharArray())
            {
                //空格
                if (t == ' ') { mapLen += size / 3; }
                //中文
                else if ((t >= 0x4e00 && t <= 0x9fbb)) { mapLen += (int)(size * 1.5); }
                else
                {
                    //数字,英文,标点符号
                    //length += 2;
                    if (char.IsUpper(t))
                    { 
                        //中文字符等同于大写英文
                        //大写英文
                        mapLen += size;
                    }
                    else
                    {
                        //小写英文,数字,符号
                        //mapLen += (int)(size /1.3);
                        mapLen += size;//有的字体未定义小写字母,所以均以大写计
                    }
                }
                if (mapLen >= maxWidth) { break; }
            }
            //进行一定的补足
            if (text.Length == 1) { mapLen += 6; }
            else if (text.Length == 2) { mapLen += 3; }
            if (mapLen > maxWidth) { mapLen = maxWidth; }
            return size;

        }
        //----------------
        private string SelFont(string type)
        {
            string font = Request["font"];
            if (string.IsNullOrEmpty(font))
            {
                if (type == "m") { font = "逐浪拉勾艺黑体";  }
                else { font = "逐浪拉勾艺黑体"; }
            }
            return font;
        }
        //-------------------------------------------------
        //  bkcolor: "#000", color: "#fff", text: "", size: "", family: "", addon: ""
        public class FontPack
        {
            public string bkcolor = "";
            public string color = "";
            public string text = "";
            public int size = 12;
            public string family = "";
            public string addon = "";
            //字体生成的方向  水平|垂直
            public string direction = "";//horizontal|vertical
            public FontPack FromJson(string json)
            {
                FontPack model = new FontPack();
                if (string.IsNullOrEmpty(json)) { return model; }
                //json = HttpUtility.UrlDecode(json);
				//json = HttpUtility.HtmlDecode(json);
                model = JsonConvert.DeserializeObject<FontPack>(json);
                //ZoomLa.BLL.ZLLog.L(model.text);
                //------------------
                //去除首尾空格,便于计算,只允许大写字母,避免竖排靠边
                model.text = model.text.Trim();
                if (string.IsNullOrEmpty(model.bkcolor)) { model.bkcolor = ""; }
                if (string.IsNullOrEmpty(model.color)) { model.color = "#fff"; }
                if (string.IsNullOrEmpty(model.direction)) { model.direction = "horizontal"; }
                if (model.size < 1) { model.size = 12; }
                model.family = model.family.Replace("_"," ");
                return model;
            }
            public System.Drawing.Color ColorHx16toRGB(string strHxColor)
            {
                if (string.IsNullOrEmpty(strHxColor))
                {
                    //return System.Drawing.Color.FromArgb(0, 0, 0);//设为黑色
                    return System.Drawing.Color.FromArgb(0);//使用透明色
                }
                else
                {
                    System.Drawing.Color color = System.Drawing.Color.FromArgb(System.Int32.Parse(strHxColor.Substring(1, 2), System.Globalization.NumberStyles.AllowHexSpecifier), System.Int32.Parse(strHxColor.Substring(3, 2), System.Globalization.NumberStyles.AllowHexSpecifier), System.Int32.Parse(strHxColor.Substring(5, 2), System.Globalization.NumberStyles.AllowHexSpecifier));
                    return color;
                }
            }
            /// <summary>
            /// [颜色：RGB转成16进制]
            /// </summary>
            /// <param name="R">红 int</param>
            /// <param name="G">绿 int</param>
            /// <param name="B">蓝 int</param>
            /// <returns></returns>
            public string ColorRGBtoHx16(int R, int G, int B)
            {
                return System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(R, G, B));
            }
        }
    }
}