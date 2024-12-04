using QuanLyKyTucXa.Common;
using QuanLyKyTucXa.Models;
using System.Runtime.InteropServices;
using System.Web.Mvc;
using System.Web.Routing;
using Unity;
using Unity.Mvc5;

namespace QuanLyKyTucXa
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Cấu hình Unity
            var container = BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            // Đăng ký kiểm tra login toàn cục
            GlobalFilters.Filters.Add(new AuthAttribute());
        }

        private IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<QLKyTucXa, QLKyTucXa>();

            return container;
        }
    }
}
