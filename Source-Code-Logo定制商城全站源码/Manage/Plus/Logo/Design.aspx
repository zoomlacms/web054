<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Design.aspx.cs" Inherits="ZoomLaCMS.Manage.Plus.Logo.Design" MasterPageFile="~/Manage/I/Index.Master"%>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>名片设计</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
    <div id="BreadDiv" class="container-fluid mysite">
        <div class="row">
            <ol id="BreadNav" class="breadcrumb navbar-fixed-top">
                <li><a href='<%=CustomerPageAction.customPath2 %>Main.aspx'>工作台</a></li>
                <li><a href="DesTlpList.aspx">名片模板</a></li>
                <li class="active">模板设计</li>
            </ol>
        </div>
    </div>

      <%=Call.SetBread(new Bread[] {
		new Bread("/{manage}/Main.aspx","工作台"),
        new Bread("DesTlpList.aspx","名片模板"),	
        new Bread() {url=Request.RawUrl, text="模板设计",addon="" }}
		)
    %>
    <div class="container">
        <canvas id="canvas" width="1100" height="500" style="border:1px solid #ddd;"></canvas>
        <div class="margin_t10">
            <table class="table table-bordered table-striped">
                <tr><td class="td_m">模板名称</td><td><asp:TextBox runat="server" ID="Alias_T" class="form-control text_300"/></td></tr>
                <tr>
                    <td>图片边框</td>
                    <td>
                        <div class="input-group" style="width:561px;">
                            <select class="form-control text_md" id="border_dp">
                                <option value="">不显示</option>
                                <option value="solid" selected="selected">实线</option>
                                <option value="dash">虚线</option>
                            </select>
                            <input type="number" class="form-control text_s" style="border-left:none;border-right:none;" id="border_size" value="3"/>
                            <span class="input-group-append"><span class="input-group-text">px</span></span>
                            <span class="input-group-text">
                                <div class="dropdown">
                                    <a href="javascript:;" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="true">ColorPicker
                                      <span class="caret"></span>
                                    </a>
                                    <ul class="colorBody dropdown-menu" aria-labelledby="drop4" data-for="border"></ul>
                                </div>
                            </span>
                            <span class="input-group-btn">
                                <button type="button" class="btn btn-info" onclick="setImageBorder();">更新边框</button>
                            </span>
                        </div>
                        <input type="text" class="form-control" id="border_color" style="display: none;" value="#ffffff"/>
                        <script>
                                    function setImageBorder() {
                                        var objects = canvas.getObjects();
                                        var model={
                                            dp:$("#border_dp").val(),
                                            size:$("#border_size").val(),
                                            color:$("#border_color").val()
                                        };
                                        var isok = false;
                                        for (var i = 0; (i < objects.length && isok == false) ; i++) {
                                            if (objects[i].cType != "img") { continue; }
                                            objects[i].stroke = model.color;
                                            objects[i].strokeWidth = parseInt(model.size);

                                            switch (model.dp) {
                                                case "":
                                                    objects[i].strokeWidth = 0;
                                                    break;
                                                case "solid":
                                                    objects[i].strokeDashArray = null;
                                                    break;
                                                case "dash":
                                                    objects[i].strokeDashArray = [4];
                                                    break;
                                            }
                                            isok = true;
                                        }
                                        canvas.renderAll();
                                    }
                        </script>
                    </td>
                </tr>
                <tr><td>背景定义</td><td>
                    <select class="form-control text_md" id="bkColor_dp">
                        <option value="white">白色</option>
                        <option value="black" selected="selected">纯黑</option>
                        <option value="transparent">透明网络</option>
                    </select>
                    <asp:TextBox runat="server" ID="BKUrl_T" class="form-control m715-50" Text="black" />
                    <input type="button" value="上传背景图片" onclick="bkup.sel();" class="btn btn-info"/>
                    </td></tr>
                <tr>
                    <td>动图约定</td>
                    <td>
                         <div class="input-group">
                            <span class="input-group-prepend"><span class="input-group-text">单帧宽</span></span><input type="text" id="sp_width" class="form-control text_100" value=""/>
                            <span class="input-group-append"><span class="input-group-text">单帧高</span></span><input type="text" id="sp_height" class="form-control text_100" value=""/>
                            <span class="input-group-prepend"><span class="input-group-text">合成PNG图</span></span>
                            <input type="text" id="sp_value" class="form-control" style="width:400px;" value=""/>
                            <span class="input-group-append">
                                <input type="button" value="添加动图" onclick="fabHelper.add('sprite');" class="btn btn-info"/>
                            </span>
                        </div>
<%--                        <button type="button" onclick="" class="btn btn-info"><i class="fa fa-camera">动画</i></button>--%>
                    </td>
                </tr>
                <tr><td class="td_m">控件选择</td><td>
                    <button type="button" onclick="fabHelper.add('img');" class="btn btn-info"><i class="fa fa-image"></i> 图片</button>
                    <button type="button" onclick="fabHelper.add('text');" class="btn btn-info"><i class="fa fa-text-height"></i> 文字</button>
                    
                    <button type="button" onclick="fabHelper.del();" class="btn btn-danger"><i class="fa fa-trash-o"></i> 删除</button>
                </td></tr>
                <tr class="img_tr item_tr">
                    <td>图片</td>
                    <td>
                        <input type="text" id="PicUrl_T" class="form-control m715-50" placeholder="http://demo.z01.com/pic.jpg" />
                        <input type="button" class="btn btn-info" value="上传图片" onclick="pic.sel('PicUrl_T');" />
                    </td>
                </tr>
                <tr class="text_tr item_tr"><td>文字属性</td><td>
                    <div class="input-group" style="width:620px;">
                        <span class="input-group-addon">文字</span>
                        <input type="text" class="form-control" maxlength="40" id="text_text"/>
                        <span class="input-group-addon">方向</span>
                        <select class="form-control" style="width:80px;" id="direction_dp">
                            <option selected="selected" value="horizontal">水平</option>
                            <option value="vertical">垂直</option>
                        </select>
                        <span class="input-group-addon">字符数</span>
                        <input type="text" class="form-control" id="text_length" placeholder="默认20"/>
         <%--               <span class="input-group-addon">尺寸</span>
                        <input type="text" class="form-control text_md" value="25" id="text_size"/>--%>
                        <span class="input-group-addon">色彩</span>
                        <input type="text" class="form-control" id="text_color" style="display: none;" />
                        <span class="input-group-addon">
                            <div class="dropdown">
                                <a href="javascript:;" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="true">ColorPicker
                                    <span class="caret"></span>
                                </a>
                                <ul class="colorBody dropdown-menu" aria-labelledby="drop4" data-for="text">
                                </ul>
                            </div>
                        </span>
                    </div>
                </td></tr>
                <tr class="text_tr item_tr">
                    <td>字体样式</td>
                    <td>
                        <ul id="font_ul"></ul>
                    </td>
                </tr>
                <tr>
                    <td><i class="zi zi_handRight"></i></td>
                    <td>
                        <asp:Button runat="server" ID="Save_Btn" Text="保存设计" OnClick="Save_Btn_Click" class="btn btn-info" OnClientClick="return preSave();" />
                         <input type="button" value="保存至本地" class="btn btn-info" onclick="saveToImg();" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="Base64_Hid" />
    <asp:HiddenField runat="server" ID="Save_Hid" />
    <input type="file" style="display:none;" id="sp_up" accept="image/jpg,image/png,image/jpeg"/>
    <input type="file"  style="display:none;" id="bk_up" accept="image/jpg,image/png,image/jpeg"/>
    <input type="file"  style="display:none;" id="pic_up" accept="image/jpg,image/png,image/jpeg" onchange="pic.upload();"/>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
<style type="text/css">
.item_tr{display:none;}
#font_ul{}
#font_ul li{margin-bottom:5px;cursor:pointer;border:1px solid #ddd;padding:3px;height:40px;overflow:hidden;float:left;margin-right:5px;margin-bottom:5px;}
#font_ul li:hover,#font_ul li.active{border:1px solid #0094ff;}
#font_ul li img{width:100%;}
.popover{width:260px;}
.colorItem span{display:inline-block;width:25px;height:25px;border-radius:3px;margin-right:5px;border:1px solid #ddd;}
</style>
<script src="/Plugins/Third/Logo/fabric/fabric.js"></script>
<script src="/Plugins/Third/Logo/sprite.class.js"></script>
<script src="/JS/Controls/ZL_Webup.js"></script>
<script src="/JS/ICMS/ZL_Common.js"></script>
<script src="/JS/ZL_Regex.js?v=3"></script>
<script src="/JS/Plugs/base64.js"></script>
<script>
    //文字不能带空格,字体名称不能带空格
    var fontList = "Architect-Bold|Architect|Balloon_Extra|Bauhaus|Belshaw|BPreplay_Regular1|BPreplay_Bold|BPreplay_Bold_Italic|BPreplay_Italics|eurofurencebold|eurofurenceregular|eurofurenceitalic|eurofurence_light|eurofurence_light_italic|Learning_Curve|Learning_Curve_Dashed|Mistress_script - Alternates|Mistress|Mistress_Script|NoodleScript|NoodleShaded|Banda_Regular|SF_Orson_Casual_Heavy_Oblique|SF_Orson_Casual_heavy|SF_Orson_Casual_light_Oblique|SF_Orson_Casual_Light|SF_Orson_Casual_Medium_Oblique|SF_Orson_Casual_Medium|SF_Orson_Casual_Shaded_Oblique|SF_Orson_Casual_Shaded|Solar|Speedball_No1_NF_Bold|Speedball_No1_NF|SquireD|Woolkarth-Bold".split('|');
    var imgUrl = "<%=iconBll.PlugDir+"CreateImg.aspx?action=text&"%>";
</script>
<script>
    var canvas = null;
    var fabHelper = { list: [] };
    fabHelper.init = function (json) {
        
        if (json == "") {fabHelper.setBackGround("<%=BKUrl_T.Text%>"); return; }
        canvas.loadFromJSON(json, function () {
            var list = canvas.getObjects();
            for (var i = 0; i < list.length; i++) {
                var src = list[i]._element.src;
                if (src.toLowerCase().indexOf("createimg.aspx") > -1) { list[i].cType = "text"; }
                else if (false) { list[i].cType = "sprite";}
                else {
                    list[i].cType = "img";
                }
                fabHelper.bindEvent(list[i]);
            }
            fabHelper.setBackGround("<%=BKUrl_T.Text%>");
        });

    }
    fabHelper.tool = {};
    //从URL中获取model信息
    fabHelper.tool.getInfoFromUrl = function (url, name) {
        var str = url.split(name + "=")[1];
        str = str.split("&")[0];
        return JSON.parse(decodeURIComponent(str));
    }
    fabHelper.newMod = function (type) {
        switch (type) {
            case "sprite":
                return { width: 0, height: 0, value: "" };
            case "text":
            default:
                return { bkcolor: "#000000", color: "#ffffff", text: "", size: 12, family: "", addon: "", length: "" };
        }
    }
    //{type:"",value:""}
    fabHelper.add = function (cfg) {
        //兼容
        if (typeof (cfg) == "string") { cfg = { "type": cfg, "value": "" }; }
        switch (cfg.type) {
            case "img":
                fabric.Image.fromURL("/UploadFiles/nopic.gif", function (image) {
                    callBack(image);
                });
                break;
            case "text":
                var model = fabHelper.newMod();
                model.text = "YOUR TEXT HERE";
                fabric.Image.fromURL(fabHelper.getTextUrl(model), function (image) {
                    callBack(image);
                });
                break;
            case "sprite":
                {
                    var model = fabHelper.newMod(cfg.type);
                    model.value = $("#sp_value").val();
                    model.width = Convert.ToInt($("#sp_width").val());
                    model.height = Convert.ToInt($("#sp_height").val());
                    if (model.value == "" || model.width < 1 || model.height < 1) { alert("宽,高,图不能为空"); return false; }
                    fabric.Sprite.fromURL(model.value, function (sprite) {
                        //如终居于最下方
                        sprite.zindex = 0;
                        //sprite.set({left: 100,top: 100});
                        //canvas.add(sprite);
                        callBack(sprite);
                        setTimeout(function () {
                            sprite.play();
                        }, fabric.util.getRandomInt(1, 10) * 100);
                        (function render() {
                            canvas.renderAll();
                            fabric.util.requestAnimFrame(render);
                        })();
                    }, { width: model.width, height: model.height });
                }
                break;
        }
        //---------------------
        function callBack(fabObj) {
            fabObj.cType = cfg.type;
            fabObj.set({ left: (canvas.width - fabObj.width) / 2, top: 50, });
            fabHelper.bindEvent(fabObj);
            if (fabObj.zindex == 0 || fabObj.zindex) { canvas.insertAt(fabObj, fabObj.zindex); }
            else { canvas.add(fabObj); }
        }
    }
    fabHelper.bindEvent = function (fabObj) {
        fabObj.on("selected", function (e) {
            var fabObj = canvas.getActiveObject();
            $(".item_tr").hide();
            $("." + fabObj.cType + "_tr").show();
            //----------更新控件数据
            var imgUrl = $(fabObj._element.outerHTML).attr("src");
            imgUrl = StrHelper.getUrlVPath(imgUrl);
            switch (fabObj.cType) {
                case "img":
                    $("#PicUrl_T").val(imgUrl);
                    break;
                case "text":
                    {
                        var model = fabHelper.tool.getInfoFromUrl(imgUrl, "model");
                        $("#text_text").val(model.text);
                        $("#text_size").val(model.size);
                        $("#text_length").val(model.length);
                        $("#text_color").val(model.color);
                    }
                    break;
                case "sprite":
                    {
                        $("#sp_width").val(fabObj.width);
                        $("#sp_height").val(fabObj.height);
                        $("#sp_value").val(imgUrl);
                    }
                    break;
            }
        });
    }
    //更新当前选定的对象
    fabHelper.update = function () {
        var fabObj = canvas.getActiveObject();
        if (!fabObj || !fabObj.cType) { console.log("无active object"); return; }
        switch (fabObj.cType) {
            case "img":
                {
                    var oldWid = fabObj.width * fabObj.scaleX;
                    fabObj.setSrc($("#PicUrl_T").val(), function (image) {
                        //fabObj.set({ width: 50, height: 50 });//cut
                        image.scale(oldWid/image.width);
                        canvas.renderAll();
                    });
                }
                break;
            case "text":
                {
                    var model = fabHelper.newMod(fabObj.cType);
                    model.text = $("#text_text").val();
                    model.color = $("#text_color").val();
                    model.length = $("#text_length").val();
                    model.size = $("#text_size").val();
                    model.direction = $("#direction_dp").val();
                    if ($("#font_ul li.active").length > 0)
                    {
                        model.family = $("#font_ul li.active").data("font");
                    }
                    fabObj.setSrc(fabHelper.getTextUrl(model), function () { canvas.renderAll(); });
                }
                break;
            case "sprite":
                {
                    var model = fabHelper.newMod(fabObj.cType);
                    model.height = $("#sp_height").val();
                    model.width = $("#sp_width").val();
                    fabObj.set({ width: model.height, height: model.height }, function () { });
                }
                break;
        }
    }
    fabHelper.del = function () {
        var fabObj = canvas.getActiveObject();
        if (!fabObj) { return; }
        canvas.remove(fabObj);
    }
    fabHelper.getTextUrl = function (model) {
        var url = imgUrl + "model=" + encodeURIComponent(JSON.stringify(model));
        return url;
    }
    //设置背景色或背景图片
    fabHelper.setBackGround = function (color) {
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
    //------------------------
    $(function () {
     for (var i = 0; i < fontList.length; i++) {
            var model = fabHelper.newMod();
            model.text = "SelectFont";
            model.bkcolor = "";
            model.color = "#000000";
            model.size = 16;
            model.family = fontList[i];
            var url = fabHelper.getTextUrl(model);
            $("#font_ul").append('<li data-font="' + fontList[i] + '"><img src="' + url + '" /></li>');
        }
        $("#font_ul li").click(function () {
            var obj = canvas.getActiveObject();
            if (!obj) { return; }
            $("#font_ul li").removeClass("active");
            $(this).addClass("active");
            fabHelper.update();
        });
        //-----------------------
        $(".item_tr input:text").change(fabHelper.update);
        $("#direction_dp").change(fabHelper.update);
        $("#bkColor_dp").change(function () {
            $("#BKUrl_T").val(this.value);
            fabHelper.setBackGround(this.value);
        });
        //---canvas init:
        setTimeout(function () {
            fabric.Object.prototype.transparentCorners = false;
            fabric.Object.prototype.cornerColor = "white";
            fabric.Object.prototype.cornerStyle = "circle";
            fabric.Object.prototype.cornerSize = 12;

            fabric.Object.prototype.borderColor = "#0094ff";
            fabric.Object.prototype.borderScaleFactor = 3;
            fabric.Object.prototype.padding = 1;
            fabric.Object.prototype.hasRotatingPoint = false;
            canvas = new fabric.Canvas('canvas');
            //if ($("#Save_Hid").val() != "") { canvas.loadFromJSON($("#Save_Hid").val()); }
            fabHelper.init($("#Save_Hid").val());
        },500);
    })
    //-----------------------colorJS
    var colorArr = [
        { name: "Red", color: "#fe0000" },
        { name: "Yellow", color: "#ffff01" },
        { name: "Purple", color: "#9700ff" },
        { name: "White", color: "#ffffff" },
        { name: "Green", color: "#01ff01" },
        { name: "Blue", color: "#1071fe" },
        { name: "Orange", color: "#ffa92e" },
        { name: "Turquoise", color: "#00ffff" },
        { name: "Pink", color: "#fe2efe" },
        { name: "Veep Green", color: "#01cc34" },
        { name: "Rich Blue", color: "#256ef1" },
        { name: "Neon Blue", color: "#4d71eb" },
    ];
    for (var i = 0; i < colorArr.length; i++) {
        var model = colorArr[i];
        var tlp = '<li class="colorItem" data-color="' + model.color + '"><a href="javascript:;"><span style="background-color:' + model.color + ';"></span>' + model.name + '</a></li>';
        $(".colorBody").append(tlp);
    }
    $(".colorItem").click(function () {
        var colorFor = $(this).parent().data("for");
        var color = $(this).data("color");
        switch (colorFor)
        {
            case "text":
                $("input#text_color").val(color);
                break;
            case "border":
                $("input#border_color").val(color);
                break;
        }
       
        fabHelper.update();
    });
//-----------------------
    function saveToImg() {
        var MIME_TYPE = "image/png";
        var imgURL = canvas.toDataURL(MIME_TYPE);//base64
        var dlLink = document.createElement('a');
        dlLink.download = "Logo";
        dlLink.href = imgURL;
        dlLink.dataset.downloadurl = [MIME_TYPE, dlLink.download, dlLink.href].join(':');
        document.body.appendChild(dlLink);
        dlLink.click();
        document.body.removeChild(dlLink);
    }
    function preSave() {
        $("#Base64_Hid").val(canvas.toDataURL("image/png"));//base64
        $("#Save_Hid").val(JSON.stringify(canvas));
        return true;
    }
//------上传图片
var pic = { id: "pic_up", txtid: null };
pic.sel = function (id) {
    $("#" + pic.id).val("");
    $("#" + pic.id).click();
    pic.txtid = id;
}
pic.upload = function () {
   var fname = $("#" + pic.id).val();
   if (!SFileUP.isWebImg(fname)) { alert("请选择图片文件"); return false; }
    SFileUP.AjaxUpFile(pic.id, function (data) {
        $("#" + pic.txtid).val(data);
        fabHelper.update();
    });
}
pic.preSave = function () {var src = $("#pic_img").attr("src");$("#pic_hid").val(src);}
//-------
var picup = function (cfg) {
    var ref = this;
    this.cfg = cfg;
    this.$up = $("#" + cfg.id);
    this.$up.change(function () {
        if (ref.$up.val() == "") { console.log("未选择文件"); return; }
        if (ref.cfg.up_before) { ref.cfg.up_before(); }
        SFileUP.AjaxUpFile(ref.$up[0], function (data) {
            if (ref.cfg.up_after) { ref.cfg.up_after(data); }
        });
    });
}
picup.prototype.sel = function () {
    this.$up.click();
}
var bkup = new picup({
    id: "bk_up"
    , up_before: function () {
        console.log("up_before");
    }
    , up_after: function (data) {
        fabHelper.setBackGround(data);
        $("#BKUrl_T").val(data);
    }
});

var spup = new picup({
    id: "sp_up"
    , up_before: function () { }
    , up_after: function (data) {
        $("#sp_value").val(data);
        fabHelper.add({ type: 'sprite',value:data});
    }
});
</script>
</asp:Content>