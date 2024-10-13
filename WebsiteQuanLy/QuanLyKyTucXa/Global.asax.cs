using QuanLyKyTucXa.Models;
using System.Web.Mvc;
using System.Web.Routing;
using Unity;

namespace QuanLyKyTucXa
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        private IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<QLKyTucXa, QLKyTucXa>();

            return container;
        }
    }
}
