using System;
using TinyMSGW.Enum;
using TinyMSGW.Entity;

namespace TinyMSGW
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
        /// 还书期限天数
        /// </summary>
        public static int ReturnDaySpan = 30;

        /// <summary>
        /// 延迟归还书籍的每日滞纳费用
        /// </summary>
        public static double DelayFeeADay = 0.5;

        /// <summary>
        /// 当前使用系统的用户
        /// </summary>
        public static User CurrentUser = null;

        #region 常数
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
        #endregion

        #region 全局计数器
        /// <summary>
        /// 全局计数器：书本唯一编号
        /// </summary>
        public static int GlobalCounterBookID = 0;

        /// <summary>
        /// 全局计数器：借书记录编号
        /// </summary>
        public static int GlobalCounterRentLogID = 0;

        /// <summary>
        /// 全局计数器：未上架仓库图书编号
        /// </summary>
        public static int GlobalCounterStoringBookID = 0;

        /// <summary>
        /// 全局计数器：用户编号
        /// </summary>
        public static int GlobalCounterUserID = 0;

        /// <summary>
        /// 全局计数器：借书卡编号
        /// </summary>
        public static int GlobalCounterUsercardID = 0;

        /// <summary>
        /// 全局计数器：仓库编号
        /// </summary>
        public static int GlobalCounterWarehouseID = 0;
        #endregion
    }
}
