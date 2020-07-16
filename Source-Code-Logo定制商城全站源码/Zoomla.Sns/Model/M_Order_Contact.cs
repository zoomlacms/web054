using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ZoomLa.Model;

namespace ZoomLa.Sns
{
    public class M_Order_Contact : M_Base
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public DateTime CDate { get; set; }
        public string Remark { get; set; }

        public override string TbName { get { return "ZL_Order_Contact"; } }
        public override string PK { get { return "ID"; } }

        public override string[,] FieldList()
        {
            string[,] Tablelist = {
                                {"ID","Int","4"},
                                {"OrderID","Int","4"},
                                {"FullName","NVarChar","100"},
                                {"Email","NVarChar","100"},
                                {"Address","NVarChar","200"},
                                {"City","NVarChar","200"},
                                {"State","NVarChar","100"},
                                {"Country","NVarChar","100"},
                                {"Zip","NVarChar","100"},
                                {"Phone","NVarChar","100"},
                                {"CDate","DateTime","8"},
                                {"Remark","NVarChar","100"}
        };
            return Tablelist;
        }

        public override SqlParameter[] GetParameters()
        {
            M_Order_Contact model = this;
            if (model.CDate <= DateTime.MinValue) { model.CDate = DateTime.Now; }
            SqlParameter[] sp = GetSP();
            sp[0].Value = model.ID;
            sp[1].Value = model.OrderID;
            sp[2].Value = model.FullName;
            sp[3].Value = model.Email;
            sp[4].Value = model.Address;
            sp[5].Value = model.City;
            sp[6].Value = model.State;
            sp[7].Value = model.Country;
            sp[8].Value = model.Zip;
            sp[9].Value = model.Phone;
            sp[10].Value = model.CDate;
            sp[11].Value = model.Remark;
            return sp;
        }
        public M_Order_Contact GetModelFromReader(DbDataReader rdr)
        {
            M_Order_Contact model = new M_Order_Contact();
            model.ID = ConvertToInt(rdr["ID"]);
            model.OrderID = ConvertToInt(rdr["OrderID"]);
            model.FullName = ConverToStr(rdr["FullName"]);
            model.Email = ConverToStr(rdr["Email"]);
            model.Address = ConverToStr(rdr["Address"]);
            model.City = ConverToStr(rdr["City"]);
            model.State = ConverToStr(rdr["State"]);
            model.Country = ConverToStr(rdr["Country"]);
            model.Zip = ConverToStr(rdr["Zip"]);
            model.Phone = ConverToStr(rdr["Phone"]);
            model.CDate = ConvertToDate(rdr["CDate"]);
            model.Remark = ConverToStr(rdr["Remark"]);
            rdr.Close();
            return model;
        }
    }
}
