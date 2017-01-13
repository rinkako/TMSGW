using System;
using System.Runtime.InteropServices;
using System.Text;

namespace TinyMSGW.Utils
{
    /// <summary>
    /// 辅助组件：处理INI配置的类
    /// </summary>
    public class IniUtil
    {
        /// <summary>
        /// 引用外部的INI库函数
        /// </summary>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="INIPath">要读写文件的位置</param>
        public IniUtil(string INIPath)
        {
            path = INIPath;
        }
        
        /// <summary>
        /// 写INI文件
        /// </summary>
        /// <param name="Section">配置所在小节</param>
        /// <param name="Key">配置的键</param>
        /// <param name="Value">配置的键值</param>
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }

        /// <summary>
        /// 读INI文件
        /// </summary>
        /// <param name="Section">配置所在小节</param>
        /// <param name="Key">配置的键</param>
        /// <returns>配置的键值</returns>
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(256);
            int i = GetPrivateProfileString(Section, Key, "", temp, 256, this.path);
            return temp.ToString();
        }

        /// <summary>
        /// 要读写文件的路径
        /// </summary>
        private string path;
    }
}
