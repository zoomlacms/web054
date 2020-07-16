namespace ZoomLaCMS.PayOnline
{
    using System;
    using ZoomLa.BLL;
    using ZoomLa.Common;
    using ZoomLa.Model;

    public partial class PayOnline : System.Web.UI.Page
    {

        private M_Payment pinfo = new M_Payment();
        private B_Payment paymentBll = new B_Payment();
        public string PayMethod { get { return Request.QueryString["method"]; } }
        public string PayNo { get { return Request.QueryString["PayNo"]; } }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(PayNo)) { function.WriteErrMsg("Unspecified order number"); }
            pinfo = paymentBll.SelModelByPayNo(PayNo);
            if (pinfo == null || pinfo.PaymentID < 1) { function.WriteErrMsg("Order does not exist"); }
            Response.Redirect("PP/Pay.aspx?Payno=" + pinfo.PayNo);
        }
    }
}