using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Logic;
using Data;
using System.Text;

namespace Presentation.Controllers
{
    public class DataController : Controller
    {
        public ActionResult ShowPowerOfPastWeek(string date, string kommun)
        {
            
            return View("PowerView", DataHandler.GetProducedPowerOfPastWeek(date , kommun));
        }
        public ActionResult ShowPowerOfDay(string date, string kommun)
        {
            date = String.Format("{0} {1} {2} {3}", DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.AddDays(-1).Day, DateTime.Today.Hour);
            return View("PowerView", DataHandler.GetProducedPowerOfDay(date, kommun));
        }
    }
}