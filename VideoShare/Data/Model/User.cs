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

        public static User GetUser(string username)
        {
            using (OracleCommand command = Global.Database.CreateCommand(SQL_CheckName))
            {
                command.Parameters.Add("uname", username);

                List<User> users = Global.Database.Select<User>(command);

                if (users.Count == 1)
                    return users[0];

                return null;
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

        private const string SQL_VideoCount = "select count(*) from \"" + Video.Table + "\" where UPLOADER=:cuid";
        private const string SQL_ViewCount = "select sum(\"" + View.Table + "\".\"Views\") from \"" + View.Table + "\", \"" + Video.Table + "\" where \"" + View.Table + "\".VIDEOID=\"" + Video.Table + "\".ID and \"" + Video.Table + "\".Uploader=:cuid";

        public Tuple<int, int> GetVideosAndViews()
        {
            int videos = 0;
            int views = 0;

            using (OracleCommand command = Global.Database.CreateCommand(SQL_VideoCount))
            {
                command.Parameters.Add("cuid", ID);

                object obj = command.ExecuteScalar();
                if (!(obj is DBNull))
                {
                    videos = (int)Convert.ChangeType(obj, typeof(Int32));
                }
            }

            using (OracleCommand command = Global.Database.CreateCommand(SQL_ViewCount))
            {
                command.Parameters.Add("cuid", ID);

                object obj = command.ExecuteScalar();
                if (!(obj is DBNull))
                {
                    views = (int)Convert.ChangeType(obj, typeof(Int32));
                }
            }

            return new Tuple<int, int>(videos, views);
        }

        private const string SQL_GetVideos = "select * from \"" + Video.Table + "\" where Uploader = :cuid order by UploadTime";

        public List<Video> GetVideosDateOrdered()
        {
            using (OracleCommand command = Global.Database.CreateCommand(SQL_GetVideos))
            {
                command.Parameters.Add("cuid", ID);

                return Global.Database.Select<Video>(command);
            }
        }
        #endregion
    }
}