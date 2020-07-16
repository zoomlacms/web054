using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.BLL.Helper;
using ZoomLa.Model;
using ZoomLa.SQLDAL;

namespace ZoomLaCMS.Cart.Comp
{
    //根据商品+金额,计算出可用的优惠券,目标ashx也需要验证
    //仅供订单页使用
    public partial class ArriveList : System.Web.UI.Page
    {
        B_Arrive avBll = new B_Arrive();
        B_User buser = new B_User();
        B_Cart cartBll = new B_Cart();
        protected void Page_Load(object sender, EventArgs e)
        {
            M_UserInfo mu = buser.GetLogin();
            double allmoney = DataConvert.CDouble(Request["allmoney"]);
            string ids = DataConvert.CStr(Request.QueryString["ids"]);
            //------用户有哪些未使用的优惠券
            DataTable avdt = avBll.U_Sel(mu.UserID, -100, 1);
            DataTable cartdt = null;
            if (avdt.Rows.Count < 1) { arrive_empty_div.Visible = true; return; }
            else
            {
                arrive_div.Style.Add("display", "block");
                arrive_data_div.Visible = true;
                avdt.Columns.Add("enable", typeof(int));//优惠券是否有效 1:有效
                avdt.Columns.Add("err", typeof(string));//不能使用的原因
                cartdt = cartBll.SelByCartID(B_Cart.GetCartID(), mu.UserID, -100, ids);
            }
            //---------------------------------------Logical
            for (int i = 0; i < avdt.Rows.Count; i++)
            {
                DataRow dr = avdt.Rows[i];
                M_Arrive avMod = new M_Arrive().GetModelFromReader(dr);
                double money = allmoney;
                M_Arrive_Result retMod = avBll.U_CheckArrive(avMod, mu.UserID, cartdt, money);
                dr["enable"] = retMod.enabled ? 1 : 0;
                dr["err"] = retMod.err;
            }
            avdt.DefaultView.RowFilter = "enable='1'";
            Arrive_Active_RPT.DataSource = avdt.DefaultView.ToTable();
            Arrive_Active_RPT.DataBind();
            avdt.DefaultView.RowFilter = "enable='0'";
            Arrive_Disable_RPT.DataSource = avdt.DefaultView.ToTable();
            Arrive_Disable_RPT.DataBind();
        }
        public string Arrive_GetMoneyRegion() { return avBll.GetMoneyRegion(DataConvert.CDouble(Eval("MinAmount")), DataConvert.CDouble(Eval("MaxAMount"))); }
        public string Arrive_GetTypeStr()
        {
            DataRowView drv = GetDataItem() as DataRowView;
            return avBll.GetTypeStr(drv.Row);
            //return avBll.GetTypeStr(Convert.ToInt32(Eval("Type")));
        }
    }
}