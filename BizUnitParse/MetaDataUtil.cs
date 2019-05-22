using BizUnitParse;
using System.Collections.Generic;
using System.Xml;

namespace EntityParse
{
    public class MetaDataUtil
    {
        public static Dictionary<string, EntityInfo> entityMap = new Dictionary<string, EntityInfo>();
        public static Dictionary<string, RelationInfo> relationMap = new Dictionary<string, RelationInfo>();

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

        //元数据解析
        public static void metadataParse(XmlTextReader reader)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(reader);
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(xml.NameTable);
            nsMgr.AddNamespace("ns", "com.kingdee.bos.metadata");
            XmlNode root = xml.LastChild;
            if (root.Name == "entityObject")
            {
                //实体 解析
                entityParse(root);
            }
            else if (root.Name == "relationship")
            {
                //关系 解析
                relationParse(root);
            }

        }

        //实体解析
        public static EntityInfo entityParse(XmlNode root)
        {
            if (root == null)
            {
                return null;
            }
            EntityInfo entityInfo = new EntityInfo();
            XmlNodeList nodeList = root.ChildNodes;
            foreach (XmlNode childNode in nodeList)
            {
                string childNodeName = childNode.Name;
                if (childNodeName == "package")
                {
                    entityInfo.package = childNode.InnerText;
                }
                else if (childNodeName == "name")
                {
                    entityInfo.name = childNode.InnerText;
                }
                else if (childNodeName == "alias")
                {
                    entityInfo.alias = childNode.InnerText;
                }
                else if (childNodeName == "bosType")
                {
                    entityInfo.bosType = childNode.InnerText;
                }
                else if (childNodeName == "bizUnitPK")
                {
                    entityInfo.bizUnitPK = childNode.InnerText;
                }
                else if (childNodeName == "baseEntity")
                {
                    XmlNodeList keyNodeList = childNode.ChildNodes;
                    if (keyNodeList.Count == 2)
                    {
                        string package = keyNodeList[0].Attributes["value"].Value;
                        string name = keyNodeList[1].Attributes["value"].Value;
                        entityInfo.baseEntity = package + "." + name;
                    }
                }
                else if (childNodeName == "table")
                {
                    XmlNodeList keyNodeList = childNode.ChildNodes;
                    if (keyNodeList.Count == 2)
                    {
                        string name = keyNodeList[1].Attributes["value"].Value;
                        entityInfo.tableName = name;
                    }
                }
                else if (childNodeName == "properties")
                {

                }
                else if (childNodeName == "resource")
                {

                }
            }
            entityInfo.fullName = entityInfo.package + "." + entityInfo.name;
            entityMap[entityInfo.fullName] = entityInfo;
            return entityInfo;
        }

        //关系解析
        public static RelationInfo relationParse(XmlNode root)
        {
            if (root == null)
            {
                return null;
            }
            RelationInfo relationInfo = new RelationInfo();
            XmlNodeList nodeList = root.ChildNodes;
            foreach (XmlNode childNode in nodeList)
            {
                string childNodeName = childNode.Name;
                if (childNodeName == "package")
                {
                    relationInfo.package = childNode.InnerText;
                }
                else if (childNodeName == "name")
                {
                    relationInfo.name = childNode.InnerText;
                }
                else if (childNodeName == "alias")
                {
                    relationInfo.alias = childNode.InnerText;
                }
                else if (childNodeName == "clientObject")
                {
                    XmlNodeList keyNodeList = childNode.ChildNodes;
                    if (keyNodeList.Count == 2)
                    {
                        string package = keyNodeList[0].Attributes["value"].Value;
                        string name = keyNodeList[1].Attributes["value"].Value;
                        relationInfo.clientObject = package + "." + name;
                    }
                }
                else if (childNodeName == "supplierObject")
                {
                    XmlNodeList keyNodeList = childNode.ChildNodes;
                    if (keyNodeList.Count == 2)
                    {
                        string package = keyNodeList[0].Attributes["value"].Value;
                        string name = keyNodeList[1].Attributes["value"].Value;
                        relationInfo.supplierObject = package + "." + name;
                    }
                }
                else if (childNodeName == "resource")
                {

                }
            }

            relationMap[relationInfo.fullName] = relationInfo;
            return relationInfo;
        }

        public static EntityInfo getEntity(string fullName)
        {
            return entityMap[fullName];
        }

        public static RelationInfo getRelation(string fullName)
        {
            return relationMap[fullName];
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
