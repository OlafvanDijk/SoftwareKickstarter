﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace Kikkerstarter.Models
{
    public class Database
    {
        /// <summary>
        /// ik maak static variabelen aan om te gaan gebruiken in de hele klasse.
        /// </summary>
        static OracleConnection m_conn;
        static OracleCommand m_command;
        public static Profiel profiel;
        static string connectionString = "Data Source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS=(PROTOCOL=TCP)(HOST=fhictora01.fhict.local)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=fhictora)));User ID=dbi336692;PASSWORD=Beijing1;";

        /// <summary>
        /// Methode om de connectie te openen.
        /// </summary>
        /// <returns></returns>
        public static bool OpenConnection()
        {
            bool returnvalue = false;
            m_conn = new OracleConnection();
            try
            {
                m_conn.ConnectionString = connectionString;
                m_conn.Open();
                //// Controleer of de verbinding open is zo ja return true.
                if (m_conn.State != System.Data.ConnectionState.Open)
                { return true; }
            }
            catch (Exception ex) { Console.WriteLine("Connection failed: " + ex.Message); }
            return returnvalue;
        }

        /// <summary>
        /// Methode om de connectie te sluiten.
        /// </summary>
        public static void CloseConnection()
        {
            try
            { m_conn.Close(); }
            catch (Exception ex)
            { Console.WriteLine("Connection failed: " + ex.Message); }
        }

        public static OracleCommand Command { get { return m_command; } }

        /// <summary>
        /// Login methode.
        /// </summary>
        /// <param name="emailacc"></param>
        /// <param name="wachtwoord"></param>
        /// <returns></returns>
        public static bool Login(string emailacc, string wachtwoord)
        {
            bool ok = false;
            try
            {
                ////Connectie wordt geopend.
                OpenConnection();
                m_command = new OracleCommand();
                m_command.Connection = m_conn;
                m_command.CommandText = "SELECT accountID, naamAccount, email, beschrijving, websites, land, stad FROM Account_Table WHERE email = :email AND wachtwoord = :wachtwoord";
                m_command.Parameters.Add("email", OracleDbType.Varchar2).Value = emailacc;
                m_command.Parameters.Add("wachtwoord", OracleDbType.Varchar2).Value = wachtwoord;
                m_command.ExecuteNonQuery();
                using (OracleDataReader _Reader = Command.ExecuteReader())
                {
                    try
                    {
                        ////Als de reader een row heeft zet hij die gegevens in het profiel object.
                        ////Deze variabele is helemaal bovenaan de klasse te vinden.
                        while (_Reader.Read())
                        {
                            profiel = new Profiel(Convert.ToInt32(_Reader["accountID"]), Convert.ToString(_Reader["naamAccount"]), Convert.ToString(_Reader["email"]), Convert.ToString(_Reader["beschrijving"]), Convert.ToString(_Reader["websites"]), Convert.ToString(_Reader["land"]), Convert.ToString(_Reader["stad"]));
                            ////Als het profiel's email gelijk is aan de meegegeven email wat ook niet anders kan dan
                            ////wordt ok true.
                            if (profiel.Email == emailacc) { ok = true; }
                        }
                    }
                    catch (OracleException ex)
                    {
                        CloseConnection();
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (OracleException ex)
            {
                CloseConnection();
                Console.WriteLine(ex.Message);
            }
            return ok;
        }

        /// <summary>
        /// Methode om te kunnen registreren.
        /// </summary>
        /// <param name="naam"></param>
        /// <param name="wachtwoord"></param>
        /// <param name="email"></param>
        /// <param name="beschrijving"></param>
        /// <param name="websites"></param>
        /// <param name="land"></param>
        /// <param name="stad"></param>
        public static void RegisterUser(string naam, string wachtwoord, string email, string beschrijving, string websites, string land, string stad)
        {
            try
            {
                ////Connectie wordt geopend.
                OpenConnection();
                m_command = new OracleCommand();
                m_command.Connection = m_conn;
                m_command.CommandText = "INSERT INTO Account_Table (naamAccount, email, wachtwoord, beschrijving, websites, land, stad) VALUES (:naam, :email, :wachtwoord, :beschrijving, :websites, :land, :stad)";
                m_command.Parameters.Add("naam", OracleDbType.Varchar2).Value = naam;
                m_command.Parameters.Add("email", OracleDbType.Varchar2).Value = email;
                m_command.Parameters.Add("wachtwoord", OracleDbType.Varchar2).Value = wachtwoord;
                m_command.Parameters.Add("beschrijving", OracleDbType.Varchar2).Value = beschrijving;
                m_command.Parameters.Add("websites", OracleDbType.Varchar2).Value = websites;
                m_command.Parameters.Add("land", OracleDbType.Varchar2).Value = land;
                m_command.Parameters.Add("stad", OracleDbType.Varchar2).Value = stad;
                m_command.ExecuteNonQuery();
                ////De meegegeven parameters worden geïnsert in de database.
            }
            catch (OracleException ex)
            {
                Database.CloseConnection();
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// List om alle projecten in op te slaan.
        /// </summary>
        public static List<Project> projectenhome = new List<Project>();
        /// <summary>
        /// Projecten homepage laden.
        /// </summary>
        public static void ProjectenHome()
        {
            try
            {
                ////Eerst wordt de list gecleared.
                projectenhome.Clear();
                ////De datum van vandaag in het juiste format zetten.
                DateTime a = DateTime.Today;
                a = Convert.ToDateTime(a.ToString("dd/MM/yyyy"));

                ////De connectie wordt geopend.
                OpenConnection();
                m_command = new OracleCommand();
                m_command.Connection = m_conn;
                m_command.CommandText = "SELECT p.projectID, a.naamAccount, p.naamProject, p.startDate, p.eindDate, p.genre, p.beschrijving, p.goal FROM Project_Table p, Account_Table a WHERE p.accountID = a.accountID AND :dat >= p.startDate AND :dat <= p.eindDate";
                m_command.Parameters.Add("dat", OracleDbType.Date).Value = a;
                m_command.ExecuteNonQuery();
                ////Select alle projecten en de naam van de maker van het project.
                ////De datum van vandaag moet tussen de begin en eind datum van het project vallen.
                using (OracleDataReader _Reader = Command.ExecuteReader())
                {
                    try
                    {
                        while (_Reader.Read())
                        {
                            ////Zet alle projecten in de List<Project> projectenhome.
                            DateTime start = Convert.ToDateTime(_Reader["startDate"]);
                            DateTime eind = Convert.ToDateTime(_Reader["eindDate"]);
                            Project project = new Project(Convert.ToInt32(_Reader["projectID"]), Convert.ToString(_Reader["naamAccount"]), Convert.ToString(_Reader["naamProject"]), start, eind, Convert.ToString(_Reader["genre"]), Convert.ToString(_Reader["beschrijving"]), Convert.ToString(_Reader["goal"]));
                            projectenhome.Add(project);
                        }
                    }
                    catch (OracleException ex)
                    {
                        CloseConnection();
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (OracleException ex)
            {
                CloseConnection();
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// Een project aanmaken met de megegeven gegevens.
        /// </summary>
        /// <param name="projectNaam"></param>
        /// <param name="eindDatum"></param>
        /// <param name="genre"></param>
        /// <param name="beschrijving"></param>
        /// <param name="goal"></param>
        public static void Cproject(string projectNaam, DateTime eindDatum, string genre, string beschrijving, string goal)
        {
            try
            {
                ////De datum van vandaag in het juiste format zetten.
                DateTime a = DateTime.Today;
                a = Convert.ToDateTime(a.ToString("dd/MM/yyyy"));

                ////Connectie wordt geopend.
                OpenConnection();
                m_command = new OracleCommand();
                m_command.Connection = m_conn;
                m_command.CommandText = "INSERT INTO Project_Table (accountID, naamProject, startDate, eindDate, genre, beschrijving, goal) VALUES (:id, :naam, :star, :eind, :genre, :beschrijving, :goal)";
                m_command.Parameters.Add("id", OracleDbType.Varchar2).Value = profiel.accountID;
                m_command.Parameters.Add("naam", OracleDbType.Varchar2).Value = projectNaam;
                m_command.Parameters.Add("start", OracleDbType.Date).Value = a;
                m_command.Parameters.Add("eind", OracleDbType.Date).Value = eindDatum;
                m_command.Parameters.Add("genre", OracleDbType.Varchar2).Value = genre;
                m_command.Parameters.Add("beschrijving", OracleDbType.Varchar2).Value = beschrijving;
                m_command.Parameters.Add("goal", OracleDbType.Varchar2).Value = goal;
                Command.ExecuteNonQuery();
            }
            catch (OracleException ex)
            {
                CloseConnection();
                Console.WriteLine(ex.Message);
            }
        }
    }
}