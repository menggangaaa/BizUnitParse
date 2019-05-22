using EntityParse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
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

        public MainPanel()
        {
            InitializeComponent();
            txtEntityFilter.Focus();
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
                GetValue("Information", "path", out outString, basePath);
                if (outString != null && outString.Length > 0)
                {
                    txtDirPath.Text = outString;
                }
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
                GetValue("Information", "isInitJar", out outString, basePath);
                if (outString == null || outString.Length == 0 || outString == "0")
                {
                    //未初始化jar包列表,初始化jar包列表
                    //initJarMetaDice(null);
                    labFilter.Visible = true;
                    initJarBar.Visible = false;
                    initJarBar.Value = 0;
                }
                else
                {
                    labFilter.Visible = false;
                    initJarBar.Visible = true;
                    initJarBar.Value = 100;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            string path = txtDirPath.Text + "\\metadata\\com\\kingdee\\eas\\hse\\scm";
            if (Directory.Exists(path) && isIDE.Checked)
            {
                //初始化 左边树
                initTree();
            }


            //设置文本框下拉显示
            txtEntityFilter.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtEntityFilter.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtEntityFilter.Focus();
        }

        private void txtDirPath_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnDirPathSelect_Click(object sender, EventArgs e)
        {

        }

        private void isIDE_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void isClient_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnInitJar_Click(object sender, EventArgs e)
        {

        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnLeft_Click(object sender, EventArgs e)
        {

        }

        private void btnRight_Click(object sender, EventArgs e)
        {

        }

        private void txtEntityFilter_TextChanged(object sender, EventArgs e)
        {

        }

        private void entityTable_DoubleClick(object sender, EventArgs e)
        {

        }

        private void bizUnitTree_DoubleClick(object sender, EventArgs e)
        {

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
                if (node == null || node.Nodes == null)
                {
                    entityTable.Rows.Clear();
                    txtFilter.Text = "";
                    return;
                }
                if (node.Nodes.Count == 0)
                {
                    string path = node.Tag.ToString();
                    //xmlParse(path);
                }
                else
                {
                    entityTable.Rows.Clear();
                    txtFilter.Text = "";
                }
            }
            else
            {
                //在左边树中找不到
                string outString = "";
                //GetValue("JarFileList", textBox3.Text, out outString, basePath);
                GetValue("metadata", txtEntityFilter.Text, out outString, entityPath);
                if (outString.Length > 0)
                {
                    string path = txtDirPath.Text + "\\metadata\\" + txtEntityFilter.Text;
                    //xmlParse(path);
                }
                else
                {
                    GetValue("metadata", txtEntityFilter.Text, out outString, bizunitPath);
                    if (outString.Length > 0)
                    {
                        string entityName = outString;
                        GetValue("metadata", entityName, out outString, entityPath);
                        if (outString.Length > 0)
                        {
                            string path = txtDirPath.Text + "\\metadata\\" + entityName;
                            //xmlParse(path);
                        }
                    }
                }
            }

        }

        private void MainPanel_Activated(object sender, EventArgs e)
        {
            txtEntityFilter.Focus();
        }
    }
}
