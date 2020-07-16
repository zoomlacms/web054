using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using ZoomLa.BLL;
using ZoomLa.BLL.Shop;
using ZoomLa.Components;
using ZoomLa.Model;
using ZoomLa.SQLDAL;

namespace ZoomLa.Sns
{
    public static class EventDeal
    {
        public static void SubscribeEvent()
        {
            OrderHelper.OrderFinish_DE func = OrderFinish;
            OrderHelper.OrderFinish_Event += func;
        }
        //仅负责将邮件再发送出去
        public static void OrderFinish(M_OrderList mod, M_Payment pinfo)
        {
            //DataTable orderDT = DBCenter.Sel(mod.TbName, "ID=" + mod.id);
            //DataTable contactDT = DBCenter.Sel("ZL_Order_Contact", "OrderID=" + mod.id);
            //DataTable cartDT = DBCenter.JoinQuery("A.*,B.PreviewImg", "ZL_CartPro",
            //    "ZL_Logo_Design", "A.Attribute=B.ID", "A.OrderListID="+mod.id+" AND (A.Attribute IS NOT NULL AND A.Attribute !='')");
            //string userHtml = Tlp_Read("订单付款后_用户");
            //userHtml = TlpDeal(userHtml, contactDT);
            //userHtml = TlpDeal(userHtml, orderDT);
            ////替换购物车列表
            //string cartHead = "<table><tr><td>Name</td><td>Image</td><td>Unit Price</td><td>Quantity</td><td>Total</td></tr>";
            //string cartHtml = "";
            //string cartEnd = "</table>";
            //foreach (DataRow dr in cartDT.Rows)
            //{
            //    var cartTlp = "<tr>"
            //            + "<td>"
            //            + "<div>{Proname}</div>"
            //            + "<div>{ProInfo}</div>"
            //            + "</td>"
            //            + "<td><img src=\"{PreviewImg}\" width=\"160\" height=\"100\"/></td>"
            //            + "<td>{Shijia}</td>"
            //            + "<td>{Pronum}</td>"
            //            + "<td>{AllMoney}</td>"
            //            + "</tr>";
            //    cartHtml += TlpDeal(cartTlp, cartDT, dr);
            //}
            //cartHtml = cartHead + cartHtml + cartEnd;
            //userHtml = userHtml.Replace("{CartList}", cartHtml);
            ////发送邮件给双方
            //SendToEmail(mod.Email, "Order Prompt", userHtml, mod.id+"");
            ////----------------------发送邮件通知管理员
            //string adminHtml = Tlp_Read("订单付款后_管理员");
            //adminHtml = TlpDeal(userHtml, contactDT);
            //adminHtml = TlpDeal(userHtml, orderDT);
            //adminHtml = adminHtml.Replace("{CartList}", cartHtml);


            StringWriter sw = new StringWriter();
            HttpContext.Current.Server.Execute("/Common/MailTlp/Order_Payed.aspx?ID=" + mod.id, sw);
            string title = "We received your order!";
            string html = sw.ToString();
            SendToEmail(mod.Email, title, html, mod.id.ToString());
            SendToEmail(SiteConfig.SiteInfo.WebmasterEmail, title, html,mod.id.ToString());
            SnsHelper.UpdateOrderField(mod.id, "EMail_Pay", "1");
        }
        /// <summary>
        /// 替换模板中的预留符号
        /// </summary>
        /// <param name="html">模板HTML</param>
        /// <param name="dt">dr所属数据表</param>
        /// <param name="dr">数据,如为空,则取dt中的第一行</param>
        /// <returns></returns>
        public static string TlpDeal(string html, DataTable dt,DataRow dr=null)
        {
            if (string.IsNullOrEmpty(html)||dt == null || dt.Rows.Count < 1) { return html; }
            //表格字段
            if (dr == null) { dr = dt.Rows[0]; }
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                string colname = dt.Columns[i].ColumnName;
                string value = dr[colname].ToString();
                if (dt.Columns[i].DataType.ToString().Equals("System.Decimal"))
                {
                    value = Convert.ToDouble(dr[colname]).ToString("f2");
                }
                html = html.Replace("{" + colname + "}", value);
            }
            return html;
        }
        public static string Tlp_Read(string name)
        {
            string html = SafeSC.ReadFileStr("/Common/MailTlp/" + name + ".html");
            return html;
        }
        private delegate void SendEmailHandler(string email, string title, string html, string errInfo);
        public static void SendEmailFunc(string email, string title, string html, string errInfo)
        {
            string errHead = "Email Send Error:[" + title + "][" + errInfo + "]";
            if (string.IsNullOrEmpty(email)) { ZLLog.L(errHead + "未指定需要发送的Email"); return; }
            try
            {
                MailInfo mailMod = new MailInfo();
                mailMod.FromName = SiteConfig.SiteInfo.WebmasterEmail;
                mailMod.Subject = title;
                mailMod.ToAddress = new System.Net.Mail.MailAddress(email);
                mailMod.MailBody = html;
                SendMail.Send(mailMod);
            }
            catch (Exception ex) { ZLLog.L(errHead + ex.Message); }
        }
        public static void SendToEmail(string email, string title, string html, string errInfo)
        {
            SendEmailHandler handler = SendEmailFunc;
            handler.BeginInvoke(email, title, html, errInfo, null, null);
        }
        /// <summary>
        /// 发送系统邮件
        /// </summary>
        public static void SendOrderEmailByType(int oid, string type, string email = "")
        {
            M_OrderList orderMod = new B_OrderList().SelReturnModel(oid);
            if (string.IsNullOrEmpty(email))
            {
                M_Order_Contact conMod = new B_Order_Contact().SelModelByOid(oid);
                email = conMod.Email;
            }
            switch (type)
            {
                case "order"://下单后(暂只用于测试)
                    {
                        StringWriter sw = new StringWriter();
                        HttpContext.Current.Server.Execute("/Common/MailTlp/Order_Payed.aspx?ID=" + oid, sw);
                        string title2 = "We received your order!";
                        string html2 = sw.ToString();
                        //throw new Exception(conMod.Email);
                        SendToEmail(email, title2, html2, "");
                    }
                    break;
                case "pay"://付款后
                    {
                        OrderFinish(orderMod, null);
                    }
                    break;
                case "exp":
                    {
                        StringWriter sw = new StringWriter();
                        HttpContext.Current.Server.Execute("/Common/MailTlp/Order_Send.aspx?ID=" + oid, sw);
                        string title = "Order has been shipped";
                        string html = sw.ToString();
                        SendToEmail(email, title, html, "");
                        SnsHelper.UpdateOrderField(oid, "EMail_Exp", "1");
                    }
                    break;
                case "after":
                    {
                        DataTable orderDT = DBCenter.Sel("ZL_OrderInfo", "ID=" + oid);
                        string title = "Order Notify";
                        string html = Tlp_Read("订单后期调查");
                        html = EventDeal.TlpDeal(html, orderDT);
                        SendToEmail(email, title, html, "");
                        SnsHelper.UpdateOrderField(orderMod.id, "EMail_After", "1");
                        SendToEmail(SiteConfig.SiteInfo.WebmasterEmail, title, html, "");
                    }
                    break;
            }

        }
    }
}
