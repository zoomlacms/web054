using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.Sns;
using ZoomLa.SQLDAL.SQL;

public partial class Extend_Admin_Coupon : System.Web.UI.Page
{
    B_Ex_Coupon cupBll = new B_Ex_Coupon();
    protected void Page_Load(object sender, EventArgs e)
    {
        RPT.SPage = MyBind;
        if (!IsPostBack)
        {
            RPT.DataBind();
        }
    }
    public DataTable MyBind(int psize, int cpage)
    {
        PageSetting setting = cupBll.SelPage(cpage,psize);
        RPT.ItemCount = setting.itemCount;
        return setting.dt;
    }

    protected void RPT_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "del":
                cupBll.Del(e.CommandArgument.ToString());
                RPT.DataBind();
                break;
        }
    }
}