using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ZoomLa.BLL;
using ZoomLa.SQLDAL;
using ZoomLa.SQLDAL.SQL;

namespace ZoomLa.Sns
{
    public class B_Order_Contact
    {
        private M_Order_Contact initMod = new M_Order_Contact();
        public string TbName = "", PK = "";
        public B_Order_Contact()
        {
            TbName = initMod.TbName;
            PK = initMod.PK;
        }
        public DataTable Sel()
        {
            return DBCenter.Sel(TbName, "", PK + " DESC");
        }
        public M_Order_Contact SelReturnModel(int ID)
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
        public M_Order_Contact SelModelByOid(int oid)
        {
            using (DbDataReader reader = DBCenter.SelReturnReader(TbName,"OrderID="+oid,"ID DESC",null))
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
        public int Insert(M_Order_Contact model)
        {
            //return Sql.insert(TbName, model.GetParameters(model), BLLCommon.GetParas(model), BLLCommon.GetFields(model));
            return DBCenter.Insert(model);
        }
        public bool UpdateByID(M_Order_Contact model)
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
    }
}
