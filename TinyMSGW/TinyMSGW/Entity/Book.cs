using System;
using System.ComponentModel;

namespace TinyMSGW.Entity
{
    /// <summary>
    /// 实体类：书籍
    /// </summary>
    [Serializable]
    internal sealed class Book
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
        /// 获取或设置发布年度
        /// </summary>
        [DefaultValue(0)]
        public int PublishYear { get; set; }

        /// <summary>
        /// 获取或设置目前书籍所在图书馆的位置
        /// </summary>
        [DefaultValue("")]
        public string LocationOfLibrary { get; set; }

        /// <summary>
        /// 获取或设置书籍在图书馆中的数量
        /// </summary>
        [DefaultValue(0)]
        public int NumberInLibrary { get; set; }

        /// <summary>
        /// 获取或设置书籍正在借出中的数量
        /// </summary>
        [DefaultValue(0)]
        public int NumberInRenting { get; set; }

        /// <summary>
        /// 获取或设置书籍首次上架的时间戳
        /// </summary>
        [DefaultValue(0)]
        public DateTime StoreIntoLibraryTimestamp { get; set; }

        /// <summary>
        /// 获取或设置书籍是否已经被注销掉不再考虑
        /// </summary>
        [DefaultValue(false)]
        public bool IsRemoved { get; set; }

        /// <summary>
        /// 获取书籍是否已经上架
        /// </summary>
        public bool IsInLibrary
        {
            get
            {
                return this.NumberInLibrary != 0 || this.NumberInRenting != 0;
            }
        }
    }
}
