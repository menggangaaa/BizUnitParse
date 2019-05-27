using System;
using System.Collections;
using System.Windows.Forms;

namespace BizUnitParse
{
    public partial class EnumUI : Form
    {
        public EnumUI()
        {
            InitializeComponent();
        }

        private void EnumUI_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        public DialogResult ShowDialog(EnumInfo enumInfo)
        {

            dataGridView1.Rows.Clear();
            if (enumInfo != null)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = enumInfo.name;
                dataGridView1.Rows[index].Cells[1].Value = enumInfo.alias;
                dataGridView1.Rows[index].Cells[2].Value = enumInfo.enumDataType;
                foreach (EnumValueInfo enumValue in enumInfo.enumValues)
                {
                    index = dataGridView1.Rows.Add();
                    dataGridView1.Rows[index].Cells[0].Value = enumValue.name;
                    dataGridView1.Rows[index].Cells[1].Value = enumValue.alias;
                    dataGridView1.Rows[index].Cells[2].Value = enumValue.value;
                }
            }
            return ShowDialog();
        }

        private void EnumUI_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}
