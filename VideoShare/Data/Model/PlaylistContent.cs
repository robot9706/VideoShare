using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoShare.Data.Model
{
    public class PlaylistContent
    {
        [SQLColumn(0, "PlaylistID")]
        public int PlaylistID { get; set; }

        [SQLColumn(1, "VideoID")]
        public int VideoID { get; set; }
    }
}