using HL.store.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

public class AdminController : Controller
{
    private quanlyEntities2 db = new    quanlyEntities2();

    public ActionResult OrderHistory()
    {
        // Eager load Customer và OrderDetails + Product
        var orders = db.OrderProes
                       .Include("Customer")
                       .Include("OrderDetails.Product")
                       .OrderByDescending(o => o.ID)
                       .ToList();

        var model = new List<OrderHistoryViewModel>();

        foreach (var order in orders)
        {
            string productList = string.Join("<br/>", order.OrderDetails.Select(d =>
                $"{d.Product?.NamePro} x {d.Quantity} = {(d.Quantity ?? 0) * (decimal)(d.UnitPrice ?? 0):N0} ₫"));

            decimal totalAmount = order.OrderDetails.Sum(d => (d.Quantity ?? 0) * (decimal)(d.UnitPrice ?? 0));

            model.Add(new OrderHistoryViewModel
            {
                OrderID = order.ID,
                DateOrder = order.DateOrder,
                CustomerName = order.Customer?.NameCus,
                CustomerPhone = order.Customer?.PhoneCus,
                CustomerEmail = order.Customer?.EmailCus,
                ProductList = productList,
                TotalAmount = totalAmount
            });
        }

        return View(model);
    }
    
    public ActionResult ChiTiet(int id)
    {
        var product = db.Products.FirstOrDefault(p => p.ProductID == id);
        if (product == null)
        {
            return HttpNotFound();
        }
        return View(product);
    }
}
