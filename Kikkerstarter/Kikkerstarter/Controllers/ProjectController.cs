using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kikkerstarter.Models;

namespace Kikkerstarter.Controllers
{
    public class ProjectController : Controller
    {
        /// <summary>
        /// Ik maak een leeg project aan zodat ik overal in deze controller erbij kan.
        /// Hetzelfde geld voor de strings.
        /// </summary>
        Project projectD;
        string user;
        string loggedin;
        string fail;

        // GET: Project
        [HttpGet]
        public ActionResult Project(int projectID)
        {
            ////Eerst roep ik User() aan om te kijken of er als iemand is ingelogd.
            User();
            ViewBag.user = user;
            ViewBag.loggedin = loggedin;
            
            ////Dan ga ik het aangeklikte project zoeken in de list die te vinden in in de database klasse.
            ////Via de View wordt het projectID meegegeven.
            projectD = Database.projectenhome.Find(
            delegate (Project pr)
            {
                return pr.ProjectID == projectID;
            }
            );

            ////Als het project niet null is vul ik de ViewBags. Als het project wel null is dan maak ik
            ////De ViewBags leeg.
            if (projectD != null)
            {
                ViewBag.projectnaam = projectD.NaamProject;
                ViewBag.accountnaam = projectD.Accountnaam;
                ViewBag.genre = projectD.Genre;
                ViewBag.beschrijving = projectD.Beschrijving;
                ViewBag.start = projectD.StartDate.ToString("dd/MM/yyyy");
                ViewBag.eind = projectD.EindDate.ToString("dd/MM/yyyy");
                ViewBag.goal = projectD.Goal;
            }
            else
            {
                ViewBag.projectnaam = string.Empty;
                ViewBag.accountnaam = string.Empty;
                ViewBag.genre = string.Empty;
                ViewBag.beschrijving = string.Empty;
                ViewBag.start = string.Empty;
                ViewBag.eind = string.Empty;
                ViewBag.goal = string.Empty;
            }
            return View();
        }

        [HttpGet]
        public ActionResult CProject()
        {
            ////Ik roep User() aan om te kijken of er als iemand is ingelogd en return daarna de View.
            User();
            ViewBag.user = user;
            ViewBag.loggedin = loggedin;
            ViewBag.fail = fail;
            return View();
        }

        [HttpPost]
        public ActionResult Cproject(string naamProject, DateTime eindDatum, string genre, string beschrijving, string goal)
        {
            ////De gegevens worden meegegeven aan de methode in de database klasse en je wordt naar de
            ////Home pagina doorgestuurd.
            Database.Cproject(naamProject, eindDatum, genre, beschrijving, goal);
            return RedirectToAction("Home", "Home");
        }

        public void User()
        {
            ////Als database.profiel != null is dan is er iemand ingelogd. Hiervan wordt de naam gepakt om te
            ////Laten zien in de view en loggedin bepaald of er dingen worden weggelaten of juist niet.
            ////Ook is er een fail message voor als je al bent ingelogd.
            if (Database.profiel != null)
            {
                user = Database.profiel.Naam;
                loggedin = "Y";
                fail = string.Empty;
            }
            else
            {
                user = string.Empty;
                loggedin = string.Empty;
                fail = "Log in om een Project aan te maken.";
            }
        }
    }
}