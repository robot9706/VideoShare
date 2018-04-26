using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoShare.Data.Model
{
    [SQLTable(Table)]
    public class View
    {
        public const string Table = "VIEW";

        [SQLColumn(0, "VideoID")]
        public int VideoID;

        [SQLColumn(1, "Date")]
        public DateTime Date;

        [SQLColumn(2, "Views")]
        public int Views;
    }
}