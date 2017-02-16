using System;
using System.IO;
using System.Collections.Generic;
using TinyMSGW.Utils;
using TinyMSGW.Entity;

namespace TinyMSGW.ViewModel
{
    /// <summary>
    /// 配置控制器类：负责加载程序数据，确定启动类型和设置的管理
    /// </summary>
    internal static class SettingManager
    {
        /// <summary>
        /// 检查软件是否是第一次运行，通过检查配置文件是否存在
        /// </summary>
        /// <returns>是否首次运行</returns>
        public static bool IsFirstTimeRun()
        {
            return File.Exists(LocalIOUtil.ParseURItoURL(GlobalDataPackage.SettingFileName, true)) == false;
        }

        /// <summary>
        /// 为首次运行程序书写配置文件
        /// </summary>
        /// <param name="isLocal">是否本地模式</param>
        /// <param name="paras">参数列表</param>
        /// <returns>操作成功与否</returns>
        public static bool InitFirstTimeSettings(bool isLocal, Dictionary<string, string> paras)
        {
            try
            {
                IniUtil ini = new IniUtil(LocalIOUtil.ParseURItoURL(GlobalDataPackage.SettingFileName, true));
                if (isLocal == true)
                {
                    ini.IniWriteValue("MSGW", "RunType", "LOCAL");
                    ini.IniWriteValue("MSGW", "DBServerIPAddress", "127.0.0.1");
                    ini.IniWriteValue("MSGW", "DBName", String.Empty);
                    ini.IniWriteValue("MSGW", "DBUsername", String.Empty);
                    ini.IniWriteValue("MSGW", "DBUsername", String.Empty);
                    // 密码前后加两个#号来防止前后有空格被消掉
                    ini.IniWriteValue("MSGW", "DBPassword", String.Format("#{0}#", String.Empty));
                }
                else
                {
                    ini.IniWriteValue("MSGW", "RunType", "ONLINE");
                    ini.IniWriteValue("MSGW", "DBServerIPAddress", paras["DBServerIPAddress"]);
                    ini.IniWriteValue("MSGW", "DBName", paras["DBName"]);
                    ini.IniWriteValue("MSGW", "DBUsername", paras["DBUsername"]);
                    ini.IniWriteValue("MSGW", "DBUsername", paras["DBUsername"]);
                    // 密码前后加两个#号来防止前后有空格被消掉
                    ini.IniWriteValue("MSGW", "DBPassword", String.Format("#{0}#", paras["DBPassword"]));
                }
                ini.IniWriteValue("MSGW", "GlobalCounterBookID", "0");
                ini.IniWriteValue("MSGW", "GlobalCounterRentLogID", "0");
                ini.IniWriteValue("MSGW", "GlobalCounterStoringBookID", "0");
                ini.IniWriteValue("MSGW", "GlobalCounterUsercardID", "0");
                ini.IniWriteValue("MSGW", "GlobalCounterUserID", "0");
                ini.IniWriteValue("MSGW", "GlobalCounterWarehouseID", "0");
                ini.IniWriteValue("MSGW", "ReturnDaySpan", "0");
                ini.IniWriteValue("MSGW", "DelayFeeADay", "0");
                ini.IniWriteValue("MSGW", "AdminName", paras["AdminName"]);
                ini.IniWriteValue("MSGW", "AdminPasswordSHA1", paras["AdminPasswordSHA1"]);
                return SettingManager.InitDataFile(paras["LibraryName"]);
            }
            catch (Exception e)
            {
                LogUtil.Log("ERROR: unable to initialize settings of system. " + e.ToString());
            }
            return false;
        }

        /// <summary>
        /// 初始化数据文件
        /// </summary>
        /// <param name="libraryName">图书馆名称</param>
        /// <returns>操作成功与否</returns>
        public static bool InitDataFile(string libraryName)
        {
            try
            {
                Library lb = new Library()
                {
                    Name = libraryName
                };
                ViewModel.LibraryManager.GetInstance().SetLibraryInstance(lb);
                return LocalIOUtil.Serialization(lb, LocalIOUtil.ParseURItoURL(GlobalDataPackage.DataFileName, true));
            }
            catch (Exception e)
            {
                LogUtil.Log("ERROR: unable to initialize data of system. " + e.ToString());
                return false;
            }
        }

        /// <summary>
        /// 将设置读取到GlobalDataPackage中
        /// </summary>
        public static void ReadSettingToGDP()
        {
            try
            {
                IniUtil ini = new IniUtil(LocalIOUtil.ParseURItoURL(GlobalDataPackage.SettingFileName, true));
                GlobalDataPackage.RunType = ini.IniReadValue("MSGW", "RunType") == "LOCAL" ? Enums.RunTypeEnum.Local : Enums.RunTypeEnum.Online;
                GlobalDataPackage.AdminName = ini.IniReadValue("MSGW", "AdminName");
                GlobalDataPackage.AdminPasswordSHA1 = ini.IniReadValue("MSGW", "AdminPasswordSHA1");
                GlobalDataPackage.ReturnDaySpan = Convert.ToInt32(ini.IniReadValue("MSGW", "ReturnDaySpan"));
                GlobalDataPackage.DelayFeeADay = Convert.ToDouble(ini.IniReadValue("MSGW", "DelayFeeADay"));
                GlobalDataPackage.GlobalCounterBookID = Convert.ToInt32(ini.IniReadValue("MSGW", "GlobalCounterBookID"));
                GlobalDataPackage.GlobalCounterRentLogID = Convert.ToInt32(ini.IniReadValue("MSGW", "GlobalCounterRentLogID"));
                GlobalDataPackage.GlobalCounterStoringBookID = Convert.ToInt32(ini.IniReadValue("MSGW", "GlobalCounterStoringBookID"));
                GlobalDataPackage.GlobalCounterUsercardID = Convert.ToInt32(ini.IniReadValue("MSGW", "GlobalCounterUsercardID"));
                GlobalDataPackage.GlobalCounterUserID = Convert.ToInt32(ini.IniReadValue("MSGW", "GlobalCounterUserID"));
                GlobalDataPackage.GlobalCounterWarehouseID = Convert.ToInt32(ini.IniReadValue("MSGW", "GlobalCounterWarehouseID"));
                // 在线模式才需要加载服务器信息
                if (GlobalDataPackage.RunType == Enums.RunTypeEnum.Online)
                {
                    GlobalDataPackage.DBServerIPAddress = ini.IniReadValue("MSGW", "DBServerIPAddress");
                    GlobalDataPackage.DBName = ini.IniReadValue("MSGW", "DBName");
                    GlobalDataPackage.DBUsername = ini.IniReadValue("MSGW", "DBUsername");
                    var dbpass = ini.IniReadValue("MSGW", "DBPassword");
                    // 从前后两个#号中取出密码，避免前后有空格被消除
                    GlobalDataPackage.DBPassword = dbpass.Substring(1, dbpass.Length - 1);
                }
                // 初始化适配器
                Adapter.AdapterFactory.InitAdapter();
                LogUtil.Log("ACK: Read Settings to GDP successfully");
            }
            catch (Exception e)
            {
                LogUtil.Log("ERROR: unable to read settings to GDP. " + e.ToString());
            }
        }

        /// <summary>
        /// 将GDP数据写到稳定储存器中
        /// </summary>
        public static void WriteSettingToStable()
        {
            try
            {
                IniUtil ini = new IniUtil(LocalIOUtil.ParseURItoURL(GlobalDataPackage.SettingFileName, true));
                if (GlobalDataPackage.RunType == Enums.RunTypeEnum.Local)
                {
                    ini.IniWriteValue("MSGW", "RunType", "LOCAL");
                }
                else
                {
                    ini.IniWriteValue("MSGW", "RunType", "ONLINE");
                }
                ini.IniWriteValue("MSGW", "AdminName", GlobalDataPackage.AdminName);
                ini.IniWriteValue("MSGW", "AdminPasswordSHA1", GlobalDataPackage.AdminPasswordSHA1);
                ini.IniWriteValue("MSGW", "DBServerIPAddress", GlobalDataPackage.DBServerIPAddress);
                ini.IniWriteValue("MSGW", "DBName", GlobalDataPackage.DBName);
                ini.IniWriteValue("MSGW", "DBUsername", GlobalDataPackage.DBUsername);
                ini.IniWriteValue("MSGW", "ReturnDaySpan", GlobalDataPackage.ReturnDaySpan.ToString());
                ini.IniWriteValue("MSGW", "DelayFeeADay", GlobalDataPackage.DelayFeeADay.ToString());
                ini.IniWriteValue("MSGW", "GlobalCounterBookID", GlobalDataPackage.GlobalCounterBookID.ToString());
                ini.IniWriteValue("MSGW", "GlobalCounterRentLogID", GlobalDataPackage.GlobalCounterRentLogID.ToString());
                ini.IniWriteValue("MSGW", "GlobalCounterStoringBookID", GlobalDataPackage.GlobalCounterStoringBookID.ToString());
                ini.IniWriteValue("MSGW", "GlobalCounterUsercardID", GlobalDataPackage.GlobalCounterUsercardID.ToString());
                ini.IniWriteValue("MSGW", "GlobalCounterUserID", GlobalDataPackage.GlobalCounterUserID.ToString());
                ini.IniWriteValue("MSGW", "GlobalCounterWarehouseID", GlobalDataPackage.GlobalCounterWarehouseID.ToString());
                // 密码前后加两个#号来防止前后有空格被消掉
                ini.IniWriteValue("MSGW", "DBPassword", String.Format("#{0}#", GlobalDataPackage.DBPassword));
            }
            catch (Exception e)
            {
                LogUtil.Log("ERROR: unable to write settings to stable. " + e.ToString());
            }
        }

        /// <summary>
        /// 将稳定储存器中的数据读取到内存中
        /// </summary>
        /// <returns>操作成功与否</returns>
        public static bool ReadDataToMemory()
        {
            // 单机模式的数据读取直接在磁盘上进行
            if (GlobalDataPackage.RunType == Enums.RunTypeEnum.Local)
            {
                // 此刻肯定是首次初始化完毕的了，因此不应该没有data文件
                if (File.Exists(LocalIOUtil.ParseURItoURL(GlobalDataPackage.DataFileName, true)) == false)
                {
                    LogUtil.Log("ERROR: Missing Data File.");
                    return false;
                }
                // 反序列化
                try {
                    var uObj = (Library)LocalIOUtil.Unserialization(LocalIOUtil.ParseURItoURL(GlobalDataPackage.DataFileName, true));
                    if (LibraryManager.GetInstance().SetLibraryInstance(uObj))
                    {
                        LogUtil.Log("WARNING: Rewrite Memory Instance: Library");
                    }
                    return true;
                }
                catch (Exception e)
                {
                    LogUtil.Log("ERROR: Unable to unserialize data file. " + e.ToString());
                    return false;
                }
            }
            // 联机模式的数据不需要直接读取，只要验证对数据库的连接是否成立
            else
            {
                // TODO： 检测连接
                return true;
            }
        }
    }
}
