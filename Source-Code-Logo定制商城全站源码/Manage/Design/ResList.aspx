﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResList.aspx.cs" Inherits="ZoomLaCMS.Manage.ResList" MasterPageFile="~/Manage/I/Index.master" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
    <title>资源管理</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<ol id="BreadNav" class="breadcrumb navbar-fixed-top">
    <li class="breadcrumb-item"><a href='<%=CustomerPageAction.customPath2 %>Main.aspx'><%=Resources.L.工作台 %></a></li>
    <li class="breadcrumb-item"><a href='Default.aspx'>动力模块</a></li>
    <li class="breadcrumb-item">资源管理 <a href='AddRes.aspx'>[上传资源]</a></li>
    <div id="help" class="pull-right text-center"><a href="javascript::" id="sel_btn" class="help_btn"><i class="zi zi_search"></i></a></div>
    <div id="sel_box" runat="server">
		<div class="d-flex flex-row">
        <div>
            <asp:RadioButtonList ID="Useage_Rad" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Text="全部" Value=""></asp:ListItem>
                <asp:ListItem Text="动力模块" Value="bk_pc"></asp:ListItem>
                <asp:ListItem Text="H5场景" Value="bk_h5"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <div>
            <div class="input-group">
                <asp:TextBox ID="Skey_T" placeholder="资源名称" runat="server" CssClass="form-control max20rem" />
                <span class="input-group-btn">
                    <asp:Button ID="Search_B" runat="server" Text="<%$Resources:L,搜索 %>" class="btn btn-outline-secondary" OnClick="Search_B_Click" />
                </span>
            </div>
        </div>
		</div>
    </div>
</ol>
<div id="template" runat="server">
    <ul class="nav nav-tabs hidden-xs hidden-sm">
        <li class="nav-item"><a class="nav-link" href="#tab_all" data-toggle="tab" onclick="ShowTabs('');">全部</a></li>
	    <li class="nav-item"><a class="nav-link" href="#tab_img" data-toggle="tab" onclick="ShowTabs('img');">图片</a></li>
	    <li class="nav-item"><a class="nav-link" href="#tab_music" data-toggle="tab" onclick="ShowTabs('music');">音乐</a></li>
        <li class="nav-item"><a class="nav-link" href="#tab_icon" data-toggle="tab" onclick="ShowTabs('icon');">图标</a></li>
        <li class="nav-item"><a class="nav-link" href="#tab_shape" data-toggle="tab" onclick="ShowTabs('shape');">形状</a></li>
        <li class="nav-item"><a class="nav-link" href="#tab_text" data-toggle="tab" onclick="ShowTabs('text');">文字</a></li>
    </ul>
	<div class="table-responsive">
     <ZL:ExGridView ID="EGV" runat="server" AutoGenerateColumns="False" PageSize="10" IsHoldState="false" 
        OnPageIndexChanging="EGV_PageIndexChanging" AllowPaging="True" AllowSorting="True" OnRowCommand="EGV_RowCommand" OnRowDataBound="EGV_RowDataBound"
        CssClass="table table-striped table-bordered table-hover" EnableTheming="False" EnableModelValidation="True" EmptyDataText="数据为空">
         <Columns>
             <asp:TemplateField ItemStyle-CssClass=""><ItemTemplate><input type="checkbox" name="idchk" value='<%# Eval("ID") %>' /></ItemTemplate></asp:TemplateField>
             <asp:BoundField HeaderText="ID" DataField="ID" ItemStyle-CssClass="" />
             <asp:TemplateField HeaderText="名称">
	            <ItemTemplate> 
                    <a href="AddRes.aspx?id=<%#Eval("ID") %>" title="修改"><%# Eval("Name") %></a>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="应用场景">
                 <ItemTemplate>
                     <a href="?useage=<%#Eval("Useage") %>&ztype=<%#ZType %>"><%# GetUseage(Eval("Useage").ToString()) %></a>
                 </ItemTemplate>
             </asp:TemplateField>
             <asp:TemplateField HeaderText="资源类型">
	            <ItemTemplate> 
                    <a href="?ztype=<%#Eval("ZType") %>&useage=<%#Useage %>"><%# GetType(Eval("ZType").ToString()) %></a>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="资源" ItemStyle-CssClass="max20rem">
                 <ItemTemplate>
                     <%#GetRes() %>
                 </ItemTemplate>
             </asp:TemplateField>
             <asp:TemplateField HeaderText="资源状态">
                <ItemTemplate>
                    <%#GetStatus() %>
                </ItemTemplate>
             </asp:TemplateField>
             <asp:BoundField HeaderText="上传时间" DataField="CDate" />
             <asp:TemplateField HeaderText="操作">
	            <ItemTemplate> 
                  <a href="AddRes.aspx?id=<%#Eval("ID") %>" title="修改"><span class="zi zi_pencilalt"></span></a>
	              <asp:LinkButton runat="server"  CommandName="Del" CommandArgument='<%#Eval("ID") %>' OnClientClick="return confirm('你确定要删除吗!');" ToolTip="删除"> <span class="zi zi_trashalt"></span>删除</asp:LinkButton>
	            </ItemTemplate>
             </asp:TemplateField>
         </Columns>
     </ZL:ExGridView>
	 </div>
</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
<script>
    function showtab(type) {
        if (!type || type == "") { type = "all"; }
        $("li a[href='#tab_" + type + "']").parent().find("a").addClass("active").siblings("li").find("a").removeClass("active");
    }
    var $audio = $('<audio id="bgm" src="" loop="" style="display: none; width: 0; height: 0;"></audio>');
    function play(url,it)
    {
        var $this = $(it);
        $audio.attr("src", url);
        $audio[0].play();
        $this.hide();
        $this.next().show();
    }
    function pause(it) {
        var $this = $(it);
        $audio[0].pause();
        $this.hide();
        $this.prev().show();
    }
    function ShowTabs(type) {
        location.href = 'ResList.aspx?ztype=' + type;
    }
    $("#sel_btn").click(function (e) {
        if ($("#sel_box").css("display") == "none") {
            $(this).addClass("active");
            $("#sel_box").slideDown(300);
            $("#template").css("margin-top", "44px");
        }
        else {
            $(this).removeClass("active");
            $("#sel_box").slideUp(200);
            $("#template").css("margin-top", "0px");
        }
    });
</script>
</asp:Content>