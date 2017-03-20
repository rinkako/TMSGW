using System;
using System.Windows.Forms;

namespace TinyMSGW.Forms
{
    /// <summary>
    /// 窗体：修改滞纳金
    /// </summary>
    public partial class DelayFeeSettingForm : Form
    {
        /// <summary>
        /// 构造器
        /// </summary>
        public DelayFeeSettingForm()
        {
            InitializeComponent();
            // 渲染控件
            this.numericUpDown1.Value = (int)(GlobalDataPackage.DelayFeeADay * 10);
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
            GlobalDataPackage.DelayFeeADay = (double)this.numericUpDown1.Value / 10.0;
            // 写到稳定储存器
            Utils.LogUtil.Log("ACK: DelayFee Changed To: " + GlobalDataPackage.DelayFeeADay.ToString() + " Yuan.");
            ViewModel.SettingManager.WriteSettingToStable();
            this.Close();
        }
    }
}
