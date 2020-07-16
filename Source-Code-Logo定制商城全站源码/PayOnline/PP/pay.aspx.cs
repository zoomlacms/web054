using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.Common;
using ZoomLa.Model;
using ZoomLa.SQLDAL;
namespace ZoomLaCMS.PayOnline.PP
{
    public partial class pay : System.Web.UI.Page
    {
        B_Payment payBll = new B_Payment();
        M_Payment payMod = null;
        public string PayNo { get { return DataConvert.CStr(Request["PayNo"]); } }
        protected void Page_Load(object sender, EventArgs e)
        {
            payMod = payBll.SelModelByPayNo(PayNo);
            CommonReturn retMod = payBll.IsCanPay(payMod);
            if (!retMod.isok) { function.WriteErrMsg("Orders are not allowed to be paid");ZLLog.L(retMod.err); }
            //-----------------------
            ZoomLa.BLL.Pay.ZLPayPal.PayPalHelper.RedirectToPay(payMod);

        }
    }
}