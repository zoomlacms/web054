using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL.AdSystem;
using ZoomLa.BLL.Helper;
using ZoomLa.SQLDAL;
using ZoomLa.SQLDAL.SQL;

namespace ZoomLaCMS.Plugins.Third.Logo
{
    public partial class List : System.Web.UI.Page
    {
        B_Logo_Design desBll = new B_Logo_Design();
        public int CPage { get { return PageCommon.GetCPage(); } }
        public int PSize { get { return 12; } }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PageSetting setting = SelPage(CPage, PSize, new F_Logo_Design() { });
                DataTable dt = setting.dt;
                foreach (DataRow dr in dt.Rows)
                {
                    dr["LogoContent"] = StrHelper.DecompressString(DataConvert.CStr(dr["LogoContent"]));
                }
                List_Hid.Value = JsonConvert.SerializeObject(dt);
                Page_Lit.Text = PageCommon.CreatePageHtml(setting.pageCount, CPage);
            }
        }
        public PageSetting SelPage(int cpage, int psize, F_Logo_Design filter)
        {
            //        SELECT* FROM (SELECT A.ID AS ProID, B.tlpid FROM ZL_Commodities A LEFT JOIN ZL_P_SHOP B ON A.Itemid = B.ID WHERE Nodeid = 7) A
            //LEFT JOIN ZL_Logo_Design B ON A.tlpid = B.ID
          



            string proField = "A.ID AS ProID,A.LinPrice,A.ProName,A.ItemId,";
            string MTbName = "(SELECT "+proField+ " B.Size,B.sku,b.variations,B.tlpid FROM ZL_Commodities A LEFT JOIN ZL_P_SHOP B ON A.Itemid = B.ID WHERE Nodeid = 2 or A.FirstNodeID=2)";
            string TbName = "ZL_Logo_Design";
            string where = "CONVERT(nvarchar(30),B.LogoContent)!='' ";
            List<SqlParameter> sp = new List<SqlParameter>();
            if (filter.ztype != -100) { where += " AND ZType=" + filter.ztype; }
            if (!string.IsNullOrEmpty(filter.skey)) { where += " AND Alias LIKE @skey"; sp.Add(new SqlParameter("skey", "%" + filter.skey + "%")); }
            if (filter.uid != -100) { where += " AND UserID=" + filter.uid; }
            //180918,去除重复
            where += " AND ItemId IN (SELECT MIN(ID) FROM ZL_P_Shop WHERE TlpId!='' GROUP BY TlpId)";
            //PageSetting setting = PageSetting.Single(cpage, psize, TbName, "ID", where, PK + " DESC", sp, "ID,LogoContent");
            PageSetting setting = PageSetting.Double(cpage,psize,MTbName,TbName,"A.ProID","A.TlpID=B.ID",
                where,"A.ProID DESC",null, "A.*,B.LogoContent");
            DBCenter.SelPage(setting);
            return setting;
        }
    }
}