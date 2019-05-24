using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizUnitParse
{
    public class MetaDataInfo : ResourceInfo
    {
        public string name { get; set; }
        public string alias { get; set; }
        public string package { get; set; }
        public string fullName { get; set; }
        public int hashCode { get; set; }
    }
}
