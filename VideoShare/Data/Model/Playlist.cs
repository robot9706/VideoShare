using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoShare.Data.Model
{
    [SQLTable(Table)]
    public class Playlist
    {
        public const string Table = "PLAYLIST";

        [SQLColumn(0, "ID", true, "PLAYLIST_ID_SEQ")]
        public int ID;

        [SQLColumn(1, "CREATOR")]
        public int Creator;

        [SQLColumn(2, "CREATIONDATE")]
        public DateTime CreationDate;

        [SQLColumn(3, "TITLE")]
        public string Title;
    }
}