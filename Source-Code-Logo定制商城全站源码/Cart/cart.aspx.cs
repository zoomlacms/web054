using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.BLL.AdSystem;
using ZoomLa.BLL.Helper;
using ZoomLa.BLL.Shop;
using ZoomLa.BLL.User;
using ZoomLa.Common;
using ZoomLa.Model;
using ZoomLa.Model.AdSystem;
using ZoomLa.Model.Shop;
using ZoomLa.Model.User;
using ZoomLa.Sns;
using ZoomLa.SQLDAL;

/*
 * 购物车页面根据所传来的ItemID,SkuID,PCount将指定商品加入购物车,按店铺分栏显示
 * 自营商品的店铺StoreID为0
 */

namespace ZoomLaCMS.Cart
{
    public partial class cart : System.Web.UI.Page
    {
        ImgHelper imgHelp = new ImgHelper();
        B_OrderBaseField fieldBll = new B_OrderBaseField();
        B_Product proBll = new B_Product();
        B_Cart cartBll = new B_Cart();
        B_Shop_SuitPro suitBll = new B_Shop_SuitPro();
        //B_Shop_RegionPrice regionBll = new B_Shop_RegionPrice();
        OrderCommon orderCom = new OrderCommon();
        public int StoreID { get { return DataConvert.CLng(Request.QueryString["StoreID"], -100); } }
        //仅用于标识显示积分商品,或普通商品,不参与其他逻辑
        public int ProClass = 1;
        public int TipID { get { return DataConvert.CLng(Request.QueryString["TipID"]); } }
        //Cookies中的购物车ID
        public string CartCookID
        {
            get
            {
                return B_Cart.GetCartID(Context);
            }
        }
        private DataTable CartDT
        {
            get
            {
                return (DataTable)ViewState["CartDT"];
            }
            set { ViewState["CartDT"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                M_UserInfo mu = SnsHelper.GetLogin();
                int proid = DataConvert.CLng(Request["id"]);
                //int suitid = DataConvert.CLng(Request["suitid"]);
                int pronum = DataConvert.CLng(Request["pronum"], 1);
                int pclass = -1;//非-1则为添加了商品,需要跳转
                if (proid > 0)
                {
                    M_Product proMod = proBll.GetproductByid(proid);
                    if (proMod == null) { function.WriteErrMsg("Goods do not exist"); }
                    AddToCart(mu, proMod, pronum);
                    pclass = proMod.ProClass;
                }
                if (function.isAjax()) { return; }//ajax下不需要数据绑定与跳转
                if (pclass > -1) { Response.Redirect(Request.Url.AbsolutePath + "?ProClass=" + pclass); return; }
                MyBind();
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            CartDT = null;
        }
        private void MyBind()
        {
            M_UserInfo mu = SnsHelper.GetLogin();
            //B_Cart.UpdateUidByCartID(CartCookID, mu);
            CartDT = SelByCartID(CartCookID, 0, ProClass);//从数据库中获取
            if (StoreID != -100)//仅显示指定商铺的商品
            {
                CartDT.DefaultView.RowFilter = "StoreID="+StoreID;
                CartDT = CartDT.DefaultView.ToTable();
            }
            RPT.DataSource = orderCom.SelStoreDT(CartDT);
            RPT.DataBind();
            totalmoney.InnerText = TotalPrice.ToString("F2");
        }
        protected void RPT_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView dr = e.Item.DataItem as DataRowView;
                Repeater rpt = e.Item.FindControl("ProRPT") as Repeater;
                CartDT.DefaultView.RowFilter = "StoreID=" + dr["ID"];
                DataTable dt = CartDT.DefaultView.ToTable();
                if (dt.Rows.Count < 1) { e.Item.Visible = false; }
                rpt.DataSource = dt;
                rpt.DataBind();
            }
        }
        protected void ProRPT_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView dr = e.Item.DataItem as DataRowView;

                Label ptLabel = e.Item.FindControl("Present_HTML") as Label;
                ptLabel.Text = PageHelper.Aspx_GetHtml("/cart/comp/cart_logo.aspx", dr);
            }
        }
        //结算,到订单页再生成AllMoney
        protected void NextStep_Click(object sender, EventArgs e)
        {
            //AJAX就先检测一遍,未登录则弹窗
            //M_UserInfo mu = SnsHelper.GetLogin();//提交前同步一次,避免即时登录造成的cart中id未与用户关联
            //B_Cart.UpdateUidByCartID(CartCookID, mu);
            string ids = Request.Form["prochk"];
            Response.Redirect("GetOrderInfo.aspx?ids=" + ids + "&ProClass=" + ProClass + "&TipID=" + TipID);//"#none"
        }
        //-----前端View
        //将计算出的单价缓存,用于避免重复计算
        private double TempPrice = 0;
        private double TotalPrice = 0;
        //单项商品小计
        public string GetPrice()
        {
            int pronum = Convert.ToInt32(Eval("Pronum"));
            double total = TempPrice * pronum;
            TotalPrice += total;
            return total.ToString("0.00");
        }
        //单价
        public string GetMyPrice()
        {
            int proID = Convert.ToInt32(Eval("ProID"));
            double linPrice = TempPrice = Convert.ToDouble(Eval("LinPrice"));
            M_UserInfo mu = SnsHelper.GetLogin();
            M_Product proMod = proBll.GetproductByid(proID);
            //多区域价格
            //if (string.IsNullOrEmpty(Region))
            //{
            //    Region = buser.GetRegion(mu.UserID);
            //}
            //TempPrice = regionBll.GetRegionPrice(proID, linPrice, Region, mu.GroupID);
            //如果多区域价格未匹配,则匹配会员价
            if (TempPrice == linPrice || TempPrice <= 0) { TempPrice = proBll.P_GetByUserType(proMod, mu); }
            string html = "<i class='fa fa-rmb'></i><span id='price_" + Eval("ID") + "'>" + TempPrice.ToString("f2") + "</span>";
            //if (orderCom.HasPrice(Eval("LinPrice_Json")))
            //{
            //    string json = DataConvert.CStr(Eval("LinPrice_Json"));
            //    M_LinPrice priceMod = JsonConvert.DeserializeObject<M_LinPrice>(json);
            //    if (priceMod.Purse > 0)
            //    {
            //        html += "余额:<span id='price_purse_" + Eval("ID") + "'>" + priceMod.Purse.ToString("f2") + "</span>";
            //    }
            //    if (priceMod.Sicon > 0)
            //    {
            //        html += "|银币:<span id='price_sicon_" + Eval("ID") + "'>" + priceMod.Sicon.ToString("f0") + "</span>";
            //    }
            //    if (priceMod.Point > 0)
            //    {
            //        html += "|积分:<span id='price_point_" + Eval("ID") + "'>" + priceMod.Point.ToString("f0") + "</span>";
            //    }
            //}
            return html;
        }
        //------------------------------Tools
        // 生成购物车编号("Shopby OrderNo"的value) 
        //返回当前登录用户,如未登录,则返回0
        private M_UserInfo GetLogin()
        {
            M_UserInfo mu = SnsHelper.GetLogin();
            if (mu == null)
            {
                mu = new M_UserInfo();
                mu.UserID = 0;
                mu.UserName = "未登录";
            }
            return mu;
        }
        //根据传参将商品加入购物车后跳转(支持按商品ID,套装ID购买)
        private void AddToCart(M_UserInfo mu, M_Product proMod, int pronum)
        {
            if (pronum < 1) { pronum = 1; }
            if (proMod == null || proMod.ID < 1) { return; }//商品不存在
            DataTable dt = DBCenter.Sel(proMod.TableName,"ID="+proMod.ItemID);
            int tlpID = DataConvert.CLng(dt.Rows[0]["TlpID"]);
            //if (tlpID < 1) { function.WriteErrMsg("商品未绑定设计模板"); }
            //复制一份作为用户新建的记录
            B_Logo_Design desBll = new B_Logo_Design();
            M_Logo_Design desMod = desBll.SelReturnModel(tlpID);
			if(desMod==null){desMod=new M_Logo_Design();}
            M_Cart_Addition addMod = new M_Cart_Addition();
            //if (desMod == null) { function.WriteErrMsg("商品未绑定模板"); }
            desMod.ID = 0;
            desMod.ZType = 0;
            desMod.CDate = DateTime.Now;
            desMod.UserID = mu.UserID;
            desMod.UserName = mu.UserName;
            //不经设计直接加入物车,则获取商品预览图片
            string imgUrl = function.GetImgUrl(proMod.Thumbnails);
            //if (File.Exists(function.VToP(imgUrl)))
            //{
            //    desMod.PreviewImg = "data:image/png;base64," + ImgToBase64(imgUrl);
            //}
            desMod.PreviewImg = imgUrl;
            desMod.ID= desBll.Insert(desMod);
            //-----------------检测完成加入购物车
            M_Cart cartMod = new M_Cart();
            cartMod.Cartid = CartCookID;
            cartMod.StoreID = proMod.UserShopID;
            cartMod.ProID = proMod.ID;
            cartMod.Pronum = pronum;
            cartMod.userid = mu.UserID;
            cartMod.Username = mu.UserName;
            cartMod.FarePrice = proMod.LinPrice.ToString();
            cartMod.AllMoney = (proMod.LinPrice * cartMod.Pronum);
            cartMod.ProAttr = desMod.ID.ToString();
            cartMod.Proname = proMod.Proname;
            cartMod.Additional = JsonConvert.SerializeObject(addMod);
            int id = cartBll.insert(cartMod);
        }

        private DataTable SelByCartID(string cartid, int uid, int proClass, string ids = "")
        {
            if (string.IsNullOrEmpty(cartid) && uid < 1) { return null; }
            string fields = " A.*";
            fields += ",B.LinPrice,B.Thumbnails,B.ProClass,B.ProUnit,B.[Class],B.NodeID";
            string where = "";
            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("cartid", cartid) };
            //if (uid > 0) { where = " (A.Cartid=@cartid OR A.UserID=" + uid + ")"; } else { where = " A.Cartid=@cartid"; }
            if (uid > 0) { where = " A.UserID=" + uid; }
            else { where = " A.CartID=@cartid"; }
            //--------------------------------------
            if (!string.IsNullOrEmpty(ids)) { SafeSC.CheckIDSEx(ids); where += " AND A.ID IN (" + ids + ")"; }
            if (proClass != -100) { where += " AND B.ProClass=" + proClass; }
            string sql = "SELECT " + fields + " FROM ZL_Cart A LEFT JOIN ZL_Commodities B ON A.ProID=B.ID WHERE " + where;
            //自营商品,店铺商品
            DataTable dt = SqlHelper.ExecuteTable(sql, sp);
            return dt;
        }
        public string ImgToBase64(string vpath)
        {
            string ppath = function.VToP(SafeSC.PathDeal(vpath));
            if (!File.Exists(ppath)) throw new Exception(vpath + "文件不存在");
            System.Drawing.Image image = System.Drawing.Image.FromFile(ppath);
            Bitmap bmp = new Bitmap(image, new Size(160, 90));
            return imgHelp.ImgToBase64ByImage(bmp);
        }
		public string GetShopImg(object p)
        {
            M_Logo_Design desMod = new M_Logo_Design();
            B_Logo_Design desBll = new B_Logo_Design();
            desMod = desBll.SelReturnModel(DataConverter.CLng(p));
            if (desMod == null || string.IsNullOrEmpty(desMod.PreviewImg)) { return ""; }
            return desMod.PreviewImg;
        }
    }
}