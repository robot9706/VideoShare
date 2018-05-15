using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace VideoShare.Data
{
    public class ObjectParser
    {
        public struct ColumnInfo
        {
            public int Index;
            public string Name;

            public FieldInfo Field;

            public bool IsKey;
            public bool IsAutoIncrement;

            public string AutoIncrementSequence;
        }

        private Dictionary<int, ColumnInfo> _fields;
        private bool _hasKey = false;

        public ObjectParser(Type type)
        {
            _fields = new Dictionary<int, ColumnInfo>();

            FieldInfo[] fields = type.GetFields();
            if (fields.Length == 0)
                throw new Exception("The type doesn't have any fields! " + type.FullName);

            SQLColumnAttribute attribute;
            foreach (FieldInfo p in fields)
            {
                attribute = GetAttribute<SQLColumnAttribute>(p);

                if (attribute != null)
                {
                    _fields.Add(attribute.Index, new ColumnInfo()
                    {
                        Field = p,
                        Index = attribute.Index,
                        Name = attribute.Name,
                        IsAutoIncrement = attribute.IsAutoIncrement,
                        IsKey = attribute.IsKey,
                        AutoIncrementSequence = attribute.AutoIncrementSequenceName
                    });

                    if (attribute.IsKey)
                    {
                        _hasKey = true;
                    }
                }
            }
        }

        private static T GetAttribute<T>(FieldInfo field) where T : Attribute
        {
            object[] atrs = field.GetCustomAttributes(true);

            foreach (object atr in atrs)
            {
                if (atr.GetType().IsEquivalentTo(typeof(T)))
                    return (T)atr;
            }

            return null;
        }

        public T ParseItem<T>(OracleDataReader reader)
        {
            T newItem = (T)Activator.CreateInstance(typeof(T));

            foreach (KeyValuePair<int, ColumnInfo> prop in _fields)
            {
                object value = reader.GetValue(prop.Key);
                object typeChangedValue = null;

                if (value is DBNull)
                {
					if (prop.Value.Field.FieldType.Equals(typeof(string)))
					{
						typeChangedValue = string.Empty;
					}
					else
					{
						typeChangedValue = Activator.CreateInstance(prop.Value.Field.FieldType);
					}
                }
                else
                {
                    typeChangedValue = Convert.ChangeType(value, prop.Value.Field.FieldType);
                }

                prop.Value.Field.SetValue(newItem, typeChangedValue);
            }

            return newItem;
        }

        public string BuildInsertStatement<T>(T value, OracleCommand command)
        {
            List<string> columns = new List<string>();
            List<string> values = new List<string>();

            foreach (KeyValuePair<int, ColumnInfo> pair in _fields)
            {
                columns.Add("\"" + pair.Value.Name + "\"");

                if (pair.Value.IsAutoIncrement)
                {
                    values.Add(pair.Value.AutoIncrementSequence + ".nextVal");
                }
                else
                {
                    object fieldValue = pair.Value.Field.GetValue(value);

                    values.Add(":p" + pair.Value.Name);
                    command.Parameters.Add(new OracleParameter("p" + pair.Value.Name, fieldValue));
                }
            }

            return "(" + String.Join(", ", columns) + ") values (" + String.Join(", ", values) + ")";
        }

        public string BuildKeyCondition<T>(T value, OracleCommand command)
        {
            List<string> conditionList = new List<string>();

            if (!_hasKey)
                throw new Exception("The entity doesn't have any keys!");

            foreach (KeyValuePair<int, ColumnInfo> pair in _fields)
            {
                if (!pair.Value.IsKey)
                    continue;

                object fieldValue = pair.Value.Field.GetValue(value);

                command.Parameters.Add(new OracleParameter("k" + pair.Value.Name, fieldValue));

                conditionList.Add("\"" + pair.Value.Name + "\"=:k" + pair.Value.Name);
            }

            return String.Join(" and ", conditionList);
        }

        public string BuildSetStatement<T>(T value, OracleCommand command)
        {
            List<string> valueList = new List<string>();

            foreach (KeyValuePair<int, ColumnInfo> pair in _fields)
            {
                if (pair.Value.IsKey)
                    continue;

                object fieldValue = pair.Value.Field.GetValue(value);

                command.Parameters.Add(new OracleParameter("v" + pair.Value.Name, fieldValue));

                valueList.Add("\"" + pair.Value.Name + "\"=:v" + pair.Value.Name);
            }

            return String.Join(", ", valueList);
        }

        public string BuildDeleteStatement<T>(T value, OracleCommand command)
        {
            List<string> valueList = new List<string>();

            foreach (KeyValuePair<int, ColumnInfo> pair in _fields)
            {
                if (_hasKey && !pair.Value.IsKey)
                {
                    continue;
                }

                object fieldValue = pair.Value.Field.GetValue(value);

                command.Parameters.Add(new OracleParameter("c" + pair.Value.Name, fieldValue));

                valueList.Add("\"" + pair.Value.Name + "\"=:c" + pair.Value.Name);
            }

            return String.Join(", ", valueList);
        }

        private const string SQL_SequenceQuery = "select {0}.currVal from dual"; //"select \"LAST_NUMBER\" from \"DBA_SEQUENCES\" where \"SEQUENCE_NAME\"=:sn";

        public int GetSequenceValue()
        {
            if (!_hasKey)
                throw new Exception("No key!");

            ColumnInfo? info = null;

            foreach (KeyValuePair<int, ColumnInfo> pair in _fields)
            {
                if (!pair.Value.IsKey)
                    continue;

                if (!pair.Value.IsAutoIncrement)
                    continue;

                if (info.HasValue)
                    throw new Exception("Multiple keys!");

                info = pair.Value;
            }

            if (!info.HasValue)
                throw new Exception("No key found!");

            using (OracleCommand command = Global.Database.CreateCommand(String.Format(SQL_SequenceQuery, info.Value.AutoIncrementSequence)))
            {
                object ret = command.ExecuteScalar();
                if (ret is DBNull)
                    return 0;

                return (int)Convert.ChangeType(ret, typeof(Int32));
            }
        }
    }
}