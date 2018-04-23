using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Logic;

namespace Presentation.Controllers
{
    public class CompanyController : Controller
    {
        public ActionResult ShowPowerOfDay()
        {
            Session["chosenACtion"] = "ShowPowerOfDay";
            var date = String.Format("{0} {1} {2} {3}", DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.AddDays(-1).Day, DateTime.Today.Hour);
            return View("PowerView", MunicipalDataHandler.GetProducedPowerOfDay(date, (string)Session["kommun"]));
        }
        public ActionResult ShowTotalPower()
        {
            Session["chosenACtion"] = "ShowTotalPower";
            return View("PowerView", MunicipalDataHandler.GetTotalProducedPower((string)Session["kommun"]));
        }
        public ActionResult ChooseMunicipality(string id)
        {
            Session["kommun"] = id;
            return RedirectToAction((string)Session["chosenAction"]);
        }
        public ActionResult SetCompareMode(bool? id)
        {
            if (id == null)
            {
                Session["compare"] = false;
            }
            else
                Session["compare"] = true;
            return RedirectToAction((string)Session["chosesnAction"]);
        }
    }
}