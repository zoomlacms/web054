<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DesTlpList.aspx.cs" Inherits="ZoomLaCMS.Manage.Plus.Logo.DesTlpList" MasterPageFile="~/Manage/I/Index.Master"%>

<script runat="server">

    protected void RPT_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

    }
</script>

<asp:Content runat="server" ContentPlaceHolderID="head"><title>名片模板</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
 <%--   <div id="BreadDiv" class="container-fluid mysite">
        <div class="row">
            <ol id="BreadNav" class="breadcrumb navbar-fixed-top">
                <li><a href='<%=CustomerPageAction.customPath2 %>Main.aspx'>工作台</a></li>
                <li><a href='IconList.aspx'>图标列表</a></li>
                <li class="active"><a href='<%=Request.RawUrl %>'>名片模板</a>  [<a href="Design.aspx"><i class="fa fa-pencil"></i>创建模板</a>]</li>
                <div id="help" class="pull-right text-center"><a href="javascript:;" id="sel_btn" class="help_btn" onclick="selbox.toggle();"><i class="fa fa-search"></i></a></div>
                <div id="sel_box" class="padding5">
                    <div class="input-group">
                        <input type="text" id="skey" runat="server" class="form-control mvcparam" placeholder="请输入关键词" onkeypress="selbox.search();" />
                        <span class="input-group-btn">
                            <asp:LinkButton runat="server" class="btn btn-default" ID="Search_Btn" OnClick="Search_Btn_Click"><i class="fa fa-search"></i></asp:LinkButton>
                        </span>
                    </div>
                </div>
            </ol>
        </div>
    </div>--%>
        <nav aria-label="breadcrumb" role="navigation" class="fixed-top">
        <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="/Admin/Main.aspx">工作台</a> </li>
            <li class="breadcrumb-item"><a href="<%=Request.RawUrl %>">作品列表</a> [<a href='Design.aspx'>创建作品</a>]</li>
            <div id="help" class="pull-right text-center"><a href="javascript:;" id="sel_btn" class="help_btn" onclick="selbox.toggle();"><i class="fa fa-search"></i></a></div>
            <div id="sel_box" class="padding5">
                 <div class="input-group">
                    <input type="text" id="skey" runat="server" class="form-control mvcparam" placeholder="请输入关键词" onkeypress="selbox.search();" />
                    <span class="input-group-append">
                        <asp:LinkButton runat="server" class="btn btn-info" ID="Search_Btn" OnClick="Search_Btn_Click">搜索</asp:LinkButton>
                    </span>
                </div>
            </div>
        </ol>
    </nav>
    <div style="height:55px;"></div>
    <table class="table table-bordered table-striped">
        <thead><tr>
            <th class="td_xs"></th>
            <th class="td_s">ID</th>
            <th>别名</th>
            <th>预览</th>
            <th class="td_l">创建时间</th>
            <th>操作</th>
               </tr></thead>
        <ZL:Repeater runat="server" ID="RPT" PageSize="10"
            PagePre="<tr><td><label class='allchk_l'><input type='checkbox' id='chkAll'/></label></td><td colspan='8'><div class='text-center'>"
            PageEnd="</div></td></tr>">
            <ItemTemplate>
                <tr>
                    <td><input type="checkbox" name="idchk" value="<%#Eval("ID") %>" /></td>
                    <td><%#Eval("ID") %></td>
                    <td><%#Eval("Alias") %></td>
                    <td><img src="<%#Eval("PreviewImg") %>" style="width:150px;height:100px;"/></td>
                    <td><%#Eval("CDate","{0:yyyy/MM/dd}") %></td>
                    <td>
                        <a href="Design.aspx?ID=<%#Eval("ID") %>" class="btn btn-info btn-sm">修改</a>
                        <asp:LinkButton runat="server" class="btn btn-danger btn-sm" CommandName="del2" CommandArgument='<%#Eval("ID") %>' OnClientClick="return confirm('确定要删除吗');">删除</asp:LinkButton>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate></FooterTemplate>
        </ZL:Repeater>
    </table>
<div>
    <asp:Button runat="server" ID="BatDel_Btn" Text="批量删除" OnClick="BatDel_Btn_Click" class="btn btn-info"/>
</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
<script src="/JS/Controls/ZL_Dialog.js"></script>
<script>
    function showDesign(id) {
        ShowComDiag("/Plugins/Third/Logo/Third.aspx?id="+id+"&mode=admin", "查看设计");
    }
</script>
</asp:Content>