using BizUnitParse;

namespace EntityParse
{
    public class FieldInfo : ResourceInfo
    {
        public EntityInfo entity { get; set; }
        public string name { get; set; }
        public string alias { get; set; }
        public string mappingField { get; set; }
        public string metadataRef { get; set; }
        public string relationship { get; set; }
        public string dataType { get; set; }
        public string fullPath { get; set; }
    }
}
