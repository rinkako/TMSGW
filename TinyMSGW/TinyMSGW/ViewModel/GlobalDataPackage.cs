using System;
using TinyMSGW.Enum;

namespace TinyMSGW.ViewModel
{
    /// <summary>
    /// 全局信息类：为程序全局提供公共数据的包装和读写
    /// </summary>
    internal static class GlobalDataPackage
    {
        /// <summary>
        /// 软件运行模式
        /// </summary>
        public static RunTypeEnum RunType;

        /// <summary>
        /// 联机模式下数据库服务器IP地址
        /// </summary>
        public static string DBServerIPAddress;

        /// <summary>
        /// 联机模式下数据库连接用户名
        /// </summary>
        public static string DBUsername;

        /// <summary>
        /// 联机模式下数据库连接密码
        /// </summary>
        public static string DBPassword;

        /// <summary>
        /// 联机模式下数据库名
        /// </summary>
        public static string DBName;

        /// <summary>
        /// 系统管理员用户名
        /// </summary>
        public static string AdminName;

        /// <summary>
        /// 系统管理员密码的SHA1
        /// </summary>
        public static string AdminPasswordSHA1;

        /// <summary>
        /// 本地日志的首行
        /// </summary>
        public static readonly string LogHello = "===BEGIN_OF_LOG===";

        /// <summary>
        /// 本地日志文件名
        /// </summary>
        public static readonly string LogFileName = "Log.txt";

        /// <summary>
        /// 本地数据文件名
        /// </summary>
        public static readonly string DataFileName = "Data.dat";

        /// <summary>
        /// 本地配置文件名
        /// </summary>
        public static readonly string SettingFileName = "Setting.dat";
    }
}
