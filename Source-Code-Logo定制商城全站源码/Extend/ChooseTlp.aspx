<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChooseTlp.aspx.cs" Inherits="Extend_ChooseTlp" %>
<%--<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">--%>

        <ul id="tlp_ul">
            <asp:Repeater runat="server" ID="Tlp_RPT">
                <ItemTemplate>
                    <li class="tlp_item" onclick="logo.setItem(this);" data-id="<%#Eval("ID") %>" data-alias="<%#Eval("Alias") %>">
                        <img src="<%#Eval("PreViewImg") %>"/>
                        <div class="tlp_alias"><%#Eval("Alias") %></div>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
            <li class="clearfix"></li>
        </ul>
<%--    </form>
</body>
</html>--%>
