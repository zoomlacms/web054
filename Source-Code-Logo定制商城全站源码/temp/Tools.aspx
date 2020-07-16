<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Tools.aspx.cs" Inherits="temp_Tools" MasterPageFile="~/Manage/I/Default.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>处理</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<asp:HiddenField runat="server" ID="Content_Hid"/>
<asp:HiddenField runat="server" ID="Mid_Hid" />
<canvas id="canvas" width="1100" height="500" style="border:2px solid #000;"></canvas>
<div style="margin-top:10px;">
    <asp:Button runat="server" ID="Save_Btn" class="btn btn-info" Text="手动保存" OnClick="Save_Btn_Click" OnClientClick="return preSave();" />
    <asp:Button runat="server" ID="Ignore_Btn" class="btn btn-info" Text="忽略" OnClick="Ignore_Btn_Click"/>
</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
<script src="/Plugins/Third/Logo/fabric/fabric.js"></script>
<script src="/Plugins/Third/Logo/sprite.class.js"></script>
<script src="/JS/Controls/ZL_Webup.js"></script>
<script src="/JS/ICMS/ZL_Common.js"></script>
<script src="/JS/ZL_Regex.js?v=3"></script>
<script src="/JS/Plugs/base64.js"></script>
<script>
    var canvas = new fabric.Canvas('canvas');
    canvas.loadFromJSON($("#Content_Hid").val(), function () {
        var objects = canvas.getObjects();
        for (var i = 0; i < objects.length; i++) {
            //自动偏移215px
            objects[i].left = parseInt(objects[i].left) + 215;
        }
        console.log("color", canvas.backgroundColor);
        if (canvas.backgroundImage && canvas.backgroundImage.src) {
            var url = HtmlUtil.removeUrlRoot(canvas.backgroundImage.src);
            console.log(url);
            setBackGround(url);
        }
        else {
            setBackGround("black");
        }
        canvas.renderAll();
    });
    var setBackGround = function (color) {
        canvas.setBackgroundImage(null);
        switch (color) {
            case "transparent":
                fabric.Object.prototype.cornerColor = "#5bc0de";
                canvas.setBackgroundColor({
                    source: "/Plugins/Third/Logo/assets/grid.jpg",
                    repeat: 'repeat',
                }, canvas.renderAll.bind(canvas));
                break;
            case "black":
                fabric.Object.prototype.cornerColor = "white";
                canvas.setBackgroundColor("#000", canvas.renderAll.bind(canvas));
                break;
            case "white":
                fabric.Object.prototype.cornerColor = "#5bc0de";
                canvas.setBackgroundColor("#fff", canvas.renderAll.bind(canvas));
                break;
            default://背景图片
                fabric.Image.fromURL(color, function (img) {
                    img.scaleX = canvas.width / img.width;
                    img.scaleY = canvas.height / img.height;
                    canvas.setBackgroundImage(img, canvas.renderAll.bind(canvas));
                });
                break;
        }
    }
    var preSave = function () {
        $("#Content_Hid").val(JSON.stringify(canvas));
    }
    //1,使用背景图片的不设定范围
    //2,单独上传了图片的限定范围(有多张图片则以第一张为准)
</script>
</asp:Content>