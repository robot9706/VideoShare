using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoShare.Data.Model;

namespace VideoShare.Data
{
	public class VideoQuery
	{
		private const string SQL_GetLatest = "select * from \"VIDEO\" where rownum<={0} order by UploadTime desc";

		public static List<Video> GetLatestVideos(int max)
		{
			using (OracleCommand command = Global.Database.CreateCommand(String.Format(SQL_GetLatest, max)))
			{
				return Global.Database.Select<Video>(command);
			}
		}

		private const string SQL_GetMostActiveUploader = "select \"VIDEO\".Uploader from \"VIDEO\" group by Uploader order by count(ID) desc";

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

		private const string SQL_GetMostActiveCommenter = "select \"VIDEOCOMMENT\".UserID from \"VIDEOCOMMENT\" group by UserID order by count(*) desc";

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

		private const string SQL_Search = "select * from \"VIDEO\" where {0}";

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

		private const string SQL_GetRandom = "select * from (select * from \"VIDEO\" order by DBMS_RANDOM.VALUE) where rownum<={0}";

		public static List<Video> GetRandomVideos(int max)
		{
			using (OracleCommand command = Global.Database.CreateCommand(String.Format(SQL_GetRandom, max)))
			{
				return Global.Database.Select<Video>(command);
			}
		}

		private const string SQL_GetRandomSimilar = "SELECT * from TABLE(SuggestPackage.SuggestVideos(:cuid, :cmaxrows))";

		public static List<Video> GetRandomSimilarVideos(User forUser, int max)
		{
			using (OracleCommand command = Global.Database.CreateCommand(SQL_GetRandomSimilar))
			{
				command.Parameters.Add("cuid", forUser.ID);
				command.Parameters.Add("cmaxrows", max);

				return Global.Database.Select<Video>(command);
			}
		}

		#region Legnézetebb és legújabb videók
		private const string SQL_FilterCategoryByTimeAll = "select \"VIDEO\".* " +
														"from \"VIDEO\" " +
														"where rownum<={0} " +
														"order by \"VIDEO\".UploadTime desc";

		private const string SQL_FilterCategoryByTime = "select \"VIDEO\".* " +
														"from \"VIDEO\", \"VIDEOCATEGORY\" " +
														"where \"VIDEO\".ID=\"VIDEOCATEGORY\".VideoID and rownum<={0} and \"VIDEOCATEGORY\".CategoryID=:fcat " +
														"order by \"VIDEO\".UploadTime desc";

		private const string SQL_FilterCategoryByViewsAll = "select * from (" +
																"select \"VIDEO\".ID " +
																"from \"VIDEO\", \"VIEW\" " +
																"where \"VIEW\".VideoID=\"VIDEO\".ID " +
																"group by \"VIDEO\".ID " +
																"order by sum(\"VIEW\".\"Views\") desc " +
															") where rownum<={0}";

		private const string SQL_FilterCategoryByViews = "select * from (" +
															"select \"VIDEO\".ID " +
															"from \"VIDEO\", \"VIEW\", \"VIDEOCATEGORY\" " +
															"where \"VIEW\".VideoID=\"VIDEO\".ID and \"VIDEOCATEGORY\".VideoID=\"VIDEO\".ID and \"VIDEOCATEGORY\".CategoryID=:fcat " +
															"group by \"VIDEO\".ID " +
															"order by sum(\"VIEW\".\"Views\") desc " +
														 ") where rownum<={0}";

		public enum CategoryFilterType
		{
			Time,
			Views
		}

		public static List<Video> GetFilteredCategoryVideos(int? category, CategoryFilterType filter, int max)
		{
			string queryString = string.Empty;
			switch(filter)
			{
				case CategoryFilterType.Time:
					queryString = String.Format((category.HasValue ? SQL_FilterCategoryByTime : SQL_FilterCategoryByTimeAll), max);
					break;
				case CategoryFilterType.Views:
					queryString = String.Format((category.HasValue ? SQL_FilterCategoryByViews : SQL_FilterCategoryByViewsAll), max);
					break;

				default:
					throw new Exception("Unknown filter");
			}

			using (OracleCommand command = Global.Database.CreateCommand(queryString))
			{
				if (category.HasValue)
				{
					command.Parameters.Add("fcat", category.Value);
				}

				if (filter == CategoryFilterType.Time)
				{
					return Global.Database.Select<Video>(command);
				}
				else
				{
					using (OracleDataReader reader = command.ExecuteReader())
					{
						List<Video> videos = new List<Video>();

						while (reader.Read())
						{
							int vID = reader.GetInt32(0);

							videos.Add(Video.FindVideo(vID));
						}

						return videos;
					}
				}
			}
		}
		#endregion

		#region Legnépszerűbb videók felbontva
		public enum DateFilterType
		{
			Day,
			Week,
			Month
		}

		private const string SQL_GetDatedVideos = "select * from (" +
													"select \"VIDEO\".ID " +
													"from \"VIDEO\", \"VIEW\" where \"VIEW\".VideoID = \"VIDEO\".ID and \"VIEW\".\"Date\" between to_date('{0}', 'yyyy-mm-dd') and to_date('{1}', 'yyyy-mm-dd') " +
													"group by \"VIDEO\".ID order by sum(\"VIEW\".\"Views\") desc " +
												  ") where rownum<=1";

		public static List<Video> GetDatedCategoryVideos(DateFilterType filter, int max)
		{
			List<Video> videos = new List<Model.Video>();

			DateTime begin = DateTime.Now;
			DateTime end;

			for (int x = 0; x < max; x++)
			{
				end = DoOffsetTime(begin, filter);

				using (OracleCommand command = Global.Database.CreateCommand(String.Format(SQL_GetDatedVideos, end.Date.ToString("yyyy-MM-dd"), begin.Date.ToString("yyyy-MM-dd"))))
				{
					using (OracleDataReader reader = command.ExecuteReader())
					{
						if (reader.HasRows && reader.Read())
						{
							int vID = reader.GetInt32(0);

							videos.Add(Video.FindVideo(vID));
						}
					}
				}

				begin = DoOffsetTime(begin, filter);
			}

			return videos;
		}

		private static DateTime DoOffsetTime(DateTime time, DateFilterType filter)
		{
			switch (filter)
			{
				case DateFilterType.Day:
					return time.AddDays(-1);

				case DateFilterType.Week:
					return time.AddDays(-7);

				case DateFilterType.Month:
					return time.AddMonths(-1);
			}

			return time;
		}
		#endregion
	}
}