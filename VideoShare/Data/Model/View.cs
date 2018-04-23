using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoShare.Data.Model
{
    public class View
    {
        [SQLColumn(0, "VideoID")]
        public int VideoID { get; set; }

        [SQLColumn(1, "Date")]
        public DateTime Date { get; set; }

        [SQLColumn(2, "Views")]
        public int Views { get; set; }
    }
}