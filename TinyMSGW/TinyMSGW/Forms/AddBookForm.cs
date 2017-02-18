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
    /// 窗体：添加书籍
    /// </summary>
    public partial class AddBookForm : Form
    {
        /// <summary>
        /// 适配器
        /// </summary>
        private IActionAdapter adapter = AdapterFactory.GetAdapter();

        /// <summary>
        /// 编辑书籍的ISBN
        /// </summary>
        private string editISBN;

        /// <summary>
        /// 构造器
        /// </summary>
        public AddBookForm(string isbn)
        {
            InitializeComponent();
            this.editISBN = isbn;
            // 编辑时要渲染控件
            if (this.editISBN != String.Empty)
            {
                this.Text = "编辑书籍 [" + this.editISBN + "]";
                Book bk;
                this.adapter.RetrieveBook(this.editISBN, out bk);
                this.textBox1.Text = bk.Name;
                this.textBox2.Text = bk.Author;
                this.textBox3.Text = bk.Type;
                this.textBox4.Text = bk.LocationOfLibrary;
                this.textBox5.Text = bk.ISBN;
                this.numericUpDown1.Value = (int)bk.Value;
                this.numericUpDown2.Value = (int)Math.Round((bk.Value - Math.Floor(bk.Value)) * 100.0f);
                this.numericUpDown3.Value = bk.PublishYear;
                this.numericUpDown4.Value = bk.NumberInLibrary;
                this.numericUpDown5.Value = bk.NumberInRenting;
            }
        }

        /// <summary>
        /// 事件：已借出数字变化时
        /// </summary>
        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown5.Value = Math.Min(this.numericUpDown5.Value, this.numericUpDown4.Maximum);
        }

        /// <summary>
        /// 事件：上架总数数字变化时
        /// </summary>
        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            this.numericUpDown5.Value = Math.Min(this.numericUpDown5.Value, this.numericUpDown4.Maximum);
        }

        /// <summary>
        /// 按钮：取消
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 按钮：确认
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            // 完整性检查
            if (this.textBox1.Text.Trim() == String.Empty ||
                this.textBox2.Text.Trim() == String.Empty ||
                this.textBox3.Text.Trim() == String.Empty ||
                this.textBox4.Text.Trim() == String.Empty ||
                this.textBox5.Text.Trim() == String.Empty)
            {
                MessageBox.Show("请完整填写信息");
                return;
            }
            // 合理性检查
            if ((this.numericUpDown1.Value == 0 &&
                this.numericUpDown2.Value == 0) ||
                this.numericUpDown4.Value == 0)
            {
                var dr = MessageBox.Show("信息中的定价和数量似乎不合理，确定要这样提交吗？", "确认",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.Cancel)
                {
                    return;
                }
            }
            // 提交到后台
            Book bkDescriptor = new Book()
            {
                ISBN = this.textBox5.Text,
                Name = this.textBox1.Text,
                Author = this.textBox2.Text,
                Type = this.textBox3.Text,
                Value = (double)this.numericUpDown1.Value + (double)this.numericUpDown2.Value / 100.0f,
                PublishYear = (int)this.numericUpDown3.Value,
                LocationOfLibrary = this.textBox4.Text,
                NumberInLibrary = (int)this.numericUpDown4.Value,
                NumberInRenting = (int)this.numericUpDown5.Value,
                StoreIntoLibraryTimestamp = DateTime.Now
            };
            // 添加的情况下
            if (this.editISBN == String.Empty)
            {
                this.adapter.LibrarianAddBook(bkDescriptor, (int)this.numericUpDown4.Value);
            }
            // 编辑的情况下
            else
            {
                // ISBN冲突检验
                if (this.editISBN != bkDescriptor.ISBN)
                {
                    Book bk;
                    this.adapter.RetrieveBook(bkDescriptor.ISBN, out bk);
                    if (bk != null)
                    {
                        MessageBox.Show("该ISBN已经被占用了，请修改");
                        return;
                    }
                }
                // 提交更改
                Book oldDescriptor = new Book()
                {
                    ISBN = this.editISBN
                };
                this.adapter.EditBook(oldDescriptor, bkDescriptor);
            }
            // 刷新主窗体
            MessageBox.Show("更改已提交");
            ((RetrieveBookForm)this.Owner).button1_Click(null, null);
            this.Close();
        }
    }
}
