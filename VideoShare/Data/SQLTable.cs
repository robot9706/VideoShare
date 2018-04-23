using System;

namespace VideoShare.Data
{
    public class SQLTable : Attribute
    {
        public string TableName { get; private set; }

        public SQLTable(string table)
        {
            TableName = table;
        }
    }
}