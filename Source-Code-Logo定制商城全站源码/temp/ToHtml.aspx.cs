using CDO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.Components;
using ZoomLa.Model;
using ZoomLa.Sns;
using ZoomLa.SQLDAL;


public partial class temp_ToHtml : System.Web.UI.Page
{
    public int OrderID { get { return DataConvert.CLng(Request.QueryString["ID"]); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        //StringWriter sw = new StringWriter();
        //Server.Execute("/Common/MailTlp/Order_Payed.aspx?ID=79", sw);
        //Response.Write(sw.ToString());


        //EventDeal.SendOrderEmailByType(1144, "order");
        //throw new Exception("Email has send");
        B_Admin.CheckIsLogged(Request.RawUrl);
        int oid = 1144;
        M_Order_Contact conMod = new B_Order_Contact().SelModelByOid(oid);
        M_OrderList orderMod = new B_OrderList().SelReturnModel(oid);
        StringWriter sw = new StringWriter();
        HttpContext.Current.Server.Execute("/Common/MailTlp/Order_Payed.aspx?ID=" + oid, sw);
        string title2 = "We received your order!";
        string html2 = sw.ToString();
        //throw new Exception(conMod.Email);
        SendMail.MailState state = SendMail.MailState.None;
        string exmsg = "";
        try
        {
            state = SendEmailFunc(conMod.Email, title2, html2, "");
        }
        catch (Exception ex) { exmsg = ex.Message; }
        //conMod.Email = "whatclass110@gmail.com";
        //conMod.Email = "469582963@qq.com";
        //conMod.Email = "whatclass@hotmail.com";
        var cfg = SiteConfig.MailConfig;
        throw new Exception(conMod.Email + exmsg);
    }
    public SendMail.MailState SendEmailFunc(string email, string title, string html, string errInfo)
    {
        string errHead = "Email Send Error:[" + title + "][" + errInfo + "]";
        if (string.IsNullOrEmpty(email)) { throw new Exception(errHead + "未指定需要发送的Email");  }
        MailInfo mailMod = new MailInfo();
        mailMod.FromName = SiteConfig.SiteInfo.WebmasterEmail;
        mailMod.Subject = title;
        mailMod.ToAddress = new System.Net.Mail.MailAddress(email);
        mailMod.IsBodyHtml = true;
        mailMod.MailBody = html;
        var cfg = SiteConfig.MailConfig;
        //return SendMail.SendSSLEmail(cfg.MailServerUserName, cfg.MailServerPassWord, cfg.MailServer, mailMod, "".Split(','));
        return SendMail.SendSSLEmail("whatclass110@gmail.com", "123123aa_", cfg.MailServer, mailMod, "".Split(','));
        //return SendMail.Send(mailMod);
    }
}

public sealed class SendMail
{
    public enum AuthenticationType
    {
        None,
        Basic,
        Ntlm
    }
    public enum MailState
    {
        AttachmentSizeLimit = 10,
        ConfigFileIsWriteOnly = 6,
        FileNotFind = 3,
        MailConfigIsNullOrEmpty = 4,
        MustIssueStartTlsFirst = 11,
        NoMailToAddress = 1,
        None = 0,
        NonsupportSsl = 12,
        NoSubject = 2,
        Ok = 0x63,
        PortError = 13,
        SaveFailure = 7,
        SendFailure = 5,
        SmtpServerNotFind = 8,
        UserNameOrPasswordError = 9
    }
    public static string GetMailStateInfo(MailState mailcode)
    {
        switch (mailcode)
        {
            case MailState.NoMailToAddress:
                return "缺少收件人地址";

            case MailState.NoSubject:
                return "缺少邮件标题";

            case MailState.FileNotFind:
                return "附件文件没找到";

            case MailState.MailConfigIsNullOrEmpty:
                return "邮件参数配置不全";

            case MailState.SendFailure:
                return "邮件发送失败";

            case MailState.ConfigFileIsWriteOnly:
                return "无法读取邮件参数配置文件";

            case MailState.SaveFailure:
                return "文件保存失败";

            case MailState.SmtpServerNotFind:
                return "邮件服务器不存在";

            case MailState.UserNameOrPasswordError:
                return "邮件服务器用户名和密码验证没通过";

            case MailState.AttachmentSizeLimit:
                return "附件大小超出限制";

            case MailState.MustIssueStartTlsFirst:
                return "邮件服务器仅接受TLS连接，可以设置支持SSL加密解决";

            case MailState.NonsupportSsl:
                return "邮件服务器不支持SSL加密";

            case MailState.PortError:
                return "邮件服务器端口错误";

            case MailState.Ok:
                return "邮件发送成功";
        }
        return "没有返回信息";
    }
   
    public static MailState SendSSLEmail(string account, string passwd, string server, MailInfo mailInfo, string[] attach)
    {
        Message mailMessage = new Message();
        //CDO.IConfiguration iConfg; 
        //iConfg = oMsg.Configuration;
        //ADODB.Fields oFields;
        //oFields = iConfg.Fields;
        Configuration conf = new ConfigurationClass();
        conf.Fields[CdoConfiguration.cdoSendUsingMethod].Value = CdoSendUsing.cdoSendUsingPort;
        conf.Fields[CdoConfiguration.cdoSMTPAuthenticate].Value = CdoProtocolsAuthentication.cdoBasic;
        conf.Fields[CdoConfiguration.cdoSMTPUseSSL].Value = true;
        conf.Fields[CdoConfiguration.cdoSMTPServer].Value = server;//必填，而且要真实可用   
        conf.Fields[CdoConfiguration.cdoSMTPServerPort].Value = 465;
        conf.Fields[CdoConfiguration.cdoSendEmailAddress].Value = account;
        conf.Fields[CdoConfiguration.cdoSendUserName].Value = account;//真实的邮件地址   
        conf.Fields[CdoConfiguration.cdoSendPassword].Value = passwd;   //为邮箱密码，必须真实   


        conf.Fields.Update();
        mailMessage.Subject = mailInfo.Subject;
        mailMessage.Configuration = conf;
        //oMsg.TextBody = "Hello, how are you doing?";
        mailMessage.HTMLBody = mailInfo.MailBody;
        //TODO: Replace with your preferred Web page
        //oMsg.CreateMHTMLBody("http://www.microsoft.com",CDO.CdoMHTMLFlags.cdoSuppressNone, "", "");                    
        mailMessage.From = account;
        mailMessage.To = mailInfo.ToAddress.Address;
        //oMsg.AddAttachment("C:\Hello.txt", "", "");
        foreach (string file in attach)//物理路径
        {
            if (string.IsNullOrEmpty(file)) continue;
            mailMessage.AddAttachment(file, "", "");
        }
        mailMessage.Send();
        return MailState.Ok;
    }
    private static MailMessage GetMailMessage(MailInfo mailInfo, string fromEmail)
    {
        MailMessage message = new MailMessage();
        message.To.Add(mailInfo.ToAddress);
        if (mailInfo.ReplyTo != null)
        {
            message.ReplyTo = mailInfo.ReplyTo;
        }
        if (!string.IsNullOrEmpty(mailInfo.FromName))
        {
            message.From = new MailAddress(fromEmail, mailInfo.FromName);
        }
        else
        {
            message.From = new MailAddress(fromEmail);
        }
        message.Subject = mailInfo.Subject;
        message.SubjectEncoding = Encoding.UTF8;
        message.Body = mailInfo.MailBody;
        message.BodyEncoding = Encoding.UTF8;
        message.Priority = mailInfo.Priority;
        message.IsBodyHtml = mailInfo.IsBodyHtml;
        return message;
    }

    //------------------Tools
    public static MailInfo GetMailInfo(string email, string from, string subject, string content)
    {
        MailAddress address = new MailAddress(email);
        MailInfo mailInfo = new MailInfo();
        mailInfo.Subject = subject;
        mailInfo.IsBodyHtml = true;
        mailInfo.FromName = from;
        mailInfo.ToAddress = address;
        mailInfo.MailBody = content;
        return mailInfo;
    }
}