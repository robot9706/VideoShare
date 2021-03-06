﻿using Oracle.ManagedDataAccess.Client;
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

        private const string SQL_SumViews = "select sum(\"VIEW\".\"Views\") from \"VIEW\" where \"VIEW\".VIDEOID=:vdid";

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

        private const string SQL_FindUser = "select * from \"USER\" where ID=:vuid";

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

        private const string SQL_AddVote = "update \"VIDEO\" set {0}={0}+1 where ID=:vuid";

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

		public List<VideoCategory> GetCategories()
		{
			return VideoCategory.GetForVideo(this);
		}

		public string GetCategoriesListed()
        {
            List<VideoCategory> catList = VideoCategory.GetForVideo(this);

            List<string> names = new List<string>();

            foreach (VideoCategory cat in catList)
            {
                if (!Global.VideoCategories.ContainsKey(cat.CategoryID))
                    continue;

                names.Add(Global.VideoCategories[cat.CategoryID]);
            }

            return String.Join(", ", names);
        }

        private const string SQL_Delete1 = "delete from \"PLAYLISTCONTENT\" where VideoID=:vdid";
        private const string SQL_Delete2 = "delete from \"VIDEOCATEGORY\" where VideoID=:vdid";
        private const string SQL_Delete3 = "delete from \"VIDEOCOMMENT\" where VideoID=:vdid";
        private const string SQL_Delete4 = "delete from \"VIEW\" where VideoID=:vdid";

        public void Delete()
        {
            using (OracleCommand command = Global.Database.CreateCommand(SQL_Delete1))
            {
                command.Parameters.Add("vdid", ID);

                command.ExecuteNonQuery();
            }

            using (OracleCommand command = Global.Database.CreateCommand(SQL_Delete2))
            {
                command.Parameters.Add("vdid", ID);

                command.ExecuteNonQuery();
            }

            using (OracleCommand command = Global.Database.CreateCommand(SQL_Delete3))
            {
                command.Parameters.Add("vdid", ID);

                command.ExecuteNonQuery();
            }

            using (OracleCommand command = Global.Database.CreateCommand(SQL_Delete4))
            {
                command.Parameters.Add("vdid", ID);

                command.ExecuteNonQuery();
            }

            Global.Database.Delete<Video>(this);
        }
		#endregion
	}
}