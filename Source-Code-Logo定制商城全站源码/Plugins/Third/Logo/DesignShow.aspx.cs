using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.BLL.AdSystem;
using ZoomLa.BLL.Helper;
using ZoomLa.Common;
using ZoomLa.Model;
using ZoomLa.Model.AdSystem;
using ZoomLa.Sns;
using ZoomLa.SQLDAL;

public partial class Plugins_Third_Logo_DesignShow : System.Web.UI.Page
{
    B_CartPro cpBll = new B_CartPro();
    B_Logo_Design desBll = new B_Logo_Design();
    B_Product proBll = new B_Product();
    public string imgUrl = "";
    //修改时传入,购物车ID
    public int CartID { get { return DataConvert.CLng(Request.QueryString["CartID"]); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            B_Admin.CheckIsLogged(Request.RawUrl);
            M_CartPro cpMod = cpBll.SelReturnModel(CartID);
            M_Product proMod = proBll.GetproductByid(cpMod.ProID);
            M_Logo_Design desMod = desBll.SelReturnModel(Convert.ToInt32(cpMod.Attribute));
            if (string.IsNullOrEmpty(desMod.LogoContent))
            {
                imgUrl = function.GetImgUrl(proMod.Thumbnails);
                img_panel.Visible = true;
            }
            else
            {
                Save_Hid.Value = StrHelper.DecompressString(desMod.LogoContent);
                design_panel.Visible = true;
            }
            //proMod = proBll.GetproductByid(cpMod.ProID);
          
            DataTable cpDT = SnsHelper.SelCart(cpMod.Orderlistid, cpMod.ID);
            Design_HTML.Text = PageHelper.Aspx_GetHtml("/cart/comp/cart_logo.aspx", cpDT.DefaultView[0]);
        }
    }
}