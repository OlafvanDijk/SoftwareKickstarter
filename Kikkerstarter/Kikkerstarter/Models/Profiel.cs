﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kikkerstarter.Models
{
    /// <summary>
    /// De klasse Profiel wordt gebruikt voor het opslaan van de gegevens van iemand die is ingelogd.
    /// De klasse kan leeg worden aangemaakt dit is voor makkelijk gebruik.
    /// </summary>
    public class Profiel
    {
        public int accountID { get; set; }

        public string Naam { get; set; }

        public string Email { get; set; }

        public string Beschrijving { get; set; }

        public string Websites { get; set; }

        public string Land { get; set; }

        public string Stad { get; set; }

        public Profiel()
        {

        }

        public Profiel(int accountID,string Naam, string Email, string Beschrijving, string Websites, string Land, string Stad)
        {
            this.accountID = accountID;
            this.Naam = Naam;
            this.Email = Email;
            this.Beschrijving = Beschrijving;
            this.Websites = Websites;
            this.Land = Land;
            this.Stad = Stad;
        }
    }
}