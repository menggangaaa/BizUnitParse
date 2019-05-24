using BizUnitParse;
using System.Collections.Generic;
using System.Xml;

namespace EntityParse
{
    public class MetaDataUtil
    {
        public static Dictionary<string, EntityInfo> entityMap = new Dictionary<string, EntityInfo>();
        public static Dictionary<string, RelationInfo> relationMap = new Dictionary<string, RelationInfo>();
        public static Dictionary<string, EnumInfo> enumMap = new Dictionary<string, EnumInfo>();

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
            //MD5 md5 = new MD5CryptoServiceProvider();
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
            else if (root.Name == "bizEnum")
            {
                //枚举 解析
                enumParse(root);
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
                    propertiesParse(childNode, entityInfo);
                }
                else if (childNodeName == "resource")
                {
                    resourceParse(childNode,entityInfo.rs);
                }
            }
            if (entityInfo.rs[entityInfo.alias] != null)
            {
                Dictionary<string, string> langMap = entityInfo.rs[entityInfo.alias];
                string langName = getLangName(langMap);
                if (langName != null)
                {
                    entityInfo.alias = langName;
                }
            }
            foreach (FieldInfo fieldInfo in entityInfo.fields)
            {
                Dictionary<string, string> langMap = entityInfo.rs[fieldInfo.alias];
                string langName = getLangName(langMap);
                if (langName != null)
                {
                    fieldInfo.alias = langName;
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
                    resourceParse(childNode, relationInfo.rs);
                }
            }
            if (relationInfo.rs[relationInfo.alias] != null)
            {
                Dictionary<string, string> langMap = relationInfo.rs[relationInfo.alias];
                string langName = getLangName(langMap);
                if (langName != null)
                {
                    relationInfo.alias = langName;
                }
            }

            relationMap[relationInfo.fullName] = relationInfo;
            return relationInfo;
        }

        //枚举解析
        public static EnumInfo enumParse(XmlNode root)
        {
            if (root == null)
            {
                return null;
            }
            EnumInfo enumInfo = new EnumInfo();
            XmlNodeList nodeList = root.ChildNodes;
            foreach (XmlNode childNode in nodeList)
            {
                string childNodeName = childNode.Name;
                if (childNodeName == "package")
                {
                    enumInfo.package = childNode.InnerText;
                }
                else if (childNodeName == "name")
                {
                    enumInfo.name = childNode.InnerText;
                }
                else if (childNodeName == "alias")
                {
                    enumInfo.alias = childNode.InnerText;
                }
                else if (childNodeName == "enumDataType")
                {
                    enumInfo.enumDataType = childNode.InnerText;
                }
                else if (childNodeName == "enumValues")
                {
                    XmlNodeList enumValueNodeList = childNode.ChildNodes;
                    foreach (XmlNode enumValueNode in enumValueNodeList)
                    {
                        EnumValueInfo enumValueInfo = new EnumValueInfo();
                        XmlNodeList proNodeList = enumValueNode.ChildNodes;
                        foreach (XmlNode proNode in proNodeList)
                        {
                            string proName = proNode.Name;
                            if (childNodeName == "name")
                            {
                                enumValueInfo.name = proNode.InnerText;
                            }
                            else if (childNodeName == "alias")
                            {
                                enumValueInfo.alias = proNode.InnerText;
                            }
                            else if (childNodeName == "userDefined")
                            {
                                enumValueInfo.userDefined = proNode.InnerText;
                            }
                            else if (childNodeName == "value")
                            {
                                enumValueInfo.value = proNode.InnerText;
                            }
                        }
                        enumValueInfo.rs = enumInfo.rs;
                        enumInfo.Add(enumValueInfo);
                    }
                }
                else if (childNodeName == "resource")
                {
                    resourceParse(childNode, enumInfo.rs);
                }
            }
            if (enumInfo.rs[enumInfo.alias] != null)
            {
                Dictionary<string, string> langMap = enumInfo.rs[enumInfo.alias];
                string langName = getLangName(langMap);
                if (langName != null)
                {
                    enumInfo.alias = langName;
                }
            }

            enumMap[enumInfo.fullName] = enumInfo;
            return enumInfo;
        }


        public static void propertiesParse(XmlNode propertiesNode, EntityInfo entityInfo)
        {
            XmlNodeList propertyNodeList = propertiesNode.ChildNodes;
            foreach (XmlNode propertyNode in propertiesNode)
            {
                XmlNodeList proNodeList = propertyNode.ChildNodes;
                FieldInfo fieldInfo = new FieldInfo();
                foreach (XmlNode proNode in proNodeList)
                {
                    string proName = proNode.Name;
                    if (proName == "name")
                    {
                        fieldInfo.name = proNode.InnerText;
                    }
                    else if (proName == "alias")
                    {
                        fieldInfo.alias = proNode.InnerText;
                    }
                    else if (proName == "dataType")
                    {
                        fieldInfo.dataType = proNode.InnerText;
                    }
                    else if (proName == "metadataRef")
                    {
                        fieldInfo.metadataRef = proNode.InnerText;
                    }
                    else if (proName == "mappingField")
                    {
                        fieldInfo.mappingField = proNode.FirstChild.Attributes["value"].Value;
                    }
                    else if (proName == "relationship")
                    {
                        XmlNodeList keyNodeList = proNode.ChildNodes;
                        if (keyNodeList.Count == 2)
                        {
                            string package = keyNodeList[0].Attributes["value"].Value;
                            string name = keyNodeList[1].Attributes["value"].Value;
                            fieldInfo.relationship = package + "." + name;
                        }
                    }
                }
                entityInfo.Add(fieldInfo);
            }
        }

        public static void resourceParse(XmlNode resourceNode, Dictionary<string, Dictionary<string, string>> rs)
        {
            XmlNodeList rsNodeList = resourceNode.ChildNodes;
            foreach (XmlNode rsNode in rsNodeList)
            {
                string key = rsNode.Attributes["key"].Value;
                Dictionary<string, string> langMap = new Dictionary<string, string>();
                XmlNodeList langNodeList = rsNode.ChildNodes;
                foreach (XmlNode langNode in langNodeList)
                {
                    string locale = langNode.Attributes["locale"].Value;
                    string value = langNode.Attributes["value"].Value;
                    langMap[locale] = value;
                }
                rs[key] = langMap;
            }
        }

        public static string getLangName(Dictionary<string, string> langMap)
        {
            if (langMap["zh_CN"] != null)
            {
                return langMap["zh_CN"];
            }
            else 
            {
                return langMap["en_US"];
            }
        }
        public static string getFullName(string path,string dir)
        {
            string fullName = path.Replace(dir,"");
            fullName = fullName.Replace("\\", ".");
            return fullName.Substring(0,fullName.LastIndexOf("."));
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
