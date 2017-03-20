using System;
using System.Windows.Forms;

namespace TinyMSGW.Forms
{
    /// <summary>
    /// 窗体：设置归还期限
    /// </summary>
    public partial class ReturnDaySettingForm : Form
    {
        /// <summary>
        /// 构造器
        /// </summary>
        public ReturnDaySettingForm()
        {
            InitializeComponent();
            // 渲染控件
            this.numericUpDown1.Value = GlobalDataPackage.ReturnDaySpan;
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
            // 更新GDP
            GlobalDataPackage.ReturnDaySpan = (int)this.numericUpDown1.Value;
            // 写到稳定储存器
            Utils.LogUtil.Log("ACK: Return day span changed to: " + GlobalDataPackage.DelayFeeADay.ToString() + " Days.");
            ViewModel.SettingManager.WriteSettingToStable();
            this.Close();
        }
    }
}
