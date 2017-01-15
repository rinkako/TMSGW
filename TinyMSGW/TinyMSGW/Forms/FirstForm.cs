using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using TinyMSGW.Utils;
using TinyMSGW.ViewModel;

namespace TinyMSGW.Forms
{
    /// <summary>
    /// 窗体：首次使用
    /// </summary>
    public partial class FirstForm : Form
    {
        /// <summary>
        /// 构造器
        /// </summary>
        public FirstForm()
        {
            InitializeComponent();
            this.Height = 272;
        }

        /// <summary>
        /// Radiobutton逻辑
        /// </summary>
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.groupBox1.Visible = this.radioButton2.Checked;
            this.Height = this.radioButton2.Checked ? 444 : 272;
        }

        /// <summary>
        /// 按钮：确定
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            // 检查管理员账户
            if (CommonUtil.IsMatchRegEx(this.textBox6.Text, "^[A-Za-z]+$") == false ||
                    this.textBox5.Text.Trim() == String.Empty)
            {
                MessageBox.Show("不合法的用户名或密码" + Environment.NewLine +
                    "请保证用户名只含有英文字母，且密码非空");
                return;
            }
            // 检查图书馆名称
            if (this.textBox1.Text.Trim() == String.Empty)
            {
                MessageBox.Show("请输入图书馆名称");
                return;
            }
            Dictionary<string, string> paraDict = new Dictionary<string, string>();
            paraDict.Add("LibraryName", this.textBox1.Text.Trim());
            paraDict.Add("AdminName", this.textBox5.Text.Trim());
            paraDict.Add("AdminPasswordSHA1", CommonUtil.EncryptToSHA1(this.textBox6.Text));
            // 选择单机
            if (this.radioButton1.Checked)
            {
                SettingManager.InitFirstTimeSettings(true, paraDict);
            }
            // 选择联机
            else
            {
                // 检查输入的合法性
                if (this.textBox2.Text.Trim() == String.Empty ||
                    this.textBox3.Text.Trim() == String.Empty ||
                    this.textBox4.Text == String.Empty)
                {
                    MessageBox.Show("不合法的服务器配置" + Environment.NewLine +
                        "请保证需要填写的项目非空");
                    return;
                }
                // 封装参数
                string ipadr = String.Format("{0}.{1}.{2}.{3}",
                    this.numericUpDown1.Value, this.numericUpDown2.Value, this.numericUpDown3.Value, this.numericUpDown4.Value);
                paraDict.Add("DBServerIPAddress", ipadr);
                paraDict.Add("DBName", this.textBox2.Text.Trim());
                paraDict.Add("DBUsername", this.textBox3.Text.Trim());
                paraDict.Add("DBPassword", this.textBox4.Text);
                SettingManager.InitFirstTimeSettings(false, paraDict);
            }
        }
    }
}
