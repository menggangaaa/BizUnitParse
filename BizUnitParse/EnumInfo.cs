using System.Collections.Generic;

namespace BizUnitParse
{
    public class EnumInfo : MetaDataInfo
    {
        public List<EnumValueInfo> enumValues = new List<EnumValueInfo>();
        public string enumDataType { get; set; }

        public void Add(EnumValueInfo enumValueInfo)
        {
            enumValueInfo.rs = rs;
            enumValues.Add(enumValueInfo);
        }

        public EnumValueInfo Get(int i)
        {
            if (enumValues.Count > i)
            {
                return enumValues[i];
            }
            return null;
        }

        public EnumValueInfo GetField(string name)
        {
            for (int i = 0; i < enumValues.Count; i++)
            {
                if (enumValues[i].name == name)
                {
                    return enumValues[i];
                }
            }
            EnumValueInfo enumValueInfo = new EnumValueInfo();
            enumValueInfo.name = name;
            enumValueInfo.alias = "null";
            enumValueInfo.value = "null";
            enumValues.Add(enumValueInfo);
            return enumValueInfo;
        }
    }
}
