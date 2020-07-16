<%@ Page Language="C#" AutoEventWireup="true" CodeFile="getOrderInfo.aspx.cs" Inherits="ZoomLaCMS.Cart.getOrderInfo"  MasterPageFile="~/Cart/order.master" EnableViewStateMac="true"%>
<%@ Import Namespace="ZoomLa.Common" %>
<%@ Import Namespace="ZoomLa.BLL.Shop" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
<script src="/JS/Controls/ZL_Dialog.js"></script>
<script src="/JS/ICMS/ZL_Common.js"></script>
<title>RaysAndSigns</title>
<style type="text/css">
.width960{width:960px;}
.modal-content {width:960px;}
.arsbtn {color:#ddd;border-color:#ddd;}
.arsbtn:hover { color:#ddd; border-color:#ddd; background:none;}
.arsbtn .fa{display:none;}
.arsbtn.checked{border:1px solid rgb(70, 184, 218);color:rgb(70, 184, 218); background:none;}
.arsbtn.checked .fa{display:inline-block;color:rgb(70, 184, 218);}
.orderbody .suit_item{background:#fff4e8;}
.orderbody .tdtext { line-height: 50px;text-align:center; }
.preimg_m{margin-right:0.4rem;max-width: 10rem;}
.error{color:red;}
</style>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<%Call.Label("{ZL.Label id=\"模板设计全站头部\"/}");%>

<div class="disgin_banner">
<h1 hidden>It's finished.</h1>
</div>

<div class="container">
<div class="gray_border">
    <div class="bordered">
        <div class="alert alert-info" style="margin-top:5px;">
            <i class="fa fa-th strong"> Please enter your billing address. You will select shipping address when you make payment with PayPal.</i>
        </div>
        <div class="indent">
            <table class="table table-bordered table-striped">
                <tr><td style="width:200px;"><strong>Full Name:</strong></td>
                    <td><input type="text" class="form-control text_300 required" id="fullname" name="fullname" title="Please provide your full name"/></td>
                </tr>
                <tr>
                    <td><strong>Email:</strong></td>
                    <td>
                        <input type="text" class="form-control text_300 required email" id="email" name="email" title="Please provide your email"/></td>
                </tr>
                <tr>
                    <td><strong>Address:</strong></td>
                    <td>
                        <input type="text" class="form-control text_300 required" id="address" name="address"/></td>
                </tr>
                <tr>
                    <td><strong>City:</strong></td>
                    <td>
                        <input type="text" class="form-control text_300 required" id="city" name="city"/></td>
                </tr>
                <tr>
                    <td><strong>State/Province:</strong></td>
                    <td>
                        <select class="form-control text_300" id="state" name="state" size="1"></select>
                  <%--      <span>Tax Rate:</span>
                        <span style="color: green;" id="taxRate_sp">0</span>
                        <span>%</span>--%>
                     </td>
                </tr>
                <tr>
                    <td><strong>Country:</strong></td>
                    <td>
                        <select class="form-control text_300" name="country" id="country" size="1" onchange="Grade.load('#state',this.value);">
                        </select>
                        <script>
                            var Grade = {};
                            Grade.load = function (id, pid,cb) {
                                $.post("orderCom.ashx?action=grade", { "pid":pid  }, function (data) {
                                    var model = APIResult.getModel(data);
                                    if (APIResult.getModel(model)) {
                                        var tlp = '<option value="@GradeID" data-gid="@GradeID">@GradeName</option>';
                                        var items = JsonHelper.FillItem(tlp, model.result);
                                        $(id).html("");
                                        $(id).append(items);
                                        if (cb) { cb(data); }
                                    }
                                    else { console.log("failed", model.retmsg); }
                                })
                            }
                            Grade.checkTaxRate = function (id) {
                                if (id == "557") { $("#taxRate_sp").text(6); }
                                else { $("#taxRate_sp").text(0); }
                                UpdateTotalPrice();
                            }
                            $(function () {
                                Grade.load("#country", 0, function () {
                                    Grade.load("#state", $("#country").val());
                                });
                            })
                        </script>
                    </td>
                </tr>
                <tr>
                    <td><strong>ZIP/Postal Code:</strong></td>
                    <td>
                        <input type="text" class="form-control text_300 required us-zip" id="zip" name="zip" maxlength="15"/></td>
                </tr>
                <tr>
                    <td><strong>Phone:</strong></td>
                    <td>
                        <input type="text" class="form-control text_300" maxlength="11" id="phone" name="phone"/></td>
                </tr>
                <tr>
                    <td><strong>Additional Information:</strong></td>
                    <td>
                        <textarea class="form-control m715-50" rows="3" id="orderMsg" name="orderMsg"></textarea></td>
                </tr>
                <tr>
                    <td><strong>Terms and conditions:</strong></td>
                    <td>
                        <label style="display:block;font-weight:bolder;margin-bottom:0px;"><input type="checkbox" class="required" id="agree_chk" name="agree_chk" value="1" title="It is necessary to agree with the agreement"/>Please click here if you are in acceptance of our terms and conditions.</label>
                        <small>To review our terms and conditions, please <a href="javascript:;" onclick="showTerms();" style="color:#0094ff;" title="">click here</a></small>
                        <label for="agree_chk" style="display:block;" class="error"></label>
                        <label style="display:block;font-weight:bolder;margin-bottom:0px;"><input type="checkbox" id="agree_save_chk" name="agree_save_chk" value="1"/>Remember me.</label>
                        <small>Use this option to write a cookie to allow us to prepopulate this form the next time you visit.</small>
                        <script>
                            function showTerms() {
                                comdiag.width = "width960";
                                ShowComDiag("/Cart/Comp/Terms.aspx", "");
                            }
                        </script>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="bordered">
        <p><i class="fa fa-cubes strong">Product List</i>
            <a runat="server" id="ReUrl_A1" visible="false" class="pull-right padding_r10">Return to cart</a>
        </p>
        <div class="indent orderbody">
            <table class="table text-center">
            <tr style="background:#eeeeee;"><td>Name</td><td>Total</td><td>Discount</td><td>Quantity</td></tr>
                <asp:Repeater runat="server" ID="Store_RPT" OnItemDataBound="Store_RPT_ItemDataBound" EnableViewState="false">
                    <ItemTemplate>
                        <tbody style="border: none;">
                                 <asp:Repeater runat="server" ID="ProRPT" EnableViewState="false" OnItemDataBound="ProRPT_ItemDataBound">
                                            <ItemTemplate>
                                                    <tr>
                                                        <td class="text-left">
                                                            <div class="pull-left">
                                                                <a href="<%#GetShopUrl() %>" target="_blank" title="浏览商品">
                                                                    <img src="<%#GetShopImg(Eval("ProAttr"))%>" onerror="this.src='<%#function.GetImgUrl(Eval("Thumbnails"))%>'" class="preimg_m" /></a>
                                                            </div>
                                                            <div class="padding_l55 tdtext" style="text-align:left;">
                                                                <a href="<%#GetShopUrl() %>" target="_blank" title="浏览商品"><span><%#Eval("ProName") %></span></a>
                                                            </div>
                                                            <div>
                                                                 <asp:Label runat="server" ID="Present_HTML"></asp:Label>
                                                            </div>
                                                        </td>
                                                        <td class="tdtext">
                                                            <i class="fa fa-dollar"><%#Eval("AllMoney","{0:F2}") %></i>
                                                        </td>
                                                        <td class="tdtext r_red"><%#GetDisCount()%></td>
                                                        <td class="tdtext">x <%#Eval("Pronum") %></td>
                                                    </tr>
                                            </ItemTemplate>
                                    </asp:Repeater>
                               <tr style="display:none;"><td colspan="6" class="text-right">
                                       <span>:</span>
                                   <asp:Literal runat="server" ID="FareType_L" EnableViewState="false"></asp:Literal>
                                   </td></tr>
                             </tbody>
                    </ItemTemplate>
                </asp:Repeater>
            <tr style="display:none;">
                <td colspan="5">
                    <div class="text-right total_count_div">
                        <div style="display:none;"><span><span runat="server" id="itemnum_span" class="r_red_x"></span>:</span><i class="fa fa-dollar" runat="server" id="totalmoney_span1">0.00</i>
                        </div>
                     
                        <div style="display:none;"><span>:</span><i class="fa fa-dollar" id="point_money_sp">0.00</i></div>
                        <div><span>:</span><i class="fa fa-dollar" id="fare_span">0.00</i></div>
                        <div class="pay_moneyAll" style="font-size:24px;"><span>Total:</span><i class="fa fa-dollar" runat="server" id="totalmoney_span2">0.00</i></div>
                    </div>
                </td>
            </tr>
        </table>
            <ul class="extend_ul" runat="server" visible="false">
                <li>
                    <div><a href="javascript:;" onclick="$('#arrive_div').toggle();"><i class="fa fa-plus-circle"> Arrive</i></a></div>
                    <asp:Literal runat="server" ID="Arrive_Lit" EnableViewState="false"></asp:Literal>
                </li>
                <li id="point_li">
                    <div><a href="javascript:;" onclick="$('.point_div').toggle();"><i class="fa fa-plus-circle">Point</i></a></div>
                    <div id="point_body" runat="server" visible="false" class="extenddiv point_div">
                        <asp:Label ID="Point_L" runat="server"></asp:Label><span id="usepoint_span" class="r_red"></span><asp:TextBox runat="server" ID="Point_T" Text="0" onkeyup="checkMyPoint(this);" CssClass="form-control text_150 num"/>
                    </div>
                    <div id="point_tips" runat="server" visible="false" class="alert alert-warning point_div extenddiv" role="alert">
                        <i class="fa fa-exclamation-circle"></i> Closed!
                    </div>
                </li>
                <li>
                   <div><a href="javascript:;" onclick="$('#oremind_div').toggle();"><i class="fa fa-plus-circle"> </i></a></div>
                    <div id="oremind_div" class="extenddiv">
                        <p></p>
                        <asp:TextBox runat="server" ID="ORemind_T" CssClass="form-control max" MaxLength="100" placeholder="" />
                    </div>
               </li>
            </ul>
        </div>
    </div>
    <div style="border-top:1px solid #ddd;border-bottom:1px solid #ddd;padding:5px 0;">
        <div class="input-group">
            <span class="input-group-prepend"><span class="input-group-text">Coupon</span></span>
            <asp:TextBox runat="server" ID="Coupon_Num" class="form-control" placeholder="Coupon Number" />
            <asp:TextBox runat="server" ID="Coupon_Pwd" class="form-control" Style="width: 200px;display:none;" placeholder="Coupon Password" />
            <span class="input-group-append">
                <button type="button" class="btn btn-info" onclick="coupon.use();">Use</button>
                <button type="button" class="btn btn-danger" onclick="coupon.cancel();">Cancel</button>
            </span>
        </div>
        <div id="Coupon_Tip" style="color:red;"></div>
    </div>
    <div class="total_div">
        <div><span>Coupon:</span><i class="fa fa-dollar" id="arrive_money_sp" style="color:red;">0.00</i></div>
        <div>
            <span class="total" style="font-size: 22px">Total:<i runat="server" id="totalmoney_i" class="fa fa-dollar">0.00</i></span>
            <asp:Button runat="server" style="margin-left:30px;position:relative;top:-5px;cursor:pointer;" class="btn btn-danger" ID="AddOrder_Btn" Text="CHECKOUT" OnClientClick="return subCheck();" OnClick="AddOrder_Btn_Click" />
        </div>
    </div>
</div>
<asp:HiddenField ID="PointRate_Hid" runat="server" />
</div>
<%Call.Label("{ZL.Label id=\"全站底部\"/}");%>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
<script src="/JS/Modal/APIResult.js"></script>
<script src="/JS/jquery.validate.min.js?v=2"></script>
<script src="/JS/Controls/ZL_Array.js"></script>
<script src="/JS/ZL_Regex.js?v=2"></script>
<script>
    $(function () {
        $(".methodul li").click(function () {
            $(this).siblings().removeClass("active");
            $(this).addClass("active");
            $(this).find(":radio")[0].checked = true;
        })
        $(".methodul li:first").click();
        //
        $(".invoice_item_rad").click(function () {
            $("#InvoTitle_T").val($(this).data("head"));
            $("#Invoice_T").val($(this).val());
        });
        $(".invoice_item_rad:first").click();
        //
        $(".fare_select").change(function () {
            UpdateTotalPrice();
        });
        //arrive.init();
        exp.init();
        UpdateTotalPrice();
        coupon.init();
        ZL_Regex.B_Num(".num");
        IsDisBtn();
        var agree = getCookie("agree_save");
        if (agree && agree == "1") {
            document.getElementById("agree_chk").checked = true;
            document.getElementById("agree_save_chk").checked = true;
        }
    })
    var coupon = {
        result: null,
        $tip: $("#Coupon_Tip"),
        $code: $("#Coupon_Num"),
        $pwd: $("#Coupon_Pwd"),
        money: 0,
        init: function () {
            this.money = $("#totalmoney_i").text();
        },
        use: function () {
            var ref = this;
            ref.$tip.text("");
            //----------------------------
            var packet = {
                code: ref.$code.val(),
                pwd: ref.$pwd.val(),
                money: ref.money
            };
            $.post("/Cart/Ordercom.ashx?action=coupon", packet, function (data) {
                var model = APIResult.getModel(data);
                if (APIResult.isok(model)) {
                    //notify to update
                    $("#arrive_money_sp").text("-" + Convert.ToMoneyStr(model.result.amount));
                    UpdateTotalPrice();
                }
                else { ref.$tip.text(model.retmsg); }
            })
        },
        cancel: function () {
            var ref = this;
            ref.$tip.text("");
            ref.$code.val("");
            ref.$pwd.val("");
            $("#arrive_money_sp").text("-0.00");
            UpdateTotalPrice();
        }
    };
    var diag = new ZL_Dialog();
    diag.width = "minwidth";
    diag.maxbtn = false;
    function AddAddress() {
        diag.title = "newaddress";
        //diag.url = "address.aspx";
        diag.ShowModal();
    }
    function EditAddress(id) {
        diag.title = "edit";
        diag.url = "address.aspx?id=" + id;
        diag.ShowModal();
    }
    function DelAddress(myid) {
        if (confirm("confirm delete")) {
            $("#addli_" + myid).remove();
            exp.addressDefChk();
            $.post("ordercom.ashx", { action: "deladdress", id: myid }, function () {});
        }
    }
    function SelInvo(dp) {
        if ($(dp).val() != "") {
            $("#InvoTitle_T").val($(dp).find(":selected").text());
            $("#Invoice_T").val($(dp).val());
        }
    }
    //
    function UpdateTotalPrice() {
        var total = parseFloat($("#totalmoney_span1").text());
        var arrive = parseFloat($("#arrive_money_sp").text());
        var point = parseFloat($("#point_money_sp").text());
        var fare = 0;
        //
        $(".fare_select").each(function () {
            fare += parseFloat($(this).find("option:selected").data("price"));
        });
        total = (total + arrive + fare - point);
        total = total > 0 ? total : 0;
        var taxRate = Convert.ToInt($("#taxRate_sp").text());
        if (taxRate > 0) { total += (total * taxRate*0.01); }
        $("#fare_span").text(fare.toFixed(2));
        $("#totalmoney_span2").text(Convert.ToMoneyStr(total));
        $("#totalmoney_i").text(Convert.ToMoneyStr(total));
        $("#totalPurse_sp").text(GetSumByFilter(".purse_sp"));
        $("#totalSicon_sp").text(GetSumByFilter(".sicon_sp"));
        $("#totalPoint_sp").text(GetSumByFilter(".point_sp"));
    }
    //
    function SumByPoint(usepoint) {
        var point = parseFloat($("#Point_L").text());
        if (usepoint > point) { usepoint = point; };
        $("#usepoint_span").text(usepoint);
        $("#Point_T").change(function () {
            var point = Convert.ToDouble(this.value);
            if (point > usepoint) { point = usepoint; }
            this.value = point;
        });
    }
    function GetSumByFilter(filter) {
        var total = 0.00;
        $(filter).each(function () {
            var price = parseFloat($(this).text());
            if (price) total += price;
        });
        return Convert.ToMoneyStr(total);
    }
    //
    function IsDisBtn() {
        return true;
    }
    //
    function subCheck() {
        return true;
    }
    function checkMyPoint(obj) {
        if (isNaN(parseFloat($(obj).val()))) { $(obj).val("0"); }
        var val = parseFloat($(obj).val());
        var usepoint = parseFloat($("#usepoint_span").text());//可用积分
        if (usepoint <= val) { val = usepoint; };
        var pointrate = parseFloat($("#PointRate_Hid").val());
        $("#point_money_sp").text(Convert.ToMoneyStr((val * pointrate)));
        UpdateTotalPrice();
    }
    function closeDiag() { diag.CloseModal(); CloseComDiag(); }
    //--------------
    var exp = {};
    exp.init = function () {
        $("#addressul").load("/cart/comp/AddressList.aspx", {}, function () {
            exp.addressDefChk();          
        });
    }
    //
    exp.addressDefChk = function () {
        if ($(".addresssul li").length > 0) {
            $(".addresssul li").click(function () {
                $(this).siblings().removeClass("active");
                $(this).addClass("active");
                $(this).find("input:radio")[0].checked = true;
                //
                $(".arsbtn").removeClass("checked");
                $("#arsInfo_wrap").hide();
                $("#arsbtn_chk")[0].checked = false;
            });
            $(".addresssul li:first").click(); $(":radio[name=address_rad]")[0].checked = true;
        }
        IsDisBtn();
    }
    //
    exp.addressBack = function () {
        diag.CloseModal(); $("#addressul").load("/cart/comp/AddressList.aspx", {}, function () {
            exp.addressDefChk();
        });
    }
    //
    exp.arsSelf = function (btn) {
        $(".addresssul li").removeClass("active");
        $(".addresssul li input:radio").each(function () { this.checked = false; });
        $(btn).addClass("checked");
        $("#arsInfo_wrap").show();
        $("#arsbtn_chk")[0].checked = true;
    }
    exp.showExpTime = function () {
        diag.title = "time";
        //diag.url = "/cart/comp/exptime.aspx";
        diag.ShowModal();
    }
    exp.expTimeBack = function (json) {
        $("#exptime_sp").text(json.txt);
        $("#exptime_hid").val(json.txt);
        //$("#exptime_hid").data("json", JSON.stringify(json));
    }
    //--------------
    var arrive = {};
    arrive.init = function () {
        $("#arrive_active_ul .item").click(function () {
            var $this = $(this);
            if ($this.hasClass("choose")) {//
                $this.removeClass("choose");
                arrive.use("");
            }
            else {
                $(".arrive_o .item").removeClass("choose");
                $this.addClass("choose");
                arrive.use($this.data("flow"));
            }
        });
    }
    arrive.use = function (flow) {

    }
//
var buser = new B_User();
buser.IsLogged(function(data,flag){
	if(flag)
	{
		data=JSON.parse(data);
		$(".home_nav .home_nav_l_login").hide();
		$(".home_nav .home_nav_l_logout").show();
        $(".home_nav .home_nav_l_logout a:nth-child(1)").html("Welcome,"+data.UserName+"!");
        
	}else{
        $(".home_nav .home_nav_l_login").show();
		$(".home_nav .home_nav_l_logout").hide();
    }
});
function LogoutFun()
{
	buser.Logout(function(){location=location;});
}


/**/
function isSearch(){
    $(".home_nav_search").show();
    $(".home_nav_r").hide();
    $(".home_nav_search .form-control").focus();
}
/**/
function isClose(){
    $(".home_nav_search").hide();
    $(".home_nav_r").show();
}
$.validator.addMethod("us-zip", function (value, element) {
    //22162||22162-1010
    var patrn = /^\d{5}(?:-\d{4})?$/;
    //M4Y 1M7
    var state = $("#state").val();
    var checkCandaZip=function(value) {
        var isChar = function (value) {
            //"D,F,I,O,Q,W,Z"
            var reg = /^[A-Za-z]+$/;
            if (!reg.test(value)) { return false; }
            else { return true; }
        }
        var isInt = function (value) {
            var reg = /[0-9]/;
            return reg.test(value);
        }
        //Canada=A9B 9C9 
        if (!value || value.length != 7) { return false; }
        if (isChar(value[0]) && isInt(value[1])
            && isChar(value[2]) && value[3] == " "
            && isInt(value[4]) && isChar(value[5]) && isInt(value[6])) { return true; }
        else { return false; }
        //isChar(value[0]) && isInt(value[1])&& isChar(value[2]) && value[3] == " "&& isInt(value[4]) && isChar(value[5]) && isInt(value[6])
    }
    return patrn.exec(value)||checkCandaZip(value);
}, "Please provide a valid zipcode.");
$.validator.addMethod("usphone", function (value) {
    if (ZL_Regex.isEmpty(value)) { return true; }
    var patrn = /^([0-9]|[\+]|[\-]){10}$/;
    return patrn.exec(value) ? true : false;
    return true;
}, "Please check your phone number.");

$(function () {
    $("form").validate({
        onkeyup: false,
        rules: {
            phone: {required: true, usphone: true }
        },
        messages: {},
    });
})
function testInfo()
{
    $("#fullname").val("John Wilson");
    $("#email").val("469582963@qq.com");
    $("#address").val("490 Winterfall st,Suite 300");
    $("#address").val("490 Winterfall st,Suite 300");
    $("#city").val("Melbourne");
    $("#zip").val("94107");
    $("#phone").val("1885055888");
    $("#orderMsg").val("Hello World!!");
}
</script>
<script runat="server">
    public string GetShopUrl()
    {
        return OrderHelper.GetShopUrl(DataConverter.CLng(Eval("StoreID")), Convert.ToInt32(Eval("ProID")));
    }
    public string GetStockStatus()
    {
        return "<span class='r_green_x'></span>";
    }
    //
    public string GetDisCount()
    {
        //return "- " + (Convert.ToDouble(Eval("AllIntegral")) - Convert.ToDouble(Eval("AllMoney"))).ToString("f2");
        return "0.00";
    }
    //
    public string getInvoText(int flag)
    {
        return flag == 0 ? Regex.Split(Eval("Invoice").ToString(), Regex.Escape("||"))[0] : Regex.Split(Eval("Invoice").ToString(), Regex.Escape("||"))[1];
    }
</script>
</asp:Content>