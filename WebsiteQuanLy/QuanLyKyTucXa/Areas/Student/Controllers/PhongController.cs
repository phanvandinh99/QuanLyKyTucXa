using QuanLyKyTucXa.Models;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyKyTucXa.Areas.Student.Controllers
{
    public class PhongController : Controller
    {
        private readonly QLKyTucXa _db;

        public PhongController(QLKyTucXa db)
        {
            _db = db;
        }

        // Tra cứu phòng
        public async Task<ActionResult> TraCuuPhong(
           int? MaLoaiKhu,                                      // Table LoaiKhu
           int? MaKhu,                                          // Table Khu
           int? MaTang,                                         // Table Tang
           int? MaLoaiPhong                                     // Table LoaiPhong
        )
        {
            // Tìm kiếm phòng với điều kiện tham số có giá trị
            var phong = await _db.Phong
                .Include(n=>n.DichVuPhong)
                .Where(n => (!MaLoaiKhu.HasValue || n.Tang.Khu.LoaiKhu.MaLoaiKhu == MaLoaiKhu) &&
                            (!MaKhu.HasValue || n.Tang.Khu.MaKhu == MaKhu) &&
                            (!MaTang.HasValue || n.Tang.MaTang == MaTang) &&
                            (!MaLoaiPhong.HasValue || n.LoaiPhong.MaLoaiPhong == MaLoaiPhong))
                .ToListAsync();

            return View(phong);
        }
    }
}