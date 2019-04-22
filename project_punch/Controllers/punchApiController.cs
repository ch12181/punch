using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using project_punch.Models;

namespace project_punch.Controllers
{
    public class punchApiController : ApiController
    {
        punchDBEntities1 db = new punchDBEntities1();
        public void Post([FromBody]string personId)
        {
            var query = (from o in db.employees
                         select o).ToList();
            string emppersonId = personId;
            string empid = "";
            string empname = "";
            foreach (var emptemp in query)
            {
                if (emptemp.personID == emppersonId)
                {
                    empid = emptemp.empID;
                    empname = emptemp.empName;
                    break;
                }
            }

            TimeSpan myDateResult = DateTime.Now.TimeOfDay;
            punch temp = new punch();
            temp.empID = empid;
            temp.date = DateTime.Today;
            temp.time = myDateResult;
            db.punches.Add(temp);
            db.SaveChanges();
            
        }
    }
}
