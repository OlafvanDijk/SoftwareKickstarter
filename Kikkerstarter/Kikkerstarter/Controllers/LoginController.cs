using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kikkerstarter.Models;

namespace Kikkerstarter.Controllers
{
    public class LoginController : Controller
    {
        string user;
        string loggedin;

        // GET: Login
        [HttpPost]
        public ActionResult Login(string Email, string Wachtwoord)
        {
            ViewBag.loginfail = "";
            ViewBag.loggedin = "";
            if (Database.profiel == null)
            {
                if (Database.Login(Email, Wachtwoord) == true)
                {
                    User();
                    ViewBag.loggedin = loggedin;
                    ViewBag.user = user;
                    return RedirectToAction("Home", "Home");
                }
                else if (Database.Login(Email, Wachtwoord) == false)
                {
                    ViewBag.loginfail = "*Foute inlog gegevens*";
                }
            }
                return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            User();
            ViewBag.loggedin = loggedin;
            ViewBag.user = user;
            if (loggedin == "Y")
            {
                ViewBag.loginfail = "*Log eerst uit om in te kunnen loggen.*";
            }
            else
            {
                ViewBag.loginfail = "";
            }
            return View();
        }

        [HttpPost]
        public ActionResult Register(string naam, string wachtwoord, string email, string beschrijving, string websites, string land, string stad)
        {
            Database.RegisterUser(naam, wachtwoord, email, beschrijving, websites, land, stad);
            return RedirectToAction("Login", "Login");
        }

        [HttpGet]
        public ActionResult Register()
        {
            User();
            ViewBag.loggedin = loggedin;
            ViewBag.user = user;
            if (loggedin == "Y")
            {
                ViewBag.loginfail = "*Log eerst uit om u te registreren*";
            }
            else
            {
                ViewBag.loginfail = "";
            }
            return View();
        }

        [HttpGet]
        public ActionResult Uitloggen()
        {
            Database.profiel = null;
            return View();
        }

        public void User()
        {
            if (Database.profiel != null)
            {
                user = Database.profiel.Naam;
                loggedin = "Y";
            }
            else
            {
                user = "";
                loggedin = "";
            }
        }
    }
}