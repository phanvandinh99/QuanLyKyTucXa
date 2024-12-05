using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKyTucXa.Areas.Student.Controllers
{
    public class PublictController : Controller
    {
        // Trang giới thiệu
        public ActionResult GioiThieu()
        {
            return View();
        }

        // Trang nội quy
        public ActionResult NoiQuy()
        {
            return View();
        }

        public ActionResult LienHe()
        {
            return View();
        }
    }
}