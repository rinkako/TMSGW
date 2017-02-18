using System;
using System.Windows.Forms;

namespace TinyMSGW.Forms
{
    /// <summary>
    /// 窗体：系统设置
    /// </summary>
    public partial class OtherSettingForm : Form
    {
        /// <summary>
        /// 构造器
        /// </summary>
        public OtherSettingForm()
        {
            InitializeComponent();
            // 渲染控件
            this.textBox2.Text = GlobalDataPackage.DBName;
            this.textBox3.Text = GlobalDataPackage.DBUsername;
            this.textBox4.Text = GlobalDataPackage.DBPassword;
            var ipItems = GlobalDataPackage.DBServerIPAddress.Split('.');
            this.numericUpDown1.Value = Convert.ToInt32(ipItems[0]);
            this.numericUpDown2.Value = Convert.ToInt32(ipItems[1]);
            this.numericUpDown3.Value = Convert.ToInt32(ipItems[2]);
            this.numericUpDown4.Value = Convert.ToInt32(ipItems[3]);
        }

        /// <summary>
        /// 按钮：确定
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            // 正确性检测
            if (this.textBox2.Text.Trim() == String.Empty ||
                this.textBox3.Text.Trim() == String.Empty ||
                this.textBox4.Text.Trim() == String.Empty)
            {
                MessageBox.Show("请完整填写！");
                return;
            }
            // 更新到GDP
            GlobalDataPackage.DBName = this.textBox2.Text;
            GlobalDataPackage.DBUsername = this.textBox3.Text;
            GlobalDataPackage.DBPassword = this.textBox4.Text;
            GlobalDataPackage.DBServerIPAddress = String.Format("{0}.{1}.{2}.{3}",
                this.numericUpDown1.Value, this.numericUpDown2.Value, this.numericUpDown3.Value, this.numericUpDown4.Value);
            // 写稳定储存器
            ViewModel.SettingManager.WriteSettingToStable();
            Utils.LogUtil.Log("ACK: Changed System Settings.");
            this.Close();
        }
    }
}
