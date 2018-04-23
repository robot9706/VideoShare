using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoShare.Data.Model
{
    public class Category
    {
        [SQLColumn(0, "ID", true)]
        public int ID { get; set; }

        [SQLColumn(1, "Name")]
        public string Name { get; set; }
    }
}