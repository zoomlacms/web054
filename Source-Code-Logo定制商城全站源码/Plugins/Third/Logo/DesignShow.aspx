<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DesignShow.aspx.cs" Inherits="Plugins_Third_Logo_DesignShow" MasterPageFile="~/Common/Master/Empty.master"%>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>Business card design</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<div class="container">
    <div style="margin:0 auto;" runat="server" id="design_panel" visible="false">
        <canvas id="mainCanvas" width="1100" height="500"></canvas>
    </div>
    <div runat="server" id="img_panel" visible="false" style="margin:0 auto;text-align:center;">
        <img src="<%=imgUrl %>" style="max-width:1100px;max-height:500px;"/>
    </div>
    <div>
        <asp:Label runat="server" ID="Design_HTML"></asp:Label>
<%--        <div style="text-align:center;">
             <input type="button" value="保存至本地" class="btn btn-info" onclick="saveToImg();" />
        </div>--%>
    </div>
</div>
<asp:HiddenField runat="server" ID="Base64_Hid" />
<asp:HiddenField runat="server" ID="Save_Hid" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
<script src="/Plugins/Third/Logo/fabric/fabric.js"></script>
<script src="/Plugins/Third/Logo/sprite.class.js"></script>
<script src="/Plugins/Third/Logo/fabHelper.js?v=1"></script>
<script src="/JS/ZL_Regex.js?v=1"></script>
<script>
    //======================================================
    function saveToImg() {
        var $form = $('<form target="_blank"  method="post" action="/Common/Label/OutToImg.aspx?name=view"></form>');
        $form.append('<input type="hidden" name="base64_hid" value=' + canvas.toDataURL("image/png") + '>');
        $("body").append($form);//兼容IE
        $form.submit();
        $form.remove();
    }
    //----------------------
    $(function () {
        fabric.Object.prototype.transparentCorners = false;
        fabric.Object.prototype.hasControls = false;
        fabric.Object.prototype.borderColor = "#0094ff";
        fabric.Object.prototype.borderScaleFactor = 3;
        fabric.Object.prototype.padding = 1;
        fabric.Object.prototype.selectable = false;
        canvas = new fabric.Canvas('mainCanvas');
        fabHelper.init($("#Save_Hid").val(), {
            itemcb: function (fabObj) {
                fabObj.lockMovementX = true;
                fabObj.lockMovementY = true;
                fabObj.lockScalingX = true;
                fabObj.lockScalingY = true;
                fabObj.lockRotation = true;
            }
        });
    });
</script>
</asp:Content>