using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kikkerstarter.Models
{
    public class Project
    {
        public int ProjectID { get; set; }

        public string Accountnaam { get; set; }

        public string NaamProject { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EindDate { get; set; }

        public string Genre { get; set; }

        public string Beschrijving { get; set; }

        public string Goal { get; set; }

        public Project(int ProjectID, string Accountnaam, string NaamProject, DateTime StartDate, DateTime EindDate, string Genre, string Beschrijving, string Goal)
        {
            this.ProjectID = ProjectID;
            this.Accountnaam = Accountnaam;
            this.NaamProject = NaamProject;
            this.StartDate = StartDate;
            this.EindDate = EindDate;
            this.Genre = Genre;
            this.Beschrijving = Beschrijving;
            this.Goal = Goal;
        }
    }
}