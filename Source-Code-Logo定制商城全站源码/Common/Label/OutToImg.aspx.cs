using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL.Helper;
using ZoomLa.Safe;

namespace ZoomLaCMS.Common.Label
{
    public partial class OutToImg : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string base64 = Request.Form["base64_hid"];
            if (string.IsNullOrEmpty(base64)) { return; }
            SafeC.DownFile(IOHelper.StreamToBytes(Base64ToImg(base64)), "view.png");
        }
        public MemoryStream Base64ToImg(string base64)
        {
            if (base64.Contains("base64,"))
            {
                base64 = Regex.Split(base64, Regex.Unescape("base64,"))[1];
            }
            byte[] arr = Convert.FromBase64String(base64);
            MemoryStream ms = new MemoryStream(arr);
            return ms;
        }
    }
}