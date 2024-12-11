using QuanLyKyTucXa.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKyTucXa.Areas.Student.Controllers
{
    public class HoaDonController : Controller
    {
        private readonly QLKyTucXa _db;

        public HoaDonController(QLKyTucXa db)
        {
            _db = db;
        }

    }
}