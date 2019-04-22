using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using project_punch.Models;

namespace project_punch.Controllers
{
    public class passwordEditController : Controller
    {
        punchDBEntities1 db = new punchDBEntities1();

        // GET: passwordEdit
        public ActionResult passwordEdit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult passwordEdit(string oldpassword, string newpassword, string newpassword2)
        {

            var a = db.logins.ToList();
            
            //密碼字數有誤
            if (oldpassword.Count() < 4 || newpassword.Count() < 4)
            {
                ViewBag.ifSuccess = false;
                return View();
            }
            
            //舊密碼輸入錯誤
            if (a[0].password.TrimEnd() != oldpassword)
            {
                ViewBag.ifSuccess = false;
                return View();
            }

            //新密碼不一致
            if(newpassword != newpassword2)
            {
                ViewBag.ifSuccess = false;
                return View();
            }

            //修改成功
            a[0].password = newpassword;
            db.SaveChanges();
            ViewBag.ifSuccess = true;
            return View();
        }

    }
}