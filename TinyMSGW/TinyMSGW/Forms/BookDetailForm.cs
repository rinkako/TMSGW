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
        /// 图书实例
        /// </summary>
        private Book bk;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="isbn">要查询的书籍的ISBN</param>
        /// <param name="readOnly">是否不可租借</param>
        public BookDetailForm(string isbn, bool readOnly)
        {
            InitializeComponent();
            // 读取图书数据
            this.adapter.RetrieveBook(isbn, out this.bk);
            // 渲染到前端
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("名字: {0}", this.bk.Name));
            sb.AppendLine(String.Format("ISBN: {0}", this.bk.ISBN));
            sb.AppendLine(String.Format("作者: {0}", this.bk.Author));
            sb.AppendLine(String.Format("类型: {0}", this.bk.Type));
            sb.AppendLine(String.Format("出版年: {0}", this.bk.PublishYear));
            sb.AppendLine(String.Format("定价: {0}", this.bk.Value));
            sb.AppendLine();
            sb.AppendLine(String.Format("馆藏位置: {0}", this.bk.LocationOfLibrary));
            sb.AppendLine(String.Format("数量: {0}/{1}", this.bk.NumberInLibrary - this.bk.NumberInRenting, this.bk.NumberInLibrary));
            this.textBox1.Text = sb.ToString();
            // 如果没有余量那么不能租借
            if (this.bk.NumberInLibrary - this.bk.NumberInRenting == 0)
            {
                this.button2.Enabled = false;
            }
            // 信息阅读模式不显示借书按钮
            if (readOnly == true)
            {
                this.button2.Visible = false;
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
            // 检查是否已经有借书卡
            if (GlobalDataPackage.CurrentUser.CardID == -1)
            {
                MessageBox.Show("当前账户还没有办理借书卡，请到柜台办理，才可以租借图书");
                return;
            }
            // 检查是否已经租借此书
            List<Book> bkList;
            List<RentLog> rList;
            this.adapter.ListAllRentingBook(GlobalDataPackage.CurrentUser.UserName, false, out bkList, out rList);
            if (bkList.Find((x) => x.ISBN == this.bk.ISBN) != null)
            {
                MessageBox.Show("当前账户已经租借了本图书并且还未归还，不能再租借");
                return;
            }
            // 检查是否有任何延期没归还图书
            if (rList.Find((x) => x.OughtReturnTimestamp < DateTime.Now) != null)
            {
                MessageBox.Show("当前账户有延期为归还的图书，暂时不可租借任何图书，请先归还延期的图书");
                return;
            }
            // 提交租借请求
            Usercard cardDescriptor = new Usercard()
            {
                UsercardID = GlobalDataPackage.CurrentUser.CardID
            };
            this.adapter.CustomerRentBook(this.bk, cardDescriptor);
            MessageBox.Show("租借成功！");
            this.Close();
        }
    }
}
