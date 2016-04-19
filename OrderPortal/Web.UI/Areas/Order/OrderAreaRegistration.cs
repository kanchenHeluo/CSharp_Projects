using System.Web.Mvc;

namespace Web.UI.Areas.Order
{
    public class OrderAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Order";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Order_OrderDashboard_GetDraft",
                "Order/OrderDashboard/GetDraft",
                new { controller = "OrderDashboard", action = "GetDraft" }
            );

            context.MapRoute(
                "Order_OrderDashboard_GetOrdersWithStatus",
                "Order/OrderDashboard/GetOrdersWithStatus",
                new { controller = "OrderDashboard", action = "GetOrdersWithStatus" }
            );

            context.MapRoute(
                "Order_dashboard_catchall",
                "Order/OrderDashboard/{*catchall}",
                new { controller = "OrderDashboard", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Order_default",
                "Order/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}