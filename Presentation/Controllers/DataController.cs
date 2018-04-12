using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Logic;
using Data;

namespace Presentation.Controllers
{
    public class DataController : Controller
    {
        public ActionResult ShowPower()
        {
            return View("PowerView", DataHandler.CalculateProducedPower());
        }
    }
}