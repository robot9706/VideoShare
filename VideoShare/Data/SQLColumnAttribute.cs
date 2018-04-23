using System;

namespace VideoShare.Data
{
    public class SQLColumnAttribute : Attribute
    {
        public int Index
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public bool IsKey
        {
            get;
            private set;
        }

        public SQLColumnAttribute(int index, string name, bool isKey = false)
        {
            Index = index;
            Name = name;
            IsKey = IsKey;
        }
    }
}