using System;
using System.IO;
using System.Text;
using TinyMSGW.ViewModel;

namespace TinyMSGW.Utils
{
    /// <summary>
    /// 辅助组件：处理日志和通知的类
    /// </summary>
    internal static class LogUtil
    {
        /// <summary>
        /// 构造器
        /// </summary>
        static LogUtil()
        {
            // 没有日志就建立
            if (File.Exists(LocalIOUtil.ParseURItoURL(GlobalDataPackage.LogFileName, true)) == false)
            {
                FileStream fs = new FileStream(LocalIOUtil.ParseURItoURL(GlobalDataPackage.LogFileName, true), FileMode.CreateNew);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(GlobalDataPackage.LogAloha);
                sw.Close();
                fs.Close();
            }
        }

        /// <summary>
        /// 书写日志，并确定是否需要发送通知
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="toAlert">是否需要发送通知</param>
        public static void Log(string msg, bool toAlert = true)
        {
            // 发通知
            if (toAlert)
            {
                LogUtil.Alert(msg);
            }
            // 写日志
            try
            {
                FileStream fs = new FileStream(LocalIOUtil.ParseURItoURL(GlobalDataPackage.LogFileName, true), FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(DateTime.Now.ToString() + "> " + msg);
                sw.Close();
                fs.Close();
            }
            catch (Exception e)
            {
                LogUtil.Alert(e.ToString());
                throw e;
            }
        }

        /// <summary>
        /// 发送通知
        /// </summary>
        /// <param name="msg">消息</param>
        public static void Alert(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
