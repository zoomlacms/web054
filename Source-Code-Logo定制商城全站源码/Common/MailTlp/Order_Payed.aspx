<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Order_Payed.aspx.cs" Inherits="Common_MailTlp_Order_Payed" EnableViewState="false" %>
<%@ Import Namespace="ZoomLa.Sns" %>
<table style="font-size: 12px; line-height: 20px;">
    <tr>
        <td colspan="3" style="">Hello <%:conMod.FullName %>,</td>
    </tr>
    <tr>
        <td style="width: 33%;"></td>
        <td></td>
        <td style="width: 33%;"></td>
    </tr>
    <tr>
        <td style="text-indent: 24px;" colspan="3">Thank you for shopping at raysandsigns.com. Your order should be completed in 2 weeks.  We’ll send you a confirmation once your order ships. If you would like to check the status of your order or make any changes to it, please contact us at sales@raysandsigns.com, or chat with us online. Below is your order confirmation. Please keep a copy for your records.
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
    <tr style="text-align: right;">
        <td></td>
        <td style="padding-right: 1rem;">
            <div style="text-align: right;">Item Subtotal:</div>
            <div style="text-align: right;">Promotion Applied:</div>
            <div style="text-align: right; font-weight: bolder;">Order Total:</div>
        </td>
        <td class="mailr">
            <div>$<%:orderMod.Ordersamount.ToString("N") %></div>
            <div>-$<%:payMod.ArriveMoney.ToString("N") %></div>
            <div style="font-weight: bolder;">$<%=payMod.MoneyPay.ToString("N")%></div>
        </td>
    </tr>
<%--订单结束--%>
    <tr>
        <td style="padding-top: 20px; font-size: 14px;vertical-align: top;">
            <div style="font-weight: bolder;">Billing Information</div>
            <div>Paypal</div>
            <div><%:conMod.FullName %></div>
            <div><%:conMod.Address %></div>
            <div><%:ExHelper.ShowCityAndState(conMod) %></div>
            <div><%:ExHelper.ShowCountry(conMod) %></div>
            <div><%:conMod.Phone %> </div>
        </td>
        <td></td>
        <td style="padding-top: 20px; font-size: 14px;vertical-align: top;">
            <div style="font-weight: bolder;">Shipping Information</div>
            <div>As selected in your Paypal account.</div>
        </td>
    </tr>
    <tr>
        <td colspan="3" style="padding-top: 50px; font-size: 14px;">Thank you for shopping with us.</td>
    </tr>
    <tr>
        <td colspan="3"><a style="text-decoration: none; font-size: 14px;" href="https://www.raysandsigns.com/" target="_blank">Raysandsigns.com</a></td>
    </tr>
</table>
<style type="text/css">
.mailr{padding-right: 11rem;}
@media screen and (max-width: 768px) { 
.mailr{padding-right: auto;}}

</style>