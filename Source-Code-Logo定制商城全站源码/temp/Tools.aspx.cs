using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL.AdSystem;
using ZoomLa.BLL.Helper;
using ZoomLa.Common;
using ZoomLa.Model.AdSystem;
using ZoomLa.SQLDAL;

public partial class temp_Tools : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dt = DBCenter.SelTop(1, "ID", "*", "ZL_Logo_Design", "Zstatus IN(0) AND ZType=1", "");
            if (dt.Rows.Count < 1) { function.WriteErrMsg("取到的值为空"); }
            int id = DataConvert.CLng(dt.Rows[0]["ID"]);
            string content = DataConvert.CStr(dt.Rows[0]["LogoContent"]);
            Mid_Hid.Value = dt.Rows[0]["ID"].ToString();
            if (string.IsNullOrEmpty(content))
            {
                Ignore_Btn_Click(null,null);
            }
            Content_Hid.Value = content;
            Content_Hid.Value = StrHelper.DecompressString(Content_Hid.Value);
            string[] domainArr = new string[] {
                "https://www.raysandsigns.com/",
                "http://www.raysandsigns.com/",
                "https://raysandsigns.com/",
                "http://raysandsigns.com/",
                "http://win10:164/"
            };
            foreach (string domain in domainArr)
            {
                Content_Hid.Value = Content_Hid.Value.Replace(domain, "/");
            }
            
        }
    }
    B_Logo_Design desBll = new B_Logo_Design();
    M_Logo_Design desMod = new M_Logo_Design();
    protected void Save_Btn_Click(object sender, EventArgs e)
    {
        int Mid = DataConvert.CLng(Mid_Hid.Value);
        string content = Content_Hid.Value;
        //-------------------
        
        desMod = desBll.SelReturnModel(Mid);
        desMod.LogoContent = StrHelper.CompressString(Content_Hid.Value);
        desMod.ZStatus = 99;
        desBll.UpdateByID(desMod);
        Response.Redirect(Request.RawUrl);

    }

    protected void Ignore_Btn_Click(object sender, EventArgs e)
    {
        int Mid = DataConvert.CLng(Mid_Hid.Value);
        DBCenter.UpdateSQL("ZL_Logo_Design", "ZStatus=99", "ID=" + Mid);
        Response.Redirect(Request.RawUrl);
    }
}