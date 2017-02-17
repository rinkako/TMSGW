using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TinyMSGW.Adapter;
using TinyMSGW.Entity;

namespace TinyMSGW.Forms
{
    public partial class BookDetailForm : Form
    {
        /// <summary>
        /// 适配器
        /// </summary>
        private IActionAdapter adapter = AdapterFactory.GetAdapter();

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="isbn">要查询的书籍的ISBN</param>
        public BookDetailForm(string isbn)
        {
            InitializeComponent();
            // 读取图书数据
            Book bk;
            this.adapter.RetrieveBook(isbn, out bk);
            // 渲染到前端
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("名字: {0}", bk.Name));
            sb.AppendLine(String.Format("ISBN: {0}", bk.ISBN));
            sb.AppendLine(String.Format("作者: {0}", bk.Author));
            sb.AppendLine(String.Format("类型: {0}", bk.Type));
            sb.AppendLine(String.Format("出版年: {0}", bk.PublishYear));
            sb.AppendLine(String.Format("定价: {0}", bk.Value));
            sb.AppendLine();
            sb.AppendLine(String.Format("馆藏位置: {0}", bk.LocationOfLibrary));
            sb.AppendLine(String.Format("数量: {0}/{1}", bk.NumberInLibrary, bk.NumberInLibrary + bk.NumberInRenting));
            this.textBox1.Text = sb.ToString();
            // 如果没有余量那么不能租借
            if (bk.NumberInLibrary == 0)
            {
                this.button2.Enabled = false;
            }
            // 把焦点放在放回
            this.textBox1.SelectedText = String.Empty;
        }

        /// <summary>
        /// 按钮：返回
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 按钮：租借
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
