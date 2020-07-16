<%@ WebHandler Language="C#" Class="Common" %>

using System;
using System.Web;
using ZoomLa.Common;
using ZoomLa.BLL;
using ZoomLa.BLL.API;
using ZoomLa.Model;
using System.Data;
using ZoomLa.Sns;
using Newtonsoft.Json;
using ZoomLa.SQLDAL;
using ZoomLa.BLL.Helper;
/*
 * 仅允许Form提交
 * 用于价格计算,增减商品
 * -1:失败,1:成功或直接返回值
 */
public class Common : API_Base,System.Web.IHttpHandler{
    B_Cart cartBll = new B_Cart();
    M_Cart cartMod = new M_Cart();
    B_User buser = new B_User();
    private double GetMoneyByProID(int proid)
    {
        if (proid < 1) { return 0; }
        return Convert.ToDouble(DBCenter.ExecuteScala("ZL_Commodities", "LinPrice", "ID=" + proid));
    }
    public void ProcessRequest(HttpContext context)
    {
        M_UserInfo mu = buser.GetLogin();
        retMod.retcode = M_APIResult.Failed;
        string CartCookID = B_Cart.GetCartID(context);
        //retMod.callback = CallBack;//暂不开放JsonP
        try
        {
            switch (Action)
            {
                case "grade":
                    {
                        int pid = DataConvert.CLng(Req("pid"));
                        DataTable dt = DBCenter.SelWithField("ZL_Grade", "GradeID,GradeName", "Cate=8 AND ParentID=" + pid, "");
                        retMod.result = JsonConvert.SerializeObject(dt);
                        retMod.retcode = M_APIResult.Success;
                    }
                    break;
                case "ledchange":
                    {
                        int id = Convert.ToInt32(Req("id"));
                        string opname = Req("opname");
                        int opval = DataConverter.CLng(Req("opval"));
                        M_Cart cartMod = cartBll.SelReturnModel(id);
                        M_Cart_Addition addMod = JsonConvert.DeserializeObject<M_Cart_Addition>(cartMod.Additional);
                        switch (opname)
                        {
                            case "flash":
                                addMod.flash = opval;
                                break;
                            case "outdoor":
                                addMod.outdoor = opval;
                                break;
                            case "backing":
                                addMod.backing = opval;
                                break;
                        }
                        //重新计算价格
                        if (cartMod.Pronum < 1) { cartMod.Pronum = 1; }
                        double linPrice = 0;
                        linPrice = GetMoneyByProID(cartMod.ProID);
                        linPrice += GetMoneyByProID(addMod.flash);
                        linPrice += GetMoneyByProID(addMod.outdoor);
                        linPrice += GetMoneyByProID(addMod.backing);
                        cartMod.FarePrice = linPrice.ToString("F2");
                        cartMod.AllIntegral = cartMod.AllMoney = linPrice * cartMod.Pronum;
                        cartMod.Additional = JsonConvert.SerializeObject(addMod);
                        cartBll.UpdateByID(cartMod);
                        retMod.result = cartMod.AllMoney.ToString("F2");
                        retMod.retcode = M_APIResult.Success;
                        break;
                    }
                case "coupon"://通过优惠券编号与密码使用该券
                    {
                        B_Ex_Coupon cupBll = new B_Ex_Coupon();
                        M_Arrive_Result result = new M_Arrive_Result();

                        string code = Req("code").Trim();
                        //string pwd = Req("pwd");
                        double money = DataConvert.CDouble(Req("money"));
                        result = cupBll.CheckArrive(code, money);
                        //---------------
                        if (result.enabled)
                        {
                            retMod.retcode = M_APIResult.Success;
                            retMod.result = JsonConvert.SerializeObject(result);
                        }
                        else
                        {
                            retMod.retmsg = result.err;
                        }
                    }
                    break;
                case "cart_del":
                    {
                        string ids = Req("ids");
                        cartBll.DelByIDS(CartCookID,"",ids);
                        retMod.retcode = M_APIResult.Success;
                    }
                    break;
                case "setnum"://兼容
                case "cart_setnum"://ID,数量,Cookies,可不登录,数量不能小于1
                    {
                        int id = DataConverter.CLng(Req("id"));
                        int pronum = DataConverter.CLng(Req("pronum"));
                        if (pronum < 1) { pronum = 1; }
                        if (id < 1 || pronum < 1)
                        {
                            retMod.retmsg = "Goods ID and quantity can't be less than 1";
                        }
                        else if (string.IsNullOrEmpty(CartCookID))
                        {
                            retMod.retmsg = "CartCookID does not exist";
                        }
                        else
                        {
                            cartMod = cartBll.SelReturnModel(id);
                            cartMod.Pronum = pronum;
                            cartMod.AllMoney = pronum * Convert.ToDouble(cartMod.FarePrice);
                            cartBll.UpdateByID(cartMod);
                            retMod.result = cartMod.AllMoney.ToString("F2");
                            retMod.retcode = M_APIResult.Success;
                        }
                    }
                    break;
                case "deladdress":
                    {
                        B_UserRecei receBll = new B_UserRecei();
                        int id = DataConverter.CLng(Req("id"));
                        if (mu == null || mu.UserID == 0 || id < 1) { OldRep("-1"); }
                        else
                        {
                            receBll.U_DelByID(id, mu.UserID); OldRep("1");
                        }
                    }
                    break;
                case "arrive":
                    {
                        //B_Arrive avBll = new B_Arrive();
                        //string flow = Req("flow");
                        //string ids = Req("ids");
                        //double money = double.Parse(Req("money"));
                        //DataTable cartdt = cartBll.SelByCartID(CartCookID, mu.UserID, -100, ids);
                        //if (cartdt.Rows.Count < 1) { retMod.retmsg = "The shopping cart is empty";RepToClient(retMod); }
                        //M_Arrive avMod = avBll.SelModelByFlow(flow, mu.UserID);
                        //M_Arrive_Result arrMod = avBll.U_CheckArrive(avMod, mu.UserID, cartdt, money);
                        //if (arrMod.enabled)
                        //{
                        //    retMod.retcode = M_APIResult.Success;
                        //    //已优惠金额,优惠后金额
                        //    retMod.result = Newtonsoft.Json.JsonConvert.SerializeObject(arrMod);
                        //}
                        //else { retMod.retmsg = arrMod.err; }
                    }
                    break;
                default:
                    retMod.retmsg = "[" + Action + "]Interface does not exist";
                    break;
            }
        }
        catch (Exception ex) { retMod.retmsg = ex.Message; }
        RepToClient(retMod);
    }
    public void OldRep(string result) { HttpResponse rep = HttpContext.Current.Response; rep.Clear(); rep.Write(result); rep.Flush(); rep.End(); }
    public bool IsReusable { get { return false; } }
}