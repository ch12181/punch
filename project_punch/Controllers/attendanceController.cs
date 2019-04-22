using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using project_punch.Models;

namespace project_punch.Controllers
{
    public class attendanceController : Controller
    {
        public class punchRecord
        {
            public string empID { get; set; }
            public string empName { get; set; }
            public string date { get; set; }
            public string start { get; set; }
            public string off { get; set; }
            public string status { get; set; }
        }

        punchDBEntities1 db = new punchDBEntities1();

        // GET: attendance
        public ActionResult attendance()
        {

            var punchList = (from o in db.punches
                             group o by new { o.empID, o.date } into g
                             orderby g.Key.date descending, g.Min(o => o.time) descending
                             select new
                             {
                                 empID = g.Key.empID,
                                 date = g.Key.date,
                                 start = g.Min(o => o.time),
                                 off = g.Max(o => o.time)
                             }
                             ).ToList();
            
            var query_emp = from o in db.employees
                            select o;

            List<punchRecord> punchRecordList = new List<punchRecord>();
            List<employee> empList = query_emp.ToList();
            
            string name_temp = " ";
            foreach (var punchList_temp in punchList)
            {

                foreach (var empList_temp in empList)
                {
                    if (punchList_temp.empID == empList_temp.empID)
                    {
                        name_temp = empList_temp.empName;
                        break;
                    }
                }

                punchRecordList.Add(
                    new punchRecord
                    {
                        empID = punchList_temp.empID,
                        empName = name_temp,
                        date = punchList_temp.date.ToString().Substring(0, 9),
                        start = punchList_temp.start.ToString().Substring(0, 8),
                        off = punchList_temp.off.ToString().Substring(0, 8),
                        status = "complete"
                    }
                );
                name_temp = " ";
            }

            foreach (var punchRecord_temp in punchRecordList)
            {
                if(punchRecord_temp.start == punchRecord_temp.off)
                {
                    punchRecord_temp.off = "null";
                    punchRecord_temp.status = "error";
                }
            }

            ViewBag.list = punchRecordList;

            return View();
        }
    }
}