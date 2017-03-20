using System;
using System.Windows.Forms;
using System.Collections.Generic;
using TinyMSGW.Adapter;

namespace TinyMSGW.Forms
{
    /// <summary>
    /// 窗体：自助注册
    /// </summary>
    public partial class RegisterForm : Form
    {
        /// <summary>
        /// 适配器
        /// </summary>
        private IActionAdapter adapter = AdapterFactory.GetAdapter();

        /// <summary>
        /// 构造器
        /// </summary>
        public RegisterForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 按钮：取消
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 按钮：确定
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            // 正确性验证
            if (this.textBox1.Text.Trim() == String.Empty ||
                this.textBox2.Text.Trim() == String.Empty ||
                this.textBox3.Text.Trim() == String.Empty)
            {
                MessageBox.Show("请完整填写");
                return;
            }
            // 用户名存在性检验
            Dictionary<string, string> descDict = new Dictionary<string, string>();
            this.adapter.RetrieveUser(this.textBox1.Text, out descDict);
            if (descDict != null)
            {
                MessageBox.Show("该用户名已经被占用了，请更换");
                this.textBox1.Focus();
                return;
            }
            // 提交更改
            Entity.User user = null;
            descDict = new Dictionary<string, string>();
            descDict["phone"] = this.textBox3.Text.Trim();
            descDict["password"] = this.textBox2.Text;
            bool aflag = this.adapter.AddUser(this.textBox1.Text.Trim(), Enums.UserType.Customer, descDict, out user);
            MessageBox.Show("创建账户成功！");
            // 返回登陆界面
            ((LoginForm)this.Owner).textBox1.Text = user.UserName;
            ((LoginForm)this.Owner).textBox2.Text = String.Empty;
            this.Close();
        }
    }
}
