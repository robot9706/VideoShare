using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoShare.Data.Model
{
    public class VideoComment
    {
        [SQLColumn(0, "UserID")]
        public int UserID { get; set; }

        [SQLColumn(1, "VideoID")]
        public int VideoID { get; set; }

        [SQLColumn(2, "Date")]
        public DateTime Date { get; set; }

        [SQLColumn(3, "Comment")]
        public string Comment { get; set; }
    }
}