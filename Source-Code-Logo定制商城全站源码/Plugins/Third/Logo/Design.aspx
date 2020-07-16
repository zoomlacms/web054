<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Design.aspx.cs" Inherits="ZoomLaCMS.Plugins.Third.Logo.Design" MasterPageFile="~/Common/Master/Empty.master" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
<style type="text/css">
body { }
.item_tr{display:none;}
#font_ul { height:225px; overflow:auto;}
#font_ul li{margin-bottom:5px;cursor:pointer;border:1px solid #ddd;padding:3px;height:70px;overflow:hidden;float:left;margin-right:5px;margin-bottom:5px;}
#font_ul li:hover,#font_ul li.active{border:1px solid #0094ff;}
#font_ul li img{width:100%;}
.popover{width:300px;}
.home_list_r {}

.colorItem span{display:inline-block;width:25px;height:25px;border-radius:3px;margin-right:5px;border:1px solid #ddd;}

@media screen and (max-width:768px) { 
.upper-canvas ,#mainCanvas{margin-left: 0!important;width: 100%!important;}
#edit_tb select{width: 100%;}	
#edit_tb .btn-info{margin-top: .5rem;}	
.home_list_r .canvas-container{width: 100%!important;}
}
		
</style>
<title>Business card design</title>
<link href="/Template/Nrays/style/global.css?Version=20170501506670" rel="stylesheet"/>
<script src="/js/scrolltopcontrol.js"></script>
<script src="/JS/Controls/B_User.js"></script>
<link href="/Plugins/Third/alert/sweetalert.css" rel="stylesheet" />
<script src="/Plugins/Third/alert/sweetalert.min.js?v=1"></script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<%Call.Label("{ZL.Label id=\"模板设计全站头部\" /}"); %>

	
<div class="disgin_banner">
<h1>Design - submit your creative and business needs.</h1>
</div>


<div class="container digin_box">


<div id="BreadDiv" class="mysite">
<ol id="BreadNav" class="breadcrumb">
<li class="breadcrumb-item"><a href="/">Home</a></li>
<li class="breadcrumb-item"><a href="/Class_2/Default.aspx">Mall</a></li>
<li  class="breadcrumb-item active">Design</li>
<span style="color:#f00;" class="pull-right">↓* Must click the element design below</span>
</ol>
</div>
	
<div style="background:#FFF;">
    <canvas id="mainCanvas" width="1100" height="500"></canvas>
    <div class="margin_t10" runat="server" id="edit_tb">
        <table class="table table-bordered table-striped">
            <tr class="img_tr item_tr">
                <td class="td_m">Logo</td>
                <td>
                    <input type="file" id="pic_up" class="hidden" onchange="pic.upload();" />
                    <input type="text" id="PicUrl_T" class="form-control m715-50" placeholder="http://demo.z01.com/pic.jpg" />
                    <input type="button" class="btn btn-info" value="Upload pictures" onclick="pic.sel('PicUrl_T');" />
                </td>
            </tr>
            <tr class="text_tr item_tr"><td>Text attribute</td><td>
                <input type="hidden" id="text_direction"/>
                <div class="input-group" style="width: 650px;">
				
				<div class="input-group col-5 pl-0">
				  <div class="input-group-prepend">
					<span class="input-group-text" id="basic-addon1">Text</span>
				  </div>
				  <input type="text" class="form-control" id="text_text" aria-label="Text" aria-describedby="basic-addon1">
				</div>


					
                    <span class="input-group-append"><span class="input-group-text">Color</span></span>
                    <input type="text" class="form-control" id="text_color" style="display: none;" />
                    <span class="input-group-text">
                        <div class="dropdown">
                            <a href="javascript:;" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="true">ColorPicker
        <span class="caret"></span>
                            </a>
                            <ul id="colorBody" class="dropdown-menu" aria-labelledby="drop4">
                            </ul>
                        </div>
                    </span>
                    <span class="input-group-btn">
                        <input type="button" class="btn btn-info" value="Update Text" onclick="fabHelper.update();" />
                    </span>
                </div>
            </td></tr>
            <tr class="text_tr item_tr">
                <td>Font Style</td>
                <td>
                    <div class="btn-group">
                        <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">
                            <i class="fa fa-font"></i>Select Font...
    <span class="caret"></span>
                        </button>
                        <ul id="font_ul" class="dropdown-menu" role="menu" style="background:#000;">
                        </ul>
                    </div>
                </td>
            </tr>
            <tr>
                <td>SKU</td>
                <td>
                    <asp:Label runat="server" ID="SKU_L"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Size</td>
                <td>
                    <asp:Label runat="server" ID="Size_L"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Flash Options</td>
                <td>
                    <select id="Flash_DP" name="Flash_DP" class="form-control text_300">
                        <%=ZoomLa.Sns.SnsHelper.H_GetDPOption("flash","Choose FLASHING Upgrade here...","0") %>
                    </select>
                </td>
            </tr>
            <tr runat="server" id="Outdoor_TR"><td>Outdoor Sign</td><td>
                <select id="OutdoorSign_DP" name="OutdoorSign_DP" class="form-control text_300">
                    <%=ZoomLa.Sns.SnsHelper.H_GetDPOption("outdoor","No","0") %>
                </select>
            </td></tr>
            <tr runat="server" id="Backing_TR">
                <td>Backing Material</td>
                <td>
                    <select id="BackGround_DP" name="BackGround_DP" class="form-control text_300">
                        <%=ZoomLa.Sns.SnsHelper.H_GetDPOption("backing","Metal Frame","0") %>
                    </select>
                </td>
            </tr>
            <tr><td>Comments</td><td>
                <asp:TextBox runat="server" ID="Comment_T" TextMode="MultiLine" MaxLength="500" class="form-control" style="height:150px;"/></td></tr>
            <tr>
                <td class="td_m">Operation</td>
                <td>
                    <asp:Button runat="server" ID="Save_Btn" Text="Add To Cart" OnClick="Save_Btn_Click" class="btn btn-info" OnClientClick="return preSave();" />
                    <input type="button" value="Save your picture to local" class="btn btn-info" onclick="saveToImg();" />
                </td>
            </tr>
        </table>
    </div>
    </div>

</div>


<%Call.Label("{ZL.Label id=\"模板设计全站头部\" /}"); %>

<asp:HiddenField runat="server" ID="Base64_Hid" />
<asp:HiddenField runat="server" ID="Save_Hid" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">

<%Call.Label("{ZL.Label id=\"模板设计全站底部\" /}"); %>
<div runat="server" visible="false" id="color_7_div">
    <script>
        var colorArr = [
    { name: "Blue", color: "#1071fe" },
    { name: "Orange", color: "#ffa92e" },
    { name: "Green", color: "#01ff01" },
    { name: "Red", color: "#fe0000" },
    { name: "Yellow", color: "#ffff01" },
    { name: "Purple", color: "#9700ff" },
    { name: "White", color: "#ffffff" },
        ];
    </script>
</div>
<div runat="server" visible="false" id="color_12_div">
    <script>
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
    </script>
</div>
<div runat="server" visible="false" id="font_div">
    <script>
        //Bubble Tea|Crazy Type|Dance Club|Fat Burger|Funky Disco|Hello June|Klasdot|Latype Brush|Latype Condensed|Latype Extras|Latype Hand|Latype Sans|Latype Script|Latype Serif|London Train|Retro Board|Rope and Bag|Strip Box|Sweet Lollipop|Whalerig|Wide West|Yellow Jelly
        var fontList = "Architect-Bold|Architect|Balloon_Extra|Bauhaus|Belshaw|BPreplay_Regular1|BPreplay_Bold|BPreplay_Bold_Italic|BPreplay_Italics|eurofurencebold|eurofurenceregular|eurofurenceitalic|eurofurence_light|eurofurence_light_italic|Learning_Curve|Learning_Curve_Dashed|Mistress_script - Alternates|Mistress|Mistress_Script|NoodleScript|NoodleShaded|Banda_Regular|SF_Orson_Casual_Heavy_Oblique|SF_Orson_Casual_heavy|SF_Orson_Casual_light_Oblique|SF_Orson_Casual_Light|SF_Orson_Casual_Medium_Oblique|SF_Orson_Casual_Medium|SF_Orson_Casual_Shaded_Oblique|SF_Orson_Casual_Shaded|Solar|Speedball_No1_NF_Bold|Speedball_No1_NF|SquireD|Woolkarth-Bold".split('|');
    </script>
</div>
<div runat="server" visible="false" id="font_led_div">
    <script>
        var fontList = "Architect-Bold|Architect|Balloon_Extra|Bauhaus|Belshaw|BPreplay_Regular1|BPreplay_Bold|BPreplay_Bold_Italic|BPreplay_Italics|eurofurencebold|eurofurenceregular|eurofurenceitalic|eurofurence_light|eurofurence_light_italic|Learning_Curve|Learning_Curve_Dashed|Mistress_script - Alternates|Mistress|Mistress_Script|NoodleScript|NoodleShaded|Banda_Regular|SF_Orson_Casual_Heavy_Oblique|SF_Orson_Casual_heavy|SF_Orson_Casual_light_Oblique|SF_Orson_Casual_Light|SF_Orson_Casual_Medium_Oblique|SF_Orson_Casual_Medium|SF_Orson_Casual_Shaded_Oblique|SF_Orson_Casual_Shaded|Solar|Speedball_No1_NF_Bold|Speedball_No1_NF|SquireD|Woolkarth-Bold".split('|');
    </script>
</div>

<script src="/Plugins/Third/Logo/fabric/fabric.js"></script>
<script src="/Plugins/Third/Logo/sprite.class.js"></script>
<script src="/JS/Controls/ZL_Webup.js"></script>
<script src="/JS/ICMS/ZL_Common.js"></script>
<script src="/JS/ZL_Regex.js?v=3"></script>
<script src="/Design/JS/Plugs/Helper/StyleHelper.js"></script>
<script src="/Plugins/Third/Logo/fabHelper.js?v=107"></script>
<script>


	
//右侧导航
function ShowMobileMenu(){
	$(".home17_mobilemenu_bg").fadeIn();;
	$(".home17_mobilemenu").addClass("active");
	$(".home17_mobilemenu_bg").click(function(){
		$(".home17_mobilemenu_bg").fadeOut();
		$(".home17_mobilemenu").removeClass("active");
	});
}
function HideMobileMenu(){
	$(".home17_mobilemenu_bg").fadeOut();
	$(".home17_mobilemenu").removeClass("active");
}
	
	
    //======================================================
    function saveToImg() {
        //Add Watermark
        fabric.Image.fromURL("/UploadFiles/bg.png?v=2", function (fabObj) {
            fabObj.set({ "left": 0, top: 0, opacity: 0.7 });
            canvas.add(fabObj);
            var $form = $('<form target="_blank"  method="post" action="/Common/Label/OutToImg.aspx?name=view"></form>');
            $form.append('<input type="hidden" name="base64_hid" value=' + canvas.toDataURL("image/png") + '>');
            $("body").append($form);//兼容IE
            $form.submit();
            $form.remove();
            canvas.remove(fabObj);
        });
    }
    function preSave() {
        $("#Save_Hid").val(JSON.stringify(canvas));
        $("#Base64_Hid").val(canvas.toDataURL("image/png"));//base64
        return true;
    }
    //----------------------
    var border = { top: 0, left: 0, bottom: 0, right: 0 };
    $(function () {
        fabric.Object.prototype.transparentCorners = false;
        fabric.Object.prototype.cornerColor = "white";
        fabric.Object.prototype.cornerStyle = "circle";
        fabric.Object.prototype.cornerSize = 12;
        fabric.Object.prototype.hasRotatingPoint = false;
        fabric.Object.prototype.hasControls = false;
        fabric.Object.prototype.borderColor = "#0094ff";
        fabric.Object.prototype.borderScaleFactor = 3;
        fabric.Object.prototype.padding = 1;
      

        fabHelper.backGround = "<%=desDR["BackGround"]%>";
        canvas = new fabric.Canvas('mainCanvas');
      
        //canvas.on('object:scaling', function (event) {
        //    var el = event.target;
        //    if (el == null || !el) { return; }
        //    //console.log(event.e.movementX, event.e.movementY);//左<0,右>0,上<0,下>0
        //    if (event.e.movementX != 0)
        //    {
        //        if (event.e.movementX < 0&&el.left<=10)
        //        {
        //            el.lockScalingX = true;
        //        }
        //        else if (event.e.movementX > 0 && (el.left + (el.width * el.scaleX)) >= canvas.width)
        //        {
        //            el.lockScalingX = true;
        //        }
        //    }
        //    if (event.e.movementY != 0)
        //    {
        //        if (event.e.movementY < 0 && el.top <= 10) {
        //            el.lockScalingY = true;
        //        }
        //        else if (event.e.movementY > 0 && (el.top + (el.height * el.scaleY)) >= canvas.height) {
        //            el.lockScalingY = true;
        //        }
        //    }
        //});
        
        canvas.observe('mouse:up', function (e) {
            var activeObject = e.target;
            if (activeObject == null) { return; }
            activeObject.lockScalingX = false;  // this will connect with object scalling event
            activeObject.lockScalingY = false;
        });
        border.bottom = canvas.height;
        border.right = canvas.width;
        canvas.on("object:moving", function () {
            var fabGroup = canvas.getActiveGroup();
            var fabObj = canvas.getActiveObject();
            //如果选定了组,则禁止拖动
            if (fabGroup != null) {
                fabGroup.lockMovementX = true;
                fabGroup.lockMovementY = true;
                return;
            }
            if (!fabObj || fabObj == null) { return; }
            var top = fabObj.top;
            var bottom = top + fabObj.height;
            var left = fabObj.left;
            var right = left + fabObj.width;

            

            //var topBound = 0;
            //var bottomBound = topBound + canvas.height;
            //var leftBound = 0;
            //var rightBound = leftBound + canvas.width;
            var width = fabObj.width * fabObj.scaleX;
            var height = fabObj.height * fabObj.scaleY;
            fabObj.set({
                left: Math.min(Math.max(left, border.left),
                    border.right - width)
            });
            fabObj.set({
                top: Math.min(Math.max(top, border.top),
                    border.bottom - height)
            });
        })

       


        canvas.fireRightClick = true;
        $(".upper-canvas").contextmenu(function (event) { event.preventDefault(); return false; });
        var texts = "<%:HttpUtility.UrlDecode((Request.QueryString["texts"]??""))%>".split('|');
        var tindex = 0;
        fabHelper.init($("#Save_Hid").val(), {
            itemcb: function (fabObj) {
                //fabObj.lockMovementX = true;
                //fabObj.lockMovementY = true;
                fabObj.lockScalingFlip = true;
                fabObj.minScaleLimit = fabObj.scaleX < fabObj.scaleY ? fabObj.scaleX : fabObj.scaleY;
                //fabObj.lockScalingX = true;
                //fabObj.lockScalingY = true;
                fabObj.lockRotation = true;
                //var pos = { left: fabObj.left, top: fabObj.top, width: fabObj.width, height: fabObj.height };
                //fabObj.pos = pos;
             
                if (fabObj.cType == "img") {
                    fabObj.selectable = false;
                    //fabObj.borderColor = "#fff";
                    //fabObj.hasBorders = true;
                    //fabObj.stroke="#fff";
                    //fabObj.strokeWidth = 5;

                    //获取拖动范围的限定值
                    border.left = fabObj.left;
                    border.top = fabObj.top;
                    border.right = parseInt(fabObj.aCoords.br.x);
                    border.bottom = parseInt(fabObj.aCoords.br.y);
                }
                if (texts.length > tindex && fabObj.cType == "text" && texts[tindex]) {
                    var src = fabHelper.tool.getImageUrl(fabObj);
                    var model = fabHelper.tool.getInfoFromUrl(src, "model");
                    model.text = texts[tindex];
                    tindex++;
                    model.width = fabObj.width;
                    fabObj.setSrc(fabHelper.getTextUrl(model), function (textObj) {
                        //fabHelper.tool.calcTextPos(textObj, textObj.pos);
                        canvas.renderAll();
                    });
                }
            }
        });
        
        //canvas.on("object:scaling", function (e) {
        //    if (e.target.cType == "text") {
        //        //src 
        //        e.target.lockScalingX = (e.target.width > 100);
        //        e.target.lockScalingY = (e.target.height > 20);
        //        console.log(e.target,e.target.width,e.target.height);
        //    }
        //});
        ////-----------------------------------------------
        for (var i = 0; i < fontList.length; i++) {
            var model = fabHelper.newMod();
            model.text = "SelectStyle";
            model.bkcolor = ""; model.color = "#FFFFFF";
            model.size = 16; model.family = fontList[i];
            var url = fabHelper.getTextUrl(model);
            $("#font_ul").append('<li data-font="' + fontList[i] + '"><img src="' + url + '" /></li>');
        }
        $("#font_ul").append('<div class="clearfix"></div>');
        $("#font_ul li").click(function () {
            var obj = canvas.getActiveObject();
            if (!obj) { return; }
            $("#font_ul li").removeClass("active");
            $(this).addClass("active");
            fabHelper.update();
        });
        for (var i = 0; i < colorArr.length; i++) {
            var model = colorArr[i];
            var tlp = '<li class="colorItem" data-color="' + model.color + '"><a href="javascript:;"><span style="background-color:' + model.color + ';"></span>' + model.name + '</a></li>';
            $("#colorBody").append(tlp);
        }
        $(".colorItem").click(function () {
            var color = $(this).data("color");
            $("input#text_color").val(color);
            fabHelper.update();
        });
    })
var pic = { id: "pic_up", txtid: null };
pic.sel = function (id) { $("#" + pic.id).val(""); $("#" + pic.id).click(); pic.txtid = id; }
pic.upload = function () {
   var fname = $("#" + pic.id).val();
   if (!SFileUP.isWebImg(fname)) { alert("Please select picture file"); return false; }
    SFileUP.AjaxUpFile(pic.id, function (data) {
        $("#" + pic.txtid).val(data);
        fabHelper.update();
    });
}
pic.preSave = function () {var src = $("#pic_img").attr("src");$("#pic_hid").val(src);}
</script>
</asp:Content>