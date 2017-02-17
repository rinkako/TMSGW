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
    /// 窗体：赔偿
    /// </summary>
    public partial class PayLostForm : Form
    {
        /// <summary>
        /// 适配器
        /// </summary>
        private IActionAdapter adapter = AdapterFactory.GetAdapter();

        /// <summary>
        /// 构造器
        /// </summary>
        public PayLostForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 按钮：返回
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 按钮：查询
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text.Trim() == String.Empty)
            {
                MessageBox.Show("请输入用户名");
                return;
            }
            // 渲染控件
            List<Book> bookList;
            List<RentLog> logList;
            this.adapter.ListAllRentingBook(this.textBox1.Text.Trim(), false, out bookList, out logList);
            if (bookList == null)
            {
                MessageBox.Show("查无此人");
                return;
            }
            this.dataGridView1.Rows.Clear();
            for (int i = 0; i < bookList.Count; i++)
            {
                var b = bookList[i];
                var l = logList[i];
                this.dataGridView1.Rows.Add(b.ISBN, b.Name, b.Author, b.PublishYear, b.Value, l.BorrowTimestamp, l.OughtReturnTimestamp);
            }
        }

        /// <summary>
        /// 按钮：结算滞纳
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            this.DoBill(true);
        }

        /// <summary>
        /// 按钮：结算损坏
        /// </summary>
        private void button4_Click(object sender, EventArgs e)
        {
            this.DoBill(false);
        }

        /// <summary>
        /// 结算金额
        /// </summary>
        /// <param name="isOnlyDelay">是否只结算滞纳金</param>
        private void DoBill(bool isOnlyDelay)
        {
            var rowC = this.dataGridView1.SelectedRows;
            if (rowC.Count > 0)
            {
                var rowItem = rowC[0];
                // 先结算本金
                double bValue = isOnlyDelay ? 0 : (double)rowItem.Cells["Column7"].Value;
                // 再结算滞纳金
                double dValue = 0.0f;
                var otime = (DateTime)rowItem.Cells["Column6"].Value;
                var ctime = DateTime.Now;
                if (otime < ctime)
                {
                    dValue = (ctime - otime).Days * GlobalDataPackage.DelayFeeADay;
                }
                // 提示柜员收钱
                var dr = MessageBox.Show(String.Format("当前处理赔偿：{0}书籍：{1}{0}ISBN：{2}{0}成本：{3}{0}滞纳金：{4}{0}--------{0}合计：{5}{0}",
                    Environment.NewLine, (string)(rowItem.Cells["Column2"].Value), (string)(rowItem.Cells["Column1"].Value),
                    bValue, dValue, bValue + dValue), "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.OK)
                {
                    // 刷新后台
                    Book bkDescriptor = new Book()
                    {
                        ISBN = (string)rowItem.Cells["Column1"].Value
                    };
                    Dictionary<string, string> usrDict;
                    this.adapter.RetrieveUser(this.textBox1.Text.Trim(), out usrDict);
                    Usercard cardDescriptor = new Usercard()
                    {
                        UsercardID = Convert.ToInt32(usrDict["cardid"])
                    };
                    this.adapter.CustomerReturnBook(bkDescriptor, cardDescriptor);
                    MessageBox.Show("已处理该赔偿，请注意入账");
                    // 刷新
                    this.button1_Click(null, null);
                }
            }
        }
    }
}
