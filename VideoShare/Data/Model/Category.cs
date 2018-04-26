using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoShare.Data.Model
{
    [SQLTable(Table)]
    public class Category
    {
        public const string Table = "CATEGORY";

        [SQLColumn(0, "ID", true, "CATEGORY_ID_SEQ")]
        public int ID;

        [SQLColumn(1, "Name")]
        public string Name;
    }
}