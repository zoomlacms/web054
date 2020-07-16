using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.BLL.Helper;
using ZoomLa.BLL.Shop;
using ZoomLa.Common;
using ZoomLa.Components;
using ZoomLa.Model;
using ZoomLa.Model.Shop;
using ZoomLa.Sns;

namespace ZoomLaCMS.Manage.Shop
{
    public partial class Orderlistinfo : CustomerPageAction
    {
        B_CartPro cartProBll = new B_CartPro();
        B_Product proBll = new B_Product();
        B_OrderList oll = new B_OrderList();
        B_PayPlat platBll = new B_PayPlat();
        B_Stock Sll = new B_Stock();
        B_Order_Exp expBll = new B_Order_Exp();
        B_Order_Back backBll = new B_Order_Back();
        B_Order_PayLog paylogBll = new B_Order_PayLog();
        B_User buser = new B_User();
        B_Admin badmin = new B_Admin();
        B_Payment payBll = new B_Payment();
        M_Order_PayLog paylogMod = new M_Order_PayLog();
        B_Order_OPLog logBll = new B_Order_OPLog();
        B_Order_Contact conBll = new B_Order_Contact();
        OrderCommon orderCom = new OrderCommon();
        public M_OrderList orderinfo = null;
        public DataRow EmailDR = null;
        public M_Order_Contact conMod = null;
        //OrderID
        public int Mid { get { return DataConverter.CLng(Request.QueryString["ID"]); } }
        public string OrderNO { get { return ViewState["orderno"].ToString(); } set { ViewState["orderno"] = value; } }
        private M_Payment GetPayment(M_OrderList orderInfo)
        {
            return payBll.SelModelByOrder(orderInfo);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            B_ARoleAuth.AuthCheckEx(ZLEnum.Auth.shop, "order");
            if (Mid < 1 && string.IsNullOrEmpty(Request["OrderNo"])) { function.WriteErrMsg("未指定订单"); }
            if (Mid > 0) { orderinfo = oll.GetOrderListByid(Mid); }
            else if (!string.IsNullOrEmpty(Request["OrderNo"])) { orderinfo = oll.GetByOrder(Request["OrderNo"], "0"); }
            if (orderinfo == null || orderinfo.id < 1) { function.WriteErrMsg("订单不存在"); }
            EmailDR = SnsHelper.SelEmailInfo(orderinfo.id);
            conMod = conBll.SelModelByOid(orderinfo.id);
            if (!IsPostBack)
            {
                OrderNO = orderinfo.OrderNo;
                //----------------------------------------------------------
                M_UserInfo mu = buser.SelReturnModel(orderinfo.Userid);
                M_Payment payMod = GetPayment(orderinfo);
                HeadTitle_L.Text = "订 单 信 息（订单编号：" + orderinfo.OrderNo + "）";
                string giveurl = customPath2 + "User/Userexp.aspx?UserID=" + orderinfo.Userid
                                  + "&orderid=" + orderinfo.id;
                give_score_a.HRef = giveurl + "&type=" + (int)M_UserExpHis.SType.Point;
                give_purse_a.HRef = giveurl + "&type=" + (int)M_UserExpHis.SType.Purse;

                //Reuser.Text = StringHelper.SubStr(orderinfo.Receiver, 12);
                //UName_L.Text = "<a href='javascript:;' onclick='showuinfo(" + mu.UserID + ");' title='查看用户'>" + mu.UserName + "</a>";
                if (orderinfo.StateLogistics != 0) { Exp_Send_Btn.Enabled = false; }
                if (orderinfo.StateLogistics == 1) { Exp_ClientSign_Btn.Enabled = true; }
                //if (orderinfo.StateLogistics == 0) { function.Script(this, "hiddLogistics();"); }
                if (orderinfo.Paymentstatus >= (int)M_OrderList.PayEnum.HasPayed)
                {
                    Paymentstatus.Text = "<span  style='color:green';>已经汇款</span>";
                    Pay_Has_Btn.Disabled = true;
                }
                else
                {
                    Paymentstatus.Text = "<span style='color:red;'>等待汇款</span>";
                    Pay_Has_Btn.Disabled = false;
                    Exp_Send_Btn.Enabled = false;
                }
                switch ((M_OrderList.StatusEnum)orderinfo.OrderStatus)
                {
                    case M_OrderList.StatusEnum.Normal:
                        break;
                    case M_OrderList.StatusEnum.DrawBack:
                        OS_NoSure_Btn.Enabled = false;
                        Exp_Send_Btn.Enabled = false;
                        CompleteOrder_Btn.Enabled = false;
                        Back_Btn.Visible = true;
                        break;
                    case M_OrderList.StatusEnum.UnDrawBack:
                    case M_OrderList.StatusEnum.CheckDrawBack:
                        OS_Invoice_Btn.Enabled = false;
                        OS_NoSure_Btn.Enabled = false;
                        Exp_Send_Btn.Enabled = false;
                        CompleteOrder_Btn.Enabled = false;
                        break;
                    case M_OrderList.StatusEnum.OrderFinish:
                    //case M_OrderList.StatusEnum.UnitFinish:
                        CompleteOrder_Btn.Enabled = false;
                        break;
                    default:
                        break;
                }
                OS_Sure_Btn.Enabled = orderinfo.IsSure == 1 ? false : true;
                #region 物流信息
                ExpStatus_L.Text = OrderHelper.GetExpStatus(orderinfo.StateLogistics);
                M_Order_Exp expMod = expBll.SelReturnModel(DataConverter.CLng(orderinfo.ExpressNum));
                if (expMod != null)
                {
                    ExpName_L.Text = expMod.ExpComp;
                    ExpCode_L.Text = expMod.ExpNo;
                    ExpStatus_L.Text += "(公司：" + expMod.ExpComp + "/单号：" + expMod.ExpNo + ")";
                    SearchExp_A.Visible = true;
                    SearchExp_A.HRef = "http://www.17track.net/zh-cn/track?nums=" + expMod.ExpNo;
                }
                switch ((M_OrderList.ExpEnum)orderinfo.StateLogistics)
                {
                    case M_OrderList.ExpEnum.NoSend:
                        Exp_Cancel_Btn.Enabled = false;
                        ExpPrint_B.Disabled= true;
                        break;
                    case M_OrderList.ExpEnum.HasSend:
                        Exp_ClientSign_Btn.Enabled = true;
                        break;
                    case M_OrderList.ExpEnum.HasReceived:
                        Exp_Send_Btn.Enabled = false;
                        break;
                }
                if (orderinfo.BackID > 0)
                {
                    M_Order_Back backMod = backBll.SelReturnModel(orderinfo.BackID);
                    Back_UserRemind_L.Text = backMod.UserRemind;
                    Back_AdminRemind_L.Text = backMod.AdminRemind;
                    Back_ZStatus_L.Text = backBll.GetZStatus(backMod.ZStatus);
                }
                //Phone.Text = orderinfo.Phone.ToString();//联系电话
                //ZipCode.Text = orderinfo.ZipCode.ToString();//邮政编码
                //Mobile.Text = orderinfo.MobileNum;//手机
                //Reusers.Text = orderinfo.Receiver.ToString();//订货人
                //Jiedao.Text = orderinfo.Shengfen + " " + orderinfo.Jiedao;//地址
                #endregion
                ExpTime_L.Text = orderinfo.ExpTime;
                OrderStatus.Text = OrderHelper.GetOrderStatus(orderinfo.OrderStatus);
                OrderType_L.Text = orderinfo.AddTime.ToString();

                Invoiceneeds.Text = orderinfo.Invoiceneeds == 1 ? ComRE.Icon_OK : ComRE.Icon_Error;
                Developedvotes.Text = orderinfo.Developedvotes == 1 ? ComRE.Icon_OK : ComRE.Icon_Error;
                IsSure_L.Text = orderinfo.IsSure == 1 ? ComRE.Icon_OK : ComRE.Icon_Error;
                OS_Freeze_Btn.Enabled = orderinfo.Suspended == 1;
                OS_Pause_Btn.Enabled = orderinfo.Suspended == 1 ? false : true;//暂停
                if (orderinfo.Aside == 1)//已作废
                {
                    Pay_Has_Btn.Disabled = true;
                    //ShowSend_Btn.Enabled = false;
                    OS_Sure_Btn.Enabled = false;
                    OS_NoSure_Btn.Enabled = false;
                    CompleteOrder_Btn.Enabled = false;
                    Exp_ClientSign_Btn.Enabled = false;
                    OS_Freeze_Btn.Enabled = false;
                    OS_Invoice_Btn.Enabled = false;
                    OS_Pause_Btn.Enabled = false;
                    OS_Aside_Btn.Enabled = false;
                    Pay_Cancel_Btn.Enabled = false;
                    Refund_B.Attributes["disabled"] = "disabled";
                }
                else
                {
                    OS_Aside_Btn.Enabled = true;
                }
                if (orderinfo.Settle == 1)
                {
                    //ShowSend_Btn.Enabled = false;
                    OS_Sure_Btn.Enabled = false;
                    OS_NoSure_Btn.Enabled = false;
                    OS_Aside_Btn.Enabled = false;
                    Exp_ClientSign_Btn.Enabled = false;
                    OS_Invoice_Btn.Enabled = false;
                    OS_Pause_Btn.Enabled = false;
                }
                if (orderinfo.Settle == 1 || string.IsNullOrEmpty(orderinfo.PaymentNo)) { Pay_Settle_Btn.Disabled = true; }
                //显示支付平台信息,订单完成支付后才有值
                if (!string.IsNullOrEmpty(orderinfo.PaymentNo) && payMod != null)//支付后才有值
                {
                   // Payment.Text = platBll.GetPayPlatName(orderinfo.PaymentNo);
                    if (payMod.Status != (int)M_Payment.PayStatus.NoPay)
                    {
                        ChangeMoney_Btn.Attributes["disabled"] = "disabled";
                        ChangeMoney_Btn.Attributes["title"] = "已支付订单不可修改金额";
                    }
                }
                //Email.Text = orderinfo.Email.ToString();//电子信
                //Invoice.Text = orderinfo.Invoice.ToString();//发票信息
                AddUser.Text = orderinfo.AddUser.ToString();//负责跟单人员
                Internalrecords_T.Text = orderinfo.Internalrecords.ToString();//内部记录
                Ordermessage_T.Text = orderinfo.Ordermessage;//订货留言
                //-------购物车
                //DataTable cplist = cartProBll.GetCartProOrderID(Mid);
                DataTable cplist = SnsHelper.SelCart(Mid);
                Procart_RPT.DataSource = cplist;
                Procart_RPT.DataBind();
                //-------
                #region 金额总计和修改
                double orderMoney = orderinfo.Ordersamount - orderinfo.Freight;
                PI_OrdersMoney_T.Text = orderMoney.ToString("F2");
                PI_OrderExpMoney_T.Text = orderinfo.Freight.ToString("F2");
                PI_OrderTotal_T.Text = orderinfo.Ordersamount.ToString("F2");
                if (payMod != null)
                {
                    PI_NeedPay_L.Text = payMod.MoneyReal.ToString("F2");
                    PI_Arrive_L.Text = payMod.ArriveMoney.ToString("F2");
                    PI_Point_L.Text = payMod.UsePoint.ToString("F0") + "个(" + payMod.UsePointArrive.ToString("F2") + "元)";
                    PI_Order_L.Text = orderMoney.ToString("F2");
                    PI_Freight_L.Text = orderinfo.Freight.ToString("F2");
                    //PI_Total_L.Text = orderinfo.Ordersamount.ToString("F2");
                }
                else
                {
                    PI_NeedPay_L.Text=orderinfo.Ordersamount.ToString("F2");
                }
                PI_ReceMoney_L.Text = orderinfo.Receivablesamount.ToString("f2") + "元";
                #endregion
                #region 判断订单所处状态
                {
                    if (orderinfo.OrderStatus < (int)M_OrderList.StatusEnum.Normal || orderinfo.Aside == 1)
                    {
                        prog_order_div.InnerHtml = OrderHelper.GetOrderStatus(orderinfo.OrderStatus, orderinfo.Aside, orderinfo.StateLogistics);
                    }
                    else
                    {
                        int current = 2;
                        if (orderinfo.OrderStatus >= (int)M_OrderList.StatusEnum.OrderFinish) { current = 5; }
                        else if (orderinfo.Paymentstatus >= (int)M_OrderList.PayEnum.HasPayed)
                        {
                            current++;
                            switch (orderinfo.StateLogistics)
                            {
                                case (int)M_OrderList.ExpEnum.HasSend:
                                    current++;
                                    break;
                                case (int)M_OrderList.ExpEnum.HasReceived:
                                    current += 2;
                                    break;
                            }
                        }
                        function.Script(this, "$('#prog_order_div').ZLSteps('订单生成,等待用户支付,等待商户发货,等待用户签收,订单完结'," + current + ")");
                    }
                }
                #endregion
              
            }
        }
        //修改合计金额(单店铺)
        protected void ChangeMoney_Btn_Click(object sender, EventArgs e)
        {
            orderinfo = oll.GetOrderListByid(Mid);
            orderinfo.Freight = DataConverter.CDouble(PI_OrderExpMoney_T.Text);
            orderinfo.Ordersamount = DataConverter.CDouble(PI_OrdersMoney_T.Text) + orderinfo.Freight;
            M_Payment payMod = GetPayment(orderinfo);
            if (payMod!=null)
            {
                if (payMod.Status != (int)M_Payment.PayStatus.NoPay) { function.WriteErrMsg("该订单已支付过,不可修改金额"); }
                payMod.MoneyReal = orderinfo.Ordersamount;
                payMod.MoneyPay = orderinfo.Ordersamount;
                payBll.Update(payMod);
            }
            if (orderinfo.Freight < 0 || orderinfo.Ordersamount < 0) { function.WriteErrMsg("指定的金额不正确"); }
            M_Order_OPLog logMod = logBll.NewLog(Mid, "修改金额", orderinfo.Ordersamount.ToString());
            oll.Update(orderinfo); logBll.Insert(logMod);
            Response.Redirect(Request.RawUrl);
        }
        #region 前端View操作
        //获取价格
        public string GetProPrice(string proclass, string type, string proid)
        {
            int pid = DataConverter.CLng(proid);
            M_Product proMod = proBll.GetproductByid(pid);
            switch (type)
            {
                case "1":
                    return proMod.ShiPrice.ToString("f2");
                case "2":
                default:
                    return proMod.LinPrice.ToString("f2");
            }
        }
        //获取价格
        public string GetPrice()
        {
            return Convert.ToDouble(Eval("AllMoney")).ToString("F2");
        }
        //获取期限
        public string GetServerTime(string cid)
        {
            //return proBll.GetServerPeriod(DataConverter.CLng(Eval("ServerPeriod")), DataConverter.CLng(Eval("ServerType")));
            return "";
        }
        public string GetShopUrl()
        {
            return OrderHelper.GetShopUrl(0, Convert.ToInt32(Eval("ProID")));
        }
        #endregion
        //------------前台禁止
        //完结订单
        protected void btnPre_Click(object sender, EventArgs e)
        {
            showData("pre");
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            showData("next");
        }
        private void showData(string str)
        {
            M_OrderList orderMod = oll.SelNext(Mid, str);
            if (str == "next")
            {
                if (orderMod == null)
                {
                    btnNext.Text = "已是最后一个订单";
                    btnNext.Enabled = false;
                    btnNext2.Enabled = false;
                }
                else { Response.Redirect("Orderlistinfo.aspx?id=" + orderMod.id); }
            }
            else if (str == "pre")
            {
                if (orderMod == null)
                {
                    btnPre.Text = "已是第一个订单";
                    btnPre.Enabled = false;
                    btnPre2.Enabled = false;
                }
                else { Response.Redirect("Orderlistinfo.aspx?id=" + orderMod.id); }
            }
        }
        protected void SaveRemind_Btn_Click(object sender, EventArgs e)
        {
            orderinfo = oll.SelReturnModel(Mid);
            orderinfo.Internalrecords = Internalrecords_T.Text;
            orderinfo.Ordermessage = Ordermessage_T.Text;
            oll.UpdateByID(orderinfo);
            function.WriteSuccessMsg("修改成功");
        }
        #region 支付状态
        protected void Pay_Cancel_Btn_Click(object sender, EventArgs e)
        {
            M_Order_OPLog logMod = logBll.NewLog(Mid, "取消支付");
            oll.UpOrderinfo("Paymentstatus=" + (int)M_OrderList.PayEnum.NoPay + ",PaymentNo=''", Mid);
            logBll.Insert(logMod);
            Response.Redirect(Request.RawUrl);
        }
        //已经支付
        protected void Pay_Has_Btn_Click(object sender, EventArgs e)
        {
            M_Order_OPLog logMod = logBll.NewLog(Mid, "已经支付");
            oll.UpOrderinfo("Paymentstatus=" + (int)M_OrderList.PayEnum.HasPayed, Mid);logBll.Insert(logMod);
            Response.Redirect(Request.RawUrl);
        }
        //退单还款
        protected void Refund_Btn_Click(object sender, EventArgs e)
        {
            M_Order_OPLog logMod = logBll.NewLog(Mid, "退单还款");
            orderinfo = oll.GetOrderListByid(Mid);
            if (orderinfo.Paymentstatus == (int)M_OrderList.PayEnum.NoPay) { function.WriteErrMsg("操作失败，订单还未支付"); }
            if (orderinfo.Paymentstatus == (int)M_OrderList.PayEnum.Refunded) { function.WriteErrMsg("操作失败，该订单已退款"); }
            buser.ChangeVirtualMoney(orderinfo.Userid, new M_UserExpHis()
            {
                score = orderinfo.Receivablesamount,
                ScoreType = 1,
                detail = "订单[" + orderinfo.id + "]退单返款,返款金额：" + orderinfo.Receivablesamount
            });
            oll.UpOrderinfo("Paymentstatus=" + (int)M_OrderList.PayEnum.Refunded, Mid);logBll.Insert(logMod);
            Response.Redirect(Request.RawUrl);
        }
        #endregion
        #region 订单状态 OS_
        //确认订单
        protected void OS_Sure_Btn_Click(object sender, EventArgs e)
        {
            M_Order_OPLog logMod = logBll.NewLog(Mid, "确认订单");
            oll.UpOrderinfo("IsSure=1", Mid); logBll.Insert(logMod);
            Response.Redirect(Request.RawUrl);
        }
        //取消确认
        protected void OS_NoSure_Btn_Click(object sender, EventArgs e)
        {
            M_Order_OPLog logMod = logBll.NewLog(Mid, "取消确认");
            oll.UpOrderinfo("IsSure=0", Mid); logBll.Insert(logMod);
            function.WriteSuccessMsg("取消确认成功", "Orderlistinfo.aspx?id=" + Mid);
        }
        //暂停处理
        protected void OS_Pause_Btn_Click(object sender, EventArgs e)
        {
            M_Order_OPLog logMod = logBll.NewLog(Mid, "暂停处理");
            oll.UpOrderinfo("Suspended=1", Mid);logBll.Insert(logMod);
            Response.Redirect(Request.RawUrl);
        }
        //订单作废
        protected void OS_Aside_Btn_Click(object sender, EventArgs e)
        {
            M_Order_OPLog logMod = logBll.NewLog(Mid, "订单作废");
            oll.UpOrderinfo("Aside=1", Mid);logBll.Insert(logMod);
            Response.Redirect(Request.RawUrl);
        }
        //冻结订单
        protected void OS_Freeze_Btn_Click(object sender, EventArgs e)
        {
            M_Order_OPLog logMod = logBll.NewLog(Mid, "暂停处理");
            oll.UpOrderinfo("Suspended=1", Mid);logBll.Insert(logMod);
            Response.Redirect(Request.RawUrl);
        }
        //已开发票
        protected void OS_Invoice_Btn_Click(object sender, EventArgs e)
        {
            M_Order_OPLog logMod = logBll.NewLog(Mid, "已开发票");
            oll.UpOrderinfo("Developedvotes=1", Mid);logBll.Insert(logMod);
            Response.Redirect(Request.RawUrl);
        }
        //重启订单
        protected void OS_Normal_Btn_Click(object sender, EventArgs e)
        {
            M_Order_OPLog logMod = logBll.NewLog(Mid, "恢复正常");
            string str = "Aside=0,Suspended=0,Settle=0,BackID=0,OrderStatus=" + (int)M_OrderList.StatusEnum.Normal;
            oll.UpOrderinfo(str, Mid); logBll.Insert(logMod);
            Response.Redirect(Request.RawUrl);
        }
        //完结订单
        protected void CompleteOrder_Btn_Click(object sender, EventArgs e)
        {
            M_Order_OPLog logMod = logBll.NewLog(Mid, "完结订单");
            //前使用必须修改,只更改状态,不执行FinalStep
            M_OrderList orderMod = oll.SelReturnModel(Mid);
            if (string.IsNullOrEmpty(orderMod.PaymentNo))//未支付则生成支付单
            {
                OrderHelper.FinalStep(orderMod);
            }
            else
            {
                M_Payment payMod = GetPayment(orderinfo);
                OrderHelper.FinalStep(payMod, orderMod, new M_Order_PayLog());
            }
            logBll.Insert(logMod);
            Response.Redirect(Request.RawUrl);
        }
        #endregion
        #region 物流管理 Exp_
        //取消发送
        protected void EXP_Cancel_Btn_Click(object sender, EventArgs e)
        {
            M_Order_OPLog logMod = logBll.NewLog(Mid, "取消发送");
            oll.UpOrderinfo("StateLogistics=" + (int)M_OrderList.ExpEnum.NoSend + ",ExpressNum=''", Mid); logBll.Insert(logMod);
            Response.Redirect(Request.RawUrl);
        }
        //客户已签收
        protected void Exp_ClientSign_Btn_Click(object sender, EventArgs e)
        {
            M_Order_OPLog logMod = logBll.NewLog(Mid, "客户已签收");
            oll.UpOrderinfo("Signed=1,StateLogistics=" + (int)M_OrderList.ExpEnum.HasReceived, Mid); logBll.Insert(logMod);
            Response.Redirect(Request.RawUrl);
        }
        #endregion

        protected void Procart_RPT_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView dr = e.Item.DataItem as DataRowView;

                Label ptLabel = e.Item.FindControl("Design_HTML") as Label;
                ptLabel.Text = PageHelper.Aspx_GetHtml("/cart/comp/cart_logo.aspx", dr);
            }
        }

        protected void SendMail_Click(object sender, EventArgs e)
        {
            EventDeal.SendOrderEmailByType(Mid,((Button)sender).CommandName);
            Response.Redirect(Request.RawUrl);
        }
        public string GetImage()
        {
            if (!string.IsNullOrEmpty(Eval("PreViewImg", "")))
            {
                return Eval("PreViewImg", "");
            }
            else
            {
                string url = function.GetImgUrl(Eval("Thumbnails", ""));
                return url;
            }
        }
    }
}