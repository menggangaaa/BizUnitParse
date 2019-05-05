using System.Xml;

namespace EntityParse
{
    public class MetaDataUtil
    {
        //获取包名或者业务单元名称
        public static string getPackageOrBizunitName(XmlTextReader reader, string typeName)
        {
            string name = "";
            if (reader != null)
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == "rs")
                        {
                            string keyStr = reader.GetAttribute("key");
                            int start = keyStr.IndexOf(typeName + "[");
                            int end = keyStr.IndexOf("].alias");
                            if (start >= 0 && end > 0)
                            {
                                if (reader.ReadToDescendant("lang"))
                                {
                                    do
                                    {
                                        keyStr = reader.GetAttribute("locale");
                                        if (keyStr == "en_US")
                                        {
                                            name = reader.GetAttribute("value");
                                        }
                                        if (keyStr == "zh_CN")
                                        {
                                            if(reader.GetAttribute("value")!= null || reader.GetAttribute("value") != "null")
                                            {
                                                name = reader.GetAttribute("value");
                                            }
                                            break;
                                        }
                                    } while (reader.ReadToNextSibling("lang"));

                                }
                            }
                        }
                    }
                }
                reader.Close();
            }
            return name;
        }
    }
}
