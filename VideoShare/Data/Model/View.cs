using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoShare.Data.Model
{
    [SQLTable(Table)]
    public class View
    {
        public const string Table = "VIEW";

        #region Structure
        [SQLColumn(0, "VIDEOID", true)]
        public int VideoID;

        [SQLColumn(1, "Date", true)]
        public DateTime Date;

        [SQLColumn(2, "Views")]
        public int Views;
        #endregion

        #region Functions
        private const string SQL_FindRow = "select * from \"VIEW\" where VideoID=:vid and \"Date\"=:cdate";

        public static void AddView(Video video)
        {
            DateTime checkDate = DateTime.Now.Date;

            //Létezik?
            View view = null;

            using (OracleCommand cmd = Global.Database.CreateCommand(SQL_FindRow))
            {
                cmd.Parameters.Add("vid", video.ID);
                cmd.Parameters.Add("cdate", checkDate);

                List<View> views = Global.Database.Select<View>(cmd);

                if (views.Count > 0)
                {
                    view = views[0];
                }
            }

            if (view != null)
            {
                view.Views++;

                Global.Database.Update<View>(view);
            }
            else
            {
                view = new View();
                view.VideoID = video.ID;
                view.Date = checkDate;
                view.Views = 1;

                Global.Database.Insert<View>(view);
            }
        }
        #endregion
    }
}