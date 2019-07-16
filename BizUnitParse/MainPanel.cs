using EntityParse;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace BizUnitParse
{
    public partial class MainPanel : Form
    {
        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(string section, string key, string value, string filepath);

        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder returnvalue, int buffersize, string filepath);

        [DllImport("kernel32.dll")]
        private static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, byte[] lpReturnedString, uint nSize, string lpFileName);

        TableDataLink thisTableNode;

        string basePath = "";//基本信息配置路径

        string entityPath = "";//实体配置路径
        string relationPath = "";//关系配置路径
        string enumPath = "";//枚举配置路径
        string bizunitPath = "";//业务单元配置路径
        string packagePath = "";//包配置路径
        string entityToPackagePath = "";//实体对应包路径配置路径

        string logPath = "";//日志路径

        //从配置文件中 取值
        private void GetValue(string section, string key, out string value, string path)

        {
            StringBuilder stringBuilder = new StringBuilder();
            GetPrivateProfileString(section, key, "", stringBuilder, 1024, path);
            value = stringBuilder.ToString();
        }

        //判断文件是否存在,不存在创建
        private void fileExists(string path)
        {
            if (!File.Exists(path))
                File.Create(path);
        }
        public void initBaseDir()
        {
            if (isIDE.Checked)
            {
                MetaDataUtil.baseDir = txtDirPath.Text + "\\metadata\\";
            }
            else if (isClient.Checked)
            {
                MetaDataUtil.baseDir = txtDirPath.Text + "\\metas\\";
            }
        }

        public MainPanel()
        {
            InitializeComponent();
            txtEntityFilter.Focus();
            if (Screen.GetWorkingArea(this).Width < 1920)
            {
                Font font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134))); ;
                Font = font;
                bizUnitTree.Font = font;
                entityTable.RowsDefaultCellStyle.Font = new Font("宋体", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                entityTable.RowTemplate.DefaultCellStyle.Font = new Font("宋体", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));

            }
            #region 窗体缩放
            //GetAllInitInfo(Controls[0]);
            #endregion
        }

        private void MainPanel_Load(object sender, EventArgs e)
        {
            basePath = Application.LocalUserAppDataPath + "\\BaseConfig.ini";//基本信息配置路径
            entityPath = Application.LocalUserAppDataPath + "\\EntityConfig.ini";//实体配置路径
            relationPath = Application.LocalUserAppDataPath + "\\RealtionConfig.ini";//关系配置路径
            enumPath = Application.LocalUserAppDataPath + "\\EnumConfig.ini";//枚举配置路径
            bizunitPath = Application.LocalUserAppDataPath + "\\BizunitConfig.ini";//业务单元配置路径
            packagePath = Application.LocalUserAppDataPath + "\\PackageConfig.ini";//包配置路径
            entityToPackagePath = Application.LocalUserAppDataPath + "\\EntityToPackageConfig.ini";//实体对应包路径配置路径
            logPath = Application.LocalUserAppDataPath + "\\Log.ini";//日志对应包路径配置路径

            fileExists(basePath);
            fileExists(entityPath);
            fileExists(relationPath);
            fileExists(enumPath);
            fileExists(bizunitPath);
            fileExists(packagePath);
            fileExists(entityToPackagePath);
            fileExists(logPath);

            //初始化 页面参数
            string outString;
            try
            {
                GetValue("Information", "select", out outString, basePath);
                if (outString != null && outString.Length > 0)
                {
                    if (outString == "1")
                    {
                        isIDE.Checked = true;
                    }
                    else
                    {
                        isClient.Checked = true;
                    }
                }
                GetValue("Information", "path", out outString, basePath);
                if (outString != null && outString.Length > 0)
                {
                    txtDirPath.Text = outString;
                }
                GetValue("Information", "isInitJar", out outString, basePath);
                if (outString == null || outString.Length == 0 || outString == "0")
                {
                    //未初始化jar包列表,初始化jar包列表
                    //initJarMetaDice(null);
                    labInitJar.Visible = true;
                    initJarBar.Visible = false;
                    initJarBar.Value = 0;
                }
                else
                {
                    labInitJar.Visible = false;
                    initJarBar.Visible = true;
                    initJarBar.Value = 100;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            initBaseDir();
            string path = txtDirPath.Text + "\\metadata\\com\\kingdee\\eas\\hse\\scm";
            if (Directory.Exists(path) && isIDE.Checked)
            {
                //初始化 左边树
                initTree();
            }

            initTxtEntityFilter();

            initSqlPanel();
        }

        public TextBox txtSql = null;
        public ToolStripDropDown dropDown = null;
        private void initSqlPanel()
        {
            txtSql = new TextBox();
            txtSql.Multiline = true;
            txtSql.WordWrap = false;
            txtSql.ScrollBars = ScrollBars.Both;
            txtSql.Font = new Font("微软雅黑", 12f);
            txtSql.Text = "select * from table";
            txtSql.BorderStyle = BorderStyle.None;
            txtSql.Margin = new Padding(0);
            ToolStripControlHost treeViewHost = new ToolStripControlHost(txtSql);
            treeViewHost.Margin = new Padding(0);
            treeViewHost.AutoSize = false;
            treeViewHost.Size = new Size(385, 445);
            dropDown = new ToolStripDropDown();
            dropDown.Margin = new Padding(0);
            dropDown.Items.Add(treeViewHost);
            //dropDown.Show(this, 880, 300);

            txtSql.KeyDown += TxtSql_KeyDown;
        }

        //初始化实体过滤框
        public void initTxtEntityFilter()
        {
            txtEntityFilter.AutoCompleteMode = AutoCompleteMode.None;
            txtEntityFilter.AutoCompleteSource = AutoCompleteSource.None;
            List<string> jarMetas = ReadSingleSection("metadata", entityToPackagePath);
            foreach (string jarMetaName in jarMetas)
            {
                txtEntityFilter.AutoCompleteCustomSource.Add(jarMetaName);
            }
            jarMetas = ReadSingleSection("metadata", bizunitPath);
            foreach (string jarMetaName in jarMetas)
            {
                txtEntityFilter.AutoCompleteCustomSource.Add(jarMetaName);
            }

            //设置文本框下拉显示
            txtEntityFilter.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtEntityFilter.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtEntityFilter.Focus();
        }
        private void TxtSql_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                ((TextBox)sender).SelectAll();
            }
        }

        private void txtDirPath_TextChanged(object sender, EventArgs e)
        {
            if (isIDE.Checked)
            {
                if (Directory.Exists(txtDirPath.Text + "\\metadata\\com\\kingdee\\eas\\hse\\scm") == true)
                {
                    WritePrivateProfileString("Information", "path", txtDirPath.Text, basePath);
                    initBaseDir();
                    initTree();
                }
                else
                {
                    MessageBox.Show("选择的文件夹不是项目的根目录,请重新选择!");
                }
            }
            else if (isClient.Checked)
            {
                if (Directory.Exists(txtDirPath.Text + "\\metas") == true)
                {
                    WritePrivateProfileString("Information", "path", txtDirPath.Text, basePath);
                    initBaseDir();
                }
                else
                {
                    MessageBox.Show("选择的文件夹不是客户端的根目录,请重新选择!");
                }
            }
        }

        private void btnDirPathSelect_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = "选择项目根目录";
            folder.SelectedPath = txtDirPath.Text;
            if (folder.ShowDialog() == DialogResult.OK)
            {
                //\\scm
                if (isIDE.Checked)
                {
                    if (Directory.Exists(folder.SelectedPath + "\\metadata\\com\\kingdee\\eas\\hse\\scm") == true)
                    {
                        txtDirPath.Text = folder.SelectedPath;
                        WritePrivateProfileString("Information", "path", txtDirPath.Text, basePath);
                        initBaseDir();
                        initTree();
                    }
                    else
                    {
                        MessageBox.Show("选择的文件夹不是项目的根目录,请重新选择!");
                    }
                }
                else if (isClient.Checked)
                {
                    if (Directory.Exists(folder.SelectedPath + "\\metas") == true)
                    {
                        txtDirPath.Text = folder.SelectedPath;
                        WritePrivateProfileString("Information", "path", txtDirPath.Text, basePath);
                        initBaseDir();
                    }
                    else
                    {
                        MessageBox.Show("选择的文件夹不是客户端的根目录,请重新选择!");
                    }
                }
            }
        }

        private void isIDE_CheckedChanged(object sender, EventArgs e)
        {
            if (isIDE.Checked)
            {
                WritePrivateProfileString("Information", "select", "1", basePath);
            }
            else if (isClient.Checked)
            {
                WritePrivateProfileString("Information", "select", "2", basePath);
            }
        }

        private void isClient_CheckedChanged(object sender, EventArgs e)
        {
            if (isIDE.Checked)
            {
                WritePrivateProfileString("Information", "select", "1", basePath);
            }
            else if (isClient.Checked)
            {
                WritePrivateProfileString("Information", "select", "2", basePath);
            }
        }

        private void btnInitJar_Click(object sender, EventArgs e)
        {
            initJarBar.Visible = true;
            initJarBar.Value = 0;
            labInitJar.Visible = false;
            Thread thread1 = new Thread(new ParameterizedThreadStart(initJarMeta));
            thread1.Start();
        }

        //字段过滤
        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            //筛选
            foreach (DataGridViewColumn column in entityTable.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            }
            for (int i = 2; i < entityTable.Rows.Count - 1; i++)
            {

                DataGridViewRow row = entityTable.Rows[i];
                DataGridViewBand band = entityTable.Rows[i];
                if (txtFilter.Text.Length > 0)
                {
                    string name = empToValue(row.Cells[0].Value).ToUpper();
                    string alias = empToValue(row.Cells[1].Value).ToUpper();
                    string selectText = txtFilter.Text.ToUpper();
                    if (name.IndexOf(selectText) > -1 || alias.IndexOf(selectText) > -1)
                    {
                        //row.DefaultCellStyle.BackColor = Color.Yellow;
                        band.Visible = true;
                    }
                    else
                    {
                        //row.DefaultCellStyle.BackColor = Color.White;
                        band.Visible = false;
                    }
                }
                else
                {
                    //row.DefaultCellStyle.BackColor = Color.White;
                    band.Visible = true;
                }
            }
            foreach (DataGridViewColumn column in entityTable.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        public string empToValue(object o)
        {
            return o == null ? "" : o.ToString();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            if (thisTableNode != null && thisTableNode.upNode != null)
            {
                thisTableNode.upNode.downNode = thisTableNode;
                thisTableNode = thisTableNode.upNode;
                mainEntityParse(thisTableNode.entity);
            }
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            if (thisTableNode != null && thisTableNode.downNode != null)
            {
                thisTableNode = thisTableNode.downNode;
                mainEntityParse(thisTableNode.entity);
            }
        }

        private void txtEntityFilter_TextChanged(object sender, EventArgs e)
        {

        }

        public EnumUI enumUI;
        private void entityTable_DoubleClick(object sender, EventArgs e)
        {
            if (entityTable.CurrentRow == null || entityTable.Rows[entityTable.CurrentRow.Index] == null)
            {
                return;
            }
            DataGridViewCellCollection cells = entityTable.Rows[entityTable.CurrentRow.Index].Cells;
            if (cells[5].Value == null)
            {
                return;
            }
            object dataType = cells[4].Value;
            string fullName = cells[5].Value.ToString();
            string fieldName = cells[0].Value.ToString();
            if (empToValue(dataType) == "")
            {
                RelationInfo relation = MetaDataUtil.getRelation(fullName);
                if (relation != null)
                {
                    string supplier = relation.supplierObject;
                    if (fieldName == "parent")
                    {
                        supplier = relation.clientObject;
                    }
                    string path = MetaDataUtil.getPath(supplier, MetaDataTypeEnum.entity);
                    if (File.Exists(path))
                    {
                        XmlDocument xml = new XmlDocument();
                        XmlTextReader reader = new XmlTextReader(path);
                        xml.Load(reader);
                        XmlNode root = xml.LastChild;
                        MetaDataUtil.entityParse(root);
                        reader.Close();
                    }
                    else if (!MetaDataUtil.entityMap.ContainsKey(supplier) || MetaDataUtil.entityMap[supplier] == null)
                    {
                        string jarPath = "";
                        GetValue("metadata", supplier, out jarPath, entityPath);
                        if (jarPath != null || jarPath.Length > 0)
                        {
                            //存在对应jar包
                            initJarFile(jarPath);
                        }
                    }
                    mainEntityParse(supplier);
                }
            }
            else if ("Enum".Equals(dataType))
            {
                EnumInfo enumInfo = enumParse(fullName);
                if (enumUI == null)
                {
                    enumUI = new EnumUI();
                }
                enumUI.ShowDialog(enumInfo);
            }
        }

        public EnumInfo enumParse(string fullName)
        {
            string path = MetaDataUtil.getPath(fullName, MetaDataTypeEnum.enums);
            if (File.Exists(path))
            {
                MetaDataUtil.metadataParse(new XmlTextReader(path));
            }
            else
            {
                string jarPath = "";
                GetValue("metadata", fullName, out jarPath, enumPath);
                if (jarPath != null || jarPath.Length > 0)
                {
                    //存在对应jar包
                    initJarFile(jarPath);
                }
            }
            return MetaDataUtil.getEnum(fullName);
        }

        private void bizUnitTree_DoubleClick(object sender, EventArgs e)
        {
            entiryParen(true);
        }

        //初始化列表树
        public void initTree()
        {
            bizUnitTree.Nodes.Clear();
            //\\scm
            string path = txtDirPath.Text + "\\metadata\\com\\kingdee\\eas\\hse";
            if (Directory.Exists(path) == false)
            {
                MessageBox.Show("项目路径不正确!");
                return;
            }
            DirectoryInfo di = new DirectoryInfo(path);
            DirectoryInfo[] dis = di.GetDirectories();
            List<Object> list = new List<Object>();
            foreach (DirectoryInfo dx in dis)
            {
                DirectoryInfo[] disx = dx.GetDirectories();
                foreach (DirectoryInfo dd in disx)
                {
                    string fileName = path + "\\" + dx.Name + "\\" + dd.Name + "\\" + dd.Name + ".package";
                    if (!File.Exists(fileName))
                    {
                        continue;
                    }
                    string rootName = MetaDataUtil.getBizOrPackAlias(new XmlTextReader(fileName), MetaDataTypeEnum.package);
                    if (rootName == "null")
                    {
                        continue;
                    }
                    //添加父节点
                    TreeNode pnode = new TreeNode();
                    pnode.Text = rootName;
                    bizUnitTree.Nodes.Add(pnode);
                    txtEntityFilter.AutoCompleteCustomSource.Add(rootName);
                    //添加子节点
                    FileInfo[] files = dd.GetFiles("*.bizunit");
                    foreach (FileInfo file in files)
                    {

                        string nodeName = MetaDataUtil.getBizOrPackAlias(new XmlTextReader(file.FullName), MetaDataTypeEnum.bizUnit);
                        TreeNode node = new TreeNode();
                        node.Text = nodeName;
                        pnode.Nodes.Add(node);
                        txtEntityFilter.AutoCompleteCustomSource.Add(nodeName);

                        //添加实体节点
                        DirectoryInfo ds = new DirectoryInfo(file.DirectoryName + "\\app");
                        FileInfo[] nodefiles = ds.GetFiles("*.entity");
                        string name = file.Name.Substring(0, file.Name.IndexOf(".bizunit"));
                        foreach (FileInfo ff in nodefiles)
                        {
                            if (ff.Name.IndexOf(name) >= 0)
                            {
                                TreeNode xnode = new TreeNode();
                                xnode.Text = ff.Name;
                                xnode.Tag = ff.FullName;
                                node.Nodes.Add(xnode);
                                txtEntityFilter.AutoCompleteCustomSource.Add(ff.Name);
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < bizUnitTree.Nodes.Count; i++)
            {
                for (int j = 0; j < bizUnitTree.Nodes[i].Nodes.Count; j++)
                {
                    if (bizUnitTree.Nodes[i].Nodes[j].Text.IndexOf("医疗销售结算") > -1
                        && bizUnitTree.Nodes[i].Nodes[j].Text.Length == 6)
                    {
                        bizUnitTree.Nodes[i].Nodes[j].Expand();
                        bizUnitTree.SelectedNode = bizUnitTree.Nodes[i].Nodes[j];
                        bizUnitTree.Focus();
                        return;
                    }
                }
            }
        }

        private void txtEntityFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string text = txtEntityFilter.Text;
                bool falg = false;
                for (int i = 0; i < bizUnitTree.Nodes.Count; i++)
                {
                    if (bizUnitTree.Nodes[i].Text.IndexOf(text) > -1
                            && bizUnitTree.Nodes[i].Text.Length == text.Length)
                    {
                        bizUnitTree.Nodes[i].Expand();
                        bizUnitTree.SelectedNode = bizUnitTree.Nodes[i];
                        bizUnitTree.Focus();
                        falg = true;
                        break;
                    }
                    for (int j = 0; j < bizUnitTree.Nodes[i].Nodes.Count; j++)
                    {
                        if (bizUnitTree.Nodes[i].Nodes[j].Text.IndexOf(text) > -1
                            && bizUnitTree.Nodes[i].Nodes[j].Text.Length == text.Length)
                        {
                            bizUnitTree.Nodes[i].Nodes[j].Expand();
                            bizUnitTree.SelectedNode = bizUnitTree.Nodes[i].Nodes[j];
                            bizUnitTree.Focus();
                            falg = true;
                            break;
                        }
                        for (int k = 0; k < bizUnitTree.Nodes[i].Nodes[j].Nodes.Count; k++)
                        {
                            if (bizUnitTree.Nodes[i].Nodes[j].Nodes[k].Text.IndexOf(text) > -1
                            && bizUnitTree.Nodes[i].Nodes[j].Nodes[k].Text.Length == text.Length)
                            {
                                bizUnitTree.SelectedNode = bizUnitTree.Nodes[i].Nodes[j].Nodes[k];
                                bizUnitTree.Focus();
                                falg = true;
                                break;
                            }
                        }
                        if (falg)
                            break;
                    }
                    if (falg)
                        break;
                }
                entiryParen(falg);
            }
        }

        private void entiryParen(bool falg)
        {
            if (falg)
            {
                //在左边树中可以找到
                TreeNode node = bizUnitTree.SelectedNode;
                if (node == null || node.Nodes == null || node.Nodes.Count != 0)
                {
                    entityTable.Rows.Clear();
                    txtFilter.Text = "";
                    return;
                }
                ideEntityParse(node.Tag.ToString());
            }
            else
            {
                //在左边树中找不到--去包中查找
                string fullName = "";
                GetValue("metadata", txtEntityFilter.Text, out fullName, bizunitPath);
                if (fullName == null || fullName.Length == 0)
                {
                    GetValue("metadata", txtEntityFilter.Text, out fullName, entityToPackagePath);
                }
                if (fullName != null || fullName.Length > 0)
                {
                    //先从当前已二次开发环境中查找--开发环境,存在重新加载
                    string path = MetaDataUtil.getPath(fullName, MetaDataTypeEnum.entity);
                    if (isIDE.Checked && File.Exists(path))
                    {
                        ideEntityParse(path);
                    }
                    else
                    {
                        if (MetaDataUtil.entityMap.ContainsKey(fullName) && MetaDataUtil.entityMap[fullName] != null)
                        {
                            //已从jar中解析过实体,直接读取
                            mainEntityParse(fullName);
                        }
                        else
                        {
                            string jarPath = "";
                            GetValue("metadata", fullName, out jarPath, entityPath);
                            if (jarPath != null || jarPath.Length > 0)
                            {
                                //存在对应jar包
                                initJarFile(jarPath);
                                mainEntityParse(fullName);
                            }
                        }
                    }
                }
            }

        }

        //开发环境解析实体 path--文件路径
        public void ideEntityParse(string path)
        {
            XmlTextReader reader = new XmlTextReader(path);
            MetaDataUtil.metadataParse(reader);
            string fullName = MetaDataUtil.getFullName(path);
            mainEntityParse(fullName);
        }

        public void mainEntityParse(string fullName)
        {
            EntityInfo entityInfo = MetaDataUtil.getEntity(fullName);
            if (entityInfo != null)
            {
                TableDataLink tableNode = new TableDataLink();
                tableNode.entity = entityInfo;
                if (thisTableNode != null)
                {
                    tableNode.upNode = thisTableNode;
                }
                thisTableNode = tableNode;

                entityTable.Rows.Clear();
                foreach (DataGridViewColumn column in entityTable.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                }
                fillTableEntity(entityInfo);
                foreach (DataGridViewColumn column in entityTable.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
            }
        }

        public void mainEntityParse(EntityInfo entityInfo)
        {
            if (entityInfo != null)
            {
                entityTable.Rows.Clear();
                foreach (DataGridViewColumn column in entityTable.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                }
                fillTableEntity(entityInfo);
                foreach (DataGridViewColumn column in entityTable.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
            }
        }

        private void MainPanel_Activated(object sender, EventArgs e)
        {
            txtEntityFilter.Focus();
        }

        #region 使用EntityInfo填充表格 fillTableEntity(info)
        private void fillTableEntity(EntityInfo info)
        {
            if (info == null)
            {
                return;
            }
            //表字段
            int index = entityTable.Rows.Add();
            entityTable.Rows[index].Cells[0].Value = info.name;
            entityTable.Rows[index].Cells[1].Value = info.alias;
            entityTable.Rows[index].Cells[2].Value = info.tableName;
            entityTable.Rows[index].Cells[3].Value = info.bosType;
            entityTable.Rows[index].Cells[4].Value = "";
            entityTable.Rows[index].Cells[5].Value = "";
            entityTable.Rows.Add();
            foreach (FieldInfo fieldInfo in info.fields)
            {
                index = entityTable.Rows.Add();
                entityTable.Rows[index].Cells[0].Value = fieldInfo.name;
                entityTable.Rows[index].Cells[1].Value = fieldInfo.alias;
                entityTable.Rows[index].Cells[2].Value = fieldInfo.mappingField;
                entityTable.Rows[index].Cells[3].Value = null;
                entityTable.Rows[index].Cells[4].Value = fieldInfo.dataType;
                if (fieldInfo.relationship != null)
                {
                    entityTable.Rows[index].Cells[5].Value = fieldInfo.relationship;
                    //从关联关系中找到下游关系表
                    string tableName = getRelationToEntityTableName(fieldInfo.relationship, fieldInfo.name);
                    entityTable.Rows[index].Cells[3].Value = tableName;
                }
                else if (fieldInfo.metadataRef != null)
                {
                    entityTable.Rows[index].Cells[5].Value = fieldInfo.metadataRef;
                }
            }
            //基类实体
            if (info.baseEntity != null)
            {
                //递归添加
                entityTable.Rows.Add();
                if (MetaDataUtil.entityMap.ContainsKey(info.baseEntity))
                {
                    //已存在 对象
                    EntityInfo baseEntity = MetaDataUtil.entityMap[info.baseEntity];
                    fillTableEntity(baseEntity);
                }
                else if (info.baseEntity != null)
                {
                    //未初始化对象
                    string path = MetaDataUtil.getPath(info.baseEntity, MetaDataTypeEnum.entity);
                    if (File.Exists(path))
                    {
                        //文件存在
                        MetaDataUtil.metadataParse(new XmlTextReader(path));
                        EntityInfo baseEntity = MetaDataUtil.getEntity(info.baseEntity);
                        if (baseEntity != null)
                        {
                            fillTableEntity(baseEntity);
                        }
                    }
                    else
                    {
                        //文件不存在
                        string metaPath = null;
                        GetValue("metadata", info.baseEntity, out metaPath, entityPath);
                        if (metaPath != null && metaPath.Length > 0)
                        {
                            initJarFile(metaPath);
                        }
                        if (MetaDataUtil.entityMap.ContainsKey(info.baseEntity))
                        {
                            //已存在 对象
                            EntityInfo baseEntity = MetaDataUtil.entityMap[info.baseEntity];
                            fillTableEntity(baseEntity);
                        }
                    }
                }
            }
        }
        #endregion

        public void initJarFile(string jarPath)
        {
            string path = null;
            if (jarPath.IndexOf(txtDirPath.Text) < 0)
            {
                if (isIDE.Checked)
                {
                    path = txtDirPath.Text + "\\basemetas\\" + jarPath;
                }
                else if (isClient.Checked)
                {
                    path = txtDirPath.Text + "\\metas\\" + jarPath;
                }
            }
            else
            {
                path = jarPath;
            }
            if (!File.Exists(path))
            {
                return;
            }
            Console.WriteLine(path);
            ZipInputStream zipStream = new ZipInputStream(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read));
            ZipEntry entry = zipStream.GetNextEntry();
            Dictionary<string, byte[]> dict = new Dictionary<string, byte[]>();
            while (entry != null)
            {
                if (!entry.IsDirectory)
                {
                    string entryName = entry.Name;
                    if (entryName.IndexOf(".entity") > -1
                        || entryName.IndexOf(".enum") > -1
                        || entryName.IndexOf(".relation") > -1
                        || entryName.IndexOf(".package") > -1
                        || entryName.IndexOf(".bizunit") > -1
                        //|| entryName.IndexOf(".table") > -1
                        )
                    {
                        //string mateName = entryName.Substring(entryName.LastIndexOf('/') + 1, entryName.Length - entryName.LastIndexOf('/') - 1);
                        string mateName = MetaDataUtil.getFullName(entryName);
                        string matePath = "";
                        if (isIDE.Checked)
                        {
                            matePath = path.Replace(txtDirPath.Text + "\\basemetas\\", "");
                        }
                        else if (isClient.Checked)
                        {
                            matePath = path.Replace(txtDirPath.Text + "\\metas\\", "");
                        }
                        //WritePrivateProfileString("JarFileList", mateName, matePath, basePath);
                        //wirteLog(DateTime.Now.ToString("yyyyMMdd-HHmm") + ":" + matePath + "\t" + entryName + "\n");
                        writeMetaDataToConfig(zipStream, entry, mateName, matePath);
                    }
                }
                //获取下一个文件
                entry = zipStream.GetNextEntry();
            }
            zipStream.Close();
        }

        //将元数据写入 配置文件
        public void writeMetaDataToConfig(ZipInputStream zipStream, ZipEntry entry, string mateName, string metaPath)
        {
            string entryName = entry.Name;
            byte[] data = new byte[zipStream.Length];
            zipStream.Read(data, 0, data.Length);
            MemoryStream memory = new MemoryStream(data);
            BufferedStream stream = new BufferedStream(memory);
            XmlTextReader reader = new XmlTextReader(stream);
            MetaDataUtil.metadataParse(reader);
            if (entryName.IndexOf(".entity") > -1)
            {
                WritePrivateProfileString("metadata", mateName, metaPath, entityPath);
                int last = entryName.LastIndexOf("/");
                WritePrivateProfileString("metadata", entryName.Substring(last + 1, entryName.Length - last - 1), mateName, entityToPackagePath);
            }
            else if (entryName.IndexOf(".relation") > -1)
            {
                WritePrivateProfileString("metadata", mateName, metaPath, relationPath);
            }
            else if (entryName.IndexOf(".enum") > -1)
            {
                WritePrivateProfileString("metadata", mateName, metaPath, enumPath);
            }
            else if (entryName.IndexOf(".bizunit") > -1)
            {
                memory = new MemoryStream(data);
                stream = new BufferedStream(memory);
                stream.Position = 0;
                string name = MetaDataUtil.getBizOrPackAlias(new XmlTextReader(stream), MetaDataTypeEnum.bizUnit);
                stream.Position = 0;
                string entityPK = MetaDataUtil.getBizEntityPK(new XmlTextReader(stream));
                WritePrivateProfileString("metadata", name, entityPK, bizunitPath);
            }
            else if (entryName.IndexOf(".package") > -1)
            {
                memory = new MemoryStream(data);
                stream = new BufferedStream(memory);
                stream.Position = 0;
                string name = MetaDataUtil.getBizOrPackAlias(new XmlTextReader(stream), MetaDataTypeEnum.package);
                WritePrivateProfileString("metadata", name, entryName.Substring(0, entryName.LastIndexOf(".")), packagePath);
            }
            reader.Close();
            stream.Close();
            memory.Close();
        }

        public void initJarMeta(object str)
        {
            WritePrivateProfileString("Information", "isInitJar", "0", basePath);
            //string[] sources = { "\\basemetas\\bos", "\\basemetas\\eas" };
            List<string> sourceList = new List<string>();
            if (isIDE.Checked)
            {
                sourceList.Add("\\basemetas\\bos");
                sourceList.Add("\\basemetas\\eas");
            }
            else if (isClient.Checked)
            {
                sourceList.Add("\\metas\\bos");
                sourceList.Add("\\metas\\eas");
                sourceList.Add("\\metas\\sp");
            }

            int one = 0;
            int two = 0;
            int ban = 100 / sourceList.Count;
            foreach (string source in sourceList)
            {
                string filePath = txtDirPath.Text + source;
                //wirteLog(DateTime.Now.ToString("yyyyMMdd-HHmm") + ":" + filePath + "\n");
                DirectoryInfo di = new DirectoryInfo(filePath);
                if (!di.Exists)
                {
                    MessageBox.Show("文件夹路径不正确!!!");
                    return;
                }
                FileInfo[] files = di.GetFiles();
                foreach (FileInfo file in files)
                {
                    //wirteLog(DateTime.Now.ToString("yyyyMMdd-HHmm") + ":" + file.FullName + "\n");
                    if (file.FullName.IndexOf("\\metas\\sp") > -1 && file.FullName.IndexOf("\\metas\\sp\\sp_scm-metas.jar") < 0)
                    {
                        continue;
                    }
                    if (!file.FullName.EndsWith(".jar"))
                    {
                        continue;
                    }
                    initJarFile(file.FullName);
                    if (initJarBar.InvokeRequired)
                    {
                        // 当一个控件的InvokeRequired属性值为真时，说明有一个创建它以外的线程想访问它
                        Action<string> actionDelegate = (x) => { initJarBar.Value = (int)(ban * one + ((double)two) / files.Length * ban); };
                        // Action<string> actionDelegate = delegate(string txt) { this.label2.Text = txt; };
                        initJarBar.Invoke(actionDelegate, str);
                    }
                    two++;
                }
                two = 0;
                one++;
            }
            if (initJarBar.InvokeRequired)
            {
                // 当一个控件的InvokeRequired属性值为真时，说明有一个创建它以外的线程想访问它
                Action<string> actionDelegate = (x) => { initJarBar.Value = 100; };
                this.labInitJar.Invoke(actionDelegate, str);
            }
            WritePrivateProfileString("Information", "isInitJar", "1", basePath);
            Action<string> actionDelegate2 = (x) =>
            {
                initTxtEntityFilter();
            };
            txtEntityFilter.Invoke(actionDelegate2, str);
            MessageBox.Show("初始化成功!!!");
        }

        // 读取指定区域Keys列表。
        public List<string> ReadSingleSection(string Section, string iniFilename)
        {
            List<string> result = new List<string>();
            byte[] buf = new byte[2097152];
            uint lenf = GetPrivateProfileString(Section, null, null, buf, (uint)buf.Length, iniFilename);
            int j = 0;
            for (int i = 0; i < lenf; i++)
                if (buf[i] == 0)
                {
                    result.Add(Encoding.Default.GetString(buf, j, i - j));
                    j = i + 1;
                }
            return result;
        }

        private void entityTable_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString().ToUpper() == "Q")
            {
                //Console.WriteLine("QQQQQQQQQQQQQQQ");
                if (dropDown != null)
                {
                    if (thisTableNode != null)
                    {
                        DataGridViewRow row = entityTable.Rows[0];
                        string mainTable = row.Cells[2].Value.ToString();
                        string alias = "main";
                        //查询部分
                        StringBuilder selectStr = new StringBuilder();
                        //子分录 或F7 表连接
                        StringBuilder entrySql = new StringBuilder();
                        DataGridViewSelectedCellCollection selectCells = entityTable.SelectedCells;
                        HashSet<int> rowIndexs = new HashSet<int>();
                        foreach (DataGridViewCell selectCell in selectCells)
                        {
                            rowIndexs.Add(selectCell.RowIndex);
                        }
                        foreach (int index in rowIndexs)
                        {
                            DataGridViewRow selectRow = entityTable.Rows[index];
                            if (selectRow.Index == 0)
                            {
                                continue;
                            }
                            var zorValue = selectRow.Cells[0].Value;
                            var oneValue = empToValue(selectRow.Cells[1].Value);
                            var twoValue = empToValue(selectRow.Cells[2].Value);
                            var thrValue = empToValue(selectRow.Cells[3].Value);
                            var forValue = empToValue(selectRow.Cells[4].Value);
                            if (zorValue == null || twoValue == null)
                            {
                                continue;
                            }
                            string columnAlias = zorValue.ToString();
                            if (twoValue.ToString() == "" && thrValue == "")
                            {
                                continue;
                            }
                            string f7TableName = thrValue.ToString();
                            if (twoValue.ToString() != "" && thrValue != "" && forValue == "")
                            {
                                //F7 字段 和表
                                string key = twoValue.ToString();
                                entrySql.Append("left join ").Append(f7TableName).Append(" ");
                                entrySql.Append(columnAlias).Append(" on ").Append(columnAlias).Append(".fid");
                                entrySql.Append("=#1.").Append(key);
                                entrySql.Append(" --").Append(oneValue).Append(Environment.NewLine);
                            }
                            else if (twoValue.ToString() != "" && thrValue != "" && forValue != "")
                            {
                                //当前表字段--枚举
                                selectStr.Append("#1.").Append(twoValue.ToString()).Append(" ").Append(columnAlias).Append(",");
                                selectStr.Append(" --").Append(oneValue);
                                selectStr.Append(Environment.NewLine);
                            }
                            else if (twoValue.ToString() != "" && thrValue == "")
                            {
                                //当前表字段
                                selectStr.Append("#1.").Append(twoValue.ToString()).Append(" ").Append(columnAlias).Append(",");
                                selectStr.Append(" --").Append(oneValue);
                                selectStr.Append(Environment.NewLine);
                            }

                            else if (twoValue.ToString() == "" && thrValue != "")
                            {
                                //下级分录
                                entrySql.Append("left join ").Append(f7TableName).Append(" ");
                                entrySql.Append(columnAlias).Append(" on ").Append(columnAlias).Append(".fparentid");
                                entrySql.Append("=#1.fid");
                                entrySql.Append(" --").Append(oneValue).Append(Environment.NewLine);
                            }
                        }
                        selectStr.Append("#1.fid ID").Append(Environment.NewLine); ;
                        //SQL主体
                        StringBuilder sql = new StringBuilder();
                        sql.Append("select").Append(Environment.NewLine); ;
                        //sql.Append("#1.*").Append(Environment.NewLine);
                        sql.Append(selectStr);
                        sql.Append("from #0 #1").Append(Environment.NewLine);
                        sql.Append(entrySql);
                        //替换
                        sql.Replace("#0", mainTable);
                        sql.Replace("#1", alias);
                        txtSql.Text = sql.ToString();
                    }
                    dropDown.Show(this, 880, 300);
                }
            }
            else if (e.KeyChar == 8)
            {
                btnLeft_Click(sender, e);
            }
        }

        private void entityTable_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.XButton1)
            {
                btnLeft_Click(sender, null);
            }
            else if (e.Button == MouseButtons.XButton2)
            {
                btnRight_Click(sender, null);
            }
        }

        public string getRelationToEntityTableName(string fullName, string fieldName)
        {
            string tableName = "";
            if (fullName == null || fullName.Length == 0)
            {
                return tableName;
            }
            string path = MetaDataUtil.getPath(fullName, MetaDataTypeEnum.relation);
            if (!MetaDataUtil.relationMap.ContainsKey(fullName) || MetaDataUtil.relationMap[fullName] == null)
            {
                if (File.Exists(path))
                {
                    XmlDocument xml = new XmlDocument();
                    XmlTextReader reader = new XmlTextReader(path);
                    xml.Load(reader);
                    XmlNode root = xml.LastChild;
                    MetaDataUtil.relationParse(root);
                    reader.Close();
                }
                else
                {
                    string jarPath = "";
                    GetValue("metadata", fullName, out jarPath, relationPath);
                    if (jarPath != null || jarPath.Length > 0)
                    {
                        //存在对应jar包
                        initJarFile(jarPath);
                    }
                }
            }

            RelationInfo relation = MetaDataUtil.getRelation(fullName);
            if (relation != null)
            {
                string supplier = relation.supplierObject;
                if (fieldName == "parent")
                {
                    supplier = relation.clientObject;
                }
                if (!MetaDataUtil.entityMap.ContainsKey(supplier) || MetaDataUtil.entityMap[supplier] == null)
                {
                    path = MetaDataUtil.getPath(supplier, MetaDataTypeEnum.entity);
                    if (File.Exists(path))
                    {
                        XmlDocument xml = new XmlDocument();
                        XmlTextReader reader = new XmlTextReader(path);
                        xml.Load(reader);
                        XmlNode root = xml.LastChild;
                        MetaDataUtil.entityParse(root);
                        reader.Close();
                    }
                    else
                    {
                        string jarPath = "";
                        GetValue("metadata", supplier, out jarPath, entityPath);
                        if (jarPath != null || jarPath.Length > 0)
                        {
                            //存在对应jar包
                            initJarFile(jarPath);
                        }
                    }
                }
                EntityInfo entityInfo = MetaDataUtil.getEntity(supplier);
                if (entityInfo != null)
                {
                    tableName = entityInfo.tableName;
                }
            }
            return tableName;
        }


        #region 控件缩放
        double formWidth;//窗体原始宽度
        double formHeight;//窗体原始高度
        double scaleX;//水平缩放比例
        double scaleY;//垂直缩放比例
        Dictionary<string, string> ControlsInfo = new Dictionary<string, string>();//控件中心Left,Top,控件Width,控件Height,控件字体Size
        #endregion
        protected void GetAllInitInfo(Control ctrlContainer)
        {
            if (ctrlContainer.Parent == this)//获取窗体的高度和宽度
            {
                formWidth = Convert.ToDouble(ctrlContainer.Width);
                formHeight = Convert.ToDouble(ctrlContainer.Height);
            }
            foreach (Control item in ctrlContainer.Controls)
            {
                if (item.Name.Trim() != "")
                {
                    //添加信息：键值：控件名，内容：据左边距离，距顶部距离，控件宽度，控件高度，控件字体。
                    ControlsInfo.Add(item.Name, (item.Left + item.Width / 2) + "," + (item.Top + item.Height / 2) + "," + item.Width + "," + item.Height + "," + item.Font.Size);
                }
                if ((item as UserControl) == null && item.Controls.Count > 0)
                {
                    GetAllInitInfo(item);
                }
            }
        }
        private void ControlsChaneInit(Control ctrlContainer)
        {
            scaleX = (Convert.ToDouble(ctrlContainer.Width) / formWidth);
            scaleY = (Convert.ToDouble(ctrlContainer.Height) / formHeight);
        }
        /// <summary>
        /// 改变控件大小
        /// </summary>
        /// <param name="ctrlContainer"></param>
        private void ControlsChange(Control ctrlContainer)
        {
            double[] pos = new double[5];//pos数组保存当前控件中心Left,Top,控件Width,控件Height,控件字体Size
            foreach (Control item in ctrlContainer.Controls)//遍历控件
            {
                if (item.Name.Trim() != "")//如果控件名不是空，则执行
                {
                    if ((item as UserControl) == null && item.Controls.Count > 0)//如果不是自定义控件
                    {
                        ControlsChange(item);//循环执行
                    }
                    string[] strs = ControlsInfo[item.Name].Split(',');//从字典中查出的数据，以‘，’分割成字符串组

                    for (int i = 0; i < 5; i++)
                    {
                        pos[i] = Convert.ToDouble(strs[i]);//添加到临时数组
                    }
                    double itemWidth = pos[2] * scaleX;     //计算控件宽度，double类型
                    double itemHeight = pos[3] * scaleY;    //计算控件高度
                    item.Left = Convert.ToInt32(pos[0] * scaleX - itemWidth / 2);//计算控件距离左边距离
                    item.Top = Convert.ToInt32(pos[1] * scaleY - itemHeight / 2);//计算控件距离顶部距离
                    item.Width = Convert.ToInt32(itemWidth);//控件宽度，int类型
                    item.Height = Convert.ToInt32(itemHeight);//控件高度
                    item.Font = new Font(item.Font.Name, float.Parse((pos[4] * Math.Min(scaleX, scaleY)).ToString()));//字体
                }
            }
        }

        private void MainPanel_SizeChanged(object sender, EventArgs e)
        {
            if (ControlsInfo.Count > 0)//如果字典中有数据，即窗体改变
            {
                ControlsChaneInit(Controls[0]);//表示pannel控件
                ControlsChange(Controls[0]);
            }
        }
    }
}
