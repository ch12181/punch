using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using project_punch.Models;
using HtmlAgilityPack;

namespace project_punch.Controllers
{
    public class dashboardController : Controller
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
        
        // GET: dashboard
        public ActionResult dashboard()
        {
            string today = DateTime.Now.ToString("yyyy/M/dd");
            
            //用LINQ取出資料庫的打卡資料
            var punchList = (from o in db.punches
                             where o.date == DateTime.Today
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

            //取出員工資料
            var query_emp = from o in db.employees
                            select o;

            //建立punchRecord的list
            List<punchRecord> punchRecordList = new List<punchRecord>();
            List<employee> empList = query_emp.ToList();

            //比對打卡list和員工list的ID，將資料存進punchRecord的list
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

            //如果上班時間和下班時間相等，則將下班時間設為null，status設為error
            foreach (var punchRecord_temp in punchRecordList)
            {
                if (punchRecord_temp.start == punchRecord_temp.off)
                {
                    punchRecord_temp.off = "null";
                    punchRecord_temp.status = "error";
                }
            }

            //將資料用viewbag傳到view
            ViewBag.list = punchRecordList;
            ViewBag.empCount = empList.Count;
            ViewBag.arrived = punchRecordList.Count;
            ViewBag.absent = empList.Count - punchRecordList.Count;

            //天氣爬蟲
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load("https://weather.com/zh-TW/weather/today/l/TWXX0019:1:TW");
            string todayTemp = doc.DocumentNode.SelectSingleNode("//*[@id=\"hero-left-Nowcard-92c6937d-b8c3-4240-b06c-9da9a8b0d22b\"]/div/div/section/div[2]/div/div[2]/span").InnerText;
            string todayWeather = doc.DocumentNode.SelectSingleNode("//*[@id=\"hero-left-Nowcard-92c6937d-b8c3-4240-b06c-9da9a8b0d22b\"]/div/div/section/div[2]/div/div[3]").InnerText;

            ViewBag.todayTemp = todayTemp.Substring(0,2);
            ViewBag.todayWeather = todayWeather;

            return View();
        }
    }
}