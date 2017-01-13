using System;
using System.IO;
using System.Collections.Generic;
using TinyMSGW.Utils;
using TinyMSGW.Entity;

namespace TinyMSGW.ViewModel
{
    /// <summary>
    /// 初始化控制器类：负责加载程序数据，确定启动类型和验证登陆
    /// </summary>
    internal static class InitManager
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
                }
                else
                {
                    ini.IniWriteValue("MSGW", "RunType", "ONLINE");
                    ini.IniWriteValue("MSGW", "DBServerIPAddress", paras["DBServerIPAddress"]);
                    ini.IniWriteValue("MSGW", "DBName", paras["DBName"]);
                    ini.IniWriteValue("MSGW", "DBUsername", paras["DBUsername"]);
                    // 密码前后加两个#号来防止前后有空格被消掉
                    ini.IniWriteValue("MSGW", "DBPassword", String.Format("#{0}#", paras["DBPassword"]));
                }
                ini.IniWriteValue("MSGW", "AdminName", paras["AdminName"]);
                ini.IniWriteValue("MSGW", "AdminPasswordSHA1", paras["AdminPasswordSHA1"]);
                return InitManager.InitDataFile(paras["LibraryName"]);
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
                GlobalDataPackage.RunType = ini.IniReadValue("MSGW", "RunType") == "LOCAL" ? Enum.RunTypeEnum.Local : Enum.RunTypeEnum.Online;
                GlobalDataPackage.AdminName = ini.IniReadValue("MSGW", "AdminName");
                GlobalDataPackage.AdminPasswordSHA1 = ini.IniReadValue("MSGW", "AdminPasswordSHA1");
                // 在线模式才需要加载服务器信息
                if (GlobalDataPackage.RunType == Enum.RunTypeEnum.Online)
                {
                    GlobalDataPackage.DBServerIPAddress = ini.IniReadValue("MSGW", "DBServerIPAddress");
                    GlobalDataPackage.DBName = ini.IniReadValue("MSGW", "DBName");
                    GlobalDataPackage.DBUsername = ini.IniReadValue("MSGW", "DBUsername");
                    var dbpass = ini.IniReadValue("MSGW", "DBPassword");
                    // 从前后两个#号中取出密码，避免前后有空格被消除
                    GlobalDataPackage.DBPassword = dbpass.Substring(1, dbpass.Length - 1);
                }
                LogUtil.Log("ACK: Read Settings to GDP successfully");
            }
            catch (Exception e)
            {
                LogUtil.Log("ERROR: unable to read settings to GDP. " + e.ToString());
            }
        }

        /// <summary>
        /// 将稳定储存器中的数据读取到内存中
        /// </summary>
        /// <returns>操作成功与否</returns>
        public static bool ReadDataToMemory()
        {
            // 单机模式的数据读取直接在磁盘上进行
            if (GlobalDataPackage.RunType == Enum.RunTypeEnum.Local)
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
                return DBUtil.CheckLinking();
            }
        }
        
    }
}
