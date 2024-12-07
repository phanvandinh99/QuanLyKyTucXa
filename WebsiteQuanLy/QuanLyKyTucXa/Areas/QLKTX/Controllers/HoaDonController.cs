using QuanLyKyTucXa.Common.Const;
using QuanLyKyTucXa.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyKyTucXa.Areas.QLKTX.Controllers
{
    public class HoaDonController : Controller
    {
        private readonly QLKyTucXa _db;

        public HoaDonController(QLKyTucXa db)
        {
            _db = db;
        }

        // Trang danh sách Loại hóa đơn
        public async Task<ActionResult> Index()
        {
            try
            {
                List<LoaiHoaDon> listLoaiHoaDon = await _db.LoaiHoaDon.ToListAsync();

                return View(listLoaiHoaDon);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Xem danh sách loại hóa đơn thất bại.";
                return RedirectToAction("Index", "HoaDon");
            }
        }

        #region Thêm mới hoá đơn
        public async Task<ActionResult> ThemMoi()
        {
            try
            {
                ViewBag.LoaiHoaDon = await _db.LoaiHoaDon.ToListAsync();

                return View();
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Thêm hóa đơn thất bại.";
                return RedirectToAction("Index", "HoaDon");
            }
        }

        public async Task<ActionResult> HoaDonKhu(int iMaLoaiHoaDon)
        {
            try
            {
                LoaiHoaDon loaiHoaDon = await _db.LoaiHoaDon.FindAsync(iMaLoaiHoaDon);
                if (loaiHoaDon == null)
                {
                    TempData["ToastMessage"] = "error|Không tồn tại loại hóa đơn.";
                    return RedirectToAction("Index", "HoaDon");
                }

                ViewBag.TenLoaiHoaDon = loaiHoaDon.TenLoaiHoaDon;

                // Danh sách khu
                List<Khu> listKhu = await _db.Khu.ToListAsync();

                ViewBag.MaLoaiHoaDon = iMaLoaiHoaDon;

                return View(listKhu);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Thêm hóa đơn theo khu thất bại.";
                return RedirectToAction("Index", "HoaDon");
            }
        }
        #endregion

        #region Thêm hóa đơn theo loại
        public async Task<ActionResult> ThemHoaDon(int iMaKhu, int iMaLoaiHoaDon)
        {
            try
            {
                DateTime currentDate = DateTime.Now;

                var listPhong = await (from p in _db.Phong
                                       join g in _db.Giuong on p.MaPhong equals g.MaPhong
                                       join h in _db.HopDong on g.MaGiuong equals h.MaGiuong
                                       join t in _db.ThoiHanDangKy on h.MaThoiHanDangKy equals t.MaThoiHanDangKy
                                       where p.Tang.MaKhu == iMaKhu &&
                                                        p.DaO != 0 &&
                                                        h.NgayDuyet != null &&
                                                        t.NgayBatDau <= currentDate &&
                                                        t.NgayKetThuc >= currentDate
                                       select p)

                 .Distinct()
                 .ToListAsync();


                //var listPhong = await (from p in _db.Phong
                //                       join g in _db.Giuong on p.MaPhong equals g.MaPhong
                //                       join h in _db.HopDong on g.MaGiuong equals h.MaGiuong
                //                       join t in _db.ThoiHanDangKy on h.MaThoiHanDangKy equals t.MaThoiHanDangKy
                //                       join hd in _db.HoaDon on p.MaPhong equals hd.MaPhong into HoaDonGroup
                //                       from maxHd in HoaDonGroup.OrderByDescending(hd => hd.MaHoaDon).Take(1).DefaultIfEmpty() // Lấy hóa đơn có id lớn nhất hoặc null
                //                       where p.Tang.MaKhu == iMaKhu &&
                //                             p.DaO != 0 &&
                //                             h.NgayDuyet != null &&
                //                             t.NgayBatDau <= currentDate &&
                //                             t.NgayKetThuc >= currentDate
                //                       select new
                //                       {
                //                           Phong = p,
                //                           HoaDon = maxHd // Hóa đơn lớn nhất hoặc null
                //                       })
                //       .Distinct()
                //       .ToListAsync();


                // Hóa đơn cũ để lấy chỉ số điện nước cũ
                //LoaiHoaDon loaiHoaDon = await _db.LoaiHoaDon.Where(n=>n.).FindAsync(iMaLoaiHoaDon);
                //if (loaiHoaDon == null)
                //{
                //    TempData["ToastMessage"] = "error|Không tồn tại loại hóa đơn.";
                //    return RedirectToAction("Index", "HoaDon");
                //}


                return View(listPhong);


            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Thêm hóa đơn thất bại.";
                return RedirectToAction("Index", "DonGia");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CapNhat(DonGia donGiaModel)
        {
            try
            {
                DonGia donGia = await _db.DonGia.FindAsync(donGiaModel.MaDonGia);
                if (donGia == null)
                {
                    TempData["ToastMessage"] = "error|Không tồn tại đơn giá.";
                    return RedirectToAction("Index", "DonGia");
                }
                donGia.DonVi = donGiaModel.DonVi;
                donGia.DonGia1 = donGiaModel.DonGia1;
                await _db.SaveChangesAsync();

                TempData["ToastMessage"] = "success|Cập nhật đơn giá thành công.";
                return RedirectToAction("Index", "DonGia");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Cập nhật đơn giá thất bại.";
                return RedirectToAction("Index", "DonGia");
            }
        }
        #endregion

        //#region xóa đơn giá
        //public async Task<ActionResult> Xoa(int iMaDonGia)
        //{
        //    try
        //    {
        //        DonGia DonGia = await _db.DonGia.FindAsync(iMaDonGia);
        //        if (DonGia == null)
        //        {
        //            TempData["ToastMessage"] = "error|Không tồn tại đơn giá.";
        //            return RedirectToAction("Index", "DonGia");
        //        }

        //        _db.DonGia.Remove(DonGia);
        //        await _db.SaveChangesAsync();
        //        TempData["ToastMessage"] = "success|Xóa đơn giá thành công.";

        //        return RedirectToAction("Index", "DonGia");
        //    }
        //    catch (Exception ex)
        //    {
        //        // logerror
        //        Console.WriteLine(ex.ToString());

        //        TempData["ToastMessage"] = "error|Lỗi khóa ngoại.";
        //        return RedirectToAction("Index", "DonGia");
        //    }
        //}
        //#endregion
    }
}