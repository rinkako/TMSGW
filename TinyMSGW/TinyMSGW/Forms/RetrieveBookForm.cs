using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TinyMSGW.Utils;
using TinyMSGW.Entity;
using TinyMSGW.Adapter;

namespace TinyMSGW.Forms
{
    public partial class  RetrieveBookForm : Form
    {
        /// <summary>
        /// 适配器
        /// </summary>
        private IActionAdapter adapter = AdapterFactory.GetAdapter();

        /// <summary>
        /// 是否为管理模式
        /// </summary>
        private bool isManageMode;

        /// <summary>
        /// 构造器
        /// </summary>
        public RetrieveBookForm(bool manager)
        {
            InitializeComponent();
            // 权限检查
            if (manager == false)
            {
                this.button3.Visible = this.button4.Visible = this.button5.Visible = false;
            }
            else
            {
                this.label2.Visible = false;
            }
            this.isManageMode = manager;
            // 做一次默认查询
            this.button1_Click(null, null);
        }

        /// <summary>
        /// 按钮：筛选
        /// </summary>
        public void button1_Click(object sender, EventArgs e)
        {
            Object ds;
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
            BookDetailForm bdf = new BookDetailForm(isbn, this.isManageMode);
            bdf.ShowDialog(this);
        }

        /// <summary>
        /// 按钮：图书返回仓库
        /// </summary>
        private void button4_Click(object sender, EventArgs e)
        {
            // 确认
            var dr = MessageBox.Show("真的要将该图书的未借出的所有项目下架并移入仓库吗？", "确认",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Cancel)
            {
                return;
            }
            // 提交更改
            var rowC = this.dataGridView1.SelectedRows;
            if (rowC.Count > 0)
            {
                var rowItem = rowC[0];
                Book bkDescriptor = new Book()
                {
                    ISBN = (string)rowItem.Cells["ISBN"].Value
                };
                this.adapter.LibrarianRestoreBook(bkDescriptor);
                MessageBox.Show("更改已提交");
                // 刷新
                this.button1_Click(null, null);
            }
        }

        /// <summary>
        /// 按钮：下架并扔掉
        /// </summary>
        private void button6_Click(object sender, EventArgs e)
        {
            var rowC = this.dataGridView1.SelectedRows;
            if (rowC.Count == 0)
            {
                return;
            }
            var dr = MessageBox.Show("你确定真的要这么做吗？" + Environment.NewLine + "未归还的图书也将被抛弃", "确认",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Cancel)
            {
                return;
            }
            // 提交到后台
            var rowItem = rowC[0];
            Book bkDescriptor = new Book()
            {
                ISBN = (string)rowItem.Cells["ISBN"].Value
            };
            this.adapter.LibrarianRemoveBook(bkDescriptor);
            MessageBox.Show("更改已提交");
            // 刷新
            this.button1_Click(null, null);
        }

        /// <summary>
        /// 按钮：修改信息
        /// </summary>
        private void button5_Click(object sender, EventArgs e)
        {
            var rowC = this.dataGridView1.SelectedRows;
            if (rowC.Count == 0)
            {
                return;
            }
            // 提交到后台
            var rowItem = rowC[0];
            AddBookForm abf = new AddBookForm((string)rowItem.Cells["ISBN"].Value);
            abf.ShowDialog(this);
        }

        /// <summary>
        /// 按钮：上架新书
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            AddBookForm abf = new AddBookForm(String.Empty);
            abf.ShowDialog(this);
        }
    }
}
