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

        [SQLColumn(0, "VideoID")]
        public int VideoID;

        [SQLColumn(1, "CategoryID")]
        public int CategoryID;
    }
}