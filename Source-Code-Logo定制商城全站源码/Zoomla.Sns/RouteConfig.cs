using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using ZoomLa.Model;

namespace ZoomLa.Sns
{
    public class RouteConfig
    {
        public static List<M_MVCRoute> GetRoutes()
        {
            List<M_MVCRoute> routeList = new List<M_MVCRoute>();
            //routeList.Add(new M_MVCRoute() { url = "User/Content/Test2", controller = "ZLPlug", action = "Test2" });
            //routeList.Add(new M_MVCRoute() { url = "ZLPlug/{action}", controller = "ZLPlug", action = "Test" });
            return routeList;
            ////控制器路由
            //routes.MapRoute("Plug1", "ZLPlug/{action}", defaults: new { controller = "ZLPlug", action = "Test" });
            ////页面路由
            //routes.MapRoute("Plug2", "User/Content/Test2", defaults: new { controller = "ZLPlug", action = "Test2" });
        }
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.MapPageRoute("ColumnList_html", "Class_{ID}/Default.html", "~/BU/Front/ColumnList.aspx");
            //routes.MapPageRoute("ColumnList2_html", "Class_{ID}/Default_{CPage}.html", "~/BU/Front/ColumnList.aspx");
            //routes.MapPageRoute("NodePage_html", "Class_{ID}/NodePage.html", "~/BU/Front/NodePage.html");
            //routes.MapPageRoute("NodePage2_html", "Class_{ID}/NodePage_{CPage}.html", "~/BU/Front/NodePage.html");
            //routes.MapPageRoute("NodeNews_html", "Class_{ID}/NodeNews.html", "~/BU/Front/NodeNews.aspx");
            //routes.MapPageRoute("NodeNews2_html", "Class_{ID}/NodeNews_{CPage}.html", "~/BU/Front/NodeNews.aspx");
            //routes.MapPageRoute("NodeHot_html", "Class_{ID}/NodeHot.html", "~/BU/Front/NodeHot.aspx");
            //routes.MapPageRoute("NodeHot2_html", "Class_{ID}/NodeHot_{CPage}.html", "~/BU/Front/NodeHot.aspx");
            //routes.MapPageRoute("NodeElite_html", "Class_{ID}/NodeElite.html", "~/BU/Front/NodeElite.aspx");
            //routes.MapPageRoute("NodeElite2_html", "Class_{ID}/NodeElite_{CPage}.html", "~/BU/Front/NodeElite.aspx");
            //routes.MapPageRoute("Shop_html", "Shop/{ID}.html", "~/BU/Front/Shop.aspx");
            //routes.MapPageRoute("Content_html", "Item/{ID}_{CPage}.html", "~/BU/Front/Content.aspx");
            //routes.MapPageRoute("Content2_html", "Item/{ID}.html", "~/BU/Front/Content.aspx");
        }
    }
}
