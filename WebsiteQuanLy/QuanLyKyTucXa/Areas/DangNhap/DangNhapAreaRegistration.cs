using System.Web.Mvc;

namespace QuanLyKyTucXa.Areas.DangNhap
{
    public class DangNhapAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DangNhap";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DangNhap_default",
                "DangNhap/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}