using System;
using System.ComponentModel;

namespace TinyMSGW.Entity
{
    /// <summary>
    /// 实体类：库存书本
    /// </summary>
    [Serializable]
    internal sealed class StoringBook
    {
        /// <summary>
        /// 获取或设置书籍的ISBN编号
        /// </summary>
        [DefaultValue("")]
        public string ISBN { get; set; }

        /// <summary>
        /// 获取或设置书籍名称
        /// </summary>
        [DefaultValue("")]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置书籍作者
        /// </summary>
        [DefaultValue("")]
        public string Author { get; set; }

        /// <summary>
        /// 获取或设置书籍类型
        /// </summary>
        [DefaultValue("")]
        public string Type { get; set; }

        /// <summary>
        /// 获取或设置库存仓库的ID
        /// </summary>
        [DefaultValue(0)]
        public int WarehouseID { get; set; }

        /// <summary>
        /// 获取或设置库存数量
        /// </summary>
        [DefaultValue(0)]
        public int NumberOfWarehouse { get; set; }

        /// <summary>
        /// 获取或设置发布年度
        /// </summary>
        [DefaultValue(0)]
        public int PublishYear { get; set; }
    }
}
