using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.BLL.Helper;
using ZoomLa.BLL.Shop;
using ZoomLa.Model;
using ZoomLa.Sns;
using ZoomLa.SQLDAL;

public partial class Cart_Comp_Cart_Logo : System.Web.UI.Page
{
    public M_Cart_Addition addMod = null;
    public DataRowView item = null;
    //public M_Product proMod = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        item = (DataRowView)PageHelper.Aspx_GetModel(Request);
        //proMod = new B_Product().GetproductByid(DataConvert.CLng(item["ProID"]));
        addMod = JsonConvert.DeserializeObject<M_Cart_Addition>(item["Additional"].ToString());
    }
}