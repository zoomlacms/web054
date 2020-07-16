<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShowSuccess.aspx.cs" Inherits="ZoomLaCMS.Prompt.ShowSuccess" EnableViewStateMac="false" ValidateRequest="false" MasterPageFile="~/Common/Common.master"%>
<asp:Content runat="server" ContentPlaceHolderID="head">
<style type="text/css">ul li{list-style-type:none;}.fa{margin-right:3px;}</style> 
<title>Success</title>
<link rel="stylesheet" href="/dist/css/animate.min.css"/>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<div class="container animated sysTips_prompt" style="min-height:240px;">
<div class="col-sm-6 offset-sm-3">
    <div class="card">
        <div class="card-header"><i class="zi zi_checkcircle"></i>Success </div>
        <div class="card-body text-center">
            <h5 class="card-title"></h5>
            <p class="card-text">
                <asp:Literal ID="LtrSuccessMessage" runat="server"></asp:Literal>
            </p>
            <div class="card-text">
                <asp:HyperLink ID="LnkReturnUrl" runat="server" class="btn btn-primary"><span class="fa fa-repeat"></span> Prev</asp:HyperLink>
            </div>
        </div>
    </div>
</div>
<script>
function AutoReturn(url, time) { setTimeout(function () { location = url }, time); }
function SetUrl(url) { $("#LnkReturnUrl").attr("href", url); }
</script>
</asp:Content>