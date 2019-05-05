using System.Collections.Generic;

namespace EntityParse
{
    public class EntityInfo
    {
        public List<FieldInfo> fields = new List<FieldInfo>();

        public string name { get; set; }
        public string alias { get; set; }
        public string tableName { get; set; }
        public string bosType { get; set; }
        public string package { get; set; }
        public string fullName { get; set; }

        public EntityInfo baseEntity { get; set; }

        public void Add(FieldInfo fieldInfo)
        {
            fields.Add(fieldInfo);
        }

        public FieldInfo Get(int i)
        {
            if (fields.Count > i)
            {
                return fields[i];
            }
            return null;
        }

        public FieldInfo GetField(string name)
        {
            for (int i = 0; i < fields.Count; i++)
            {
                if (fields[i].name == name)
                {
                    return fields[i];
                }
            }
            FieldInfo fieldInfo = new FieldInfo();
            fieldInfo.name = name;
            fieldInfo.alias = "null";
            fieldInfo.tableName = "null";
            fieldInfo.dataType = "default";
            fields.Add(fieldInfo);
            return fieldInfo;
        }
    }
}
