using EntityParse;

namespace BizUnitParse
{
    public class RelationInfo : ResourceInfo
    {
        public string name { get; set; }
        public string alias { get; set; }
        public string package { get; set; }
        public string fullName { get; set; }
        public string supplierObject { get; set; }
        public string clientObject { get; set; }
    }
}
