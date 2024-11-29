using QuanLyKyTucXa.Models;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace QuanLyKyTucXa.Areas.Student.Controllers
{
    public class PartialController : Controller
    {
        private readonly QLKyTucXa _db;

        public PartialController(QLKyTucXa db)
        {
            _db = db;
        }

        // Hiển thị menu partial trang chi tiết
        public ActionResult LeftMenuOfDetailed()
        {
            ViewBag.listPhong = _db.Phong.Where(n => n.ConTrong != 0)
                                         .ToList().Take(10);

            return PartialView("LeftMenuOfDetailed");
        }
    }
}