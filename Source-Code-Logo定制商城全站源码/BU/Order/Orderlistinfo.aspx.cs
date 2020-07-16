using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.BLL.Shop;
using ZoomLa.Common;
using ZoomLa.Model;
using ZoomLa.Model.Shop;
using ZoomLa.Sns;
using ZoomLa.SQLDAL;

namespace ZoomLaCMS.BU.Order
{
    public partial class Orderlistinfo : System.Web.UI.Page
    {
        B_OrderList orderBll = new B_OrderList();
        B_Order_Exp expBll = new B_Order_Exp();
        B_Payment payBll = new B_Payment();

        public M_OrderList orderMod = null;
        public M_Order_Contact conMod = null;
        public M_Payment payMod = null;
        public M_Order_Exp expMod = null;
        public string OrderNo { get { return DataConvert.CStr(Request.QueryString["OrderNo"]); } }
        protected void Page_Load(object sender, EventArgs e)
        {
            orderMod = orderBll.SelModelByOrderNo(OrderNo);
            if (orderMod == null) { function.WriteErrMsg("Order does not exist!"); }


            conMod = new B_Order_Contact().SelModelByOid(orderMod.id);
            payMod = payBll.SelModelByOrder(orderMod);
            expMod = expBll.SelReturnModel(DataConvert.CLng(orderMod.ExpressNum));
            if (expMod == null) { expMod = new M_Order_Exp(); }
            StringWriter sw = new StringWriter();
            Server.Execute("/Common/MailTlp/Cart_List.aspx?OrderID=" + orderMod.id,sw);
            CartList_HTML.Text = sw.ToString();
         
        }


      
    }
}