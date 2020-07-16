using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.BLL.AdSystem;
using ZoomLa.BLL.CreateJS;
using ZoomLa.BLL.Helper;
using ZoomLa.BLL.Shop;
using ZoomLa.Common;
using ZoomLa.Components;
using ZoomLa.Model;
using ZoomLa.Model.AdSystem;
using ZoomLa.Model.Shop;
using ZoomLa.Sns;
using ZoomLa.SQLDAL;

namespace ZoomLaCMS.Cart
{
    // ==其中 A 不能是 D, F, I, O, Q, W, Z。B 、C不能是D, F, I, O, Q9 – 可以是任意数字
    public partial class getOrderInfo : System.Web.UI.Page
    {
        B_Arrive avBll = new B_Arrive();
        B_Cart cartBll = new B_Cart();
        B_CartPro cartProBll = new B_CartPro();
        B_UserRecei receBll = new B_UserRecei();
        B_OrderList orderBll = new B_OrderList();
        B_OrderBaseField fieldBll = new B_OrderBaseField();
        B_Shop_FareTlp fareBll = new B_Shop_FareTlp();
        B_Product proBll = new B_Product();
        B_Payment payBll = new B_Payment();
        B_Shop_RegionPrice regionBll = new B_Shop_RegionPrice();
        B_Shop_Present ptBll = new B_Shop_Present();
        OrderCommon orderCom = new OrderCommon();
        private double allmoney = 0;//购物车中商品金额统计
        public int ProClass { get { return DataConvert.CLng(Request.QueryString["Proclass"]); } }
        public string ids { get { return Request.QueryString["ids"]; } }
        public int TipID { get { return DataConvert.CLng(Request.QueryString["TipID"]); } }
        //用于区域价格
        private string Region { get { return ViewState["Region"] as string; } set { ViewState["Region"] = value; } }
        private DataTable CartDT
        {
            get
            {
                return (DataTable)ViewState["CartDT"];
            }
            set { ViewState["CartDT"] = value; }
        }
        /*----------------------------------------------------------------------------------------------------*/
        protected void Page_Load(object sender, EventArgs e)
        {
            //B_User.CheckIsLogged(Request.RawUrl);
            M_UserInfo mu = SnsHelper.GetLogin();
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(ids)) { function.WriteErrMsg("Please choose the goods you need to buy first"); }
                ReUrl_A1.HRef = "/Cart/Cart.aspx?Proclass=" + ProClass;
                ReUrl_A1.Visible = true;
                MyBind();
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            CartDT = null;
        }
        public void MyBind()
        {
            M_UserInfo mu = SnsHelper.GetLogin();
            CartDT = SelByCartID(B_Cart.GetCartID(), mu.UserID, ProClass, ids);
            if (CartDT.Rows.Count < 1)
            {
                function.WriteErrMsg("You have not yet chosen the goods");
            }
            //------核算费用
            allmoney = UpdateCartAllMoney(CartDT);
            //------费用统计
            itemnum_span.InnerText = CartDT.Rows.Count.ToString();
            totalmoney_span1.InnerText = allmoney.ToString("f2");
            //------店铺
            Store_RPT.DataSource = orderCom.SelStoreDT(CartDT);
            Store_RPT.DataBind();
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
        public string GetShopImg(object p)
        {
            M_Logo_Design desMod = new M_Logo_Design();
            B_Logo_Design desBll = new B_Logo_Design();
            desMod = desBll.SelReturnModel(DataConverter.CLng(p));
            return desMod.PreviewImg.ToString();
        }
        protected void Store_RPT_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = e.Item.DataItem as DataRowView;
                //按店铺展示商品列表
                Repeater rpt = e.Item.FindControl("ProRPT") as Repeater;
                CartDT.DefaultView.RowFilter = "StoreID=" + drv["ID"];
                DataTable dt = CartDT.DefaultView.ToTable();
                if (dt.Rows.Count < 1) { e.Item.Visible = false; }
                else
                {
                    rpt.DataSource = dt;
                    rpt.DataBind();
                    //运费计算";
                    //Literal html_lit = e.Item.FindControl("FareType_L") as Literal;
                    //DataTable fareDT = GetFareDT(dt);
                    //html_lit.Text = CreateFareHtml(fareDT);
                }
            }
        }
        protected void ProRPT_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView dr = e.Item.DataItem as DataRowView;
                switch (DataConvert.CLng(dr["Class"]))
                {
                    case 2://套装促销
                        Repeater rpt = e.Item.FindControl("SuitPro_RPT") as Repeater;
                        rpt.DataSource = proBll.Suit_GetProduct(DataConvert.CStr(dr["procontent"]), DataConvert.CLng(dr["Pronum"]));
                        rpt.DataBind();
                        break;
                    default://普通商品,支持赠品等促销逻辑
                        {
                            Label ptLabel = e.Item.FindControl("Present_HTML") as Label;
                            ptLabel.Text = PageHelper.Aspx_GetHtml("/cart/comp/cart_present.aspx", dr);
                        }
                        break;
                }
            }
        }
        protected void AddOrder_Btn_Click(object sender, EventArgs e)
        {
            //1,生成订单,2,关联购物车中商品为已绑定订单
            M_UserInfo mu = SnsHelper.GetLogin();
            DataTable cartDT = SelByCartID(B_Cart.GetCartID(), mu.UserID, ProClass, ids);//需要购买的商品
            if (cartDT.Rows.Count < 1) function.WriteErrMsg("You have not yet chosen the goods");
            //------检测End
            //按店铺生成订单
            DataTable storeDT = cartDT.DefaultView.ToTable(true, "StoreID");
            List<M_OrderList> orderList = new List<M_OrderList>();//用于生成临时订单,统计计算(Disuse)
            foreach (DataRow dr in storeDT.Rows)
            {
                M_OrderList Odata = new M_OrderList();
                Odata.Ordertype = OrderHelper.GetOrderType(ProClass);

                Odata.OrderNo = GetOrderNo();
                Odata.StoreID = Convert.ToInt32(dr["StoreID"]);
                cartDT.DefaultView.RowFilter = "StoreID=" + Odata.StoreID;
                DataTable storeCartDT = cartDT.DefaultView.ToTable();


                Odata.Promoter = TipID;
                Odata.Invoiceneeds = DataConverter.CLng(Request.Form["invoice_rad"]);//是否需开发票
                //Odata.Invoice = Odata.Invoiceneeds == 0 ? "" : InvoTitle_T.Text + "||" + Invoice_T.Text;
                Odata.Rename = mu.UserName;
                Odata.AddUser = mu.UserName; ;
                Odata.Userid = mu.UserID;
                Odata.Ordermessage = Server.HtmlEncode((Request.Form["orderMsg"] ?? ""));// ORemind_T.Text;//订货留言
                //-----金额计算
                Odata.Balance_price = GetTotalMoney(storeCartDT);
                Odata.Freight = 0;
                Odata.Ordersamount = Odata.Balance_price + Odata.Freight;//订单金额
                Odata.AllMoney_Json = orderCom.GetTotalJson(storeCartDT);//附加需要的虚拟币
                Odata.Specifiedprice = Odata.Ordersamount; //订单金额;
                Odata.OrderStatus = (int)M_OrderList.StatusEnum.Normal;//订单状态
                Odata.Paymentstatus = (int)M_OrderList.PayEnum.NoPay;//付款状态
                                                                     //Odata.Integral = DataConverter.CLng(Request.QueryString["jifen"]);
                                                                     //Odata.ExpTime = exptime_hid.Value;                                                                
                M_Grade countryMod = B_GradeOption.GetGradeOption(Convert.ToInt32(Request.Form["country"]));
                M_Grade stateMod = B_GradeOption.GetGradeOption(Convert.ToInt32(Request.Form["state"]));
                Odata.Money_rate = 0;

                Odata.Receiver = Request.Form["FullName"];
                Odata.Phone = Request.Form["phone"];
                Odata.MobileNum = Request.Form["phone"];
                Odata.Email = Request.Form["email"];
                Odata.Diqu = countryMod.GradeName;
                Odata.Shengfen = stateMod.GradeName;
                Odata.Chengshi = Request.Form["city"];
                Odata.Jiedao = Request.Form["address"];
                Odata.ZipCode = Request.Form["zip"];





                Odata.id = orderBll.insert(Odata);
                CopyToCartPro(mu, storeCartDT, Odata.id);
                orderList.Add(Odata);
                //orderCom.SendMessage(Odata, null, "ordered");
                //-----联系人
                B_Order_Contact conBll = new B_Order_Contact();
                M_Order_Contact conMod = new M_Order_Contact();

                conMod.OrderID = Odata.id;
                conMod.FullName = Request.Form["fullname"];
                conMod.Email = Request.Form["email"];
                conMod.Address = Request.Form["address"];
                conMod.City = Request.Form["city"];
                conMod.State = stateMod.GradeName;
                conMod.Country = countryMod.GradeName;
                conMod.Zip = Request.Form["zip"];
                conMod.Phone = Request.Form["phone"];
                conMod.Remark = "";
                conBll.Insert(conMod);
            }
            //cartBll.DelByids(ids);//客户希望保留购物车中信息
            //-----------------订单生成后处理
            M_Payment payMod = payBll.CreateByOrder(orderList);
            //优惠券,编号与密码
            if (!string.IsNullOrEmpty(Coupon_Num.Text))
            {
                B_Ex_Coupon cupBll = new B_Ex_Coupon();
                string code = Coupon_Num.Text.Trim();
                //string pwd = Coupon_Pwd.Text.Trim();
                //M_Arrive avMod = SnsHelper.AV_SelModel(code);
                //M_Arrive_Result retMod = SnsHelper.CheckArrive(code, pwd, payMod.MoneyPay);
                //if (retMod.enabled)
                //{
                //    payMod.MoneyPay = retMod.money;
                //    payMod.ArriveMoney = retMod.amount;
                //    payMod.ArriveDetail = "优惠券号:" + code + ",密码:" + pwd;
                //    SnsHelper.Use_Arrive(avMod, "支付单[" + payMod.PayNo + "]使用,优惠金额:" + retMod.amount.ToString("F2"));
                //}
                //else { function.WriteErrMsg(retMod.err); }
                M_Arrive_Result retMod = cupBll.CheckArrive(code, payMod.MoneyPay);
                if (retMod.enabled)
                {
                    payMod.MoneyPay = retMod.money;
                    payMod.ArriveMoney = retMod.amount;
                    payMod.ArriveDetail = "优惠券号:" + code;
                }
            }
            payMod.MoneyReal = payMod.MoneyPay;
            payMod.Remark = cartDT.Rows.Count > 1 ? "[" + cartDT.Rows[0]["ProName"] as string + "]等" : cartDT.Rows[0]["ProName"] as string;
            payMod.PaymentID = payBll.Add(payMod);
            //取消下面注释激活下单立即发送邮件
            EventDeal.SendOrderEmailByType(orderList[0].id, "order", SiteConfig.SiteInfo.WebmasterEmail);
            Response.Cookies["agree_save"].Value = Request.Form["agree_save_chk"];
            Response.Cookies["agree_save"].Expires = DateTime.Now.AddYears(1);
            Response.Redirect("/PayOnline/Orderpay.aspx?PayNo=" + payMod.PayNo);
        }
        /*----------------------------------------------------------------------------------------------------*/
        #region 重算商品金额
        //更新购物车中的AllMoney(实际购买总价),便于后期查看详情
        private double UpdateCartAllMoney(DataTable dt)
        {
            //M_UserInfo mu = SnsHelper.GetLogin();
            double allmoney = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                M_Cart cartMod = new M_Cart().GetModelFromReader(dr);
                //M_Product proMod = proBll.GetproductByid(Convert.ToInt32(dr["Proid"]));
                //--附加币值计价
                //if (orderCom.HasPrice(proMod.LinPrice_Json))
                //{
                //    M_LinPrice priceMod = JsonConvert.DeserializeObject<M_LinPrice>(proMod.LinPrice_Json);
                //    priceMod.Purse = priceMod.Purse * cartMod.Pronum;
                //    priceMod.Sicon = priceMod.Sicon * cartMod.Pronum;
                //    priceMod.Point = priceMod.Point * cartMod.Pronum;
                //    dr["AllMoney_Json"] = JsonConvert.SerializeObject(priceMod);
                //    cartMod.AllMoney_Json = DataConvert.CStr(dr["AllMoney_Json"]);
                //}
                //根据商品价格类型,看使用  零售|批发|会员|会员组价格
                //多区域价格
                //if (string.IsNullOrEmpty(Region))
                //{
                //    Region = buser.GetRegion(mu.UserID);
                //}
                //double price = regionBll.GetRegionPrice(proMod.ID, proMod.LinPrice, Region, mu.GroupID);
                ////如果多区域价格未匹配,则匹配会员价
                //if (price == proMod.LinPrice) { price = proBll.P_GetByUserType(proMod, mu); }
                //double price = proBll.P_GetByUserType(proMod, mu);
                //--多价格编号,则使用多价格编号的价钱,ProName(已在购物车页面更新)
                //double price =proBll.GetPriceByCode(dr["code"], proMod.Wholesalesinfo, ref price);
                //cartMod.AllMoney = price * cartMod.Pronum;
                //cartMod.AllIntegral = cartMod.AllMoney;
                //cartMod.FarePrice = price.ToString("F2");
                //----检查有无价格方面的促销活动,如果有,检免多少金额
                //{
                //    W_Filter filter = new W_Filter();
                //    filter.cartMod = cartMod;
                //    filter.TypeFilter = "money";
                //    ptBll.WhereLogical(filter);
                //    cartMod.AllMoney = cartMod.AllMoney - filter.DiscountMoney;
                //}
                //----计算折扣
                dr["AllMoney"] = cartMod.AllMoney;
                dr["AllIntegral"] = cartMod.AllIntegral;
                //if (proMod.Recommend > 0)
                //{
                //    dr["AllMoney"] = (cartMod.AllIntegral - (cartMod.AllIntegral * ((double)proMod.Recommend / 100)));
                //    cartMod.AllMoney = Convert.ToDouble(dr["AllMoney"]);
                //}
                //cartBll.UpdateByID(cartMod);
                allmoney += cartMod.AllMoney;
            }
            return allmoney;
        }
        //获取总金额
        private double GetTotalMoney(DataTable dt)
        {
            //不需要再重新计算,因为每次进入页面都会重算
            return Convert.ToDouble(dt.Compute("SUM(AllMoney)", ""));
        }
        #endregion
        #region Common
        // True有库存
        public bool HasStock(object allowed, int stock, int pronum)
        {
            bool flag = true;
            if (allowed.ToString().Equals("0") && stock < pronum)
            {
                flag = false;
            }
            return flag;
        }
        #endregion
        #region 积分抵扣
        //用户最大能使用多少带你分
        private int Point_CanBeUse(double orderMoney)
        {
            return 0;
        }
        //积分兑换为资金
        private double Point_ToMoney(int points)
        {
            if (points <= 0) { return 0; }
            return points * SiteConfig.ShopConfig.PointRate;
        }
        #endregion
        /// <summary>
        /// 拷贝一份至ZL_CartPro长久保存
        /// </summary>
        public void CopyToCartPro(M_UserInfo mu, DataTable dt, int oid)
        {
            B_Product proBll = new B_Product();
            string[] fields = "Additional,StoreID,AllMoney_Json,code".Split(',');
            foreach (string field in fields)
            {
                if (!dt.Columns.Contains(field)) { dt.Columns.Add(new DataColumn(field, typeof(string))); }
            }
            foreach (DataRow dr in dt.Rows)
            {
                M_Product proMod = proBll.GetproductByid(Convert.ToInt32(dr["Proid"]));
                M_CartPro model = new M_CartPro();
                model.Orderlistid = oid;
                model.ProID = proMod.ID;
                model.Pronum = DataConverter.CLng(dr["Pronum"]);
                model.Proname = proMod.Proname;
                model.Shijia = Convert.ToDouble(dr["FarePrice"]);
                model.Danwei = proMod.ProUnit;
                model.Addtime = DateTime.Now;
                model.StoreID = DataConvert.CLng(dr["StoreID"]);
                model.code = DataConvert.CStr(dr["code"]);
                model.Attribute = DataConvert.CStr(dr["ProAttr"]);
                if (!dt.Columns.Contains("AllMoney")) { model.AllMoney = proMod.LinPrice * model.Pronum; } else { model.AllMoney = Convert.ToDouble(dr["AllMoney"]); }
                //model.Additional = DataConvert.CStr(dr["Additional"]);
                //model.AllMoney_Json = DataConvert.CStr(dr["AllMoney_Json"]);
                //如果非促销组合,则不保存商品简介和详情
                if (proMod.Class != 2) { proMod.Procontent = ""; proMod.Proinfo = ""; }
                //原价与优惠信息(便于展示)
                //model.Usepoint = 0;
                //model.UsePointArrive = 0;
                //model.FarePrice = "0";
                //model.ArriveMoney = 0;
                //model.ArriveRemind = "";
                #region 保存购买时用户的信息
                model.Username = mu.UserName;
                model.Additional = DataConvert.CStr(dr["Additional"]);
                //model.Additional = JsonHelper.GetJson(new string[] { "UserID", "GroupID", "PUserID" }, new object[] { mu.UserID, mu.GroupID, mu.ParentUserID });
                #endregion
                #region 将整个商品信息备份(主要是价格和配置部分)
                model.PClass = proMod.Class.ToString();
                //model.ProInfo = JsonConvert.SerializeObject(backup);
                #endregion
                model.ID = cartProBll.GetInsert(model);
            }
        }
        public string GetOrderNo()
        {
            string where = "AddTime>'" + DateTime.Now.ToString("yyyy/MM/dd 00:00:00") + "' AND AddTime<'" + DateTime.Now.ToString("yyyy/MM/dd 23:59:59") + "'";
            int count = DBCenter.Count("ZL_OrderInfo", where);
            string order = DateTime.Now.ToString("MMddyy") + (300 + count + 1);
            return order;
        }
    }
}