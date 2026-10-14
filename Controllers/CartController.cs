using HL.store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HL.store.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        quanlyEntities2 db = new quanlyEntities2();

        private List<CartItem> GetCartItems()
        {
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null)
            {
                cart = new List<CartItem>();
                Session["Cart"] = cart;
            }
            return cart;
        }
        // GET: Cart
        public ActionResult IndexCart()
        {
            var cart = GetCartItems();
            ViewBag.Total = cart.Sum(s => s.total);
            ViewBag.Count = cart.Sum(s => s.quantity);
            ViewBag.Customers = db.Customers.ToList();
            return View(cart);
        }
        public ActionResult Index()
        {
            var cart = GetCartItems();
            return View("IndexCart", cart);
        }
        public ActionResult Add(int id)
        {
            var cart = GetCartItems();
            var product = db.Products.Find(id);
            if (product == null) return RedirectToAction("Index");

            var item = cart.FirstOrDefault(s => s.idPro == id);
            if (item != null)
            {
                item.quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    idPro = product.ProductID,
                    namePro = product.NamePro,
                    quantity = 1,
                    price = (decimal)product.Price,
                    ImagePro = product.ImagePro ?? "/Hinh/default.jpg"
                });
            }

            return RedirectToAction("Index");
        }
        public ActionResult Remove(int id)
        {
            var cart = GetCartItems();
            var item = cart.FirstOrDefault(x => x.idPro == id);
            if (item != null) cart.Remove(item);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult ThanhToan(int IDCus, string DCNhanHang)
        {
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null || !cart.Any())
            {
                TempData["Error"] = "Lỗi sản phẩm";
                return RedirectToAction("Index");
            }

            var order = new OrderPro
            {
                DateOrder = DateTime.Now,
                IDCus = IDCus,
                AddressDeliverry = DCNhanHang , 
                TotalAmount = cart.Sum(s => s.total)
            };

            db.OrderProes.Add(order);
            db.SaveChanges();

            foreach (var item in cart)
            {
                var orderdetails = new OrderDetail
                {
                    IDOrder = order.ID,
                    IDProduct = item.idPro,
                    Quantity = item.quantity,
                    UnitPrice = (double?)item.price
                };
                db.OrderDetails.Add(orderdetails);
            }

            db.SaveChanges();
            Session["Cart"] = null;
            TempData["Success"] = "Đặt hàng thành công!";
            return RedirectToAction("ThanhCong","Cart");
        }

        public ActionResult ThanhCong()
        {
            return View();
        }

    }
}