﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegionPrice.aspx.cs" Inherits="ZoomLaCMS.Manage.Shop.RegionPrice"  MasterPageFile="~/Manage/I/Index.master" %>
<asp:Content runat="server" ContentPlaceHolderID="head"><title></title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<div class="container">
<div class="row">
<div class="col-12 col-md-4">
<div id="province_div"></div> 
</div>
<div class="col-12 col-md-8">
<div id="city_div"></div>
<table class="table table-bordered table-striped" >
    <tr><td class="td_m">会员组</td><td>价格</td></tr>
    <asp:Repeater runat="server" ID="Group_RPT" EnableViewState="false">
    <ItemTemplate>
        <tr>
            <td><%#Eval("GroupName") %></td>
            <td>
                <input type="text" value="0" class="form-control price_t float" data-gid="<%#Eval("GroupID") %>" />
            </td>
        </tr>
    </ItemTemplate>
    </asp:Repeater>
    </table>
</div>
</div></div>
<div class="Conent_fix">
    <input type="button" value="保存信息" class="btn btn-outline-info" onclick="save();" />
    <asp:Button runat="server" ID="Save_Btn" OnClick="Save_Btn_Click" style="display:none;"/>
</div>
<asp:HiddenField runat="server" ID="Save_Hid" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
<script src="/JS/ICMS/area.js"></script>
<script src="/JS/Controls/ZL_Array.js"></script>
<script src="/JS/ZL_Regex.js"></script>
<script>
$(function () {
    ZL_Regex.B_Float(".float");
    var tlp = '<label class="btn btn-default wrapchk"><input class="itemchk" name="province_chk" type="checkbox" data-region="@Name" value="@ID" />@Name</label>';
    var $items = JsonHelper.FillItem(tlp, AreaMod, function ($item, model) {
        $item.find("input").click(function () {
            AddCitys($item, model);
        });
    });
    $("#province_div").append($items);
})
//每个省份的城市,单独一个div
function AddCitys($item, model) {
    $container = $("#city_div");
    var chk = $item.find("input")[0];
    var id = "city_" + model.ID;
    var list = AreaMod.GetByID(model.ID, "ID").CityList;
    $info_div = $container.find("#" + id);
    if (chk.checked) {
        //是否有隐藏,无隐藏的话则新增
        if ($info_div.length < 1) {
            var $wrap = $('<div class="panel panel-warning" id="' + id + '"><div class="panel-heading">' + model.Name + ' <input type="button" value="隐藏县信息" class="btn btn-outline-info" onclick="showChild(this,\''+id+'\');"></div><div class="panel-body"></div></div>');
            var tlp = '<label class="wrapchk"><input type="checkbox" class="itemchk" name="city_chk" data-region="' + model.Name + '@Name" value="@ID">@Name</label>';
            var $citys = JsonHelper.FillItem(tlp, list, function ($item, model) {
                $item.find("input").click(function () {
                    AddCountys($item, model);
                });
            });
            $wrap.find(".panel-body").append($citys);
            $container.append($wrap);
        } else { $info_div.show(); }
    }
    else {
        //隐掉即可,用于避免误操作
        $info_div.hide();
    }
}
//市下面的县信息
function AddCountys($item, model) {
    $container = $("#city_" + model.Pid);
    var chk = $item.find("input")[0];
    var id = "county_" + model.ID;
    var list = AreaMod.GetByID(model.Pid, "ID").CityList.GetByID(model.ID, "ID").CountyList;
    $info_div = $container.find("#" + id);
    if (chk.checked) {
        if ($info_div.length < 1) {
            var $wrap = $('<div class="item" id="' + id + '"><div class="i_head">' + model.Name + '</div><div class="i_content"></div></div>');
            var tlp = '<label class="wrapchk"><input type="checkbox" class="itemchk" name="county_chk" data-region="' +$(chk).data("region")+ '@Name" value="@ID">@Name</label>';
            var $citys = JsonHelper.FillItem(tlp, list, function () { });
            $wrap.find(".i_content").append($citys);
            $container.append($wrap);
        } else { $info_div.show(); }
    }
    else {
        $info_div.hide();
    }
}
function showChild(obj, id) {
    var $obj = $(obj);
    if ($obj.val() == "隐藏县信息") { $obj.val("显示县信息"); $("#" + id).find(".item").hide(); }
    else
    { $obj.val("隐藏县信息"); $("#" + id).find(".item").show(); }
}
//后期是否将价格模板化,用于方便修改
//改为根据所选只取最下一级,如勾选了北京市--西城市,则不会再保存北京与北京--北京市
function save() {
    var list = [];
    var price = [];
    var isChildBeChoose = function (region) {
        //检测的时候子级尚未被存入
        for (var i = 0; i < list.length; i++) {
            var str = list[i].region;
            if (str != region && str.indexOf(region) == 0) { return true; }
        }
        return false;
    }
    $(".price_t").each(function () {
        var $input = $(this);
       
        price.push({ gid: $input.data("gid"), price: Convert.ToDouble($input.val()) });
    });
    //县-->市-->省
    $(".itemchk[name='county_chk']:visible:checked").each(function () {
        var $chk = $(this);
        var model = { region: $chk.data("region"), id: $chk.val(), "price": price };
        list.push(model);
    });
    $(".itemchk[name='city_chk']:visible:checked").each(function () {
        var $chk = $(this);
        var model = { region: $chk.data("region"), id: $chk.val(), "price": price };
        //如果其下有子级被选中,则不存储自身
        if (!isChildBeChoose($chk.data("region"))) {
            list.push(model);
        }
    });
    $(".itemchk[name='province_chk']:visible:checked").each(function () {
        var $chk = $(this);
        var model = { region: $chk.data("region"), id: $chk.val(), "price": price };
        if (!isChildBeChoose($chk.data("region"))) {
            list.push(model);
        }
    });
    $("#Save_Hid").val(JSON.stringify(list));
    $("#Save_Btn").click();
}
</script>
</asp:Content>