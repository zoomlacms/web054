using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL.AdSystem;
using ZoomLa.SQLDAL;
using ZoomLa.SQLDAL.SQL;

public partial class Extend_ChooseTlp : System.Web.UI.Page
{
    B_Logo_Design desBll = new B_Logo_Design();
    public string SKey { get { return HttpUtility.UrlDecode(Request["SKey"]); } }
    public int Mid { get { return DataConvert.CLng(Request["id"]); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        //PageSetting setting = desBll.SelPage(1, 50, new F_Logo_Design()
        //{
        //    skey = SKey,
        //    ztype = 1
        //});
        Tlp_RPT.DataSource = Sel();
        Tlp_RPT.DataBind();
    }
    public DataTable Sel()
    {
        List<SqlParameter> sp = new List<SqlParameter>();
        string where = "ZType=1 ";
        if (!string.IsNullOrEmpty(SKey))
        {
            sp.Add(new SqlParameter("skey", "%" + SKey + "%"));
            where += " AND Alias LIKE @skey";
        }
        if (Mid > 0)
        {
            where += " AND ID=" + Mid;
        }
        return DBCenter.SelTop(50, "ID", "ID,Alias,PreviewImg", "ZL_Logo_Design", where, "ID DESC", sp);
    }
}