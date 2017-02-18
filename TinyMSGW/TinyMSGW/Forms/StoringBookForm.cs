using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TinyMSGW.Entity;
using TinyMSGW.Adapter;

namespace TinyMSGW.Forms
{
    /// <summary>
    /// 窗体：管理库存
    /// </summary>
    public partial class StoringBookForm : Form
    {
        /// <summary>
        /// 适配器
        /// </summary>
        private IActionAdapter adapter = AdapterFactory.GetAdapter();

        /// <summary>
        /// 构造器
        /// </summary>
        public StoringBookForm()
        {
            InitializeComponent();
            // 刷新窗体
            this.button1_Click(null, null);
        }

        /// <summary>
        /// 按钮：筛选
        /// </summary>
        public void button1_Click(object sender, EventArgs e)
        {
            // 暂时只考虑一个仓库
            Warehouse whDescriptor = new Warehouse()
            {
                WarehouseID = 0
            };
            object outDs;
            this.adapter.ListAllStoringBook(whDescriptor, this.textBox1.Text, out outDs);
            DataSet ds = outDs as DataSet;
            this.dataGridView1.DataSource = ds.Tables[0].DefaultView;
        }

        /// <summary>
        /// 按钮：新书入仓
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            AddBookForm abf = new AddBookForm(true, string.Empty);
            abf.ShowDialog(this);
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
            AddBookForm abf = new AddBookForm(true, (string)rowItem.Cells["ISBN"].Value);
            abf.ShowDialog(this);
        }

        /// <summary>
        /// 按钮：扔掉
        /// </summary>
        private void button6_Click(object sender, EventArgs e)
        {
            var rowC = this.dataGridView1.SelectedRows;
            if (rowC.Count == 0)
            {
                return;
            }
            var dr = MessageBox.Show("你确定真的要这么做吗？", "确认",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Cancel)
            {
                return;
            }
            // 提交到后台
            var rowItem = rowC[0];
            StoringBook bkDescriptor = new StoringBook()
            {
                ISBN = (string)rowItem.Cells["ISBN"].Value
            };
            Warehouse whDescriptor = new Warehouse()
            {
                WarehouseID = 0
            };
            this.adapter.KeeperRemoveBook(whDescriptor, bkDescriptor);
            MessageBox.Show("更改已提交");
            // 刷新
            this.button1_Click(null, null);
        }

        /// <summary>
        /// 按钮：返回
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 按钮：图书出仓
        /// </summary>
        private void button4_Click(object sender, EventArgs e)
        {
            var rowC = this.dataGridView1.SelectedRows;
            if (rowC.Count == 0)
            {
                return;
            }
            var rowItem = rowC[0];
            var dr = MessageBox.Show(String.Format("确认要将库存图书 {0} 中的 {1} 本拿出仓库吗？",
                (string)rowItem.Cells["Name"].Value, this.numericUpDown1.Value), "确认", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.Cancel)
            {
                return;
            }
            // 提交更改
            Warehouse whDescriptor = new Warehouse()
            {
                WarehouseID = 0
            };
            StoringBook sbDescriptor = new StoringBook()
            {
                ISBN = rowItem.Cells["ISBN"].Value.ToString()
            };
            this.adapter.KeeperShopBook(whDescriptor, sbDescriptor, (int)this.numericUpDown1.Value);
            MessageBox.Show("出库成功！请通知柜员及时录入");
            // 刷新
            this.button1_Click(null, null);
        }

        /// <summary>
        /// 事件：单元格点击
        /// </summary>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var rowC = this.dataGridView1.SelectedRows;
            if (rowC.Count > 0)
            {
                var rowItem = rowC[0];
                this.numericUpDown1.Maximum = (int)rowItem.Cells["NumberOfWarehouse"].Value;
                this.numericUpDown1.Value = this.numericUpDown1.Maximum;
            }
        }
    }
}
