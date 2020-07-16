using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace ZoomLaCMS.Manage.Plus.Logo
{
    public partial class Design : System.Web.UI.Page
    {
        B_Logo_Design desBll = new B_Logo_Design();
        public B_Logo_Icon iconBll = new B_Logo_Icon();
        public M_Logo_Design desMod = null;
        public int Mid { get { return DataConvert.CLng(Request.QueryString["ID"]); } }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Mid > 0) {
                    desMod = desBll.SelReturnModel(Mid);
                    Alias_T.Text = desMod.Alias;
                    BKUrl_T.Text = GetBk(desMod.ID);
                    Save_Hid.Value = StrHelper.DecompressString(desMod.LogoContent);
                    //对其中的URL做处理
                    string[] urlArr = new string[] { "http://win10:164", "http://www.raysandsigns.com",
                        "http://raysandsigns.com","https://www.raysandsigns.com", "https://raysandsigns.com"
                        };
                    foreach (string url in urlArr)
                    {
                        Save_Hid.Value = Save_Hid.Value.Replace(url, "");
                    }
                    //function.Script(this,"fabHelper.init("+StrHelper.DecompressString(desMod.LogoContent)+");");
                }
            }
        }
        private string GetBk(int id, string def = "black")
        {
            string bk = DataConvert.CStr(DBCenter.ExecuteScala("ZL_Logo_Design", "BackGround", "ID=" + id));
            return string.IsNullOrEmpty(bk) ? def : bk;
        }
        protected void Save_Btn_Click(object sender, EventArgs e)
        {
            if (Mid > 0)
            {
                desMod = desBll.SelReturnModel(Mid);
            }
            else { desMod = new M_Logo_Design(); }
            desMod.Alias = Alias_T.Text;
            desMod.LogoContent = StrHelper.CompressString(Save_Hid.Value);
            ImgHelper imghelp = new ImgHelper();
            System.Drawing.Bitmap bmp= imghelp.Base64ToImg(Base64_Hid.Value);
            //用于减小尺寸
            desMod.PreviewImg = "data:image/png;base64,"+imghelp.ImgToBase64ByImage(bmp);

            if (desMod.ID > 0) { desMod.ZType = 1; desBll.UpdateByID(desMod); }
            else
            {
                M_AdminInfo adminMod = B_Admin.GetLogin();
                desMod.ZType = 1;
                desMod.ZStatus = 99;
                desMod.AdminID = adminMod.AdminId;
                desMod.ID = desBll.Insert(desMod);
            }
            SnsHelper.UpdateModel(desMod.ID
            , new string[] { "BackGround" }
            , new string[] { BKUrl_T.Text.Trim() });
            function.WriteSuccessMsg("操作成功", "DesTlpList.aspx");
        }
    }
}