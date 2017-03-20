using System;
using System.Windows.Forms;

namespace TinyMSGW
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Forms.LoginForm());
            // 初次运行则要弹出设置窗体
            if (ViewModel.SettingManager.IsFirstTimeRun())
            {
                Application.Run(new Forms.FirstForm());
            }
            // 否则，加载程序设置，并进入登录窗口
            else
            {
                ViewModel.SettingManager.ReadSettingToGDP();
                if (ViewModel.SettingManager.ReadDataToMemory() == false)
                {
                    MessageBox.Show("无法连接到服务器、服务器数据库不匹配或者本地数据文件已损坏，请重新配置系统！");
                    Environment.Exit(0);
                }
                Adapter.AdapterFactory.InitAdapter();
                Application.Run(new Forms.LoginForm());
            }
        }
    }
}
