<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cart.aspx.cs" Inherits="ZoomLaCMS.Cart.cart" MasterPageFile="~/Cart/order.master" EnableViewState="false"%>
<%@ Import Namespace="ZoomLa.Common" %>
<%@ Import Namespace="ZoomLa.BLL.Shop" %>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>RaysAndSigns</title> </asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<%Call.Label("{ZL.Label id=\"模板设计全站头部\"/}");%>
<div class="disgin_banner">
<h1 hidden>Only one step away from your purchase.</h1>
</div>

<div class="container">
<table id="EGV" class="table cartbody">
        <tr class="table_title">
            <td class="td_s"></td>
            <td>Name</td>
<%--            <td class="hidden-xs" runat="server" id="ptype_td">单价</td>--%>
            <td class="td_m">Quantity</td>
            <td class="td_m hidden-xs">Status</td>
            <td class="td_m">Total</td>
            <td class="td_s">Manage</td>
        </tr>
        <asp:Repeater runat="server" ID="RPT" OnItemDataBound="RPT_ItemDataBound">
            <ItemTemplate>
                <tbody style="border:none;" class="cart_body">
<%--                    <tr>
                        <td colspan="7" class="storename">
                            <label><input type="checkbox" class="store_chk" name="store_chk" checked="checked" value="<%#Eval("ID") %>" />
                                <i class="fa fa-home" title="店铺"></i> <%#Eval("StoreName") %></label></td>
                    </tr>--%>
                    <asp:Repeater runat="server" ID="ProRPT" OnItemDataBound="ProRPT_ItemDataBound">
                        <ItemTemplate>
                                <tr data-id="<%#Eval("ID") %>" class="cart_item">
                                    <td style="display:none;">
                                       <input type="checkbox" name="prochk" data-store="<%#Eval("StoreID") %>" checked="checked" value="<%#Eval("ID") %>" />
                                    </td>
                                    <td colspan="2">
                                        <a href="<%#GetShopUrl() %>" target="_blank" title="浏览商品">
                                            <img src="<%#GetShopImg(Eval("ProAttr"))%>" onerror="this.src='<%#GetImgUrl(Eval("Thumbnails"))%>'" style="max-width:10rem;" class="preimg_l" />
                                        </a>
                                        <a href="<%#GetShopUrl() %>" target="_blank"><%#Eval("ProName") %>(This is the name of the template you chose)</a>
                                        <asp:Label runat="server" ID="Present_HTML"></asp:Label>
                                    </td>
                             <%--   <td class="tdline hidden-xs"><%#GetMyPrice() %></td>--%>
                                    <td class="pronum_td">
                                        <div class="input-group margin_t20 td_m">
                                            <span class="input-group-prepend" onclick="cart.setnum('<%#Eval("ID") %>','+');"><span class="input-group-text"><i class="fa fa-minus"></i></span></span>
                                            <input type="text" class="form-control pronum_text" id="pronum_<%#Eval("ID") %>" value="<%#Eval("Pronum") %>" autocomplete="off" onblur="cart.setnum('<%#Eval("ID") %>',this.value);">
                                            <span class="input-group-append" onclick="cart.setnum('<%#Eval("ID") %>','-');"><span class="input-group-text"><i class="fa fa-plus"></i></span></span>
                                        </div>
                                    </td>
                                    <td class="tdline hidden-xs"></td>
                                    <td class="tdline"><span id="trprice_<%#Eval("ID") %>" class="trprice"><%#Eval("AllMoney","{0:N}") %></span></td>
                                    <td class="tdline">
                                         <a href="javascript:;" class="btn btn-danger btn-sm" onclick="cart.del('<%#Eval("ID") %>');"><i class="fa fa-trash-o"></i></a>
                                    </td>
                                </tr>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </ItemTemplate>
        </asp:Repeater>
        <tr><td colspan="7">
            <%--<label>
                <input type="checkbox" checked="checked" onclick="ChkAll(this);" /></label>--%>
              <%--  <input type="button" value="Delete" class="btn btn-primary btn-xs margin_l5" onclick="cart.batdel();" />--%>
            <a href="/" class="btn btn-success">Continue Shopping</a>
            <div class="pull-right">
                <span style="font-size:24px;">$<span runat="server" id="totalmoney" class="totalmoney">0.00</span></span>
                <input type="button" id="NextStep" value="NEXT" class="btn btn-primary" onclick="GetNextStep();" style="color:#fff;cursor:pointer;"/>
                <asp:Button runat="server" ID="RealNext_Btn" OnClick="NextStep_Click" Style="display: none;" />
            </div>
        </td></tr>
</table>
<div id="remind_max" style="padding: 20px; display: none;">
    <div>
        <span class="fa fa-warning" style="font-size: 48px; color: #ffd800;"></span>
        <span style="font-size: 18px; font-weight: bold; margin-left: 10px;">The quantity of goods must be <=200</span>
    </div>
</div>
</div>
<%Call.Label("{ZL.Label id=\"模板设计全站底部\"/}");%>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
<style type="text/css">
@media(max-width:768px) {.container {padding-left:0px;padding-right:0px;}}
.cartbody .suit_item{background:#fff4e8;}
.cartbody .line_circle{background:url(/App_Themes/Admin/shop/cart-line-02.png) 0 -17px no-repeat;top:0px;left:10px;height:30px;display:block;position:absolute;width:7px;}
.cartbody .line_vertical{border:none; border-left:1px dashed #ddd;width:1px;height:100%;display:block;position:absolute;left:13px;min-height:90px;}
</style>
<script src="/JS/Controls/B_User.js"></script>
<script src="/JS/Controls/ZL_Dialog.js"></script>
<script src="/JS/Controls/Control.js"></script>
<script src="/JS/ICMS/ZL_Common.js"></script>
<script src="/JS/ZL_Regex.js?v=1"></script>
<script src="/JS/Modal/APIResult.js"></script>
<script>
    var diag = new ZL_Dialog();
    var cart = { api: "/cart/OrderCom.ashx?action=", suit: {}, price: {} };
    cart.del = function (id) {
        if (!confirm("confirm delete?")) { return false; }
        $.post(cart.api + "cart_del", { "ids": id }, function (data) {
            APIResult.ifok(data, function () { location = location; }, function () { console.log("failed", data); });
        })
    }
    cart.batdel = function () {
        var ids = "";
        var $chks = $("input[name='prochk']:checked");
        if ($chks.length < 1) { alert("Unselected products"); }
        $chks.each(function () { ids += this.value + ","; });
        ids = ids.substr(0, ids.length - 1);
        cart.del(ids);
    }
    cart.setnum = function (id, nowNum) {
        var num = cart.getnum(id, nowNum);
        $.post(cart.api + "cart_setnum", { "id": id, pronum: num }, function (data) {
            APIResult.ifok(data, function (result) {
                $("#trprice_" + id).text(result);
                UpdateTotalPrice();
            }, function () { console.log("setnum failed:", data); });
        });
    }
    cart.updatePrice = function (id) {
        var pronum = $("#pronum_" + id).val();
        var price = $("#price_" + id).text();
        var obj = $("#trprice_" + id);
        obj.text(Convert.ToMoneyStr((price * pronum)));
        UpdateTotalPrice();
    }
    //-----------------公用方法
    cart.getnum = function (id, num) {
        var $text = $("#pronum_" + id);
        var v = Convert.ToInt($text.val(), 1);
        switch (num) {
            case "+":
                if (v > 1) { --v; }
                $text.val(v);
                break;
            case "-":
                $text.val(++v);
                break;
            default:
                $text.val(Convert.ToInt(num, 1));
                break;
        }
        return parseInt($text.val());
    }
    //-------------------------------------------
    $(function () {
        ZL_Regex.B_Num(".pronum_text");
        ZL_Regex.B_Value(".pronum_text", {
            min: 1, max: 200, overmin: function () { }, overmax: function () {
                ShowRemind();
            }
        });
        $("input[name=prochk]").click(function () {
            UpdateTotalPrice();
            UpdateItemNum();
        });
        $(".store_chk").click(function () {
            ChkByStore(this);
            UpdateItemNum();
            UpdateTotalPrice();
        });
        UpdateItemNum();
        UpdateTotalPrice();
        Control.EnableEnter();
    })
    //------AJAX
    function GetNextStep() {
        disBtn($("#NextStep")[0], 2000);
        $("#RealNext_Btn").click();
    }
    function LoginSuccess() {
        diag.CloseModal();
        $("#RealNext_Btn").click();
    }
    function AutoHeight() { diag.AutoHeight(); }
    //------Page
    function skey()
    {
        var key = $("#skey_t").val();
        window.open("/Search/SearchList?node=0&keyword="+key);
    }
    //全选本店商品
    function ChkByStore(obj) {
        $(":checkbox[name=prochk][data-store=" + obj.value + "]").each(function () { this.checked = obj.checked; });
    }
    //--商品数量相关
    function ChkAll(obj) {
        $("#EGV :checkbox").each(function () { this.checked = obj.checked; });
        UpdateItemNum();
    }
    //更新数量,确定是否禁用按钮
    function UpdateItemNum() {
        var num = $("[name=prochk]:checked").length;
        $("#pronum_span").text(num);
        document.getElementById("NextStep").disabled = (num < 1) ? "disabled" : "";
    }
    //更新总金额
    function UpdateTotalPrice() {
        var $chkarr = $("[name=prochk]:checked");
        var money = 0.00, purse = 0.00, sicon = 0.00, point = 0.00;
        for (var i = 0; i < $chkarr.length; i++) {
            var id = $chkarr[i].value;
            var num = $("#pronum_" + id).val();
            var pursePrice = parseFloat($("#price_purse_" + id).text());
            var siconPrice = parseFloat($("#price_sicon_" + id).text());
            var pointPrice = parseFloat($("#price_point_" + id).text());
            if (pursePrice)
                purse += pursePrice * num;
            if (siconPrice)
                sicon += siconPrice * num;
            if (pointPrice)
                point += pointPrice * num;
            money += parseFloat($("#trprice_" + id).text());
        }
        $("#totalmoney").text(Convert.ToMoneyStr(money));
        $("#totalPurse_sp").text(purse.toFixed(2));
        $("#totalSicon_sp").text(sicon.toFixed(2));
        $("#totalPoint_sp").text(point.toFixed(2));
    }
    //---------------------------数量提示窗
    var reminddiv;
    $(function () {
        reminddiv = $("#remind_max");
        reminddiv.remove();
        reminddiv.show();
    })
    function ShowRemind() {
        var diag = new ZL_Dialog();
        diag.width = "minwidth";
        diag.maxbtn = false;
        diag.backdrop = false;
        diag.title = "tips";
        diag.body = reminddiv[0].outerHTML;
        diag.ShowModal();
    }
//会员ajax登录状态需要结合JS/Controls/B_User.js引用同步生效


/*显示搜索框*/
function isSearch(){
    $(".home_nav_search").show();
    $(".home_nav_r").hide();
    $(".home_nav_search .form-control").focus();
}
/*关闭搜索框*/
function isClose(){
    $(".home_nav_search").hide();
    $(".home_nav_r").show();
}
</script>
<script>
    $(function () {
        $(".flash_dp,.outdoor_dp").change(function () {
            var $item = $(this).closest(".cart_item");
            var id = $item.data("id");
            var opname = $(this).data("name");
            var opval = $(this).val();//重新计算更新价格
            $.post("OrderCom.ashx?action=ledchange", { "id": id, "opname": opname, "opval": opval }, function (data) {
                var model = APIResult.getModel(data);
                if (APIResult.isok(model)) {
                    $("#trprice_" + id).text(model.result);
                    UpdateTotalPrice();
                }
            })
        });
    })
</script>
<script runat="server">
    public string GetShopUrl()
    {
        return OrderHelper.GetShopUrl(DataConverter.CLng(Eval("StoreID")), Convert.ToInt32(Eval("ProID")));
    }
    public string GetImgUrl(object o)
    {
        return function.GetImgUrl(o);
    }
    public string GetStockStatus()
    {
        //if (Eval("Allowed").ToString().Equals("0"))
        //{
        //    int pronum = Convert.ToInt32(Eval("Pronum"));
        //    int stock = Convert.ToInt32(Eval("Stock"));
        //    if (stock < pronum)
        //    {
        //        return "<span class='r_red_x'>缺货</span>";
        //    }
        //}
        return "<span class='r_green_x'>In stock</span>";
    }
</script>
</asp:Content>