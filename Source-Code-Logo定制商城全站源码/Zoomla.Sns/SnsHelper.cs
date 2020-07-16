using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ZoomLa.BLL;
using ZoomLa.Common;
using ZoomLa.Model;
using ZoomLa.SQLDAL;

namespace ZoomLa.Sns
{
    public class SnsHelper
    {
        private static string OrderTbName = "ZL_OrderInfo";
        public static M_UserInfo GetLogin()
        {
            M_UserInfo mu = new M_UserInfo();
            mu.UserID = 10000;
            mu.TrueName = mu.HoneyName = mu.UserName = "AnonymousUser";
            mu.UserPwd = StringHelper.MD5("123123sdfsavvx");
            mu.Question = "sfasdfdsaf";
            mu.Answer = "sfsdafsdafsdaf";
            mu.Email = function.GetRandomEmail();
            mu.Status = 0;
            return mu;
        }
        /// <summary>
        /// 返回option,自己外加<select>包裹
        /// </summary>
        /// <param name="type">flash|outdoor|backing</param>
        /// <param name="value">选中值</param>
        public static string H_GetDPOption(string type,string emptyText,string value="")
        {
            DataTable dt = new DataTable();
            switch (type)
            {
                case "flash":
                    dt = DBCenter.SelWithField("ZL_Commodities", "id,proname,linprice", "NodeID=" + 100);
                    break;
                case "outdoor":
                    dt = DBCenter.SelWithField("ZL_Commodities", "id,proname,linprice", "NodeID=" + 101);
                    break;
                case "backing":
                    dt = DBCenter.SelWithField("ZL_Commodities", "id,proname,linprice", "NodeID=" + 102);
                    break;
            }
            {
                if (!string.IsNullOrEmpty(emptyText))
                {
                    DataRow dr = dt.NewRow();
                    dr["id"] = "0";
                    dr["proname"] = emptyText;
                    dt.Rows.InsertAt(dr, 0);
                }
            }
            string html = "";
            foreach (DataRow dr in dt.Rows)
            {
                string id = DataConvert.CStr(dr["ID"]);
                string price = DataConvert.CDouble(dr["LinPrice"]) <= 0 ? "" : "(+$" + DataConvert.CLng(dr["LinPrice"]) + ")";
                string selected = id.Equals(value) ? "selected=\"selected\"" : "";
                html += "<option value=\"" + id + "\" " + selected + ">" + dr["proname"] + price + "</option>";
            }
            return html;
            //DataTable flashDT = proBll.Sel(100);
            //DataTable signDT = proBll.Sel(101);
            //DataTable backDT = proBll.Sel(102);
        }
        /// <summary>
        /// Logo附加了字段,但没存在模型中
        /// </summary>
        public static void UpdateModel(int id,string[] fields, string[] values)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            string setSql = "";
            for (int i = 0; i < fields.Length; i++)
            {
                sp.Add(new SqlParameter(fields[i], values[i]));
                setSql += fields[i] + "=@" + fields[i] + ",";
            }
            setSql = setSql.TrimEnd(',');
            DBCenter.UpdateSQL("ZL_Logo_Design", setSql, "ID=" + id, sp);
        }
        /// <summary>
        /// 带缩略图的列表
        /// </summary>
        public static DataTable SelCart(int oid,int cid=-100)
        {
            string where = "A.OrderlistID=" + oid + " AND (A.Attribute !='' AND A.Attribute IS NOT NULL)";
            string mTbName = "(SELECT A.*,B.NodeID,B.Thumbnails FROM ZL_CartPro A LEFT JOIN ZL_Commodities B ON A.ProID=B.ID)";
            if (cid != -100) { where += " AND A.ID=" + cid; }
            return DBCenter.JoinQuery("A.*,B.PreViewImg", mTbName, "ZL_Logo_Design",
                 "A.Attribute=B.ID", where);

        }
        /// <summary>
        /// 获取Email的发送记录
        /// </summary>
        /// <returns></returns>
        public static DataRow SelEmailInfo(int oid)
        {
            DataTable dt = DBCenter.SelWithField(OrderTbName, "ID,EMail_Pay,EMail_Exp,EMail_After", "ID=" + oid);
            return dt.Rows[0];
        }
        public static void UpdateOrderField(int oid, string field, string value)
        {
            DBCenter.UpdateSQL(OrderTbName, field + "=" + value, "ID=" + oid);
        }

        #region 优惠券
        public static M_Arrive AV_SelModel(string avNo)
        {
            M_Arrive model = new M_Arrive();
            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("arriveNo", avNo)};
            using (SqlDataReader sqlDataReader = Sql.SelReturnReader(model.TbName, " Where ArriveNo=@arriveNo", sp))
            {
                if (sqlDataReader.Read())
                {
                    return model.GetModelFromReader(sqlDataReader);
                }
                return null;
            }
        }
        public static void Use_Arrive(M_Arrive model, string remind)
        {
            List<SqlParameter> list = new List<SqlParameter>
            {
                new SqlParameter("remind", remind),
                new SqlParameter("usetime", DateTime.Now)
            };
            DBCenter.UpdateSQL("ZL_Arrive", "State=10,UseRemind=@remind,UseTime=@usetime", "ID=" + model.ID, list);
        }
        public static M_Arrive_Result CheckArrive(string code, string pwd, double money)
        {
            M_Arrive_Result result = new M_Arrive_Result();
            if (string.IsNullOrEmpty(code))
            {
                result.err = "The coupon number cannot be empty";
            }
            //else if (string.IsNullOrEmpty(pwd))
            //{
            //    result.err = "The coupon password cannot be empty";
            //}
            else if (money < 1.0)
            {
                result.err = "Incorrect order amount";
            }
            B_Arrive avBll = new B_Arrive();
            M_Arrive avMod = SnsHelper.AV_SelModel(code);
            result.money = money;
            if (avMod==null||avMod.ID<1)
            {
                result.err = "Coupon do not exist";
            }
            else if (avMod.State == 10)
            {
                result.err = "Coupon have been used";
            }
            else if (avMod.State == 0)
            {
                result.err = "Coupon have not been activated";
            }
            else if (avMod.Amount < 1.0)
            {
                result.err = "Abnormal value of coupon[" + avMod.Amount.ToString("N") + "]";
            }
            else if (avMod.EndTime < DateTime.Now)
            {
                result.err = "Coupon have expired";
            }
            else if (avMod.AgainTime> DateTime.Now)
            {
                result.err = "Coupon have not yet reached the available time";
            }
            if (!string.IsNullOrEmpty(result.err))
            {
                return result;
            }
            result.flow = avMod.Flow;
            avMod.MaxAmount = avMod.MaxAmount == 0 ? 5000000 : avMod.MaxAmount;
            if (avMod.MinAmount > money)
            {
                result.err = "No minimum amount of use limit";
                return result;
            }
            if (avMod.MaxAmount < money)
            {
                result.err = "More than maximum use limit";
                return result;
            }
            money -= avMod.Amount;
            money = ((money < 0.0) ? 0.0 : money);
            result.money = money;
            result.amount = avMod.Amount;
            result.enabled = true;
            return result;
        }
        #endregion
        #region 订单
        public static List<M_OrderList> OrdersCheck(M_Payment payMod)
        {
            B_OrderList val = new B_OrderList();
            if (payMod.Status != 1)
            {
                function.WriteErrMsg("Incorrect order payment condition");
                return null;
            }
            if (payMod.IsDel == 1)
            {
                function.WriteErrMsg("The order has been deleted");
                return null;
            }
            string[] array = payMod.PaymentNum.Split(',');
            List<M_OrderList> list = new List<M_OrderList>();
            for (int i = 0; i < array.Length; i++)
            {
                M_OrderList val2 = val.SelModelByOrderNo(array[i]);
                SnsHelper.CheckIsCanPay(val2);
                list.Add(val2);
            }
            return list;
        }

        public static void CheckIsCanPay(M_OrderList orderMod)
        {
            if ((object)orderMod == null)
            {
                function.WriteErrMsg("Order does not exist");
            }
            if (string.IsNullOrEmpty(orderMod.OrderNo))
            {
                function.WriteErrMsg("Unspecified order number");
            }
            if (orderMod.OrderStatus < 0)
            {
                function.WriteErrMsg(orderMod.OrderNo + "The order status is abnormal and the payment can not be completed");
            }
            if (orderMod.Ordersamount < 0.0)
            {
                function.WriteErrMsg(orderMod.OrderNo + "订单应付金额异常");
            }
            if (orderMod.Paymentstatus != 0)
            {
                function.WriteErrMsg(orderMod.OrderNo + "The order has been paid and can not be duplicated.");
            }
        }
        #endregion
    }
}
