using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoShare.Data.Model
{
    [SQLTable(Table)]
    public class Video
    {
        public const string Table = "VIDEO";

        #region Structure
        [SQLColumn(0, "ID", true, "VIDEO_ID_SEQ")]
        public int ID;

        [SQLColumn(1, "TITLE")]
        public string Title;

        [SQLColumn(2, "DESCRIPTION")]
        public string Description;

        [SQLColumn(3, "Length")]
        public int Length;

        [SQLColumn(4, "UPLOADER")]
        public int Uploader;

        [SQLColumn(5, "UPLOADTIME")]
        public DateTime UploadTime;

        [SQLColumn(6, "Likes")]
        public int Likes;

        [SQLColumn(7, "DISLIKES")]
        public int Dislikes;
        #endregion

        #region Functions
        public string GetContentLink()
        {
            return "Content/placeholder.mp4";
        }

        public string GetThumbnailLink()
        {
            return "Content/placeholder.png";
        }

        private const string SQL_SumViews = "select sum(\"" + View.Table + "\".\"Views\") from \"" + View.Table + "\" where \"" + View.Table + "\".VIDEOID=:vdid";

        public int GetViews()
        {
            int time = 0;

            using (OracleCommand command = Global.Database.CreateCommand(SQL_SumViews))
            {
                command.Parameters.Add("vdid", ID);

                object obj = command.ExecuteScalar();
                if (!(obj is DBNull))
                {
                    time = (int)Convert.ChangeType(obj, typeof(Int32));
                }
            }

            return time;
        }

        private const string SQL_FindUser = "select * from \"" + User.Table + "\" where ID=:vuid";

        public User GetUploader()
        {
            using (OracleCommand command = Global.Database.CreateCommand(SQL_FindUser))
            {
                command.Parameters.Add("vuid", Uploader);

                List<User> users = Global.Database.Select<User>(command);

                if (users.Count == 1)
                    return users[0];

                return null;
            }
        }

        private const string SQL_FindVideo = "select * from \"" + Video.Table + "\" where ID=:vuid";

        public static Video FindVideo(int id)
        {
            using (OracleCommand command = Global.Database.CreateCommand(SQL_FindVideo))
            {
                command.Parameters.Add("vuid", id);

                List<Video> videos = Global.Database.Select<Video>(command);

                if (videos.Count == 1)
                    return videos[0];

                return null;
            }
        }

        private const string SQL_AddVote = "update \"" + Video.Table + "\" set {0}={0}+1 where ID=:vuid";

        public bool Upvote()
        {
            using (OracleCommand command = Global.Database.CreateCommand(String.Format(SQL_AddVote, "\"Likes\"")))
            {
                command.Parameters.Add("vuid", ID);

                return (command.ExecuteNonQuery() > 0);
            }
        }

        public bool Downvote()
        {
            using (OracleCommand command = Global.Database.CreateCommand(String.Format(SQL_AddVote, "\"DISLIKES\"")))
            {
                command.Parameters.Add("vuid", ID);

                return (command.ExecuteNonQuery() > 0);
            }
        }
        #endregion
    }
}