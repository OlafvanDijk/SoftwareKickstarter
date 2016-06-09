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
        string user;
        string email;
        string beschrijving;
        string websites;
        string land;
        string stad;
        string loggedin;

        // GET: Prelated
        [HttpGet]
        public ActionResult Profile()
        {
            Laadprofiel();
            ViewBag.user = user;
            ViewBag.email = email;
            ViewBag.loggedin = loggedin;
            ViewBag.beschrijving = beschrijving;
            ViewBag.websites = websites;
            ViewBag.land = land;
            ViewBag.stad = stad;

            return View();
        }

        public void Laadprofiel()
        {
            if (Database.profiel != null)
            {
                user = Database.profiel.Naam;
                email = Database.profiel.Email;
                beschrijving = Database.profiel.Beschrijving;
                websites = Database.profiel.Beschrijving;
                land = Database.profiel.Land;
                stad = Database.profiel.Stad;
                loggedin = "Y";
            }
            else
            {
                user = "";
                email = "";
                beschrijving = "";
                websites = "";
                land = "";
                stad = "";
                loggedin = "";
            }
        }
    }
}