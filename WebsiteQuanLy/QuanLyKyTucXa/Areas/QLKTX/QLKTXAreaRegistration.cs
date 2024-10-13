using System.Web.Mvc;

namespace QuanLyKyTucXa.Areas.QLKTX
{
    public class QLKTXAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "QLKTX";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "QLKTX_default",
                "QLKTX/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}