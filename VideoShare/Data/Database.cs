using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace VideoShare.Data
{
    public class Database
    {
        private const string ConnectionString = "User Id={0};Password={1};Data Source={2}";

        private OracleConnection _connection;
        private Dictionary<Type, ObjectParser> _objectParsers;

        public Database()
        {
            string dbServer = ConfigurationManager.AppSettings["DB_Server"];
            string dbUser = ConfigurationManager.AppSettings["DB_User"];
            string dbPassword = ConfigurationManager.AppSettings["DB_Password"];

            _connection = new OracleConnection(String.Format(ConnectionString, dbUser, dbPassword, dbServer));

            _objectParsers = new Dictionary<Type, Data.ObjectParser>();
        }

        #region Connection
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
        #endregion

        #region Tools
        private ObjectParser GetParser<T>()
        {
            Type t = typeof(T);

            if (!_objectParsers.ContainsKey(t))
                CacheParser<T>();

            return _objectParsers[t];
        }

        public void CacheParser<T>()
        {
            Type t = typeof(T);

            if (_objectParsers.ContainsKey(t))
                return;

            _objectParsers.Add(t, new ObjectParser(t));
        }

        private string GetTableNameFromType<T>()
        {
            SQLTable[] tables = (SQLTable[])typeof(T).GetCustomAttributes(typeof(SQLTable), false);

            if (tables.Length != 1)
                throw new Exception("Invalid table type: " + typeof(T).FullName);

            return tables[0].TableName;
        }

        public OracleCommand CreateCommand(string commandText)
        {
            OracleCommand command = new OracleCommand();

            command.Connection = _connection;
            command.CommandText = commandText;
            command.CommandType = CommandType.Text;

            return command;
        }

        public int GetSequenceCurrentID<T>()
        {
            ObjectParser parser = GetParser<T>();

            return parser.GetSequenceValue();
        }
        #endregion

        #region Select
        public List<T> SelectAll<T>()
        {
            List<T> list = new List<T>();

            ObjectParser parser = GetParser<T>();
            string table = GetTableNameFromType<T>();

            using (OracleCommand command = new OracleCommand())
            {
                command.Connection = _connection;
                command.CommandText = "select * from \"" + table.ToUpper() + "\"";
                command.CommandType = CommandType.Text;

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(parser.ParseItem<T>(reader));
                    }
                }
            }

            return list;
        }

        public List<T> Select<T>(OracleCommand command)
        {
            List<T> list = new List<T>();

            ObjectParser parser = GetParser<T>();

            using (OracleDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(parser.ParseItem<T>(reader));
                }
            }

            return list;
        }
        #endregion

        #region Insert
        public bool Insert<T>(T newValue)
        {
            ObjectParser parser = GetParser<T>();
            string table = GetTableNameFromType<T>();

            using (OracleCommand command = new OracleCommand())
            {
                command.Connection = _connection;
                command.CommandText = "insert into \"" + table.ToUpper() + "\" " + parser.BuildInsertStatement<T>(newValue, command);
                command.CommandType = CommandType.Text;

                return (command.ExecuteNonQuery() > 0);
            }
        }
        #endregion

        #region Update
        public bool Update<T>(T newValue)
        {
            ObjectParser parser = GetParser<T>();
            string table = GetTableNameFromType<T>();

            using (OracleCommand command = new OracleCommand())
            {
                command.Connection = _connection;
                command.CommandText = "update \"" + table.ToUpper() + "\" set " + parser.BuildSetStatement<T>(newValue, command) + " where " + parser.BuildKeyCondition<T>(newValue, command);
                command.CommandType = CommandType.Text;

                return (command.ExecuteNonQuery() > 0);
            }
        }
        #endregion

        #region Delete
        public bool Delete<T>(T value)
        {
            ObjectParser parser = GetParser<T>();
            string table = GetTableNameFromType<T>();

            using (OracleCommand command = new OracleCommand())
            {
                command.Connection = _connection;
                command.CommandText = "delete from \"" + table.ToUpper() + "\" where " + parser.BuildDeleteStatement<T>(value, command);
                command.CommandType = CommandType.Text;

                return (command.ExecuteNonQuery() > 0);
            }
        }
        #endregion
    }
}