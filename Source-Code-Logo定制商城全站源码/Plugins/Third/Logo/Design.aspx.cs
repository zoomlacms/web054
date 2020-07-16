using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.BLL.AdSystem;
using ZoomLa.BLL.Helper;
using ZoomLa.Common;
using ZoomLa.Model;
using ZoomLa.Model.AdSystem;
using ZoomLa.Sns;
using ZoomLa.SQLDAL;

namespace ZoomLaCMS.Plugins.Third.Logo
{
    public partial class Design : System.Web.UI.Page
    {
        public B_Logo_Icon iconBll = new B_Logo_Icon();
        B_OrderList orderBll = new B_OrderList();
        B_Cart cartBll = new B_Cart();
        B_CartPro cpBll = new B_CartPro();
        B_Logo_Design desBll = new B_Logo_Design();
        B_Product proBll = new B_Product();
        B_Payment payBll = new B_Payment();
        //public int TlpID { get { return DataConvert.CLng(Request.QueryString["TlpID"]); } }
        public int ProID { get { return DataConvert.CLng(Request.QueryString["ProID"]); } }
        public int Mid { get { return DataConvert.CLng(Request.QueryString["ID"]); } }
        public string CartCookID { get { return B_Cart.GetCartID(Context); } }
        public M_Product proMod = null;
        M_Logo_Design desMod = null;
        M_UserInfo mu = null;
        public DataRow desDR = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ProID < 1) { function.WriteErrMsg("Unspecified commodity ID"); }
                int tlp = GetTlpIDByProID(ProID);
                desMod = desBll.SelReturnModel(tlp);
                proMod = proBll.GetproductByid(ProID);
                if (desMod == null || desMod.ZType != 1) {
                    //function.WriteErrMsg("Templates do not exist");
                    Response.Redirect("/Plugins/third/logo/list.aspx?page=1");
                }
                //--------------------------------------------
                if (desMod == null) { function.WriteErrMsg("Unspecified information"); }
                desDR = DBCenter.Sel(desMod.TbName, "ID=" + desMod.ID).Rows[0];
                if (string.IsNullOrEmpty(DataConvert.CStr(desDR["BackGround"])))
                {
                    desDR["BackGround"] = "black";
                }
                //NEON SIGNS
                if (proMod.Nodeid == 7 || proMod.Nodeid == 104)
                {
                    Size_L.Text = "3";
                    font_div.Visible = true;
                    color_12_div.Visible = true;
                }
                //LED Signs商品
                if (proMod.Nodeid == 8 || proMod.Nodeid == 105)
                {
                    Size_L.Text = "1";
                    Backing_TR.Visible = false;
                    Outdoor_TR.Visible = false;
                    font_led_div.Visible = true;
                    color_7_div.Visible = true;
                }
                //LED STRIP SIGNS
                if (proMod.Nodeid == 106)
                {
                    Size_L.Text = "1";
                    Backing_TR.Visible = false;
                    Outdoor_TR.Visible = false;
                    font_div.Visible = true;
                    color_7_div.Visible = true;
                }
                //有些商品未设置好,做兼容处理
                if (font_div.Visible == false && font_led_div.Visible == false)
                { font_div.Visible = true; }
                if (color_7_div.Visible == false && color_12_div.Visible == false)
                { color_12_div.Visible = true; }
                DataTable proInfo = DBCenter.Sel(proMod.TableName, "ID=" + proMod.ItemID);
                Size_L.Text = proInfo.Rows[0]["Size"].ToString();
                SKU_L.Text = proInfo.Rows[0]["SKU"].ToString();
                Save_Hid.Value = StrHelper.DecompressString(desMod.LogoContent);
                string[] urlArr = new string[] { "http://win10:164", "http://www.raysandsigns.com",
                        "http://raysandsigns.com","https://www.raysandsigns.com", "https://raysandsigns.com"
                        };
                foreach (string url in urlArr)
                {
                    Save_Hid.Value = Save_Hid.Value.Replace(url, "");
                }
                //function.Script(this, "fabHelper.init(" + StrHelper.DecompressString(desMod.LogoContent) + ");");
            }
        }
        protected void Save_Btn_Click(object sender, EventArgs e)
        {
            //B_User.CheckIsLogged(Request.RawUrl);
            mu = SnsHelper.GetLogin();
            //if (Mid > 0)
            //{
            //    desMod = desBll.SelReturnModel(Mid);
            //}
            //else { desMod = new M_Logo_Design(); }
            desMod = new M_Logo_Design();
            desMod.LogoContent = StrHelper.CompressString(Save_Hid.Value);
            ImgHelper imghelp = new ImgHelper();
            System.Drawing.Bitmap bmp = imghelp.Base64ToImg(Base64_Hid.Value);
            //用于减小尺寸
            desMod.PreviewImg = "data:image/png;base64," + imghelp.ImgToBase64ByImage(bmp);

            //if (desMod.ID > 0) { desBll.UpdateByID(desMod); }
            //else
            //{
            desMod.ZType = 0;
            desMod.ZStatus = 99;
            desMod.UserID = mu.UserID;
            desMod.UserName = mu.UserName;
            desMod.ID = desBll.Insert(desMod);
            //}
            //CreateOrder(desMod);
            CreateCart(desMod);
        }
        private int GetTlpIDByProID(int proid)
        {
            int tlp = 0;
            M_Product proMod = proBll.GetproductByid(ProID);
            if (proMod == null) { function.WriteErrMsg("Goods do not exist_"); }
            tlp = DataConvert.CLng(DBCenter.ExecuteScala(proMod.TableName, "TlpID", "ID=" + proMod.ItemID));
            return tlp;
        }
        
        private void CreateCart(M_Logo_Design desMod)
        {
            M_UserInfo mu = SnsHelper.GetLogin();
            M_Product mainProMod = proBll.GetproductByid(ProID);
            DataTable proInfo = DBCenter.Sel(mainProMod.TableName, "ID=" + mainProMod.ItemID);
            //-----购物车记录(按收前台各种传值)
            M_Cart cartMod = NewCart(mu, mainProMod);
            cartMod.ProAttr = desMod.ID.ToString();//存储设计好的信息ID
            M_Cart_Addition addMod = new M_Cart_Addition();
            addMod.flash = DataConvert.CLng(Request.Form["Flash_DP"]);
            addMod.outdoor = DataConvert.CLng(Request.Form["OutdoorSign_DP"]);
            addMod.backing = DataConvert.CLng(Request.Form["Request"]);
            addMod.text = Request.QueryString["texts"];
            addMod.size = proInfo.Rows[0]["size"].ToString();
            cartMod.Additional = JsonConvert.SerializeObject(addMod);
            if (addMod.flash > 0)
            {
                M_Product proMod = proBll.GetproductByid(addMod.flash);
                cartMod.AllMoney += proMod.LinPrice;
            }
            if (addMod.outdoor > 0)
            {
                M_Product proMod = proBll.GetproductByid(addMod.outdoor);
                cartMod.AllMoney += proMod.LinPrice;
            }
            if (addMod.backing > 0)
            {
                M_Product proMod = proBll.GetproductByid(addMod.backing);
                cartMod.AllMoney += proMod.LinPrice;
            }
            cartMod.FarePrice = cartMod.AllMoney.ToString("F2");
            cartBll.Add(cartMod);
            Response.Redirect("/Cart/Cart.aspx?ProClass=1");
        }
        private M_Cart NewCart(M_UserInfo mu,M_Product proMod){

            M_Cart cart = new M_Cart();
            cart.Cartid = CartCookID;
            cart.StoreID = proMod.UserShopID;
            cart.ProID = proMod.ID;
            cart.Pronum = 1;
            cart.userid = mu.UserID;
            cart.Username = mu.UserName;
            cart.FarePrice = proMod.LinPrice.ToString();
            cart.AllMoney = proMod.LinPrice;
            cart.Proname = proMod.Proname;
            return cart;
        }
    }
}