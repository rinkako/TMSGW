using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace TinyMSGW.Utils
{
    /// <summary>
    /// 辅助组件：本地文件操作类
    /// </summary>
    internal static class LocalIOUtil
    {
        /// <summary>
        /// 把一个相对URI转化为绝对路径
        /// </summary>
        /// <param name="uri">相对程序运行目录的相对路径</param>
        /// <param name="addSlash">是否加上一个反斜杠</param>
        /// <returns>绝对路径</returns>
        public static string ParseURItoURL(string uri, bool addSlash)
        {
            if (addSlash)
            {
                return System.AppDomain.CurrentDomain.BaseDirectory + "\\" + uri;
            }
            else
            {
                return System.AppDomain.CurrentDomain.BaseDirectory + uri;
            }
        }

        /// <summary>
        /// 把字符串用反斜杠组合成Windows风格的路径字符串
        /// </summary>
        /// <param name="uriObj">路径项目</param>
        /// <returns>组合完毕的路径字符串</returns>
        public static string JoinPath(params string[] uriObj)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < uriObj.Length - 1; i++)
            {
                sb.Append(uriObj[i] + "\\");
            }
            sb.Append(uriObj.Last());
            return sb.ToString();
        }

        /// <summary>
        /// 把一个实例序列化
        /// </summary>
        /// <param name="instance">类的实例</param>
        /// <param name="savePath">保存路径</param>
        /// <returns>操作成功与否</returns>
        public static bool Serialization(object instance, string savePath)
        {
            try
            {
                Stream myStream = File.Open(savePath, FileMode.Create);
                if (myStream == null)
                {
                    throw new IOException();
                }
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(myStream, instance);
                myStream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        /// <summary>
        /// 把二进制文件反序列化
        /// </summary>
        /// <param name="loadPath">二进制文件路径</param>
        /// <returns>类的实例</returns>
        public static object Unserialization(string loadPath)
        {
            try
            {
                Stream s = File.Open(loadPath, FileMode.Open);
                if (s == null)
                {
                    throw new IOException();
                }
                BinaryFormatter bf = new BinaryFormatter();
                object ob = bf.Deserialize(s);
                s.Close();
                return ob;
            }
            catch
            {
                return null;
            }
        }
    }
}
