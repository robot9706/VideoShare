using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoShare.Data.Model
{
    public class VideoCategory
    {
        [SQLColumn(0, "VideoID")]
        public int VideoID { get; set; }

        [SQLColumn(1, "CategoryID")]
        public int CategoryID { get; set; }
    }
}