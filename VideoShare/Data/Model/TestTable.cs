using System;

namespace VideoShare.Data.Model
{
    [SQLTable("TestTable")]
    public class TestTable
    {
        [SQLColumn(0, "ID", true, "TT_IS")]
        public int ID;

        [SQLColumn(1, "StringColumn")]
        public string StringColumn;

        [SQLColumn(2, "IntColumn")]
        public int IntColumn;

        [SQLColumn(3, "DateColumn")]
        public DateTime DateColumn;
    }
}