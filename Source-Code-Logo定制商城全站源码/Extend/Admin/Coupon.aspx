<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Coupon.aspx.cs" Inherits="Extend_Admin_Coupon" MasterPageFile="~/Manage/I/Index.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>优惠券</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<%=Call.SetBread(new Bread[] {
new Bread("/Admin/Main.aspx","工作台"),
new Bread() {url=Request.RawUrl,text="优惠券管理",addon="<a href='javascript:;' onclick='showDiag(0);'>[新建优惠券]</a>" }
}) %>
<div style="height:55px;"></div>
<table class="table table-bordered table-striepd">
    <thead>
        <tr>
            <td>优惠券码</td>
            <td>优惠类型</td>
            <td class="td_m">金额/比率</td>
            <td class="td_m">状态</td>
            <td>创建日期</td>
            <td class="td_m">操作</td>
        </tr>
        <ZL:Repeater runat="server" ID="RPT" OnItemCommand="RPT_ItemCommand" PageSize="10"
            PagePre="<tr><td colspan='10' class='text-center'>" PageEnd="</td></tr>">
            <ItemTemplate>
                <tr>
                    <td><%#Eval("Code") %></td>
                    <td><%#Eval("ZType") %></td>
                    <td><%#Eval("AMount","{0:F2}") %></td>
                    <td><%#Eval("ZStatus") %></td>
                    <td><%#Eval("CDate") %></td>
                    <td>
                        <asp:LinkButton runat="server" CommandArgument='<%#Eval("ID") %>' CommandName="del" class="btn btn-danger btn-sm" OnClientClick="return confirm('确定要删除吗?');">删除</asp:LinkButton>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate></FooterTemplate>
        </ZL:Repeater>
    </thead>
</table>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
<script src="/JS/Controls/ZL_Dialog.js"></script>
<script>
    function showDiag(id)
    {
        ShowComDiag("CouponAdd.aspx?ID="+id,"优惠券信息");
    }
    function mybind() { CloseComDiag(); location = location; }
</script>
</asp:Content>