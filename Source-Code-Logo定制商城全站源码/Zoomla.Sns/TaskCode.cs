using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ZoomLa.BLL;
using ZoomLa.BLL.CreateJS;
using ZoomLa.Components;
using ZoomLa.Model;
using ZoomLa.SQLDAL;

namespace ZoomLa.Sns
{
    public class TaskCode
    {
        //微信推送消息
        public void WX_PushMsg()
        {
            WxAPI api = WxAPI.Code_Get();
            api.SendAllBySingle("这是消息");
        }
        //每半小时检测一次,如未发送过邮件且超过24小时,则发送
        public void SendEmailToUser()
        {
            string siteUrl = SiteConfig.SiteInfo.SiteUrl + "/BU/Comment.aspx?p=";
            string mailHtml = EventDeal.Tlp_Read("订单成交24小时后_用户");
            //已支付,已满24小时,未发送过邮件的订单
            string where = "ParentID=0 AND (Payment!='' AND Payment IS NOT NULL) ";
            where += " AND DATEDIFF(HOUR,AddTime,GETDATE())>=24";
            DataTable orderDT = DBCenter.Sel("ZL_OrderInfo", where);

            for (int i = 0; i < orderDT.Rows.Count; i++)
            {
                try
                {
                    DataRow dr = orderDT.Rows[i];
                    M_OrderList orderMod = new M_OrderList().GetModelFromReader(orderDT.Rows[i]);
                    string url = siteUrl + EncryptHelper.AESEncrypt(orderMod.id.ToString());
                    string html = mailHtml.Replace("{LinkUrl}", "<a href='" + siteUrl + "' target='_blank'>" + siteUrl + "</a>");
                    EventDeal.SendToEmail(orderMod.Email, "Order Comment", mailHtml, orderMod.id.ToString());
                    DBCenter.UpdateSQL(orderMod.TbName, "ParentID=1", "ID=" + orderMod.id);
                }
                catch (Exception ex) { ZLLog.L("邮件24小时通知 Error:[" + orderDT.Rows[i]["ID"] + "]" + ex.Message); }
            }

        }
    }
}
