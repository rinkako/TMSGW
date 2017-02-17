using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using TinyMSGW.Adapter;

namespace TinyMSGW.Forms
{
    /// <summary>
    /// 窗体：添加或编辑用户
    /// </summary>
    public partial class UserAddEditForm : Form
    {
        /// <summary>
        /// 适配器
        /// </summary>
        private IActionAdapter adapter = AdapterFactory.GetAdapter();

        /// <summary>
        /// 是否修改用户模式
        /// </summary>
        private bool isEdit;

        /// <summary>
        /// 编辑UID
        /// </summary>
        private int editUid;

        /// <summary>
        /// 重名检测向量
        /// </summary>
        private List<string> nameCheckVec;

        /// <summary>
        /// 构造器
        /// </summary>
        public UserAddEditForm(int uid = -1, string username = "", List<string> nameVec = null)
        {
            InitializeComponent();
            if (uid == -1)
            {
                isEdit = false;
                this.Text = "添加用户";
                this.textBox1.Text = GlobalDataPackage.GlobalCounterUserID.ToString();
                this.comboBox1.SelectedIndex = 2;
            }
            else
            {
                if (uid == 0)
                {
                    // 管理员不能修改自身等级
                    this.comboBox1.Enabled = false;
                }
                isEdit = true;
                this.Text = String.Format("编辑用户[{0}]", username);
                this.textBox1.Text = uid.ToString();
                Dictionary<string, string> descDict = null;
                this.adapter.RetrieveUser(username, out descDict);
                this.textBox2.Text = username;
                this.textBox4.Text = descDict["phone"];
                this.comboBox1.SelectedIndex = Convert.ToInt32(descDict["type"]) - 1;
            }
            this.editUid = uid;
            this.nameCheckVec = nameVec;
        }
        
        /// <summary>
        /// 按钮：确定
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            // 可行性检查
            if (this.textBox2.Text.Trim() == String.Empty ||
                this.textBox4.Text.Trim() == String.Empty)
            {
                MessageBox.Show("请完整填写信息！");
                return;
            }
            // 添加的情况
            if (this.isEdit == false)
            {
                // 密码非空
                if (this.textBox3.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("密码不能为空！");
                    return;
                }
                // 提交到后台
                Dictionary<string, string> paras = new Dictionary<string, string>();
                paras.Add("password", this.textBox3.Text);
                paras.Add("phone", this.textBox4.Text.Trim());
                Entity.User oUsr;
                bool flag = this.adapter.AddUser(this.textBox2.Text.Trim(), (Enums.UserType)(this.comboBox1.SelectedIndex + 1), paras, out oUsr);
                if (flag == false)
                {
                    MessageBox.Show("添加失败，用户名已存在！");
                    this.textBox2.Focus();
                    return;
                }
                else
                {
                    MessageBox.Show("更改已提交");
                    this.Close();
                }
            }
            // 修改的情况
            else
            {
                // 重名检查
                if (this.nameCheckVec.Contains(this.textBox2.Text.Trim()))
                {
                    MessageBox.Show("该用户名已经被占用了，请更换");
                    this.textBox2.Focus();
                    return;
                }
                Dictionary<string, string> paras = new Dictionary<string, string>();
                // 判断是否需要修改密码
                if (this.textBox3.Text.Trim() == String.Empty)
                {
                    paras.Add("password", String.Empty);
                }
                else
                {
                    paras.Add("password", this.textBox3.Text.Trim());
                }
                // 提交到后台
                paras.Add("username", this.textBox2.Text.Trim());
                paras.Add("phone", this.textBox4.Text.Trim());
                paras.Add("type", (this.comboBox1.SelectedIndex + 1).ToString());
                Entity.User descriptor = new Entity.User()
                {
                    UserID = this.editUid
                };
                bool flag = this.adapter.EditUser(descriptor, (Enums.UserType)(this.comboBox1.SelectedIndex + 1), paras);
                if (flag == false)
                {
                    MessageBox.Show("修改失败！");
                    return;
                }
                else
                {
                    MessageBox.Show("更改已提交");
                    this.Close();
                }
            }
        }
    }
}
