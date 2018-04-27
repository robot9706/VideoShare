using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoShare.Data.Model
{
    [SQLTable(Table)]
    public class PlaylistContent
    {
        public const string Table = "PLAYLISTCONTENT";

        [SQLColumn(0, "PLAYLISTID")]
        public int PlaylistID;

        [SQLColumn(1, "VIDEOID")]
        public int VideoID;
    }
}