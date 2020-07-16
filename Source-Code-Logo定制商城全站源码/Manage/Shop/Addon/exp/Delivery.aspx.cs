using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.BLL.Shop;
using ZoomLa.Common;
using ZoomLa.Model;
using ZoomLa.Model.Shop;
using ZoomLa.Sns;
using ZoomLa.SQLDAL;

namespace ZoomLaCMS.Manage.Shop
{
    public partial class Delivery : System.Web.UI.Page
    {
        B_OrderList orderBll = new B_OrderList();
        B_Admin badmin = new B_Admin();
        B_Order_Exp expBll = new B_Order_Exp();
        B_Order_Contact conBll = new B_Order_Contact();
        public M_Order_Contact conMod = null;
        //订单号
        private int Mid { get { return DataConvert.CLng(Request.QueryString["ID"]); } }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!badmin.CheckLogin())
            {
                function.WriteErrMsg("您没有管理员权限!");
            }
            conMod = conBll.SelModelByOid(Mid);
            if (!IsPostBack)
            {
                MyBind();
            }
        }
        private void MyBind()
        {
            M_OrderList orderMod = orderBll.SelReturnModel(Mid);
            OrderNo_L.Text = orderMod.OrderNo;
            if (!string.IsNullOrEmpty(orderMod.ExpressNum))
            {
                M_Order_Exp expMod = expBll.SelReturnModel(Convert.ToInt32(orderMod.ExpressNum));
                ExpNo_T.Text = expMod.ExpNo;
                //ExpComp_DP.SelectedValue = expMod.CompType;
                //ExpOther_T.Text = expMod.ExpComp;
            }
        }
        protected void Save_Btn_Click(object sender, EventArgs e)
        {
            M_OrderList orderMod = orderBll.SelReturnModel(Mid);
            M_Order_Exp expMod = new M_Order_Exp();
            if (!string.IsNullOrEmpty(orderMod.ExpressNum))
            {
                expMod = expBll.SelReturnModel(Convert.ToInt32(orderMod.ExpressNum));
            }
            expMod.ExpNo = ExpNo_T.Text.Trim();
            //expMod.CompType = ExpComp_DP.SelectedValue;
            //if (expMod.CompType.Equals("Other ")) { expMod.ExpComp = ExpOther_T.Text; }
            //else { expMod.ExpComp = ExpComp_DP.SelectedValue; }
            if (expMod.ID > 0) { expBll.UpdateByID(expMod); }
            else
            {
                expMod.OrderID = orderMod.id;
                expMod.UserID = orderMod.Userid;
                expMod.ID = expBll.Insert(expMod);
            }
            orderMod.StateLogistics = 1;
            orderMod.ExpressNum = expMod.ID.ToString();
            orderBll.UpdateByID(orderMod);
            if (Email_Chk.Checked)
            {
                EventDeal.SendOrderEmailByType(orderMod.id, "exp");
            }
            function.Script(this, "parent.window.location= parent.location;");
        }
        private string GetProName(M_OrderList orderMod)
        {
            B_CartPro cartBll = new B_CartPro();
            DataTable dt = cartBll.SelByOrderID(orderMod.id);
            string result = "";
            foreach (DataRow dr in dt.Rows)
            {
                result += "[" + dr["ProName"] + "(数量:" + dr["ProNum"] + ")]<br />";
            }
            return result;
        }
    }
}