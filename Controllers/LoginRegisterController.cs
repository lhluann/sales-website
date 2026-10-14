using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HL.store.Models;

namespace HL.store.Controllers

{
    public class LoginRegisterController : Controller
    {
        quanlyEntities2 db = new quanlyEntities2();
        // GET: LoginRegister
        public ActionResult IndexLoginRegister()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(AdminUser adminUser)
        {
            bool hasError = false;

            // Kiểm tra username
            if (string.IsNullOrWhiteSpace(adminUser.NameUser))
            {
                ViewBag.ErrRegister = "Vui lòng nhập tên";
                hasError = true;
            }

            // Kiểm tra password
            if (string.IsNullOrWhiteSpace(adminUser.PasswordUser))
            {
                ViewBag.ErrPassword = "Vui lòng nhập mật khẩu";
                hasError = true;
            }

            // Kiểm tra confirm password
            if (string.IsNullOrWhiteSpace(adminUser.ConfirmPass))
            {
                ViewBag.ErrConfirm = "Vui lòng nhập xác nhận mật khẩu";
                hasError = true;
            }

            // Kiểm tra confirm password khớp
            if (!string.IsNullOrWhiteSpace(adminUser.PasswordUser) &&
                adminUser.PasswordUser.Trim() != adminUser.ConfirmPass?.Trim())
            {
                ViewBag.ErrConfirm = "Mật khẩu không khớp!";
                hasError = true;
            }

            // Kiểm tra username tồn tại
            if (!string.IsNullOrWhiteSpace(adminUser.NameUser) &&
                db.AdminUsers.Any(s => s.NameUser == adminUser.NameUser))
            {
                ViewBag.ErrRegister = "Tài khoản đã tồn tại!";
                hasError = true;
            }

            // Nếu có lỗi, giữ form Register mở
            if (hasError)
            {
                ViewBag.ShowRegisterForm = true;
                return View("IndexLoginRegister", adminUser);
            }

            // Tạo ID thủ công
            adminUser.ID = db.AdminUsers.Count() == 0 ? 1 : db.AdminUsers.Max(s => s.ID) + 1;
            if (string.IsNullOrEmpty(adminUser.RoleUser))
                adminUser.RoleUser = "user";

            // Thêm vào DB khi chắc chắn không có lỗi
            db.AdminUsers.Add(adminUser);
            db.SaveChanges();

            ViewBag.Success = "Đăng ký thành công!";
            return View("IndexLoginRegister");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AdminUser adminUser)
        {
            var checkName = db.AdminUsers
                .Where(s => s.NameUser.Equals(adminUser.NameUser)).FirstOrDefault();
            if (checkName == null)
            {
                ViewBag.ErrLoginID = "Tài khoản không tồn tại!";
                return View("IndexLoginRegister");
            }

            if (checkName.PasswordUser.Trim() != adminUser.PasswordUser.Trim())
            {
                ViewBag.ErrLoginPass = "Mật khẩu không chính xác!";
                return View("IndexLoginRegister");
            }

            // Login success
            Session["UserID"] = checkName.ID;
            Session["NameUser"] = checkName.NameUser;
            Session["RoleUser"] = checkName.RoleUser;
            if (checkName.RoleUser != null && checkName.RoleUser.ToLower() == "admin")
            {
                return RedirectToAction("Index", "Product");
            }
            else
            {
                return RedirectToAction("Home", "trangchu");
            }
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Home", "trangchu");
        }
        

    }

}