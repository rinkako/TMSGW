using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TinyMSGW.Utils;

namespace TinyMSGW.Forms
{
    public partial class RetrieveBookForm : Form
    {
        public RetrieveBookForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 按钮：筛选
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            DataSet ds = DBUtil.GetDataSet(DBUtil.Conn, CommandType.Text, "select * from tw_book", null);
            this.dataGridView1.DataSource = ds.Tables[0].DefaultView;
        }
    }
}
