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
        static OracleConnection m_conn;
        static OracleCommand m_command;
        public static Profiel profiel;
        static string connectionString = "Data Source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS=(PROTOCOL=TCP)(HOST=fhictora01.fhict.local)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=fhictora)));User ID=dbi336692;PASSWORD=Beijing1;";


        public static bool OpenConnection()
        {
            bool returnvalue = false;
            m_conn = new OracleConnection();
            try
            {
                m_conn.ConnectionString = connectionString;
                m_conn.Open();
                // Controleer of de verbinding open is
                if (m_conn.State != System.Data.ConnectionState.Open)
                { return true; }
            }
            catch (Exception ex) { Console.WriteLine("Connection failed: " + ex.Message); }
            return returnvalue;
        }

        public static void CloseConnection()
        {
            try
            { m_conn.Close(); }
            catch (Exception ex)
            { Console.WriteLine("Connection failed: " + ex.Message); }
        }

        public static OracleCommand Command { get { return m_command; } }

        // Login
        public static bool Login(string emailacc, string wachtwoord)
        {
            bool ok = false;
            try
            {
                OpenConnection();
                m_command = new OracleCommand();
                m_command.Connection = m_conn;
                m_command.CommandText = "SELECT naam, email, beschrijving, websites, land, stad FROM Account_Table WHERE email = :email AND wachtwoord = :wachtwoord";
                m_command.Parameters.Add("email", OracleDbType.Varchar2).Value = emailacc;
                m_command.Parameters.Add("wachtwoord", OracleDbType.Varchar2).Value = wachtwoord;
                m_command.ExecuteNonQuery();
                using (OracleDataReader _Reader = Command.ExecuteReader())
                {
                    try
                    {
                        while (_Reader.Read())
                        {
                            profiel = new Profiel(Convert.ToString(_Reader["naam"]), Convert.ToString(_Reader["email"]), Convert.ToString(_Reader["beschrijving"]), Convert.ToString(_Reader["websites"]), Convert.ToString(_Reader["land"]), Convert.ToString(_Reader["stad"]));
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

        // Registreren
        public static void RegisterUser(string naam, string wachtwoord, string email, string beschrijving, string websites, string land, string stad)
        {
            try
            {
                OpenConnection();
                m_command = new OracleCommand();
                m_command.Connection = m_conn;
                m_command.CommandText = "INSERT INTO Account_Table (naam, email, wachtwoord, beschrijving, websites, land, stad) VALUES (:naam, :email, :wachtwoord, :beschrijving, :websites, :land, :stad)";
                m_command.Parameters.Add("naam", OracleDbType.Varchar2).Value = naam;
                m_command.Parameters.Add("email", OracleDbType.Varchar2).Value = email;
                m_command.Parameters.Add("wachtwoord", OracleDbType.Varchar2).Value = wachtwoord;
                m_command.Parameters.Add("beschrijving", OracleDbType.Varchar2).Value = beschrijving;
                m_command.Parameters.Add("websites", OracleDbType.Varchar2).Value = websites;
                m_command.Parameters.Add("land", OracleDbType.Varchar2).Value = land;
                m_command.Parameters.Add("stad", OracleDbType.Varchar2).Value = stad;
                m_command.ExecuteNonQuery();
            }
            catch (OracleException ex)
            {
                Database.CloseConnection();
                Console.WriteLine(ex.Message);
            }
        }

        public static List<Project> projectenhome = new List<Project>();
        // Projecten homepage laden
        public static void ProjectenHome()
        {
            try
            {
                OpenConnection();
                m_command = new OracleCommand();
                m_command.Connection = m_conn;
                m_command.CommandText = "SELECT p.projectID, a.naam, p.naam, p.startDate, p.eindDate, p.genre, p.beschrijving, p.goal FROM Project_Table p LEFT JOIN Account_Table a ON p.accountID = a.accountID";
                //WHERE: date >= p.startDate AND: date <= p.eindDate
                //m_command.Parameters.Add("date", OracleDbType.Varchar2).Value = DateTime.Today.ToString("dd/MM/yyyy");
                m_command.ExecuteNonQuery();
                using (OracleDataReader _Reader = Command.ExecuteReader())
                {
                    try
                    {
                        while (_Reader.Read())
                        {
                            CultureInfo provider = CultureInfo.InvariantCulture;
                            string start = Convert.ToString(_Reader["p.startDate"]);
                            string end = Convert.ToString(_Reader["p.eindDate"]);
                            DateTime startdate = DateTime.ParseExact(start, "dd/MM/yyyy", provider);
                            DateTime enddate = DateTime.ParseExact(end, "dd/MM/yyyy", provider);
                            Project project = new Project(Convert.ToInt32(_Reader["p.accountID"]), Convert.ToString(_Reader["a.naam"]), Convert.ToString(_Reader["p.naam"]), startdate, enddate, Convert.ToString(_Reader["p.genre"]), Convert.ToString(_Reader["p.beschrijving"]), Convert.ToString(_Reader["p.goal"]));
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
    }
}