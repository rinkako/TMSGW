using System;
using System.ComponentModel;
using TinyMSGW.Enums;

namespace TinyMSGW.Entity
{
    /// <summary>
    /// 实体类：用户
    /// </summary>
    [Serializable]
    internal sealed class User
    {
        /// <summary>
        /// 获取或设置用户ID
        /// </summary>
        [DefaultValue(-1)]
        public int UserID { get; set; }

        /// <summary>
        /// 获取或设置用户名
        /// </summary>
        [DefaultValue("")]
        public string UserName { get; set; }

        /// <summary>
        /// 获取或设置用户密码SHA1字符串
        /// </summary>
        [DefaultValue("")]
        public string UserPasswordSHA1 { get; set; }

        /// <summary>
        /// 获取或设置手机
        /// </summary>
        [DefaultValue("")]
        public string UserPhone { get; set; }

        /// <summary>
        /// 获取或设置用户类型
        /// </summary>
        [DefaultValue(UserType.Customer)]
        public UserType Type { get; set; }

        /// <summary>
        /// 获取或设置用户是否有效（可登陆）
        /// </summary>
        [DefaultValue(true)]
        public bool UserValid { get; set; }

        /// <summary>
        /// 获取或设置用户借书卡
        /// </summary>
        [DefaultValue(null)]
        public Usercard Card { get; set; }

        /// <summary>
        /// 获取或设置用户借书卡ID
        /// </summary>
        [DefaultValue(-1)]
        public int CardID { get; set; }
    }


}
