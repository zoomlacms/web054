<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Cart_Logo.aspx.cs" Inherits="Cart_Comp_Cart_Logo" %>
<%@ Import Namespace="ZoomLa.Common" %>
<table class="table table-bordered table-striped" style="margin-top:10px;color:#999;font-size:12px;">
<%-- <tr><td class="td_m">PreView：</td><td><a href="<%=addMod.preview %>" target="_blank"><img src="<%=addMod.preview %>" style="width:50px;height:50px;"/></a></td></tr>--%>
 <tr><td class="td_m">Text：</td><td><%=addMod.text %></td></tr>
 <tr><td>Size：</td><td><%=addMod.size %></td></tr>
 <tr><td>Flash：</td><td>
     <select class="form-control text_300 flash_dp" data-name="flash">
         <%=ZoomLa.Sns.SnsHelper.H_GetDPOption("flash","none",addMod.flash.ToString()) %>
     </select>
       </td></tr>
       <tr>
        <td>Outdoor Sign：</td>
        <td>
            <select class="form-control text_300 flash_dp" data-name="outdoor">
                <%=ZoomLa.Sns.SnsHelper.H_GetDPOption("outdoor","No",addMod.outdoor.ToString()) %>
            </select>
        </td>
    </tr>
    <%if (Convert.ToInt32(item["NodeID"]) != 8 && Convert.ToInt32(item["NodeID"]) != 106)
        { %>
    <tr>
        <td>Backing Material：</td>
        <td>
            <select class="form-control text_300 flash_dp" data-name="backing">
                <%=ZoomLa.Sns.SnsHelper.H_GetDPOption("backing", "Metal Frame", addMod.backing.ToString()) %>
            </select>
        </td>
        <%} %>
    </tr>
</table>
