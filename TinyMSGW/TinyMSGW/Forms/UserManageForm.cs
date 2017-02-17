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
    public partial class UserManageForm : Form
    {
        /// <summary>
        /// 适配器
        /// </summary>
        private IActionAdapter adapter = AdapterFactory.GetAdapter();

        /// <summary>
        /// 构造器
        /// </summary>
        public UserManageForm()
        {
            InitializeComponent();
            // 权限控制
            if (GlobalDataPackage.CurrentUser.Type == Enums.UserType.Librarian)
            {
                this.button3.Visible = this.button4.Visible = this.button5.Visible = false;
                this.button7.Location = this.button3.Location;
                this.button6.Location = this.button4.Location;
            }
            // 刷新
            this.button1_Click(null, null);
        }

        /// <summary>
        /// 按钮：筛选
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            Object ds;
            this.adapter.ListAllUser(out ds, this.textBox1.Text);
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
        /// 按钮：删除
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            var rowObjC = this.dataGridView1.SelectedRows;
            if (rowObjC != null && rowObjC.Count > 0)
            {
                var rowObj = rowObjC[0];
                // 不能删除管理员
                if ((int)(rowObj.Cells["UserID"].Value) == 0)
                {
                    MessageBox.Show("管理员账号不能删除");
                    return;
                }
                var dr = MessageBox.Show("真的要删除用户 " + rowObj.Cells["UserName"].Value + " 吗？" + Environment.NewLine + 
                    "该操作会同时移除用户的借书卡，这将导致其未归还的图书丢失。" + Environment.NewLine +
                    "并且该用户名会被保留而无法再被其他人注册或修改为", "确认",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.Yes)
                {
                    // 提交更改到后台
                    User descriptor = new User()
                    {
                        UserID = (int)(rowObj.Cells["UserId"].Value),
                        UserName = (string)(rowObj.Cells["UserName"].Value)
                    };
                    this.adapter.DeleteUser(descriptor);
                    // 刷新
                    this.button1_Click(null, null);
                }
            }
        }

        /// <summary>
        /// 按钮：修改
        /// </summary>
        private void button4_Click(object sender, EventArgs e)
        {
            var rowObjC = this.dataGridView1.SelectedRows;
            if (rowObjC != null && rowObjC.Count > 0)
            {
                var rowObj = rowObjC[0];
                List<string> nameVec = new List<string>();
                for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                {
                    if ((string)(this.dataGridView1.Rows[i].Cells["UserName"].Value) != (string)(rowObj.Cells["UserName"].Value))
                    {
                        nameVec.Add((string)(this.dataGridView1.Rows[i].Cells["UserName"].Value));
                    }
                }
                UserAddEditForm uaef = new UserAddEditForm((int)(rowObj.Cells["UserID"].Value), (string)(rowObj.Cells["UserName"].Value), nameVec);
                uaef.ShowDialog(this);
                // 刷新
                this.button1_Click(null, null);
            }
        }

        /// <summary>
        /// 按钮：添加
        /// </summary>
        private void button5_Click(object sender, EventArgs e)
        {
            UserAddEditForm uaef = new UserAddEditForm();
            uaef.ShowDialog(this);
            // 刷新
            this.button1_Click(null, null);
        }

        /// <summary>
        /// 按钮：办理借书卡
        /// </summary>
        private void button6_Click(object sender, EventArgs e)
        {
            var rowC = this.dataGridView1.SelectedRows;
            if (rowC.Count > 0)
            {
                var rowItem = rowC[0];
                // 重复性检验
                if ((int)rowItem.Cells["CardID"].Value != -1)
                {
                    MessageBox.Show("该用户已经办理借书卡了");
                    return;
                }
                // 提交更改
                User userDescriptor = new User()
                {
                    UserID = (int)rowItem.Cells["UserID"].Value
                };
                Usercard outCard;
                this.adapter.CustomerHandleUsercard(userDescriptor, out outCard);
                MessageBox.Show("办理成功！");
                // 刷新
                this.button1_Click(null, null);
            }
        }

        /// <summary>
        /// 按钮：注销借书卡
        /// </summary>
        private void button7_Click(object sender, EventArgs e)
        {
            var rowC = this.dataGridView1.SelectedRows;
            if (rowC.Count > 0)
            {
                var rowItem = rowC[0];
                // 重复性检验
                if ((int)rowItem.Cells["CardID"].Value == -1)
                {
                    MessageBox.Show("该用户并未拥有借书卡");
                    return;
                }
                // 书籍归还检验
                List<Book> bkList;
                List<RentLog> rlList;
                this.adapter.ListAllRentingBook((string)rowItem.Cells["UserName"].Value, false, out bkList, out rlList);
                if (bkList != null && bkList.Count > 0)
                {
                    MessageBox.Show("该用户还有未归还的图书，不能注销借书卡");
                    return;
                }
                // 提交更改
                User userDescriptor = new User()
                {
                    UserID =(int)rowItem.Cells["UserID"].Value
                };
                Usercard cardDescriptor = new Usercard()
                {
                    UsercardID = (int)rowItem.Cells["CardID"].Value
                };
                this.adapter.CustomerCancelUsercard(userDescriptor, cardDescriptor);
                MessageBox.Show("注销成功！");
                // 刷新
                this.button1_Click(null, null);
            }
        }
    }
}
