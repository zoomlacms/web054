using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.Common;
using ZoomLa.Sns;
using ZoomLa.SQLDAL;
using ZoomLa.SQLDAL.SQL;

public partial class Extend_Admin_CouponAdd : System.Web.UI.Page
{
    B_Ex_Coupon cupBll = new B_Ex_Coupon();
    public int Mid { get { return DataConvert.CLng(Request.QueryString["ID"]); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Mid > 0)
            {
                M_Ex_Coupon model = cupBll.SelReturnModel(Mid);
                Code_T.Text = model.Code;
                Amount_T.Text = model.AMount.ToString("F2");
                status_chk.Checked = model.ZStatus == 1;
                function.ScriptRad(this, "ztype", model.ZType.ToString());
            }
            else
            {
                Code_T.Text = "HS" + DateTime.Now.ToString("yyyyMMddHHmmss");
            }
        }
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        M_Ex_Coupon model = new M_Ex_Coupon();
        if (Mid > 0)
        {
           model = cupBll.SelReturnModel(Mid);
        }
        model.Code = Code_T.Text.Trim();
        model.AMount = DataConvert.CDouble(Amount_T.Text);
        model.ZStatus = status_chk.Checked ? 1 : 0;
        model.ZType = Request.Form["ztype"];
        if (string.IsNullOrEmpty(model.Code))
        { function.WriteErrMsg("用户券编号不能为空"); }
        if (model.AMount <= 0)
        { function.WriteErrMsg("优惠额度不能<=0"); }
        if (model.ZType == "比率" && model.AMount > 0.99)
        { function.WriteErrMsg("优惠额度错误,必须在0.01-0.99之间"); }
        if (model.ID > 0)
        {
            cupBll.UpdateByID(model);
        }
        else
        {
            cupBll.Insert(model);
        }
        function.Script(this,"alert('操作成功');parent.mybind();");
    }
}