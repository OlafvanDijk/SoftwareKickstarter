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
            if (Database.profiel != null)
            {
                ViewBag.user = Database.profiel.Naam;
                ViewBag.loggedin = "Y";
            }
            else
            {
                ViewBag.user = "";
                ViewBag.loggedin = "";
            }
            return View();
        }
    }
}