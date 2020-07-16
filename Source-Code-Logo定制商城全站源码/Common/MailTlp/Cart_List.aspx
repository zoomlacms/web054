<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Cart_List.aspx.cs" Inherits="Common_MailTlp_Cart_List" %>
<asp:repeater runat="server" id="RPT" onitemdatabound="RPT_ItemDataBound">
    <ItemTemplate>
        <tr>
            <td colspan="2" style="height: 85px;">
                <img src="<%#GetImage() %>" style="width: 110px; height: 50px; position: absolute;" />
                <div style="margin-left:120px;">
                    <%#Eval("Proname") %>
                    <asp:Label runat="server" ID="Present_HTML" EnableViewState="false"></asp:Label>
                </div>
            </td>
            <td style="vertical-align: bottom;text-align: right;padding-right: 11rem;">$<%#Eval("AllMoney","{0:F2}") %></td>
        </tr>
    </ItemTemplate>
</asp:repeater>