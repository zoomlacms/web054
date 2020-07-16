<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Orderpay.aspx.cs" Inherits="ZoomLaCMS.PayOnline.Orderpay" EnableViewStateMac="false" MasterPageFile="~/Cart/order.master"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<title>RaysAndSigns</title>
<style>
.totalmoney{font-size:1.6em;}
.totinfo_div{display:none;}
.totinfo{color:#ccc;font-size:1.1em; cursor:pointer;}
.totinfo:hover{color:#999;}
.btn{margin-bottom:1rem;}
</style>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<%Call.Label("{ZL.Label id=\"模板设计全站头部\"/}");%>
<div class="disgin_banner">
<h1 hidden>Thank you, looking forward to shopping again for you service.</h1>
</div>
        <div class="container pay_select" style="padding-bottom:0px;">
            <div class="pay_head">
      
                <div style=" padding: 1rem; margin-top: 1rem; background: #ccc;font-size: 1.5rem;"><span class="list_sp" >Order No: </span><asp:Label ID="OrderNo_L" runat="server" ForeColor="Green"></asp:Label></div>
                <div>
                    <span class="list_sp">Amounts payable:</span>
                    <span class="totalmoney"><i class="fa fa-dollar"></i> <asp:Label ID="TotalMoney_L" CssClass="" runat="server"></asp:Label></span>
                    <span class="totinfo"> <i class="fa fa-caret-down"></i></span>
                </div>
                <div class="totinfo_div">
                    <span class="list_sp">Details of the amount:</span>
                    <asp:Label ID="TotalMoneyInfo_T" runat="server" />
                </div>
            </div>
            <div class="PayPlat_s">
                <div class="divline paytitle">Online Payment:</div>
                <ul class="list-unstyled margin_t5">
                    <asp:Repeater runat="server" ID="PayPlat_RPT">
                        <ItemTemplate>
                            <li class="payplat_li plat_item col-lg-3 col-md-4 col-sm-6 col-xs-12 text-left" title="<%#Eval("PayPlatName") %>">
                                <input type="radio" class="payplat_rad" name="payplat_rad" value="<%#Eval("PayPlatID") %>" />
                                <img src="<%#GetPlatImg()%>" class="plat_item_img" />
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>

                    <li class="clearfix"></li>
                </ul>
				<p>Pay via PayPal; you can pay with your credit card if you don’t have a PayPal account.</p>
                <asp:Button CssClass="btn btn-warning btn-lg" runat="server" OnClick="BtnSubmit_Click" Text="Proceed to PayPal"/>
				<a href="/" Class="btn btn-warning btn-lg">Continue Shopping</a>
                <div class="clear"></div>
            </div>
        </div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
<script>
$(function () {
	$(".plat_item_img").click(function () { $(this).siblings(":radio")[0].checked = true; });
	$(".dashpay_ul li").click(function () {
		$(".dashpay_ul .active").removeClass('active');
		$(this).find('input')[0].checked = true;
		$(this).addClass('active');
	});
	if ($(".payplat_rad").length > 0) { $(".payplat_rad:first")[0].checked = true; }
	$(".totinfo").click(function () {
	    var $info = $(".totinfo_div");
	    if ($info.css("display") == "none") {
	        $info.slideDown(200);
	    } else {
	        $info.slideUp(100);
	    }
	});
})
</script>
</asp:Content>