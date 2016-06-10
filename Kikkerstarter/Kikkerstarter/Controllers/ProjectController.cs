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
                begindat = projectD.StartDate.ToString("dd/MM/yyyy");
                einddat = projectD.EindDate.ToString("dd/MM/yyyy");
                ViewBag.projectnaam = projectD.NaamProject;
                ViewBag.accountnaam = projectD.Accountnaam;
                ViewBag.genre = projectD.Genre;
                ViewBag.beschrijving = projectD.Beschrijving;
                ViewBag.start = begindat;
                ViewBag.eind = einddat;
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
                einddat = "";
                begindat = "";
            }
            return View();
        }
    }
}