using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TinyMSGW.Utils;
using TinyMSGW.Adapter;

namespace TinyMSGW.Forms
{
    public partial class RetrieveBookForm : Form
    {
        /// <summary>
        /// 适配器
        /// </summary>
        private IActionAdapter adapter = AdapterFactory.GetAdapter();

        /// <summary>
        /// 构造器
        /// </summary>
        public RetrieveBookForm()
        {
            InitializeComponent();
            //this.comboBox1.SelectedIndex = 0;
            // 做一次默认查询
            this.button1_Click(null, null);
        }

        /// <summary>
        /// 按钮：筛选
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            Object ds;
            //this.adapter.ListAllLibraryBook(out ds, this.textBox1.Text, this.comboBox1.SelectedIndex == 0 ? String.Empty : this.comboBox1.SelectedValue.ToString());
            this.adapter.ListAllLibraryBook(out ds, this.textBox1.Text, String.Empty);
            this.dataGridView1.DataSource = (ds as DataSet).Tables[0].DefaultView;
        }

        /// <summary>
        /// 按钮：返回
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Owner.Show();
            this.Close();
        }

        /// <summary>
        /// 动作：双击单元格
        /// </summary>
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string isbn = (string)this.dataGridView1.Rows[e.RowIndex].Cells[0].Value;
            BookDetailForm bdf = new BookDetailForm(isbn);
            bdf.ShowDialog(this);
        }
    }
}
