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
    public class PhongController : Controller
    {
        private readonly QLKyTucXa _db;

        public PhongController(QLKyTucXa db)
        {
            _db = db;
        }

        // Trang danh sách Phòng
        public async Task<ActionResult> Index()
        {
            try
            {
                List<Phong> listPhong = await _db.Phong.OrderByDescending(n => n.MaPhong).ToListAsync();

                return View(listPhong);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Xem danh sách phòng thất bại.";
                return RedirectToAction("Index", "Phong");
            }
        }

        #region Thêm mới Phòng
        public async Task<ActionResult> ThemMoi()
        {
            try
            {
                ViewBag.Tang = await _db.Tang.ToListAsync();
                ViewBag.LoaiPhong = await _db.LoaiPhong.ToListAsync();
                ViewBag.DichVu = await _db.DichVu.ToListAsync();

                return View();
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Thêm phòng thất bại.";
                return RedirectToAction("Index", "Phong");
            }
        }

        [HttpPost]
        public async Task<ActionResult> ThemMoi(Phong phongModel, List<int> listDichVu)
        {
            try
            {
                // Lấy thông tin loại phòng để lấy số giường
                LoaiPhong loaiPhong = await _db.LoaiPhong.FindAsync(phongModel.MaLoaiPhong);
                if (loaiPhong == null)
                {
                    TempData["ToastMessage"] = "error|Loại phòng không tồn tại.";
                    return RedirectToAction("Index", "Phong");
                }

                // Thiết lập các thuộc tính mặc định cho phòng
                phongModel.DaO = 0;
                phongModel.ConTrong = loaiPhong.SoGiuong;
                phongModel.GiaDichVu = 0;  // Khởi tạo tổng giá dịch vụ là 0
                phongModel.MaTrangThai = Constant.DangHoatDong;
                _db.Phong.Add(phongModel);

                // Tạo số giường cho phòng
                for (int i = 1; i <= loaiPhong.SoGiuong; i++)
                {
                    Giuong giuong = new Giuong
                    {
                        TenGiuong = "Giường " + i,
                        TrangThai = Constant.GiuongTrong,
                        MaPhong = phongModel.MaPhong
                    };
                    _db.Giuong.Add(giuong);
                }

                // Tạo dịch vụ phòng và tính tổng giá dịch vụ
                double tongGiaDichVu = 0;
                if (listDichVu?.Any() == true)
                {
                    var dsDichVu = listDichVu.Select(maDichVu => new DichVuPhong
                    {
                        Xoa = Constant.DichVuPhong,
                        NgayThem = DateTime.Now,
                        MaPhong = phongModel.MaPhong,
                        MaDichVu = maDichVu
                    });

                    _db.DichVuPhong.AddRange(dsDichVu);

                    // Tính tổng giá dịch vụ dựa trên các dịch vụ đã chọn
                    foreach (var maDichVu in listDichVu)
                    {
                        var dichVu = await _db.DichVu.FindAsync(maDichVu);
                        if (dichVu != null)
                        {
                            tongGiaDichVu += dichVu.DonGia;
                        }
                    }
                }

                // Cập nhật tổng giá dịch vụ cho phòng
                phongModel.GiaDichVu = tongGiaDichVu;
                await _db.SaveChangesAsync();

                TempData["ToastMessage"] = "success|Thêm phòng thành công.";
                return RedirectToAction("Index", "Phong");
            }
            catch (Exception ex)
            {
                // Ghi log lỗi (tùy thuộc vào cơ chế log của bạn)
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Thêm phòng thất bại.";
                return RedirectToAction("Index", "Phong");
            }
         }

        #endregion

        #region Cập nhật Phòng
        public async Task<ActionResult> CapNhat(int iMaPhong)
        {
            try
            {
                Phong phong = await _db.Phong.FindAsync(iMaPhong);
                ViewBag.Khu = await _db.Khu.ToListAsync();

                return View(phong);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Cập nhật phòng thất bại.";
                return RedirectToAction("Index", "Phong");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CapNhat(Phong phongModel)
        {
            try
            {
                Phong phong = await _db.Phong.FindAsync(phongModel.MaPhong);
                if (phong == null)
                {
                    TempData["ToastMessage"] = "error|Không tồn tại phòng.";
                    return RedirectToAction("Index", "Phong");
                }
                phong.TenPhong = phongModel.TenPhong;
                phong.MaTang = phongModel.MaTang;
                phong.LoaiPhong = phongModel.LoaiPhong;
                await _db.SaveChangesAsync();

                TempData["ToastMessage"] = "success|Cập nhật phòng thành công.";
                return RedirectToAction("Index", "Phong");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Cập nhật phòng thất bại.";
                return RedirectToAction("Index", "Phong");
            }
        }
        #endregion

        #region Xóa khu
        public async Task<ActionResult> Xoa(int iMaPhong)
        {
            try
            {
                Phong phong = await _db.Phong.FindAsync(iMaPhong);
                if (phong == null)
                {
                    TempData["ToastMessage"] = "error|Không tồn tại phòng.";
                    return RedirectToAction("Index", "Phong");
                }

                _db.Phong.Remove(phong);
                await _db.SaveChangesAsync();
                TempData["ToastMessage"] = "success|Xóa phòng thành công.";

                return RedirectToAction("Index", "Phong");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Lỗi khóa ngoại.";
                return RedirectToAction("Index", "Phong");
            }
        }
        #endregion
    }
}