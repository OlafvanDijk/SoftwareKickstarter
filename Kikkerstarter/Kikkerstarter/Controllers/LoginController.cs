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
        /// <summary>
        /// Deze strings gebruik ik over de hele controller om zo bij verschillende views de waarde te behouden.
        /// </summary>
        string user;
        string loggedin;

        // GET: Login
        [HttpPost]
        public ActionResult Login(string Email, string Wachtwoord)
        {
            ViewBag.loginfail = string.Empty;
            ViewBag.loggedin = string.Empty;
            ////Als niemand is ingelogd dan wordt er gekeken of de ingevulde gegevens juist zijn
            if (Database.profiel == null)
            {
                if (Database.Login(Email, Wachtwoord) == true)
                {
                    ////Als de gegevens juist zijn vul ik door User(); de lege strings bovenaan de controller
                    ////Dan vul ik de ViewBags met deze strings en stuur ik je door naar de home pagina.
                    User();
                    ViewBag.loggedin = loggedin;
                    ViewBag.user = user;
                    return RedirectToAction("Home", "Home");
                }
                else if (Database.Login(Email, Wachtwoord) == false)
                {
                    ////Als de gegevens niet kloppen dan geef ik de ViewBag.loginfail mee
                    ViewBag.loginfail = "*Foute inlog gegevens*";
                }
            }
                return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            ////Eerst roep ik User() aan om te kijken of er als iemand is ingelogd
            ////Aan de hand van de inhoud van de string loggedin geef ik een waarde mee aan loginfail
            ////Deze waarde bepaald of de gebruiker de input ziet of de string in ViewBag.loginfail
            User();
            ViewBag.loggedin = loggedin;
            ViewBag.user = user;
            if (loggedin == "Y")
            {
                ViewBag.loginfail = "*Log eerst uit om in te kunnen loggen.*";
            }
            else
            {
                ViewBag.loginfail = string.Empty;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Register(string naam, string wachtwoord, string email, string beschrijving, string websites, string land, string stad)
        {
            ////De gegevens worden meegegeven aan de database methode en daarna wordt je doorgestuurd naar
            ////de login pagina
            Database.RegisterUser(naam, wachtwoord, email, beschrijving, websites, land, stad);
            return RedirectToAction("Login", "Login");
        }

        [HttpGet]
        public ActionResult Register()
        {
            ////Eerst roep ik User() aan om te kijken of er als iemand is ingelogd
            ////Aan de hand van de inhoud van de string loggedin geef ik een waarde mee aan loginfail
            ////Deze waarde bepaald of de gebruiker de input ziet of de string in ViewBag.loginfail
            User();
            ViewBag.loggedin = loggedin;
            ViewBag.user = user;
            if (loggedin == "Y")
            {
                ViewBag.loginfail = "*Log eerst uit om u te registreren*";
            }
            else
            {
                ViewBag.loginfail = string.Empty;
            }
            return View();
        }

        [HttpGet]
        public ActionResult Uitloggen()
        {
            ////Het profiel in database wordt null gemaakt en je wordt doorverwezen naar de view van
            ////uitloggen
            Database.profiel = null;
            return View();
        }

        public void User()
        {
            ////Als database.profiel != null is dan is er iemand ingelogd. Hiervan wordt de naam gepakt om te
            ////Laten zien in de view en loggedin bepaald of er dingen worden weggelaten of juist niet.
            if (Database.profiel != null)
            {
                user = Database.profiel.Naam;
                loggedin = "Y";
            }
            else
            {
                user = string.Empty;
                loggedin = string.Empty;
            }
        }
    }
}