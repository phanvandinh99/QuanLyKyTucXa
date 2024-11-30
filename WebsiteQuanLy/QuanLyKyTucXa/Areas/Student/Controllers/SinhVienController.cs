using QuanLyKyTucXa.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKyTucXa.Areas.Student.Controllers
{
    public class SinhVienController : Controller
    {
        private readonly QLKyTucXa _db;

        public SinhVienController(QLKyTucXa db)
        {
            _db = db;
        }

        // Trang đăng ký sinh viên
        public async Task<ActionResult> DangKy()
        {
            try
            {
                // Dữ liệu tìm kiếm
                ViewBag.listLoaiKhu = await _db.LoaiKhu.ToListAsync();
                ViewBag.listKhu = await _db.Khu.ToListAsync();
                ViewBag.listTang = await _db.Tang.ToListAsync();

                List<DienChinhSach> listDienChinhSach = await _db.DienChinhSach.ToListAsync();
                return View(listDienChinhSach);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Đăng ký thông tin thất bại.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> DangKy(HttpPostedFileBase AnhChanDung, SinhVien sinhVienModel, List<int> ChinhSach)
        {
            try
            {
                // Kiểm tra mã sinh viên đã tồn tại hay chưa
                SinhVien maSinhVien = await _db.SinhVien.FindAsync(sinhVienModel.MaSinhVien);
                if (maSinhVien == null)
                {
                    TempData["ToastMessage"] = "error|Mã sinh viên đã tồn tại trong hệ thống.";
                    return View();
                }

                // Xử lý upload ảnh
                string tenAnh = null;
                if (AnhChanDung != null && AnhChanDung.ContentLength > 0)
                {
                    tenAnh = Path.GetFileName(AnhChanDung.FileName);
                    var duongDan = Path.Combine(Server.MapPath("~/Assets/Student/images/AnhSinhVien"), tenAnh);

                    // Lưu ảnh nếu chưa tồn tại
                    if (!System.IO.File.Exists(duongDan))
                    {
                        AnhChanDung.SaveAs(duongDan);
                    }
                }

                // Khởi tạo đối tượng sinh viên
                var sinhVien = new SinhVien
                {
                    MaSinhVien = sinhVienModel.MaSinhVien,
                    MatKhau = sinhVienModel.MatKhau,
                    AnhChanDung = tenAnh,
                    Ho = sinhVienModel.Ho,
                    Ten = sinhVienModel.Ten,
                    GioiTinh = sinhVienModel.GioiTinh,
                    NgaySinh = sinhVienModel.NgaySinh,
                    Email = sinhVienModel.Email,
                    SDT = sinhVienModel.SDT,
                    DanToc = sinhVienModel.DanToc,
                    DiemUuTien = 0,
                    TrangThai = false
                };

                _db.SinhVien.Add(sinhVien);

                // Thêm chính sách nếu có
                if (ChinhSach?.Any() == true)
                {
                    var dsChinhSach = ChinhSach.Select(maDichVu => new SinhVienChinhSach
                    {
                        MaSinhVien = sinhVien.MaSinhVien,
                        MaDienChinhSach = maDichVu,
                        TrangThai = false
                    });

                    _db.SinhVienChinhSach.AddRange(dsChinhSach);
                }

                // Lưu thay đổi vào DB
                await _db.SaveChangesAsync();

                TempData["ToastMessage"] = "success|Đăng ký thông tin thành công.";
                return View();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Console.WriteLine($"Thuộc tính: {validationError.PropertyName}, Lỗi: {validationError.ErrorMessage}");
                    }
                }

                TempData["ToastMessage"] = "error|Đăng ký thông tin thất bại do lỗi xác thực dữ liệu.";
                return View();
            }
        }

    }
}