using System.Collections.Generic;
using System.Windows.Forms;
using TinyMSGW.Entity;
using TinyMSGW.Adapter;

namespace TinyMSGW.Forms
{
    /// <summary>
    /// 窗体：借书记录
    /// </summary>
    public partial class RentlogForm : Form
    {
        /// <summary>
        /// 适配器
        /// </summary>
        private IActionAdapter adapter = AdapterFactory.GetAdapter();

        /// <summary>
        /// 构造器
        /// </summary>
        public RentlogForm()
        {
            InitializeComponent();
            // 渲染控件
            List<Book> bkList;
            List<RentLog> rlList;
            this.adapter.ListAllRentingBook(GlobalDataPackage.CurrentUser.UserName, true, out bkList, out rlList);
            for (int i = 0; i < bkList.Count; i++)
            {
                var b = bkList[i];
                var l = rlList[i];
                this.dataGridView1.Rows.Add(b.ISBN, b.Name, b.Author, b.PublishYear, l.BorrowTimestamp, l.OughtReturnTimestamp,
                    l.ActualReturnTimestamp == null ? "-" : l.ActualReturnTimestamp.ToString());
            }
        }
    }
}
