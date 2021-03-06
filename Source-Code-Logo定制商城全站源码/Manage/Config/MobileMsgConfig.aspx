﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MobileMsgConfig.aspx.cs"  MasterPageFile="~/Manage/I/Index.Master" Inherits="ZoomLaCMS.Manage.Config.MobileMsgConfig" %>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>短信配置</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
    <%=Call.SetBread(new Bread[] {
        new Bread("/{manage}/I/Main.aspx","工作台"),
        new Bread("SiteInfo.aspx","网站配置"),
        new Bread() {url="", text="短信配置",addon="" }},
		Call.GetHelp(4)
		)
    %>
	
<div class="container-fluid pr-0">
<div class="row sysRow list_choice">
<table class="table table-bordered sys_table">
<tr>
    <th class=" w12rem_lg" scope="row"><%:lang.LF("选择默认短信接口") %><span class="rd_red">*</span></th>
    <td>
        <asp:DropDownList ID="ddlMessageCheck_DP" runat="server" CssClass="form-control max20rem" OnSelectedIndexChanged="ddlMessageCheck_SelectedIndexChanged" AutoPostBack="True">
            <asp:ListItem Value="0">关闭</asp:ListItem>
            <asp:ListItem Value="1">深圳全网(推荐)</asp:ListItem>
            <asp:ListItem Value="2">北京东时方</asp:ListItem>
            <asp:ListItem Value="3">亿美软通</asp:ListItem>
            <asp:ListItem Value="qcloud">腾迅云短信</asp:ListItem>
      <%--      <asp:ListItem Value="aliyun">阿里云短信</asp:ListItem>--%>
        </asp:DropDownList>
	</td>
</tr>
<tbody runat="server" id="ShenZhen_P" visible="false">
    <tr>
        <th scope="row"><%:lang.LF("短信通用户") %><span class="rd_red">*</span></th>
        <td>
            <asp:TextBox ID="TxtMssUser" runat="server" class="form-control max20rem"/>
		</td>
    </tr>
    <tr>
        <th scope="row"><%:lang.LF("短信通密码") %><span class="rd_red">*</span></th>
        <td>
            <asp:TextBox ID="TxtMssPsw" runat="server" TextMode="Password" class="form-control max20rem"/>
		</td>
    </tr>
<%--    <tr>
        <td colspan="2">
            <a href="http://www.z01.com/server" target="view_window">没有帐号?点此联系官方授权短信通>></a>
        </td>
    </tr>--%>
</tbody>
<tbody runat="server" id="East_P">
    <tr>
        <th  class="td_s" scope="row"><%:lang.LF("东时方企业号") %><span class="rd_red">* </span></td></th>
        <td>
            <asp:TextBox ID="txtg_eid" runat="server" class="form-control max20rem"/>
    </tr>
    <tr>
        <th scope="row"><%:lang.LF("东时方用户") %><span class="rd_red">*</span></th>
        <td>
            <asp:TextBox ID="txtg_uid" runat="server" class="form-control max20rem"/>
		</td>
    </tr>
    <tr>
        <th scope="row"><%:lang.LF("东时方密码") %><span class="rd_red">*</span></th>
        <td>
            <asp:TextBox ID="txtg_pwd" runat="server" TextMode="Password" class="form-control max20rem"/>
		</td>
    </tr>
    <tr>
        <th scope="row"><%:lang.LF("东时方通道") %><span class="rd_red">*</span></th>
        <td>
            <asp:TextBox ID="txt_h_gate_id" runat="server" class="form-control max20rem"/>
		</td>
    </tr>
<%--    <tr>
        <td colspan="2"><a href="http://www.xhsms.com/" target="view_window">东时方短信接口申请>></a></td>
    </tr>--%>
</tbody>
<tbody runat="server" id="YiMei_P" visible="false">
    <tr>
        <th scope="row"><%:lang.LF("序列号") %><span class="rd_red">*</span></th>
        <td>
            <asp:TextBox ID="smskeyT" runat="server" class="form-control max20rem" />
		</td>
    </tr>
    <tr>
        <th scope="row">密码<span class="rd_red">* </span></th>
        <td>
            <asp:TextBox ID="smspwdT" runat="server" class="form-control max20rem" TextMode="Password" />
		</td>
    </tr>
<%--       <tr>
        <td colspan="2"><strong><a href="http://www.emay.cn/" target="view_window">亿美软通接口申请>></a></strong></td>
    </tr>--%>
</tbody>
<tbody runat="server" id="QCloud_P" visible="false">
    <tr><th scope="row">APPID</th><td><asp:TextBox runat="server" ID="QC_APPID" class="form-control max20rem"/></td></tr>
    <tr><th scope="row">APPKey</th><td><asp:TextBox runat="server" ID="QC_APPKey" class="form-control max20rem"/></td></tr>
    <tr><th scope="row">签名</th><td><asp:TextBox runat="server" ID="QC_Sign" class="form-control max20rem"/></td></tr>
</tbody>
<tr><td colspan="2" class="text-center"><strong>短信模板/模板ID</strong></td></tr>
    <tr>
        <th scope="row">注册验证码文本</th>
        <td>
            <asp:TextBox runat="server" ID="Tlp_Reg" class="form-control max20rem" MaxLength="100" />
            <small>示例:您的校验码:{vcode}.或填入云端模板ID</small>
        </td>
    </tr>
     <tr>
        <th scope="row">找回密码文本</th>
        <td>
            <asp:TextBox runat="server" ID="Tlp_GetBack" class="form-control max20rem" MaxLength="100" />
            <small>示例:你正在使用找回密码服务,校验码:{code}</small>
        </td>
    </tr>
    <tr>
        <th scope="row">修改手机号文本</th>
        <td>
            <asp:TextBox runat="server" ID="Tlp_ChangeMobile" class="form-control max20rem" MaxLength="100" />
            <small>示例:你正在使用手机号码修改服务,校验码:{code}</small>
        </td>
    </tr>
<tr><td colspan="2" class="text-center"><strong>通用配置</strong></td></tr>
<tr>
    <th scope="row">每日手机号发送次数</th>
    <td>
        <asp:TextBox ID="MaxPhoneMsg" runat="server" Text="10" CssClass="form-control max20rem"/>
		<small id="MaxPhoneMsgtxt" class="text-muted">为0则不按手机号限定次数</small>
    </td>
</tr>
<tr>
    <th scope="row">每日IP发送次数</th>
    <td>
        <asp:TextBox ID="MaxIpMsg" runat="server" Text="50" CssClass="form-control  max20rem"/>
		<small class="text-muted">为0则不按IP限定次数</small>
    </td>
</tr>
<tr>
<th scope="row">会员变更手机号权限</th>
<td>
<asp:RadioButtonList runat="server" ID="userMobilAuth" RepeatDirection="Horizontal">
	<asp:ListItem Value="0">短信验证</asp:ListItem>
	<asp:ListItem Value="1" Selected="True">自由修改</asp:ListItem>
</asp:RadioButtonList></td>
</tr>
<tr><td></td><td><asp:Button runat="server" ID="Save_Btn" Text="保存配置" OnClick="Save_Btn_Click" CssClass="btn btn-outline-info" /></td></tr>
</table>
</div>
</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
    <script src="/JS/ZL_Regex.js"></script>
    <script>
        $(function () {
            ZL_Regex.B_Num('.num');
        });
    </script>
</asp:Content>