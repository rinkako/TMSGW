using System;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace TinyMSGW.Utils
{
    /// <summary>
    /// 辅助组件：为系统提供通用的公共辅助函数
    /// </summary>
    internal static class CommonUtil
    {
        /// <summary>
        /// 测试一个字符串是否满足一个正则式
        /// </summary>
        /// <param name="parStr">待校验字符串</param>
        /// <param name="regEx">正则表达式</param>
        /// <returns>正则式真值</returns>
        public static bool IsMatchRegEx(string parStr, string regEx)
        {
            Regex myRegex = new Regex(regEx);
            return myRegex.IsMatch(parStr);
        }

        /// <summary>
        /// 将一个字符串转化为对应的SHA1串
        /// </summary>
        /// <param name="parStr">要处理的字符串</param>
        /// <returns>对应原字串的SHA1串</returns>
        public static string EncryptToSHA1(string parStr)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] str1 = Encoding.UTF8.GetBytes(parStr);
            byte[] str2 = sha1.ComputeHash(str1);
            sha1.Clear();
            (sha1 as IDisposable).Dispose();
            return Convert.ToBase64String(str2);
        }

        /// <summary>
        /// 将用户类型枚举转化为用户可理解字符串
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <returns>用户可理解文本</returns>
        public static string ParseUserETypeToCString(Enums.UserType type)
        {
            switch (type)
            {
                case Enums.UserType.Admin:
                    return "管理员";
                case Enums.UserType.Keeper:
                    return "仓库管理人";
                case Enums.UserType.Librarian:
                    return "图书馆柜员";
                default:
                    return "";
            }
        }
    }
}
