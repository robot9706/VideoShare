using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoShare.Data.Model
{
    [SQLTable(Table)]
    public class VideoCategory
    {
        public const string Table = "VIDEOCATEGORY";

        #region Structure
        [SQLColumn(0, "VIDEOID")]
        public int VideoID;

        [SQLColumn(1, "CATEGORYID")]
        public int CategoryID;
        #endregion

        #region Functions
        private const String SQL_GetCategory = "select * from \"" + Table + "\" where \"VIDEOID\"=:vdid";

        public static List<VideoCategory> GetForVideo(Video v)
        {
            using (OracleCommand command = Global.Database.CreateCommand(SQL_GetCategory))
            {
                command.Parameters.Add("vdid", v.ID);

                return Global.Database.Select<VideoCategory>(command);
            }
        }
        #endregion
    }
}