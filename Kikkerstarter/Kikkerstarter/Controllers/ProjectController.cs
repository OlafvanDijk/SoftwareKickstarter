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
        Project projectD;
        string begindat;
        string einddat;
        string user;
        string loggedin;
        string fail;

        // GET: Project
        [HttpGet]
        public ActionResult Project(int projectID)
        {
            User();
            ViewBag.user = user;
            ViewBag.loggedin = loggedin;

            projectD = Database.projectenhome.Find(
            delegate (Project pr)
            {
                return pr.ProjectID == projectID;
            }
            );
            if (projectD != null)
            {
                this.begindat = projectD.StartDate.ToString("dd/MM/yyyy");
                this.einddat = projectD.EindDate.ToString("dd/MM/yyyy");
                ViewBag.projectnaam = projectD.NaamProject;
                ViewBag.accountnaam = projectD.Accountnaam;
                ViewBag.genre = projectD.Genre;
                ViewBag.beschrijving = projectD.Beschrijving;
                ViewBag.start = this.begindat;
                ViewBag.eind = this.einddat;
                ViewBag.goal = projectD.Goal;
            }
            else
            {
                ViewBag.projectnaam = "Geen project gevonden";
                ViewBag.accountnaam = string.Empty;
                ViewBag.genre = string.Empty;
                ViewBag.beschrijving = string.Empty;
                ViewBag.start = string.Empty;
                ViewBag.eind = string.Empty;
                ViewBag.goal = string.Empty;
                this.einddat = string.Empty;
                this.begindat = string.Empty;
            }
            return View();
        }

        [HttpGet]
        public ActionResult CProject()
        {
            User();
            ViewBag.user = user;
            ViewBag.loggedin = loggedin;
            ViewBag.fail = fail;
            return View();
        }

        [HttpPost]
        public ActionResult Cproject(string naamProject, DateTime eindDatum, string genre, string beschrijving, string goal)
        {
            Database.Cproject(naamProject, eindDatum, genre, beschrijving, goal);
            return RedirectToAction("Home", "Home");
        }

        public void User()
        {
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