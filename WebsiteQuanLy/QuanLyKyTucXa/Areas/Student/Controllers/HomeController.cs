using QuanLyKyTucXa.Models;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyKyTucXa.Areas.Student.Controllers
{
    public class HomeController : Controller
    {
        private readonly QLKyTucXa _db;

        public HomeController(QLKyTucXa db)
        {
            _db = db;
        }

        // Trang chủ Sinh Viên
        public async Task<ActionResult> Index()
        {
            ViewBag.listLoaiKhu = await _db.LoaiKhu.ToListAsync();
            ViewBag.listKhu = await _db.Khu.ToListAsync();
            ViewBag.listTang = await _db.Tang.ToListAsync();
            ViewBag.listLoaiPhong = await _db.LoaiPhong.ToListAsync();

            return View();
        }

        public async Task<JsonResult> GetKhuByLoaiKhu(int maLoaiKhu)
        {
            var listKhu = await _db.Khu
                                   .Where(k => k.MaLoaiKhu == maLoaiKhu)
                                   .Select(k => new
                                   {
                                       k.MaKhu,
                                       k.TenKhu
                                   })
                                   .ToListAsync();
            return Json(listKhu, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetTangByKhu(int maKhu)
        {
            var listTang = await _db.Tang
                                    .Where(t => t.MaKhu == maKhu)
                                    .Select(t => new
                                    {
                                        t.MaTang,
                                        t.TenTang
                                    })
                                    .ToListAsync();
            return Json(listTang, JsonRequestBehavior.AllowGet);
        }


        // Tra Cứu Phòng
        public async Task<ActionResult> TraCuuPhong(
           int? MaLoaiKhu,   // Table LoaiKhu
           int? MaKhu,       // Table Khu
           int? MaTang,      // Table Tang
           int? MaLoaiPhong  // Table LoaiPhong
        )
        {
            // Tìm kiếm phòng với điều kiện tham số có giá trị
            var phong = await _db.Phong
                .Where(n => (!MaLoaiKhu.HasValue || n.Tang.Khu.LoaiKhu.MaLoaiKhu == MaLoaiKhu) &&
                            (!MaKhu.HasValue || n.Tang.Khu.MaKhu == MaKhu) &&
                            (!MaTang.HasValue || n.Tang.MaTang == MaTang) &&
                            (!MaLoaiPhong.HasValue || n.LoaiPhong.MaLoaiPhong == MaLoaiPhong))
                .ToListAsync();

            return View(phong);
        }

    }
}