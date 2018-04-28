﻿using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoShare.Data.Model;

namespace VideoShare.Data
{
	public class VideoQuery
	{
		private const string SQL_GetLatest = "select * from \"" + Video.Table + "\" where rownum<={0} order by UploadTime desc";

		public static List<Video> GetLatestVideos(int max)
		{
			using (OracleCommand command = Global.Database.CreateCommand(String.Format(SQL_GetLatest, max)))
			{
				return Global.Database.Select<Video>(command);
			}
		}

		private const string SQL_GetMostActiveUploader = "select \"" + Video.Table + "\".Uploader from \"" + Video.Table + "\" group by Uploader order by count(ID) desc";

		public static List<Video> GetMostActiveUploaderVideos(int max)
		{
			using (OracleCommand command = Global.Database.CreateCommand(SQL_GetMostActiveUploader))
			{
				object uploader = command.ExecuteScalar();
				if (uploader is DBNull)
					return new List<Video>();

				int uploaderID = (int)Convert.ChangeType(uploader, typeof(Int32));

				User user = User.GetUserByID(uploaderID);
				if (user == null)
					return new List<Video>();

				return user.GetVideosDateOrdered(-1, max);
			}
		}

		private const string SQL_GetMostActiveCommenter = "select \"" + VideoComment.Table + "\".UserID from \"" + VideoComment.Table + "\" group by UserID order by count(*) desc";

		public static List<Video> GetMostActiveCommenterVideos(int max)
		{
			using (OracleCommand command = Global.Database.CreateCommand(SQL_GetMostActiveCommenter))
			{
				object uploader = command.ExecuteScalar();
				if (uploader is DBNull)
					return new List<Video>();

				int uploaderID = (int)Convert.ChangeType(uploader, typeof(Int32));

				User user = User.GetUserByID(uploaderID);
				if (user == null)
					return new List<Video>();

				return user.GetVideosDateOrdered(-1, max);
			}
		}

		private const string SQL_Search = "select * from \"" + Video.Table + "\" where {0}";

		public static List<Video> Search(string query)
		{
			string[] parts = query.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

			if (parts.Length == 0)
				return null;

			List<string> likes = new List<string>();
			foreach(string part in parts)
			{
				likes.Add("LOWER(Title) like '%" + part + "%'");
			}

			string like = string.Join(" or ", likes);

			using (OracleCommand command = Global.Database.CreateCommand(String.Format(SQL_Search, like)))
			{
				return Global.Database.Select<Video>(command);
			}
		}
	}
}