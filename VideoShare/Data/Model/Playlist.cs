using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoShare.Data.Model
{
    public class Playlist
    {
        [SQLColumn(0, "ID", true)]
        public int ID { get; set; }

        [SQLColumn(1, "Creator")]
        public int Creator { get; set; }

        [SQLColumn(2, "CreationDate")]
        public DateTime CreationDate { get; set; }

        [SQLColumn(3, "Title")]
        public string Title { get; set; }
    }
}