using QuanLyKyTucXa.Common.Const;
using QuanLyKyTucXa.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKyTucXa.Areas.Admin.Controllers
{
    public class NhanVienController : Controller
    {
        private readonly QLKyTucXa _db;

        public NhanVienController(QLKyTucXa db)
        {
            _db = db;
        }

        // Trang danh sách nhân viên ban quản lý
        public async Task<ActionResult> Index()
        {
            try
            {
                List<NhanVien> listNhanVien = await _db.NhanVien.Where(n => n.TrangThai == Constant.HoatDong &&
                                                                       n.MaQuyen == Constant.BanQuanLy)
                                                                .ToListAsync();

                return View(listNhanVien);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Xem danh sách ban quản lý thất bại.";
                return RedirectToAction("Index", "NhanVien");
            }
        }

        // Trang danh sách nhân viên ban quản lý đã xóa
        public async Task<ActionResult> NhanVienDaXoa()
        {
            try
            {
                List<NhanVien> listNhanVien = await _db.NhanVien.Where(n => n.TrangThai == Constant.Xoa &&
                                                                       n.MaQuyen == Constant.BanQuanLy)
                                                                .ToListAsync();

                return View(listNhanVien);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Xem danh sách ban quản lý thất bại.";
                return RedirectToAction("Index", "NhanVien");
            }
        }

        #region Thêm mới nhân viên
        [HttpGet]
        public ActionResult ThemMoi()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> ThemMoi(HttpPostedFileBase AnhChanDung, NhanVien nhanVienModel)
        {
            try
            {
                // Kiểm tra tài khoản đã tồn tại hay chưa
                NhanVien taiKhoanNV = await _db.NhanVien.FindAsync(nhanVienModel.TaiKhoanNV);
                if (taiKhoanNV != null)
                {
                    TempData["ToastMessage"] = "error|Tài khoản nhân viên đã tồn tại.";
                    return View();
                }

                // Xử lý upload ảnh
                string tenAnh = null;
                if (AnhChanDung != null && AnhChanDung.ContentLength > 0)
                {
                    tenAnh = Path.GetFileName(AnhChanDung.FileName);
                    var duongDan = Path.Combine(Server.MapPath("~/Assets/Admin/images/AnhNhanVien"), tenAnh);

                    // Lưu ảnh nếu chưa tồn tại
                    if (!System.IO.File.Exists(duongDan))
                    {
                        AnhChanDung.SaveAs(duongDan);
                    }
                }

                var nhanVien = new NhanVien
                {
                    TaiKhoanNV = nhanVienModel.TaiKhoanNV,
                    MatKhau = nhanVienModel.MatKhau,
                    AnhChanDung = tenAnh,
                    Ho = nhanVienModel.Ho,
                    Ten = nhanVienModel.Ten,
                    GioiTinh = nhanVienModel.GioiTinh,
                    NgaySinh = nhanVienModel.NgaySinh,
                    Email = nhanVienModel.Email,
                    SDT = nhanVienModel.SDT,
                    TrangThai = nhanVienModel.TrangThai,
                    MaQuyen = Constant.BanQuanLy
                };

                _db.NhanVien.Add(nhanVien);
                await _db.SaveChangesAsync();

                TempData["ToastMessage"] = "success|Thêm mới nhân viên thành công.";
                return RedirectToAction("Index", "NhanVien");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                TempData["ToastMessage"] = "error|Thêm mới nhân viên thất bại.";
                return RedirectToAction("Index", "NhanVien");
            }
        }
        #endregion

        #region Cập nhật nhân viên
        public async Task<ActionResult> CapNhat(String sTaiKhoanNV)
        {
            try
            {
                NhanVien nhanVien = await _db.NhanVien.FindAsync(sTaiKhoanNV);
                if (null == nhanVien)
                {
                    TempData["ToastMessage"] = "error|Không tìm thấy nhân viên.";
                    return RedirectToAction("Index", "NhanVien");
                }

                return View(nhanVien);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Cập nhật nhân viên thất bại.";
                return RedirectToAction("Index", "NhanVien");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CapNhatMatKhau(String sTaiKhoanNV, String sMatKhau)
        {
            try
            {
                NhanVien nhanVien = await _db.NhanVien.FindAsync(sTaiKhoanNV);
                if (null == nhanVien)
                {
                    TempData["ToastMessage"] = "error|Không tìm thấy nhân viên.";
                    return RedirectToAction("Index", "NhanVien");
                }
                nhanVien.MatKhau = sMatKhau;
                await _db.SaveChangesAsync();

                TempData["ToastMessage"] = "success|Cập nhật mật khẩu thành công.";
                return RedirectToAction("CapNhat", "NhanVien", new { sTaiKhoanNV = sTaiKhoanNV });
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Cập nhật nhân viên thất bại.";
                return RedirectToAction("Index", "NhanVien");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> CapNhatThongTin(HttpPostedFileBase AnhChanDung, NhanVien nhanVienModel)
        {
            try
            {
                // Kiểm tra tài khoản đã tồn tại hay chưa
                NhanVien nhanVien = await _db.NhanVien.FindAsync(nhanVienModel.TaiKhoanNV);
                if (nhanVien == null)
                {
                    TempData["ToastMessage"] = "error|Tài khoản nhân viên không tồn tại.";
                    return View();
                }

                // Xử lý upload ảnh nếu có
                if (AnhChanDung != null && AnhChanDung.ContentLength > 0)
                {
                    string tenAnh = Path.GetFileName(AnhChanDung.FileName);
                    var duongDan = Path.Combine(Server.MapPath("~/Assets/Admin/images/AnhNhanVien"), tenAnh);

                    // Lưu ảnh nếu chưa tồn tại
                    if (!System.IO.File.Exists(duongDan))
                    {
                        AnhChanDung.SaveAs(duongDan);
                        nhanVien.AnhChanDung = tenAnh;
                    }
                }

                nhanVien.Ho = nhanVienModel.Ho;
                nhanVien.Ten = nhanVienModel.Ten;
                nhanVien.GioiTinh = nhanVienModel.GioiTinh;
                nhanVien.NgaySinh = nhanVienModel.NgaySinh;
                nhanVien.Email = nhanVienModel.Email;
                nhanVien.SDT = nhanVienModel.SDT;
                await _db.SaveChangesAsync();

                TempData["ToastMessage"] = "success|Cập nhật thông tin thành công.";
                return RedirectToAction("CapNhat", "NhanVien", new { sTaiKhoanNV = nhanVien.TaiKhoanNV });
            }
            catch (Exception ex)
            {
                // Ghi log chi tiết lỗi
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Cập nhật nhân viên thất bại.";
                return RedirectToAction("Index", "NhanVien");
            }
        }

        #endregion


        // Cập nhật trạng thái xóa nhân viên
        public async Task<ActionResult> CapNhatXoaNhanVien(String sTaiKhoanNV)
        {
            try
            {
                NhanVien nhanVien = await _db.NhanVien.FindAsync(sTaiKhoanNV);
                if (null == nhanVien)
                {
                    TempData["ToastMessage"] = "error|Không tìm thấy nhân viên.";
                    return RedirectToAction("Index", "NhanVien");
                }
                if (nhanVien.TrangThai == Constant.HoatDong)
                {
                    nhanVien.TrangThai = Constant.Xoa;
                    await _db.SaveChangesAsync();

                    TempData["ToastMessage"] = "success|Đã xóa nhân viên.";

                    return RedirectToAction("Index", "NhanVien");
                }
                else
                {
                    nhanVien.TrangThai = Constant.HoatDong;
                    await _db.SaveChangesAsync();

                    TempData["ToastMessage"] = "success|Đã hoàn tác nhân viên.";

                    return RedirectToAction("NhanVienDaXoa", "NhanVien");
                }
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Xóa nhân viên thất bại.";
                return RedirectToAction("Index", "NhanVien");
            }
        }
    }
}