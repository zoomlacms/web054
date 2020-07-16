using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.Model;
using ZoomLa.SQLDAL;

public partial class Common_MailTlp_Pub : System.Web.UI.Page
{
    //用户提交请求后,生成邮件

    public int Mid { get { return DataConvert.CLng(Request["ID"]); } }
    public DataTable dt = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        dt = DBCenter.Sel("ZL_Pub_dzmb", "ID=" + Mid);
    }
    public string ShowVal(string name)
    {
        if (dt == null || dt.Rows.Count < 1) { return ""; }
        else { return DataConvert.CStr(dt.Rows[0][name]); }
    }
    //显示文件的名称(上传时进行了改名处理)
    //public string ShowFile()
    //{
    //    string value = ShowVal("pic");
    //    if (string.IsNullOrEmpty(value)) { return ""; }
    //    else { return System.IO.Path.GetFileName(value); }
    //}
}