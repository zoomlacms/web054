using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ZoomLa.BLL;
using ZoomLa.Model;
using ZoomLa.SQLDAL;
using ZoomLa.SQLDAL.SQL;

namespace ZoomLa.Sns
{
    public class B_Ex_Coupon
    {
        private M_Ex_Coupon initMod = new M_Ex_Coupon();
        public string TbName = "", PK = "";
        public B_Ex_Coupon()
        {
            TbName = initMod.TbName;
            PK = initMod.PK;
        }
        public DataTable Sel()
        {
            return DBCenter.Sel(TbName, "", PK + " DESC");
        }
        public M_Ex_Coupon SelReturnModel(int ID)
        {
            using (DbDataReader reader = DBCenter.SelReturnReader(TbName, PK, ID))
            {
                if (reader.Read())
                {
                    return initMod.GetModelFromReader(reader);
                }
                else
                {
                    return null;
                }
            }
        }
        public M_Ex_Coupon SelModelByCode(string code)
        {
            List<SqlParameter> sp = new List<SqlParameter>()
            {
                new SqlParameter("code",code.Trim())
            };
            using (DbDataReader reader = DBCenter.SelReturnReader(TbName, "Code=@code", "ID DESC", sp))
            {
                if (reader.Read())
                {
                    return initMod.GetModelFromReader(reader);
                }
                else
                {
                    return null;
                }
            }
        }
        public int Insert(M_Ex_Coupon model)
        {
            //return Sql.insert(TbName, model.GetParameters(model), BLLCommon.GetParas(model), BLLCommon.GetFields(model));
            return DBCenter.Insert(model);
        }
        public bool UpdateByID(M_Ex_Coupon model)
        {
            return DBCenter.UpdateByID(model, model.ID);
        }
        public void Del(string ids)
        {
            if (string.IsNullOrEmpty(ids)) { return; }
            SafeSC.CheckIDSEx(ids);
            DBCenter.DelByIDS(TbName, PK, ids);
        }
        public PageSetting SelPage(int cpage, int psize)
        {
            string where = "1=1 ";
            List<SqlParameter> sp = new List<SqlParameter>();
            PageSetting setting = PageSetting.Single(cpage, psize, TbName, PK, where, PK + " DESC", sp);
            DBCenter.SelPage(setting);
            return setting;
        }
        public M_Arrive_Result CheckArrive(string code,double money)
        {
            M_Arrive_Result result = new M_Arrive_Result();
            try
            {
                if (string.IsNullOrEmpty(code)) { throw new Exception("The coupon number cannot be empty"); }
                if (money < 1) { throw new Exception("Incorrect order amount"); }
                M_Ex_Coupon cupMod = SelModelByCode(code);
                if (cupMod == null) { throw new Exception("Coupon do not exist"); }
                if (cupMod.ZStatus != 1) { throw new Exception("Coupon have not been activated"); }
                if (cupMod.AMount <= 0) { throw new Exception("Abnormal value of coupon[" + cupMod.AMount.ToString("N") + "]"); }

                result.flow = code;
                result.enabled = true;
                switch (cupMod.ZType)
                {
                    case "金额":
                        {
                            money -= cupMod.AMount;
                            money = ((money < 0.0) ? 0.0 : money);
                            result.money = money;
                            result.amount = cupMod.AMount;
                        }
                        break;
                    case "比率":
                        {
                            double amount =Convert.ToDouble((money * cupMod.AMount).ToString("F2"));
                            money -= amount;
                            money = ((money < 0.0) ? 0.0 : money);
                            result.money = money;
                            result.amount = amount;
                        }
                        break;
                }
            }
            catch (Exception ex) { result.err = ex.Message;result.enabled = false; }
            return result;
        }
    }
    public class M_Ex_Coupon : M_Base
    {
        public int ID { get; set; }
        /// <summary>
        /// 优惠券编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 优惠类型
        /// </summary>
        public string ZType { get; set; }
        /// <summary>
        /// 1:启用
        /// </summary>
        public int ZStatus { get; set; }
        public double AMount { get; set; }
        public string Remind { get; set; }
        public DateTime CDate { get; set; }
        public M_Ex_Coupon()
        {
            CDate = DateTime.Now;
            ZStatus = 1;
            AMount = 0;
        }
        public override string TbName { get { return "ZL_Ex_Coupon"; } }
        public override string PK { get { return "ID"; } }

        public override string[,] FieldList()
        {
            string[,] Tablelist = {
                                {"ID","Int","4"},
                                {"Code","NVarChar","100"},
                                {"ZType","NVarChar","50"},
                                {"ZStatus","Int","4"},
                                {"AMount","Money","8"},
                                {"Remind","NVarChar","500"},
                                {"CDate","DateTime","8"}
        };
            return Tablelist;
        }

        public override SqlParameter[] GetParameters()
        {
            M_Ex_Coupon model = this;
            SqlParameter[] sp = GetSP();
            sp[0].Value = model.ID;
            sp[1].Value = model.Code;
            sp[2].Value = model.ZType;
            sp[3].Value = model.ZStatus;
            sp[4].Value = model.AMount;
            sp[5].Value = model.Remind;
            sp[6].Value = model.CDate;
            return sp;
        }
        public M_Ex_Coupon GetModelFromReader(DbDataReader rdr)
        {
            M_Ex_Coupon model = new M_Ex_Coupon();
            model.ID = ConvertToInt(rdr["ID"]);
            model.Code = ConverToStr(rdr["Code"]);
            model.ZType = ConverToStr(rdr["ZType"]);
            model.ZStatus = ConvertToInt(rdr["ZStatus"]);
            model.AMount = ConverToDouble(rdr["AMount"]);
            model.Remind = ConverToStr(rdr["Remind"]);
            model.CDate = ConvertToDate(rdr["CDate"]);
            rdr.Close();
            return model;
        }
    }
}
