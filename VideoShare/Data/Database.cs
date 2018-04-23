using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace VideoShare.Data
{
    public class Database
    {
        private const string ConnectionString = "User Id={0};Password={1};Data Source={2}";

        private OracleConnection _connection;

        public Database()
        {
            string dbServer = ConfigurationManager.AppSettings["DB_Server"];
            string dbUser = ConfigurationManager.AppSettings["DB_User"];
            string dbPassword = ConfigurationManager.AppSettings["DB_Password"];

            _connection = new OracleConnection(String.Format(ConnectionString, dbUser, dbPassword, dbServer));
        }

        public bool Open()
        {
            try
            {
                _connection.Open();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Close()
        {
            try
            {
                _connection.Close();
            }
            catch
            { }
        }
    }
}