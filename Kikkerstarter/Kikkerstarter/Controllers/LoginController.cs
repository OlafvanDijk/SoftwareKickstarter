﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kikkerstarter.Models;

namespace Kikkerstarter.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [HttpPost]
        public ActionResult Login(string email, string wachtwoord)
        {
            ViewBag.loginfail = "";

            if (Database.Login(email, wachtwoord) == true)
            {
                ViewBag.user = Database.accountNaam;
                return RedirectToAction("Home", "Home");
            }
            else if (Database.Login(email, wachtwoord) == false)
            {
                ViewBag.loginfail = "*Incorrect credentials*";
            }
                return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string naam, string wachtwoord, string email, string beschrijving, string websites, string land, string stad)
        {
            Database.RegisterUser(naam, wachtwoord, email, beschrijving, websites, land, stad);
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
    }
}