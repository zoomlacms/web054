<%@ Page Language="C#" AutoEventWireup="true" CodeFile="List.aspx.cs" Inherits="ZoomLaCMS.Plugins.Third.Logo.List" MasterPageFile="~/Common/Master/Empty.master" %>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>Mall Center</title>
    <link href="/Template/Nrays/style/global.css?Version=20170501506670" rel="stylesheet" />
    <script src="/js/scrolltopcontrol.js"></script>
    <script src="/JS/Controls/B_User.js"></script>
    <link href="/Plugins/Third/alert/sweetalert.css" rel="stylesheet" />
    <script src="/Plugins/Third/alert/sweetalert.min.js?v=1"></script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
  <style type="text/css">

.proItem{margin-right:5px;margin-bottom:5px; width:24%;float:left;text-align:center;}
.proSize{height:30px;line-height:30px;}
.proPrice span{color:red;}
.imgItem{width:100%; border:1px solid #ddd;cursor:pointer;}

</style>
<%Call.Label("{ZL.Label id=\"全站头部\" /}"); %>

    <div class="csbanner">
        <h2>Batch style preview more convenient!</h2>
    </div>

    <div class="container pt-2">
        <nav aria-label="breadcrumb" role="navigation">
            <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="/">Home</a></li>
                <li class="breadcrumb-item active">Custom Signs Request</li>
            </ol>
        </nav>
    </div>

    <div class="container mt-2">
        <div class="home_list_r">
            <div class="row mt-2">
                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                    <input id="ct1" maxlength="20" type="text" class="form-control  ctext" placeholder="Your Text Here" />
                </div>
                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                    <input id="ct2" maxlength="20" type="text" class="form-control  ctext" placeholder="Your Text Here" />
                </div>
                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                    <input id="ct3" maxlength="20" type="text" class="form-control  ctext" placeholder="Your Text Here" />
                </div>
                <span class="input-group-btn">
                    <input type="button" class="btn btn-info" value="☞ Update Text" onclick="ledList.updateText();" />
                </span>
            </div>
            <div style="height: 1em;"></div>
            <asp:HiddenField runat="server" ID="List_Hid" />
            <div id="img_wrap" style="margin-top: 10px;"></div>
			<div class="neon_signs pd_lis2t">
			<div class="center-block text-center" style="margin:auto;width:68%;">
            <asp:Literal runat="server" EnableViewState="false" ID="Page_Lit"></asp:Literal>
			</div>
			</div>
        </div>
    </div>
<%Call.Label("{ZL.Label id=\"全站底部\" /}"); %>
    
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Script">
<style type="text/css">
</style>
<script src="/Plugins/Third/Logo/fabric/fabric.js"></script>
<script src="/Plugins/Third/Logo/sprite.class.js"></script>
<script src="/JS/ZL_Regex.js?v=1"></script>
  <%--  //1,加载指定分页的JSON模板,并处理为图片展示(即时获取)(20*4)
    //2,如果更新文字则遍历每个json模板,将其text文字修改后重新导入canvas,生成图片
    //3,点击分页加载新的canvas并重新生成--%>
    <script>
        $(function () {
            $(".ctext").change(function () {
                setCookie(this.id,this.value);
            });
            $(".ctext").each(function () {
                var $this = $(this);
                var value = getCookie(this.id);
                if (!ZL_Regex.isEmpty(value)) { $this.val(value); }
            });
        })
        function removeRootUrl(text) {

            var text = HtmlUtil.removeUrlRoot(text);
            text = text.replace(/https:\/\/raysandsigns.com/ig, "");
            return text;
        }
        
        var ledList = { list: [] };
        ledList.isText = function (obj) {
            return (obj.src.toLowerCase().indexOf(".aspx") > 0);
        }
        ledList.init = function () {
            var ref = this;
            ref.list = JSON.parse($("#List_Hid").val());
            if (ref.list.length < 1) { return; }
            //Create Image and append
            for (var i = 0; i < ref.list.length; i++) {
                //Remove RootUrl
                try {
                    ref.list[i].LogoContent = removeRootUrl(ref.list[i].LogoContent);
                    ref.list[i].LogoContent = JSON.parse(ref.list[i].LogoContent);
                } catch (ex) { console.log("removeUrlRoot", ex, ref.list[i].LogoContent); }
            }
            ref.render();
        }
        ledList.render = function () {
            $("#img_wrap").html('');
            var ref = this;
            var index = 0;
            //ensure that the sequence will not be dislocated, and will not redraw the Bug
            function showImg() {
                var fabcav = new fabric.Canvas($('<canvas id="canvas" width="1100" height="500"></canvas>')[0]);
                fabcav.loadFromJSON(ref.list[index].LogoContent, function () {
                    try{
                        var model=ref.list[index];
                        var base64 = fabcav.toDataURL("image/png");
                        var $img = $('<div class="proItem">'
                            + '<img src="' + base64 + '" class="imgItem" onclick="desTo(' + model.ProID + ');"/>'
                            + '<div class="proSize">Size: ' + model.Size + '</div>'
                            + '<div class="proSize sku">Specifications: ' + model.sku + '</div>'
                            //+ '<div class="proSize sku">SKU: ' + model.sku + '</div>'
                            //+ '<div class="proSize">VARIATIONS: ' + model.variations+ '</div>'							
                            + '<div class="proPrice">Price: $<span>' + parseFloat(model.LinPrice).toFixed(2) + '</span></div>'
                            + '<a class="btn btn-primary btn-xs" onclick="desTo(' + model.ProID + ');">CUSTOMIZE OR BUY</a>'
                            + '</div>');
                        $("#img_wrap").append($img);
                        index++;
                        if (index < ref.list.length) {
                            //if have next,still continue
                            showImg();
                        } else if (index == ref.list.length) { $("#img_wrap").append('<div style=""></div>'); }
                    } catch (ex) { console.log(ex.message,index, ref.list[index].LogoContent); }
                });
            }
            showImg();
        }
        ledList.updateText = function () {
            function getInfoFromUrl(url, name) {
                var str = url.split(name + "=")[1];
                str = str.split("&")[0];
                return JSON.parse(decodeURIComponent(str));
            }
            function getTextUrl(model,obj) {
                var imgUrl = "/Plugins/Third/Logo/CreateImg.aspx?action=text&";
                if (model.color.indexOf("(") > -1) {
                    model.color = StyleHelper.RGBTo16(model.color);
                }
                var url = imgUrl + "model=" + encodeURIComponent(JSON.stringify(model));
                //width
                if (obj) {
                    url += "&width=" + obj.width;
                    url += "&height=" + obj.height;
                }
                console.log(url);
                return url;
            }
            //update these text in the json and then render it
            var $texts = $(".ctext");
            var needCheck = [];
            ledList.list.forEach(function (value, index, array) {
                var objects = value.LogoContent.objects;
                var cindex = 0;
                for (var i = 0; i < objects.length && cindex < 3; i++) {
                    var obj = objects[i];
                    if (!ledList.isText(obj)) { continue; }
                    //update model and render
                    var model = getInfoFromUrl(obj.src, "model");
                    model.text = $texts[cindex].value;
                    if (!model.text) { model.text = "YOUR TEXT HERE"; }
                    //--------Notification to redraw these images
                    cindex++;
                    obj.src = getTextUrl(model,obj);
                    needCheck.push(obj);
                }
            });
            //IE compatible processing
            var checkImg = function (index) {
                var obj = needCheck[index];
                $('<img src="' + obj.src + '">').bind("load", function () {
                    obj.height = this.height;
                    obj.width = this.width;
                    index++;
                    if (index >= needCheck.length) { ledList.render(); return; }
                    else { checkImg(index); }
                });
            };
            checkImg(0);
        }
        ledList.init();
        function desTo(proid) {
            ///Plugins/Third/Logo/Design.aspx?ProID=' + ref.list[index].ProID+ '";
            var url = "/Plugins/Third/Logo/Design.aspx?ProID=" + proid;
            var texts = "";
            $(".ctext").each(function () {
                texts += this.value + "|";
            });
            texts = texts.substr(0,texts.length-1);
            url += "&texts=" + encodeURI(texts);
            location = url;
        }
  
    </script>
    
    
<script>	
//顶部菜单状态绑定
//顶部菜单状态绑定
$().ready(function(e) {
    $("#menu1").addClass("active");

});	
//显示搜索框
function isSearch(){
    $(".home_search").css("opacity","1").css("z-index","1").css("right","");
    $(".home_top_nav").fadeOut(200);
    $(".home_search .form-control").focus();
}
/*关闭搜索框*/
function isClose(){
    $(".home_search").css("opacity","0").css("z-index","-1").css("right","");
    $(".home_top_nav").fadeIn(1000);
}
//var swiper = new Swiper('.swiper-container', {
//  slidesPerView: 4,
//  spaceBetween: 40,
//  pagination: {
//	el: '.swiper-pagination',
//	clickable: true,
//  },
//	navigation: {
//	nextEl: '.swiper-button-next',
//	prevEl: '.swiper-button-prev',
//  }
//});
	
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
</script>    
</asp:Content>