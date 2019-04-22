using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using project_punch.Models;

namespace project_punch.Controllers
{
    public class homeController : Controller
    {
        // GET: Home
        punchDBEntities1 db = new punchDBEntities1();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string fUserId, string fPwd)
        {
            var member = db.logins
                .Where(m => m.account == fUserId && m.password == fPwd)
                .FirstOrDefault();

            if (member == null)
            {
                ViewBag.Message = "帳密錯誤，登入失敗";
                return View();
            }
            
            return RedirectToAction("dashboard", "dashboard");
        }
    }
}