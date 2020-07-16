using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL.AdSystem;
using ZoomLa.SQLDAL.SQL;

namespace ZoomLaCMS.Manage.Plus.Logo
{
    public partial class DesTlpList : System.Web.UI.Page
    {
        B_Logo_Design desBll = new B_Logo_Design();
        protected void Page_Load(object sender, EventArgs e)
        {
            RPT.SPage = MyBind;
            if (!IsPostBack)
            {
                RPT.DataBind();
            }
        }
        private DataTable MyBind(int psize,int cpage)
        {
            PageSetting setting= desBll.SelPage(cpage,psize, new F_Logo_Design()
            {
                ztype = 1,
                skey = skey.Value
            });
            RPT.ItemCount = setting.itemCount;
            return setting.dt;
        }
        protected void BatDel_Btn_Click(object sender, EventArgs e)
        {
            string ids = Request.Form["idchk"];
            desBll.Del(ids);
            RPT.DataBind();
        }
        protected void RPT_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "del2":
                    desBll.Del(Convert.ToInt32(e.CommandArgument).ToString());
                    RPT.DataBind();
                    break;
            }
        }

        protected void Search_Btn_Click(object sender, EventArgs e)
        {
            RPT.DataBind();
        }
    }
}