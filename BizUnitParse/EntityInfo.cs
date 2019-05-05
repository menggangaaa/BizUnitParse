using System.Collections.Generic;

namespace EntityParse
{
    class EntityInfo
    {
        public List<FieldInfo> fields = new List<FieldInfo>();
        public string name { get; set; }
        public string alias { get; set; }
        public string tableName { get; set; }
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
            return null;
        }
    }
}
