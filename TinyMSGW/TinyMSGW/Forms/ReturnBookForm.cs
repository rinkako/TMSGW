using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TinyMSGW.Adapter;
using TinyMSGW.Entity;

namespace TinyMSGW.Forms
{
    /// <summary>
    /// 窗体：自助还书
    /// </summary>
    public partial class ReturnBookForm : Form
    {
        /// <summary>
        /// 适配器
        /// </summary>
        private IActionAdapter adapter = AdapterFactory.GetAdapter();

        /// <summary>
        /// 构造器
        /// </summary>
        public ReturnBookForm()
        {
            InitializeComponent();
            // 渲染控件
            List<Book> bookList;
            List<RentLog> logList;
            this.adapter.ListAllRentingBook(GlobalDataPackage.CurrentUser.UserName, false, out bookList, out logList);
            for (int i = 0; i < bookList.Count; i++)
            {
                var b = bookList[i];
                var l = logList[i];
                this.dataGridView1.Rows.Add(b.ISBN, b.Name, b.Author, b.PublishYear, l.BorrowTimestamp, l.OughtReturnTimestamp);
            }
        }

        /// <summary>
        /// 按钮：返回
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 按钮：归还
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            var rowC = this.dataGridView1.SelectedRows;
            if (rowC.Count > 0)
            {
                var row = rowC[0];
                // 超时检验
                if (((DateTime)row.Cells["Column6"].Value) < DateTime.Now)
                {
                    MessageBox.Show("此图书已延迟归还，请到柜台处办理归还手续");
                    return;
                }
                // 提交更改
                Book bkDescriptor = new Book()
                {
                    ISBN = (string)row.Cells["Column1"].Value
                };
                Usercard cardDescriptor = new Usercard()
                {
                    UsercardID = GlobalDataPackage.CurrentUser.CardID
                };
                this.adapter.CustomerReturnBook(bkDescriptor, cardDescriptor);
                MessageBox.Show("自助归还成功，请将书籍放置到一侧的图书回收柜上");
                this.Close();
            }
        }
    }
}
