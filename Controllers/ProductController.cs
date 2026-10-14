using HL.store.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HL.store.Controllers
{
    public class ProductController : Controller
    {
        quanlyEntities2 db =new quanlyEntities2();
        // GET: Product
        public ActionResult IndecCustomer(decimal? _min, decimal? _max)
        {
            var proFilter = db.Products.AsQueryable();
            if (_min.HasValue)
                proFilter = proFilter.Where(s => s.Price >= _min.Value);
            if (_max.HasValue)
                proFilter = proFilter.Where(s => s.Price <= _max.Value);
            return View(db.Products.ToList());
        }
        public ActionResult Index(decimal? _min, decimal? _max)
        {
            var proFilter = db.Products.AsQueryable();
            if (_min.HasValue) proFilter = proFilter.Where(s => s.Price >= _min.Value);
            if (_max.HasValue) proFilter = proFilter.Where(s => s.Price <= _max.Value);

            var list = proFilter.OrderByDescending(p => p.ProductID).ToList();

            // Dòng này fix lỗi tên danh mục
            var cateDict = db.Categories.ToDictionary(c => c.IDCate, c => c.NameCate);
            ViewBag.CategoryNames = list.ToDictionary(
                p => p.ProductID,
                p => cateDict.ContainsKey(p.IDCate) ? cateDict[p.IDCate] : "Chưa chọn"
            );

            return View(list);
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.listCate = new SelectList(db.Categories, "IDCate", "NameCate");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product1 pro)
        {
            ViewBag.listCate = new SelectList(db.Categories, "IDCate", "NameCate");
            if (ModelState.IsValid)
            {
                if (pro.ImagePath != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(pro.ImagePath.FileName);
                    string extention = Path.GetExtension(pro.ImagePath.FileName);
                    fileName = fileName + extention;
                    pro.ImagePath.SaveAs(Path.Combine(Server.MapPath("~/Hinh/"), fileName));
                    pro.ImagePro = "~/Hinh/" + fileName;
                }
                db.Products.Add(pro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pro);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var pro = db.Products.Find(id);
            if (pro == null) return HttpNotFound();

            ViewBag.listCate = new SelectList(db.Categories, "IDCate", "NameCate", pro.IDCate);
            return View(pro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product1 pro, HttpPostedFileBase ImagePath)
        {
            if (ModelState.IsValid)
            {
                var proEdit = db.Products.Find(pro.ProductID);
                if (proEdit == null) return HttpNotFound();

                proEdit.NamePro = pro.NamePro;
                proEdit.DecriptionPro = pro.DecriptionPro;
                proEdit.Price = pro.Price;
                proEdit.IDCate = pro.IDCate;

                // Xử lý ảnh mới nếu có upload
                if (ImagePath != null && ImagePath.ContentLength > 0)
                {
                    // Tạo thư mục nếu chưa có
                    string folder = Server.MapPath("~/Hinh/");
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    string fileName = Path.GetFileName(ImagePath.FileName);
                    string path = Path.Combine(folder, fileName);
                    ImagePath.SaveAs(path);

                    // Xóa ảnh cũ (tùy chọn)
                    if (!string.IsNullOrEmpty(proEdit.ImagePro))
                    {
                        string oldPath = Server.MapPath(proEdit.ImagePro);
                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    proEdit.ImagePro = "~/Hinh/" + fileName;
                }

                db.SaveChanges();
                TempData["Success"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction("Index");
            }

            ViewBag.listCate = new SelectList(db.Categories, "IDCate", "NameCate", pro.IDCate);
            return View(pro);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var pro = db.Products.Find(id);
            if (pro == null) return HttpNotFound();
            return View(pro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var pro = db.Products.Find(id);
            if (pro == null)
            {
                TempData["Error"] = "Sản phẩm không tồn tại!";
                return RedirectToAction("Index");
            }
            try
            {
                if (!string.IsNullOrEmpty(pro.ImagePro))
                {
                    string path = Server.MapPath(pro.ImagePro);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }

                db.Products.Remove(pro);
                db.SaveChanges();
                TempData["Success"] = "Xóa sản phẩm thành công!";
            }
            catch (Exception)
            {
                TempData["Error"] = "Không thể xóa! Sản phẩm đang nằm trong đơn hàng.";
            }

            return RedirectToAction("Index");
        }
        
    }
}