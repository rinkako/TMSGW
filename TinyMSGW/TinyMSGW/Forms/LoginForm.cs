using System;
using System.Windows.Forms;
using TinyMSGW.Adapter;

namespace TinyMSGW.Forms
{
    /// <summary>
    /// 窗体：登录
    /// </summary>
    public partial class LoginForm : Form
    {
        /// <summary>
        /// 适配器
        /// </summary>
        IActionAdapter adapter = AdapterFactory.GetAdapter();

        /// <summary>
        /// 构造器
        /// </summary>
        public LoginForm()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// 按钮：取消
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            this.adapter.Terminate();
        }

        /// <summary>
        /// 按钮：登录
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            // 登录验证
            bool allowFlag;
            bool linkFlag = this.adapter.LoginValid(this.textBox1.Text, Utils.CommonUtil.EncryptToSHA1(this.textBox2.Text), out allowFlag);
            if (linkFlag == false)
            {
                MessageBox.Show("无法连接到检验服务器或本地检验文件失效", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (allowFlag == false)
            {
                MessageBox.Show("用户名或密码不正确", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // 登陆成功
            this.adapter.LoginSuccess(this.textBox1.Text);
            // 跳转到主页面
            Forms.MainForm mf = new MainForm();
            mf.ReSizeWindowByType();
            mf.Show(this);
            this.Hide();
        }

        /// <summary>
        /// 按钮：注册
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            RegisterForm rf = new RegisterForm();
            rf.ShowDialog(this);
        }

        /// <summary>
        /// 事件：用户名输入改动
        /// </summary>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.textBox2.Text = String.Empty;
        }
    }
}
