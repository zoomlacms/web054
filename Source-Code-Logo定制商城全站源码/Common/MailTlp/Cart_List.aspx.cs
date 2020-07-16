using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.Common;
using ZoomLa.Components;
using ZoomLa.Sns;
using ZoomLa.SQLDAL;

public partial class Common_MailTlp_Cart_List : System.Web.UI.Page
{
    public int OrderID { get { return DataConvert.CLng(Request["OrderID"]); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable cartDT = SnsHelper.SelCart(OrderID);
        RPT.DataSource = cartDT;
        RPT.DataBind();
    }
    protected void RPT_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView dr = e.Item.DataItem as DataRowView;
            Label ptLabel = e.Item.FindControl("Present_HTML") as Label;
            try
            {
                string addition = DataConvert.CStr(dr["Additional"]);
                //M_Cart_Addition addMod = JsonConvert.DeserializeObject<M_Cart_Addition>(addition);
                JObject jobj = JsonConvert.DeserializeObject<JObject>(addition);
                string itemTlp = "<div style='color:#999;'>{0}:{1}</div>";
                foreach (var item in jobj)
                {
                    if (string.IsNullOrEmpty(item.Value.ToString())) { continue; }
                    if (DataConvert.CLng(item.Value) < 1) { continue; }
                    if (item.Key == "flash" || item.Key == "outdoor" || item.Key == "backing")
                    {
                        string value = DataConvert.CStr(DBCenter.ExecuteScala("ZL_Commodities", "Proname", "ID=" + DataConvert.CLng(item.Value)));
                        ptLabel.Text += string.Format(itemTlp, item.Key, item.Value);
                    }
                    else
                    {
                        ptLabel.Text += string.Format(itemTlp, item.Key, item.Value);
                    }
                }
            }
            catch (Exception) { }
        }
    }
    public string GetImage()
    {
        if (!string.IsNullOrEmpty(Eval("PreViewImg", "")))
        {
            string preView = Eval("PreViewImg", "");
            if (preView.Length < 200)
            {
                string url = function.GetImgUrl(preView);
                return SiteConfig.SiteInfo.SiteUrl.TrimEnd('/') + url;
            }
            return preView;
        }
        else
        {
            string url = function.GetImgUrl(Eval("Thumbnails", ""));
            return SiteConfig.SiteInfo.SiteUrl.TrimEnd('/') + url;
        }
    }
}