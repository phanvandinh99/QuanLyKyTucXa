using QuanLyKyTucXa.Models;
using System.Web.Mvc;

namespace QuanLyKyTucXa.Areas.DangNhap.Controllers
{
    public class DangNhapController : Controller
    {
        private readonly QLKyTucXa _db;

        public DangNhapController(QLKyTucXa db)
        {
            _db = db;
        }

        // GET: DangNhap/DangNhap
        public ActionResult Login()
        {
            return View();
        }
    }
}