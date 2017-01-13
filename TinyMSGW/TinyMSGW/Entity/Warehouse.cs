using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace TinyMSGW.Entity
{
    /// <summary>
    /// 实体类：仓库
    /// </summary>
    [Serializable]
    internal sealed class Warehouse
    {
        /// <summary>
        /// 构造器
        /// </summary>
        public Warehouse()
        {
            this.KeeperList = new List<User>();
            this.StoringList = new List<StoringBook>();
        }

        /// <summary>
        /// 获取或设置仓库ID
        /// </summary>
        [DefaultValue(0)]
        public int WarehouseID { get; set; }

        /// <summary>
        /// 获取或设置仓库名
        /// </summary>
        [DefaultValue("")]
        public string Name { get; set; }

        /// <summary>
        /// 获取仓库管理员列表
        /// </summary>
        public List<User> KeeperList { get; private set; }

        /// <summary>
        /// 获取仓库管理员列表
        /// </summary>
        public List<StoringBook> StoringList { get; private set; }
    }
}
