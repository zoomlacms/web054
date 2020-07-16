using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL.Pay.ZLPayPal;
using PayPal.Api;
using ZoomLa.Model;
using ZoomLa.BLL;
using Newtonsoft.Json;
using ZoomLa.SQLDAL;
using ZoomLa.Common;
using ZoomLa.AppCode.Base;
using ZoomLa.Sns;
using System.Data;
using System.Data.SqlClient;
using ZoomLa.Components;

namespace ZoomLaCMS.PayOnline.PP
{
    public partial class Return : Base_PayPage
    {
        Payment executedPayment = null;
        B_OrderList orderBll = new B_OrderList();
        B_Payment payBll = new B_Payment();
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    // ?guid=33124&paymentId=PAY-3LF63527FM9288419LGADK5Q&token=EC-5RE63978FV6432229&PayerID=48RFZZ9N2KRBS
        //}
        public M_Payment payMod = null;
        public M_OrderList orderMod = null;
        public override string PayPlat
        {
            get
            {
                return "PayPal支付";
            }
        }
        public M_PayPlat platMod = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            platMod = new B_PayPlat().SelModelByClass(M_PayPlat.Plat.PayPal);
            try
            {
                if (CheckSign())
                {
                    string payno = executedPayment.transactions[0].invoice_number;
                    double amount = DataConvert.CDouble(executedPayment.transactions[0].related_resources[0].sale.amount.total);
                    PaySuccess(payno, amount);
                    //-----------------
                    payMod = payBll.SelModelByPayNo(payno);
                    orderMod = orderBll.SelModelByOrderNo(payMod.PaymentNum.Trim(','));
                    M_Order_Contact conMod = new B_Order_Contact().SelModelByOid(orderMod.id);
                    PayNo_L.Text = payMod.PayNo;
                    OrderNo_L.Text = orderMod.OrderNo;
                    Amount_L.Text = payMod.MoneyPay.ToString("F2");
                    Email_L.Text = conMod.Email;
                    DBCenter.DelByWhere("ZL_Cart", "CartId=@cartid", new List<SqlParameter>() {
                        new SqlParameter("cartid",B_Cart.GetCartID(Context))
                    });
                    EventDeal.SendOrderEmailByType(orderMod.id, "payed", SiteConfig.SiteInfo.WebmasterEmail);
                    EventDeal.SendOrderEmailByType(orderMod.id, "payed");
                }
                else
                {
                    ZLLog.L(ZLEnum.Log.pay, "[" + PayPlat + "]签名验证失败,返回:" + ServerReturn);
                    function.WriteErrMsg("Validation failed, information mismatch");
                }
            }
            catch (Exception ex)
            {
                Log("[" + PayPlat + "]出错,原因:" + ex.Message + ",返回:" + ServerReturn);
                success_div.Visible = false;
                fail_div.Visible = true;
                //------------------------------------
                payMod = payBll.SelModelByPayNo(Request["payno"]);
                if (payMod == null) { function.WriteErrMsg("order does not exist"); }
                if (payMod.Status != (int)M_Payment.PayStatus.NoPay) { function.WriteErrMsg("The order has been paid and cannot be repeated"); }
                orderMod = orderBll.SelModelByOrderNo(payMod.PaymentNum.Trim(','));
                Fail_PayNo_L.Text = payMod.PayNo;
                Fail_OrderNo_L.Text = orderMod.OrderNo;
                Fail_Amount_L.Text = payMod.MoneyPay.ToString("F2");
                //------------------------------------
                //if (string.IsNullOrEmpty(payMod.code))
                //{
                //    string cartId = B_Cart.GetCartID(Context);
                //    B_Cart cartBll = new B_Cart();
                //    B_CartPro cpBll = new B_CartPro();
                //    DataTable cartDT = cpBll.SelByOrderID(orderMod.id);
                //    foreach (DataRow cartDR in cartDT.Rows)
                //    {
                //        M_Cart cartMod = new M_Cart().GetModelFromReader(cartDR);
                //        cartMod.userid = 10000;
                //        cartMod.Cartid = cartId;
                //        cartMod.ProAttr = DataConvert.CStr(cartDR["Attribute"]);
                //        cartBll.insert(cartMod);
                //    }
                //    DBCenter.UpdateSQL(payMod.TbName,"Code='1'", "PaymentID=" + payMod.PaymentID);
                //}
                //function.WriteErrMsg("Error:Unfinished payment");
            }
        }
        public override bool CheckSign()
        {
            var apiContext = Configuration.GetAPIContext();
            string payerId = Request["PayerID"];//PayPal生成
            string paymentId = Request["paymentId"];
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            var payment = new Payment() { id = paymentId };
            //查询目标订单
            executedPayment = payment.Execute(apiContext, paymentExecution);
            //检测 invoice_number==Payno,state==completed
            //throw new Exception(JsonConvert.SerializeObject(executedPayment));
            //ZLLog.L(ZoomLa.Model.ZLEnum.Log.pay,JsonConvert.SerializeObject(executedPayment));
            string state = executedPayment.transactions[0].related_resources[0].sale.state;
            if (state != "completed") { return false; }
            else { return true; }
        }

    }
}