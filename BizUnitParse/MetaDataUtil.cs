using BizUnitParse;
using System.Xml;

namespace EntityParse
{
    public class MetaDataUtil
    {
        //获取包名或者业务单元名称
        public static string getBizOrPackAlias(XmlTextReader reader, MetaDataTypeEnum metaDatetype)
        {
            string alias = "";
            if (reader == null)
            {
                return alias;
            }
            XmlDocument xml = new XmlDocument();
            xml.Load(reader);
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(xml.NameTable);
            nsMgr.AddNamespace("ns", "com.kingdee.bos.metadata");
            XmlNode root = null;
            switch (metaDatetype)
            {
                case MetaDataTypeEnum.bizUnit:
                    root = xml.SelectSingleNode("/ns:bizUnit", nsMgr);
                    break;
                case MetaDataTypeEnum.package:
                    root = xml.SelectSingleNode("/ns:package", nsMgr);
                    break;
            }
            if (root != null)
            {
                XmlNode aliasNode = root.SelectSingleNode("ns:alias", nsMgr);
                if (aliasNode != null)
                {
                    string aliasName = aliasNode.InnerText;
                    XmlNodeList rsList = root.SelectNodes("ns:resource/ns:rs", nsMgr);
                    foreach (XmlNode rs in rsList)
                    {
                        string key = rs.Attributes["key"].Value;
                        if (key == aliasName)
                        {
                            foreach (XmlNode lang in rs.ChildNodes)
                            {
                                string locale = lang.Attributes["locale"].Value;
                                if (locale == "en_US")
                                {
                                    alias = lang.Attributes["value"].Value;
                                }
                                else if (locale == "zh_CN")
                                {
                                    alias = lang.Attributes["value"].Value;
                                    return alias;
                                }
                            }
                        }
                    }
                }
            }
            return alias;
        }

        public static MetaDataTypeEnum xmlPathIsMetaDateType(string path)
        {
            MetaDataTypeEnum metaDateType = MetaDataTypeEnum.none;
            if (path == null || path.Trim().Length == 0)
            {
                return metaDateType;
            }
            if (path.IndexOf(".entity") > -1)
            {
                metaDateType = MetaDataTypeEnum.entity;
            }
            else if (path.IndexOf(".bizunit") > -1)
            {
                metaDateType = MetaDataTypeEnum.bizUnit;
            }
            else if (path.IndexOf(".package") > -1)
            {
                metaDateType = MetaDataTypeEnum.package;
            }
            else if (path.IndexOf(".relation") > -1)
            {
                metaDateType = MetaDataTypeEnum.relation;
            }
            else if (path.IndexOf(".enum") > -1)
            {
                metaDateType = MetaDataTypeEnum.enums;
            }
            else if (path.IndexOf(".table") > -1)
            {
                metaDateType = MetaDataTypeEnum.table;
            }
            else if (path.IndexOf(".query") > -1)
            {
                metaDateType = MetaDataTypeEnum.query;
            }
            else if (path.IndexOf(".ui") > -1)
            {
                metaDateType = MetaDataTypeEnum.ui;
            }
            return metaDateType;
        }
    }
}
