using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kikkerstarter.Models;

namespace Kikkerstarter.Controllers
{
    public class HomeController : Controller
    {

        // GET: Home
        [HttpGet]
        public ActionResult Home()
        {
            ////Als database.profiel != null is dan is er iemand ingelogd. Hiervan wordt de naam gepakt om te
            ////Laten zien in de view en loggedin bepaald of er dingen worden weggelaten of juist niet.
            if (Database.profiel != null)
            {
                ViewBag.user = Database.profiel.Naam;
                ViewBag.loggedin = "Y";
            }
            else
            {
                ViewBag.user = string.Empty;
                ViewBag.loggedin = string.Empty;
            }

            ////Zet alle projecten in de database in een List<Project>
            Database.ProjectenHome();

            ////Als er projecten aanwezig zijn dan worden die gesorteerd op id meegegeven aan de view
            ////Zo niet dan wordt niks meegegeven en wordt Viewbag.projecten 0
            ////Hiermee voorkom ik een exception in de view
            if (Database.projectenhome.Count != 0)
            {
                ViewBag.projecten = string.Empty;
                var projecten = Database.projectenhome.OrderBy(x => x.ProjectID);
                return View(projecten);
            }
            else
            {
                ViewBag.projecten = "0";
            }
            return View();
        }
    }
}