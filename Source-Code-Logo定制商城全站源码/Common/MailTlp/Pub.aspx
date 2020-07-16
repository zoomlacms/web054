<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Pub.aspx.cs" Inherits="Common_MailTlp_Pub" EnableViewState="false" %>
<form id="form1" runat="server">
    <table style="width:100%;line-height:25px;">
        <tr><td colspan="2">Dear <%:ShowVal("PubTitle") %><br/></td></tr>
        <tr><td colspan="2">Thank you for the request. We are working on it. You will receive quote and design within 24 hours. Below is the detail of your request. <br/></td></tr>
        <tr><td colspan="2" style="border-bottom:2px double #ddd;padding-top:5px;"></td></tr>
        <tr><td style="padding-top:5px;">Your name</td><td><%:ShowVal("PubTitle") %></td></tr>
        <tr><td style="width:200px;">Email</td><td><%:ShowVal("Email") %></td></tr>
        <tr><td>Phone number</td><td><%:ShowVal("phone") %></td></tr>
        <tr><td>Sign Type</td><td><%:ShowVal("fhlx") %></td></tr>
        <tr><td>Sign Size</td><td><%:ShowVal("size") %></td></tr>
        <tr><td>Sign Text</td><td><%:ShowVal("bpwz") %></td></tr>
        <tr><td>Color</td><td><%:ShowVal("Color")+" " %></td></tr>
        <tr><td>Font</td><td><%:ShowVal("Font") %></td></tr>
        <tr><td>Border</td><td><%:ShowVal("Border") %></td></tr>
        <tr><td>Flash</td><td><%:ShowVal("flash") %></td></tr>
        <tr><td>Additional instructions</td><td><%:ShowVal("orther") %></td></tr>
      
        <tr><td colspan="2" style="border-bottom:2px double #ddd;padding-top:5px;padding-bottom:10px;"></td></tr>
        <tr><td colspan="2" style="padding-top:5px;"><br/>Should you need to send us additional information and request, please contact us at <a href="https://www.raysandsigns.com" target="_blank">sales@raysandsigns.com.</a></td></tr>
        <tr><td colspan="2"><br/>Cheers,</td></tr>
        <tr><td colspan="2">raysandsigns.com</td></tr>
    </table>
</form>
