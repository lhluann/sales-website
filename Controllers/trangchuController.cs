using HL.store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HL.store.Controllers
{
    public class trangchuController : Controller
    {
        // GET: trangchu
        quanlyEntities2 db = new quanlyEntities2();
        public ActionResult DangKy()
        {
            return View();
        }
        public ActionResult ThanhToan()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Logout()
        {
            return View();
        }
        public ActionResult Home(string keyword, string category, string price)
        {
            var product1 = db.Products.AsQueryable()
            .OrderBy(p => p.ProductID)
            .Take(8)
            .ToList();
            ViewBag.Categories = db.Categories.ToList();

            return View(product1.ToList());
        }
        public ActionResult Support()
        {
            return View();
        }
        public ActionResult Product(string keyword = "", string category = "", string price = "")
        {
            var products = db.Products.AsQueryable();

            // lọc tìm kiếm
            if (!string.IsNullOrEmpty(keyword))
            {
                products = products.Where(p => p.NamePro.Contains(keyword.Trim()));
            }

            // lọc loại
            if (!string.IsNullOrEmpty(category) && category != "all")
            {
                products = products.Where(p => p.IDCate == category);
            }

            // lọc giá
            switch (price)
            {
                case "1":
                    products = products.Where(p => p.Price < 15000000);
                    break;
                case "2":
                    products = products.Where(p => p.Price >= 15000000 && p.Price <= 25000000);
                    break;
                case "3":
                    products = products.Where(p => p.Price > 25000000);
                    break;
            }

            ViewBag.Categories = db.Categories.ToList();

            // trả về kq lọc
            return View(products.ToList());
        }
        public ActionResult Service()
        {
            return View();
        }
        public ActionResult ThanhCongContact()
        {
            return View();
        }
        public ActionResult ChiTietSanPham(int id)
        {
            var product = db.Products.FirstOrDefault(p => p.ProductID == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        
    }
}