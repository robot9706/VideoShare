using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace VideoShare.Data.Model
{
    [SQLTable(Table)]
    public class User
    {
        public const string Table = "USER";

        #region Structure
        [SQLColumn(0, "ID", true, "USER_ID_SEQ")]
        public int ID;

        [SQLColumn(1, "Username")]
        public string Username;

        [SQLColumn(2, "Email")]
        public string Email;

        [SQLColumn(3, "PasswordHash")]
        public string PasswordHash;

        [SQLColumn(4, "DisplayName")]
        public string DisplayName;

        [SQLColumn(5, "RegistrationDate")]
        public DateTime RegistrationDate;

        [SQLColumn(6, "Info")]
        public string Info;
        #endregion

        #region Functions
        private const string SQL_FindUser = "select * from \"" + Table + "\" where USERNAME=:uname and PASSWORDHASH=:pwh";

        public static User FindUser(string username, string passwordHash)
        {
            using (OracleCommand command = Global.Database.CreateCommand(SQL_FindUser))
            {
                command.Parameters.Add("uname", username);
                command.Parameters.Add("pwh", passwordHash);

                List<User> users = Global.Database.Select<User>(command);

                if (users.Count == 1)
                    return users[0];

                return null;
            }
        }

        private const string SQL_CheckName = "select * from \"" + Table + "\" where USERNAME=:uname";

        public static bool CheckName(string username)
        {
            using (OracleCommand command = Global.Database.CreateCommand(SQL_CheckName))
            {
                command.Parameters.Add("uname", username);

                List<User> users = Global.Database.Select<User>(command);

                return (users.Count > 0);
            }
        }

        public static User Register(string username, string email, string pw)
        {
            User user = new User();

            user.DisplayName = username;
            user.Username = username;
            user.PasswordHash = pw;
            user.Email = email;
            user.Info = "";
            user.RegistrationDate = DateTime.Now;

            if (Global.Database.Insert<User>(user))
                return user;

            return null;
        }
        #endregion
    }
}