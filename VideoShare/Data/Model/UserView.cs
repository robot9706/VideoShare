using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoShare.Data.Model
{
	[SQLTable(Table)]
	public class UserView
	{
		public const string Table = "USERVIEWS";

		#region Structure
		[SQLColumn(0, "USERID", true)]
		public int User;

		[SQLColumn(1, "ViewedCategory", true)]
		public int ViewedCategory;
		#endregion

		#region Functions
		private const string SQL_GetViewedIDs = "select * from \"" + Table + "\" where UserID=:cuid";

		public static List<UserView> GetViews(User user)
		{
			using (OracleCommand command = Global.Database.CreateCommand(SQL_GetViewedIDs))
			{
				command.Parameters.Add("cuid", user.ID);

				return Global.Database.Select<UserView>(command);
			}
		}

		public static void UpdateUserViews(User user, Video video)
		{
			List<VideoCategory> cats = video.GetCategories();
			List<UserView> views = GetViews(user);

			foreach (VideoCategory cat in cats)
			{
				if (views.SingleOrDefault(x => (x.ViewedCategory == cat.CategoryID)) == null) //Ha a user ezt a kategóriát még nem látta
				{
					UserView newView = new Model.UserView();
					newView.User = user.ID;
					newView.ViewedCategory = cat.CategoryID;

					Global.Database.Insert<UserView>(newView);
				}
			}
		}
		#endregion
	}
}