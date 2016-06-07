using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace Kikkerstarter.Models
{
    public class Database
    {
        static OracleConnection m_conn;
        static OracleCommand m_command;
        static string connectionString = "Data Source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SID=xe)));User ID=system;PASSWORD=Beijing1;";


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

        public static string email = "";
        public static string accountNaam = "";
        public static bool Login(string emailacc, string wachtwoord)
        {
            string emaild = "";
            bool ok = false;
            try
            {
                OpenConnection();
                m_command = new OracleCommand();
                m_command.Connection = m_conn;
                m_command.CommandText = "SELECT naam, email, wachtwoord FROM Account_Table WHERE wachtwoord = :wachtwoord AND email = :email";
                m_command.Parameters.Add("wachtwoord", OracleDbType.Varchar2).Value = wachtwoord;
                m_command.Parameters.Add("email", OracleDbType.Varchar2).Value = emailacc;
                m_command.ExecuteNonQuery();
                using (OracleDataReader _Reader = Command.ExecuteReader())
                {
                    try
                    {
                        while (_Reader.Read())
                        {
                            string account = Convert.ToString(_Reader["naam"]);
                            accountNaam = account;
                            emaild = Convert.ToString(_Reader["email"]);
                            email = emaild;
                            if (emaild == emailacc) { ok = true; }
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
    }
}