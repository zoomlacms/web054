using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.Model;
using ZoomLa.SQLDAL;
using ZoomLa.SQLDAL.SQL;
using ZoomLa.BLL.Helper;
using ZoomLa.Common;

namespace ZoomLaCMS.Manage.Pub
{
    public partial class Pubsinfo : CustomerPageAction
    {
        B_Pub pubBll = new B_Pub();
        B_Model modBll = new B_Model();
        B_ModelField fieldBll = new B_ModelField();
        public PageSetting setting = new PageSetting();
        public int Status { get { return DataConvert.CLng(Request.QueryString["Status"], -100); } }
        public string Ignores
        {
            get
            {
                string must = "ID,Pubstart,Pubupid,PubUserID,PubContentid,PubInputer,Parentid,Optimal,Pubnum,PubContent,cookflag,PubUserName";
                return must + DataConvert.CStr(ViewState["Ignores"]);
            }
            set
            {
                ViewState["Ignores"] = value;
            }
        }
        public DataTable FieldDT
        {
            get { return ViewState["FieldDT"] as DataTable; }
            set { ViewState["FieldDT"] = value; }
        }
        public int PubID { get { return DataConvert.CLng(Request.QueryString["PubID"]); } }
        public int ParentID { get { return DataConvert.CLng(Request.QueryString["ParentID"]); } }
        public M_Pub pubMod = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            RPT.SPage = MyBind;
            pubMod = pubBll.SelReturnModel(PubID);
            if (!IsPostBack)
            {
                M_ModelInfo modMod = modBll.SelReturnModel(pubMod.PubModelID);
                //--------------------------
                FieldDT = GetFieldDT(modMod.ModelID); 
                skey_dp.DataSource = FieldDT;
                skey_dp.DataBind();
                Field_RPT.DataSource = FieldDT;
                Field_RPT.DataBind();
                ModelName_L.Text = "[" + modMod.ModelName + "]";
                RPT.DataBind();
            }
        }
        private DataTable MyBind(int psize, int cpage)
        {
            M_ModelInfo modMod = modBll.SelReturnModel(pubMod.PubModelID);
            string where = "1=1 ";
            List<SqlParameter> sp = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(skey.Value))
            {
                if (!IsExistInFieldDT(skey_dp.SelectedValue)) { function.WriteErrMsg("搜索的字段不存在"); }
                where += " AND " + skey_dp.SelectedValue + " LIKE @skey";
                sp.Add(new SqlParameter("skey", "%" + skey.Value.Trim() + "%"));
            }
            if (Status != -100)
            {
                where += " AND PubStart=" + Status;
            }
            if (ParentID > 0)
            {
                where += " AND Parentid="+ParentID;
            }
            setting = PageSetting.Single(cpage, psize, modMod.TableName, "ID", where, "ID DESC", sp);
            DBCenter.SelPage(setting);
            foreach (DataRow dr in setting.dt.Rows)
            {
                dr["PubIP"] = dr["PubIP"]+ "("+ IPScaner.IPLocation(DataConvert.CStr(dr["PubIP"])) + ")";
            }
            RPT.ItemCount = setting.itemCount;
            return setting.dt;
        }
        protected void Search_Btn_Click(object sender, EventArgs e)
        {
            RPT.DataBind();
        }
        protected void RPT_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            M_ModelInfo modMod = modBll.SelReturnModel(pubMod.PubModelID);
            int id = DataConvert.CLng(e.CommandArgument);
            switch (e.CommandName)
            {
                case "del":
                    DBCenter.Del(modMod.TableName, "ID", id);
                    break;
                case "audit":
                    DBCenter.UpdateSQL(pubMod.PubTableName, "PubStart=1", "ID=" + id);
                    break;
            }
            RPT.DataBind();
        }
        private DataTable GetFieldDT(int modelID)
        {
            DataTable dt= fieldBll.SelByModelID(modelID);
            string[] fieldArr = ("PubUserName:用户名,PubTitle:标题,PubContent:内容,PubAddTime:添加时间,PubIP:IP"
                + ",Pubnum:参与人数").Split(',');
            foreach (string field in fieldArr)
            {
                string name = field.Split(':')[0];
                string alias = field.Split(':')[1];
                DataRow dr = dt.NewRow();
                dr["FieldName"] = name;
                dr["FieldAlias"] = alias;
                dt.Rows.Add(dr);
            }
            return dt;
        }
        //-----------------------------------------------------
        //true则忽略不显示
        public bool IsIgnoreField(string columnName)
        {
            string[] ignores = Ignores.Split(',');
            if (ignores.Length < 1) { return false; }
            string result = ignores.FirstOrDefault(p => p.ToLower().Equals(columnName.ToLower()));
            return !string.IsNullOrEmpty(result);
        }
        //避免前端搜索的方式注入,true:存在
        public bool IsExistInFieldDT(string field)
        {
           DataRow[] drs= FieldDT.Select("FieldName IN ('"+field+"')");
            return drs.Length > 0;
        }
        public string GetFieldAlias(string columName)
        {
            DataRow[] drs = FieldDT.Select("FieldName='" + columName + "'");
            if (drs.Length > 0) { return drs[0]["FieldAlias"].ToString(); }
            else { return columName; }
        }
        public string ShowStatus()
        {
            int status = DataConvert.CLng(Eval("PubStart"));
            if (status == 1 || status == 99)
                return "<span style='color:blue'>已审核</span>";
            else
                return "未审核";
        }
        protected void RPT_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = e.Item.DataItem as DataRowView;
                Label label = e.Item.FindControl("ItemHtml_L") as Label;
                string tlp = "<td class=\"td_{1}\">{0}</td>";
                foreach (DataColumn dc in drv.Row.Table.Columns)
                {
                    if (IsIgnoreField(dc.ColumnName)) { continue; }
                    //不输出ignore的行
                    label.Text += string.Format(tlp, HttpUtility.HtmlEncode(drv[dc.ColumnName]),dc.ColumnName);
                }
            }
        }
        protected void Ignore_Btn_Click(object sender, EventArgs e)
        {
            Ignores = Request.Form["ignore_chk"];
            RPT.DataBind();
        }

        protected void BatDel_Btn_Click(object sender, EventArgs e)
        {
            string ids = Request.Form["idchk"];
            if (!string.IsNullOrEmpty(ids))
            {
                SafeSC.CheckIDSEx(ids);
                DBCenter.DelByIDS(pubMod.PubTableName, "ID", ids);
            }
            RPT.DataBind();
        }

        protected void BatAudit_Btn_Click(object sender, EventArgs e)
        {
            string ids = Request.Form["idchk"];
            if (!string.IsNullOrEmpty(ids))
            {
                SafeSC.CheckIDSEx(ids);
                DBCenter.UpdateSQL(pubMod.PubTableName,"PubStart=1","ID IN ("+ids+")");
            }
            RPT.DataBind();
        }

        protected void BatCancel_Btn_Click(object sender, EventArgs e)
        {
            string ids = Request.Form["idchk"];
            if (!string.IsNullOrEmpty(ids))
            {
                SafeSC.CheckIDSEx(ids);
                DBCenter.UpdateSQL(pubMod.PubTableName, "PubStart=0", "ID IN (" + ids + ")");
            }
            RPT.DataBind();
        }
    }
}