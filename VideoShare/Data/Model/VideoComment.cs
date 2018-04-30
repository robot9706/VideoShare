using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoShare.Data.Model
{
    [SQLTable(Table)]
    public class VideoComment
    {
        public const string Table = "VIDEOCOMMENT";

        #region Structure
        [SQLColumn(0, "USERID", true)]
        public int UserID;

        [SQLColumn(1, "VIDEOID", true)]
        public int VideoID;

        [SQLColumn(2, "Date")]
        public DateTime Date;

        [SQLColumn(3, "Comment")]
        public string Comment;
        #endregion

        #region Functions
        private const string SQL_FindComments = "select * from \"VIDEOCOMMENT\" where VIDEOID=:vdid order by \"Date\"";

        public static List<VideoComment> GetComments(int vid)
        {
            using (OracleCommand command = Global.Database.CreateCommand(SQL_FindComments))
            {
                command.Parameters.Add("vdid", vid);

                List<VideoComment> comments = Global.Database.Select<VideoComment>(command);

                return comments;
            }
        }
        #endregion
    }
}