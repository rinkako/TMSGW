using System;
using System.Collections.Generic;

namespace TinyMSGW.Entity
{
    /// <summary>
    /// 实体类：图书馆
    /// 即展示在客户可以借书和阅读的地方
    /// 它是唯一的，仓库作为它的附属
    /// </summary>
    [Serializable]
    internal sealed class Library
    {
        /// <summary>
        /// 获取或设置书店名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 获取图书馆中书的列表
        /// </summary>
        public List<Book> OnShopBookList { get; private set; }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        public List<User> UserList { get; private set; }

        /// <summary>
        /// 获取仓库列表
        /// </summary>
        public List<Warehouse> WarehouseList { get; private set; }
    }
}
