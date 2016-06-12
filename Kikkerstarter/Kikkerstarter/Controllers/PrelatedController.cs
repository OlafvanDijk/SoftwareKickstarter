using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kikkerstarter.Models;

namespace Kikkerstarter.Controllers
{
    public class PrelatedController : Controller
    {
        /// <summary>
        /// Ik maak een leeg profiel aan en een lege loggedin string zodat ik hier in de hele
        /// controller bij kan.
        /// </summary>
        Profiel p;
        string loggedin;

        // GET: Prelated
        [HttpGet]
        public ActionResult Profile()
        {
            ////Ik roep LaadProfiel() aan. Deze methode bepaald of Profiel p leeg moet zijn of gevuld moet zijn
            ////Als p leeg is dan zijn alle ViewBags ook leeg
            LaadProfiel();
            ViewBag.user = p.Naam;
            ViewBag.email = p.Email;
            ViewBag.beschrijving = p.Beschrijving;
            ViewBag.websites = p.Websites;
            ViewBag.land = p.Land;
            ViewBag.stad = p.Stad;
            ViewBag.loggedin = loggedin;
            return View();
        }

        public void LaadProfiel()
        {
            ////Als database.profiel != null is dan is er iemand ingelogd. Het database profiel wordt dan in p
            ////gezet en loggedin bepaald of er dingen worden weggelaten of juist niet.
            ////Als niemand is ingelogd is p gelijk aan null.
            if (Database.profiel != null)
            {
                p = Database.profiel;
                loggedin = "Y";
            }
            else
            {
                p = null;
                loggedin = string.Empty;
            }
        }
    }
}