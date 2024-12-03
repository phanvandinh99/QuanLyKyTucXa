using QuanLyKyTucXa.Common.Const;
using QuanLyKyTucXa.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyKyTucXa.Areas.Admin.Controllers
{
    public class SinhVienController : Controller
    {
        private readonly QLKyTucXa _db;

        public SinhVienController(QLKyTucXa db)
        {
            _db = db;
        }

        // Danh sách sinh viên hoạt động
        public async Task<ActionResult> Index()
        {
            try
            {
                List<SinhVien> listSinhVien = await _db.SinhVien.Where(n => n.TrangThai == Constant.DaXacThucTaiKhoan &&
                                                                       n.DaXoa == Constant.HoatDong)
                                                                .ToListAsync();

                return View(listSinhVien);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Không tìm thấy danh sách sinh viên.";
                return RedirectToAction("Index", "SinhVien");
            }
        }

        // Danh sách sinh viên đã xóa
        public async Task<ActionResult> SinhVienDaXoa()
        {
            try
            {
                List<SinhVien> listSinhVien = await _db.SinhVien.Where(n => n.TrangThai == Constant.DaXacThucTaiKhoan &&
                                                                       n.DaXoa == Constant.Xoa)
                                                                .ToListAsync();

                return View(listSinhVien);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Không tìm thấy danh sách sinh viên.";
                return RedirectToAction("Index", "SinhVien");
            }
        }

        // Danh sách sinh viên cần xác thực tài khoản
        public async Task<ActionResult> XacThucSinhVien()
        {
            try
            {
                List<SinhVien> listSinhVien = await _db.SinhVien.Where(n => n.TrangThai == Constant.CanXacThucTaiKhoan)
                                                                .ToListAsync();

                return View(listSinhVien);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Không tìm thấy danh sách sinh viên.";
                return RedirectToAction("Index", "SinhVien");
            }
        }

        // Xóa sinh viên khỏi hệ thống
        public async Task<ActionResult> XoaSinhVien(String sMaSinhVien)
        {
            try
            {
                SinhVien sinhVien = await _db.SinhVien.FindAsync(sMaSinhVien);
                if (null == sinhVien)
                {
                    TempData["ToastMessage"] = "error|Không tìm thấy sinh viên.";
                    return RedirectToAction("XacThucSinhVien", "SinhVien");
                }
                _db.SinhVien.Remove(sinhVien);
                await _db.SaveChangesAsync();

                TempData["ToastMessage"] = "success|Xóa sinh viên thành công.";

                return RedirectToAction("XacThucSinhVien", "SinhVien");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Không thể xóa sinh viên.";
                return RedirectToAction("XacThucSinhVien", "SinhVien");
            }
        }

        // Xem chi tiết sinh viên
        public async Task<ActionResult> XemChiTiet(String sMaSinhVien)
        {
            try
            {
                SinhVien sinhVien = await _db.SinhVien.FindAsync(sMaSinhVien);
                if (null == sinhVien)
                {
                    TempData["ToastMessage"] = "error|Không tìm thấy sinh viên.";
                    return RedirectToAction("XacThucSinhVien", "SinhVien");
                }

                return View(sinhVien);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Xem chi tiết thất bại.";
                return RedirectToAction("XacThucSinhVien", "SinhVien");
            }
        }

        // Xem chi tiết sinh viên
        public async Task<ActionResult> CapNhat(String sMaSinhVien)
        {
            try
            {
                SinhVien sinhVien = await _db.SinhVien.FindAsync(sMaSinhVien);
                if (null == sinhVien)
                {
                    TempData["ToastMessage"] = "error|Không tìm thấy sinh viên.";
                    return RedirectToAction("XacThucSinhVien", "SinhVien");
                }

                return View(sinhVien);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Xem chi tiết thất bại.";
                return RedirectToAction("XacThucSinhVien", "SinhVien");
            }
        }

        // Xóa diện chính sách sinh viên
        public async Task<ActionResult> XoaDienChinhSach(int iMaSVChinhSach)
        {
            try
            {
                SinhVienChinhSach sinhVienChinhSach = await _db.SinhVienChinhSach.FindAsync(iMaSVChinhSach);
                if (null == sinhVienChinhSach)
                {
                    TempData["ToastMessage"] = "error|Không tìm thấy diện chính sách sinh viên.";
                    return RedirectToAction("XacThucSinhVien", "SinhVien");
                }
                String maSinhVien = sinhVienChinhSach.MaSinhVien;

                _db.SinhVienChinhSach.Remove(sinhVienChinhSach);
                await _db.SaveChangesAsync();

                TempData["ToastMessage"] = "success|Xóa diện chính sách sinh viên.";

                return RedirectToAction("CapNhat", "SinhVien", new { sMaSinhVien = maSinhVien });
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Xem diện chính sách thất bại.";
                return RedirectToAction("XacThucSinhVien", "SinhVien");
            }
        }

        // Cập nhật trạng thái xóa sinh viên
        public async Task<ActionResult> CapNhatXoaSinhVien(String sMaSinhVien)
        {
            try
            {
                SinhVien sinhVien = await _db.SinhVien.FindAsync(sMaSinhVien);
                if (null == sinhVien)
                {
                    TempData["ToastMessage"] = "error|Không tìm thấy sinh viên.";
                    return RedirectToAction("Index", "SinhVien");
                }
                if (sinhVien.DaXoa == Constant.HoatDong)
                {
                    sinhVien.DaXoa = Constant.Xoa;
                    await _db.SaveChangesAsync();

                    TempData["ToastMessage"] = "success|Đã xóa sinh viên.";

                    return RedirectToAction("Index", "SinhVien");
                }
                else
                {
                    sinhVien.DaXoa = Constant.HoatDong;
                    await _db.SaveChangesAsync();

                    TempData["ToastMessage"] = "success|Đã hoàn tác sinh viên.";

                    return RedirectToAction("SinhVienDaXoa", "SinhVien");
                }
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Xóa sinh viên thất bại.";
                return RedirectToAction("Index", "SinhVien");
            }
        }

        public async Task<ActionResult> DuyetSinhVien(String sMaSinhVien)
        {
            try
            {
                SinhVien sinhVien = await _db.SinhVien.FindAsync(sMaSinhVien);
                if (null == sinhVien)
                {
                    TempData["ToastMessage"] = "error|Không tìm thấy sinh viên.";
                    return RedirectToAction("XacThucSinhVien", "SinhVien");
                }

                var sinhVienChinhSach = await _db.SinhVienChinhSach.Where(sv => sv.MaSinhVien == sMaSinhVien)
                                                                   .ToListAsync();

                double diemUuTien = 0;
                foreach (var item in sinhVienChinhSach)
                {
                    var dienChinhSach = await _db.DienChinhSach.FindAsync(item.MaDienChinhSach);
                    if (dienChinhSach != null)
                    {
                        diemUuTien += dienChinhSach.DiemDienChinhSach;
                    }
                }

                sinhVien.DiemUuTien = diemUuTien;
                sinhVien.TrangThai = Constant.DaXacThucTaiKhoan;

                await _db.SaveChangesAsync();

                TempData["ToastMessage"] = "success|Đã duyệt thông tin sinh viên.";

                return RedirectToAction("XacThucSinhVien", "SinhVien");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Đã duyệt thông tin thất bại.";
                return RedirectToAction("XacThucSinhVien", "SinhVien");
            }
        }

        public async Task<ActionResult> DuyetDanhSach(List<string> sMaSinhVien)
        {
            try
            {
                if (sMaSinhVien != null && sMaSinhVien.Any())
                {
                    foreach (var maSinhVien in sMaSinhVien)
                    {
                        SinhVien sinhVien = await _db.SinhVien.FindAsync(maSinhVien);

                        var sinhVienChinhSach = await _db.SinhVienChinhSach.Where(sv => sv.MaSinhVien == maSinhVien)
                                                                   .ToListAsync();

                        double diemUuTien = 0;
                        foreach (var item in sinhVienChinhSach)
                        {
                            var dienChinhSach = await _db.DienChinhSach.FindAsync(item.MaDienChinhSach);
                            if (dienChinhSach != null)
                            {
                                diemUuTien += dienChinhSach.DiemDienChinhSach;
                            }
                        }

                        sinhVien.DiemUuTien = diemUuTien;
                        sinhVien.TrangThai = Constant.DaXacThucTaiKhoan;
                    }
                    await _db.SaveChangesAsync();

                    TempData["ToastMessage"] = "success|Duyệt danh sách sinh viên.";
                }
                else
                {
                    TempData["ToastMessage"] = "error|Không có sinh viên duyệt.";
                }

                return RedirectToAction("XacThucSinhVien", "SinhVien");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Đã duyệt thông tin thất bại.";
                return RedirectToAction("XacThucSinhVien", "SinhVien");
            }
        }


    }
}