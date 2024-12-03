using QuanLyKyTucXa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKyTucXa.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly QLKyTucXa _db;

        public HomeController(QLKyTucXa db)
        {
            _db = db;
        }

        // Trang chủ Admin
        public async Task<ActionResult> Index()
        {
            return View();
        }
    }
}