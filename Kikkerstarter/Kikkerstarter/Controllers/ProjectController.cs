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

        // GET: Project
        [HttpGet]
        public ActionResult Project(int projectID)
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

            projectD = Database.projectenhome.Find(
            delegate (Project pr)
            {
                return pr.ProjectID == projectID;
            }
            );
            if (projectD != null)
            {
                ViewBag.projectnaam = projectD.NaamProject;
                ViewBag.accountnaam = projectD.Accountnaam;
                ViewBag.genre = projectD.Genre;
                ViewBag.beschrijving = projectD.Beschrijving;
                ViewBag.start = projectD.StartDate;
                ViewBag.eind = projectD.EindDate;
                ViewBag.goal = projectD.Goal;
            }
            else
            {
                ViewBag.projectnaam = "Geen project gevonden";
                ViewBag.accountnaam = "";
                ViewBag.genre = "";
                ViewBag.beschrijving = "";
                ViewBag.start = "";
                ViewBag.eind = "";
                ViewBag.goal = "";
            }
            return View();
        }
    }
}