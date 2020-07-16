<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Return.aspx.cs" Inherits="ZoomLaCMS.PayOnline.PP.Return" MasterPageFile="~/Cart/order.master"%>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>Result</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<%Call.Label("{ZL.Label id=\"模板设计全站头部\"/}");%>
<div runat="server" id="fail_div" visible="false">
     <div class="disgin_banner">
        <h1 style="display:none;">The order has been cancelled</h1>
    </div>
    <div class="container">
        <table class="table table-bordered table-striped" style="margin-top:5px;">
       <tr style="display:none;"><td style="width:100px;display:none;">PayNO</td><td><asp:Label runat="server" ID="Fail_PayNo_L" /></td></tr>
       <tr><td>OrderNO</td><td><asp:Label runat="server" ID="Fail_OrderNo_L" /></td></tr>
       <tr><td>Amount</td><td><asp:Label runat="server" ID="Fail_Amount_L" /></td></tr>
       <tr>
           <td>Result</td>
           <td>
               <a href="/Cart/Cart.aspx?Proclass=1" target="_blank" class="btn btn-info">View cart</a>
               <a href="/Payonline/Orderpay.aspx?Payno=<%:payMod.PayNo %>" target="_blank" class="btn btn-info">Continue to complete the payment</a>
           </td>
       </tr>
       <tr><td></td><td><a href="/" class="btn btn-info"><i class="fa fa-home"></i> Continue Shopping</a></td></tr>
   </table>
    </div>
</div>
<div runat="server" id="success_div">
    <div class="disgin_banner">
        <h1>The order payment is successful.</h1>
    </div>
<div class="container">
   <table class="table table-bordered table-striped" style="margin-top:5px;">
       <tr><td style="width:100px;">PayNO</td><td><asp:Label runat="server" ID="PayNo_L" /></td></tr>
       <tr><td>OrderNO</td><td><asp:Label runat="server" ID="OrderNo_L" /></td></tr>
       <tr><td>Amount</td><td><asp:Label runat="server" ID="Amount_L" /></td></tr>
       <tr><td>Email</td><td><asp:Label runat="server" ID="Email_L" /></td></tr>
       <tr>
           <td>Result</td>
           <td>
               <i class="fa fa-check fa-2x" style="color: green;"></i>
               Please go to the mailbox to check the mail
           </td>
       </tr>
       <tr><td></td><td><a href="/" class="btn btn-info"><i class="fa fa-home"></i> Continue Shopping</a></td></tr>
   </table>
</div> 
</div>   
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">

</asp:Content>