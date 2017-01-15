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
            // 初次运行则要弹出设置窗体
            if (ViewModel.SettingManager.IsFirstTimeRun())
            {
                Application.Run(new Forms.FirstForm());
            }
            // 否则，加载程序设置，并进入登录窗口
            else
            {
                ViewModel.SettingManager.ReadSettingToGDP();
                ViewModel.SettingManager.ReadDataToMemory();
                Application.Run(new Forms.LoginForm());
            }
        }
    }
}
