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

        [SQLColumn(0, "UserID", true)]
        public int UserID;

        [SQLColumn(1, "VideoID", true)]
        public int VideoID;

        [SQLColumn(2, "Date")]
        public DateTime Date;

        [SQLColumn(3, "Comment")]
        public string Comment;
    }
}