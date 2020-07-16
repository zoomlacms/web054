using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZoomLaCMS.Prompt
{
    using System;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using ZoomLa.Common;
    using ZoomLa.Web;
    public partial class ShowError : CustomerPageAction
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string msg = HttpContext.Current.Items["Message"] as string;
                if (msg.Contains("您访问的商品信息不存在!")) { msg = "The commodity information you visited does not exist!"; }
                else if (msg.Contains("商品信息有误,NodeID或ModelID指定错误")) { msg = "Error in commodity information, NodeID or ModelID designation"; }
                else if (msg.Contains("对不起,商品已删除!")) { msg = "Sorry, the goods have been deleted!"; }
                else if (msg.Contains("对不起,商品正在审核中!")) { msg = "Sorry, the goods are under review!"; }
                else if (msg.Contains("对不起,商品已停止销售!")) { msg = "Sorry, the sale of the goods has stopped."; }
                else if (msg.Contains("商品价格有误,停止销售!")) { msg = "Commodity prices are wrong"; }
                //else if (msg.Equals("")) { msg = ""; }
                this.LtrSuccessMessage.Text = msg;
                string url = HttpContext.Current.Items["ReturnUrl"] as string;
                string title = HttpContext.Current.Items["MessageTitle"] as string;
                if (string.IsNullOrEmpty(url))
                {
                    if (base.Request.UrlReferrer == null)
                    {
                        this.LnkReturnUrl.Text = "<span class='fa fa-remove'></span>Close";
                        this.LnkReturnUrl.NavigateUrl = "#";
                        this.LnkReturnUrl.Attributes.Add("onclick", "window.close();");
                    }
                    else
                    {
                        this.LnkReturnUrl.NavigateUrl = "javascript:history.back();";// Request.UrlReferrer.ToString();
                    }
                }
                else if ((url.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase) || url.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase)) || url.StartsWith("javascript:", StringComparison.CurrentCultureIgnoreCase))
                {
                    this.LnkReturnUrl.NavigateUrl = url;
                }
                else
                {
                    function.Script(this, "SetUrl('" + url + "');");
                }
            }
            catch (Exception ex) { Response.Clear(); Response.Write(ex.Message); Response.End(); }
        }
    }
}