<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Order_Payed.aspx.cs" Inherits="Common_MailTlp_Order_Payed" EnableViewState="false" %>
<%@ Import Namespace="ZoomLa.Sns" %>
            <table style="font-size:12px;line-height:20px;">
                <tr>
                    <td colspan="3" style="">Hello <%:conMod.FullName %>,</td>
                </tr>
                <tr>
                    <td style="width: 33%;"></td>
                    <td></td>
                    <td style="width: 33%;"></td>
                </tr>
                <tr>
                    <td style="text-indent: 24px;" colspan="3">
                        Your order shipped on <%=dateTime %> and is on its way to you. Below is a recap of the details.
                    </td>
                </tr>


<%--订单开始--%>
    <tr>
        <td colspan="3" style="padding-top: 10px; padding-bottom: 5px;">
            <div style="float: left;">
                <span style="font-weight: bolder;">Order #: </span><span><a href="https://www.raysandsigns.com/BU/Order/OrderListInfo.aspx?OrderNo=<%:orderMod.OrderNo%>" target="_blank"><%:orderMod.OrderNo %></a></span>
            </div>
            <div style="float: right; margin-right: 50px;">
                <span style="font-weight: bolder;">Order Date: </span><span><%=dateTime %></span>
            </div>
            <div style="clear: both;"></div>
        </td>
    </tr>
    <asp:Label runat="server" ID="CartList_HTML"></asp:Label>
    <tr>
        <td colspan="3" style="border-bottom: 1px solid #999; padding-bottom: 10px;"></td>
    </tr>
    <tr>
        <td></td>
        <td style="padding-right: 100px;">
            <div style="text-align: right;">Item Subtotal:</div>
            <div style="text-align: right;">Promotion Applied:</div>
            <div style="text-align: right; font-weight: bolder;">Order Total:</div>
        </td>
        <td>
            <div>$<%:orderMod.Ordersamount.ToString("N") %></div>
            <div>-$<%:payMod.ArriveMoney.ToString("N") %></div>
            <div style="font-weight: bolder;">$<%=payMod.MoneyPay.ToString("N")%></div>
        </td>
    </tr>
<%--订单结束--%>       

<tr>
	<td style="padding-top: 20px; font-size: 14px;">
		<div style="font-weight: bolder;">Shipping Information</div>
		<div><%:conMod.FullName %></div>
		<div><%:conMod.Address %></div>
		<div><%:ExHelper.ShowCityAndState(conMod) %></div>
		<div><%:ExHelper.ShowCountry(conMod) %></div>
		<div><%:conMod.Phone %> </div>
	</td>
	<td></td>
	<td></td>
</tr>
<tr><td><strong>Carrier:</strong> <%:expMod.ExpComp %> Priority Mail Express</td></tr>
<tr><td colspan="3">
		<span style="font-weight: bolder;">Tracking Number:</span> <%:expMod.ExpNo %>
	</td>
</tr>
<tr><td colspan="3" style="padding-top:50px;">
	<div>
		The shipment is dispatched from our factory in China.  It may take a few days to become active in USPS tracking system. 
	</div>
	<div>
		We appreciate your business.
	</div>
</td></tr>
<tr><td colspan="3"><a style="text-decoration: none; font-size: 14px;" href="https://www.raysandsigns.com/" target="_blank">Raysandsigns.com</a></td></tr>
</table>