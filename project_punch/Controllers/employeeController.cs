using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using project_punch.Models;

namespace project_punch.Controllers
{
    public class employeeController : Controller
    {
        punchDBEntities1 db = new punchDBEntities1();
        // GET: Employee
        public ActionResult employee()
        {
            var query_employees = from o in db.employees
                                  select o;
            List<employee> employeeList = query_employees.ToList();

            return View(employeeList);
        }
        public ActionResult Edit(string id)
        {
            var query = from o in db.employees
                        where o.empID == id
                        select o;
            employee emp = query.Single();
            return View(emp);
        }

        [HttpPost]
        public ActionResult Edit(string id, string firstName, string department, string hiredate, string phone, string address)
        {

            if (Request.Form["btnOKCancel"] == "Cancel")
            {
                return RedirectToAction("employee");
            }
            var query = from o in db.employees
                        where o.empID == id
                        select o;
            employee empDb = query.SingleOrDefault();
            empDb.empName = firstName;
            empDb.department = department;
            empDb.phone = phone;
            empDb.address = address;
            DateTime date = DateTime.Parse(hiredate);
            empDb.hireDate = date;

            db.SaveChanges();

            return RedirectToAction("employee");
        }

        public ActionResult Create()
        {
            employee person = new employee();
            return View(person);
        }
        
        [HttpPost]
        public ActionResult Create(string id, string firstName, string department, string phone, string hiredate, string address, string personID)
        {
            if (Request.Form["btnOKCancel"] == "Cancel")
            {
                return RedirectToAction("employee");
            }

            if (id == "" || firstName == "" || department == "" || phone == "" || hiredate == "" || address == "" || personID == "")
            {
                ViewBag.message = "尚有欄位未輸入!!";
                return View();
            }
            
            employee person = new employee();
            person.empID = id;
            person.empName = firstName;
            person.department = department;
            person.phone = phone;
            DateTime date = DateTime.Parse(hiredate);
            person.hireDate = date;
            person.address = address;
            person.personID = personID;
            
            db.employees.Add(person);
            db.SaveChanges();

            return RedirectToAction("employee");
        }
    }
}