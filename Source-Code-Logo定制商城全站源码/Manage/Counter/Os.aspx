﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Os.aspx.cs" Inherits="ZoomLaCMS.Manage.Counter.Os"MasterPageFile="~/Manage/I/Index.master"%>
<asp:Content runat="server" ContentPlaceHolderID="head">
    <title>操作系统统计报表</title>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
    <%=Call.SetBread(new Bread[] {
		new Bread("/{manage}/Main.aspx","工作台"),
        new Bread("Counter.aspx","访问统计 "),
        new Bread() {url="Os.aspx", text="操作系统统计报表",addon="" }}
		)
    %>
    <div class="container-fluid list_choice">
		<div class="row">
        <div class="col-12 col-md-4 col-lg-2">
            <div class="card">
                <div class="card-header">
                <h3 class="card-title"><i class="zi zi_circleComponents"></i> 操作系统统计</h3>
                </div>
                <ul class="list-group">
                <asp:Repeater ID="CountRPT" runat="server">
                    <ItemTemplate>
                        <li class="list-group-item"><%#GetOSInfo() %><span class="badge" title="访问量"><%#Eval("NUM") %></span></li>
                    </ItemTemplate>
                </asp:Repeater>
                </ul>
            </div>
        </div>
        <div class="col-12 col-md-8 col-lg-10">
            <ZL:ExGridView runat="server" ID="EGV" AutoGenerateColumns="false" PageSize="15" AllowPaging="true"  EnableTheming="False"  
                CssClass="table table-striped table-bordered table-hover" EmptyDataText="当前没有信息!!" 
                OnPageIndexChanging="EGV_PageIndexChanging" >
                  <Columns>
                      <asp:BoundField ControlStyle-CssClass="td_s" HeaderText="ID" DataField="ID" />
                      <asp:TemplateField HeaderText="来源">
                          <ItemTemplate>
                              <%#Eval("Source") %>
                          </ItemTemplate>
                      </asp:TemplateField>
                      <asp:TemplateField HeaderText="用户">
                          <ItemTemplate>
                              <%#Eval("UserName") %>
                          </ItemTemplate>
                      </asp:TemplateField>
                      <asp:TemplateField HeaderText="IP">
                          <ItemTemplate>
                              <%#Eval("IP") %>
                          </ItemTemplate>
                      </asp:TemplateField>
                      <asp:TemplateField HeaderText="操作系统">
                          <ItemTemplate>
                              <%#Eval("OSVersion") %>
                          </ItemTemplate>
                      </asp:TemplateField>
                      <asp:TemplateField HeaderText="设备">
                          <ItemTemplate>
                              <%#Eval("Device") %>
                          </ItemTemplate>
                      </asp:TemplateField>
                      <asp:TemplateField HeaderText="浏览器版本">
                          <ItemTemplate>
                              <%#Eval("BrowerVersion") %>
                          </ItemTemplate>
                      </asp:TemplateField>
                      <asp:TemplateField HeaderText="创建时间">
                          <ItemTemplate>
                              <%#Eval("CDate") %>
                          </ItemTemplate>
                      </asp:TemplateField>
                  </Columns>
              </ZL:ExGridView>
        </div>
		</div>
    </div>    
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
    <script src="/JS/Counter.js" type="text/javascript"></script>
</asp:Content>