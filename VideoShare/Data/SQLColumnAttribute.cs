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

        public string AutoIncrementSequenceName
        {
            get;
            private set;
        }

        public bool IsAutoIncrement
        {
            get
            {
                return (!string.IsNullOrEmpty(AutoIncrementSequenceName));
            }
        }

        public SQLColumnAttribute(int index, string name, bool isKey = false, string autoIncrementSequence = null)
        {
            Index = index;
            Name = name;
            IsKey = isKey;

            AutoIncrementSequenceName = autoIncrementSequence;
        }
    }
}