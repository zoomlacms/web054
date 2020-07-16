using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;

namespace ZoomLaCMS.Cart.Comp
{
    public partial class AddressList : System.Web.UI.Page
    {
        B_User buser = new B_User();
        B_UserRecei receBll = new B_UserRecei();
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable addressDT = receBll.SelByUID(buser.GetLogin().UserID);
            AddressRPT.DataSource = addressDT;
            AddressRPT.DataBind();
            EmptyDiv.Visible = addressDT.Rows.Count < 1;//地址为空提醒
        }
        public string GetAddress()
        {
            string tlp = "{0} {1}({2} 收) {3} ";
            return string.Format(tlp, Eval("Provinces"), Eval("Street"), Eval("ReceivName"), Eval("MobileNum"));
        }
        public string GetIsDef()
        {
            if (Eval("isDefault").ToString().Equals("1"))
            {
                return "<span class='grayremind'>默认地址</span>";
            }
            else
                return "";
            //else
            //{
            //    return "<a href='javascript:;' onclick='SetDefault();'>设为默认地址</a>";
            //}
        }
    }
}