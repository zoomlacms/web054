<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CouponAdd.aspx.cs" Inherits="Extend_Admin_CouponAdd" MasterPageFile="~/Manage/I/Index.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>优惠券管理</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
  <style type="text/css">
  .chatButtonContainer {display:none;}
  </style>
<table class="table table-bordered table-striepd">
    <tr><td class="td_m">优惠券码</td><td><ZL:TextBox runat="server" ID="Code_T" AllowEmpty="false" class="form-control text_300"/></td></tr>
    <tr><td>优惠类型</td><td>
        <label><input type="radio" name="ztype" value="金额" checked="checked"/>金额</label>
        <label><input type="radio" name="ztype" value="比率"/>比率</label>
    </td></tr>
    <tr><td>优惠额度</td>
        <td>
            <ZL:TextBox runat="server" ID="Amount_T" CssClass="form-control text_300" AllowEmpty="false" ValidType="FloatPostive" Text="0"/>
            <div><small>请输入优惠金额,或优惠比率:0.01-0.99,(示例计算:100元的商品,优惠比率0.09,则会减免9元)</small></div>
        </td>
    </tr>
    <tr><td>状态</td><td>
        <label><input type="checkbox" runat="server" id="status_chk" value="1" checked="checked"/>已启用</label>
                   </td></tr>
    <tr><td></td>
        <td>
            <asp:Button runat="server" ID="Save" OnClick="Save_Click" Text="保存信息" class="btn btn-info"/>
        </td>
    </tr>
</table>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">

</asp:Content>