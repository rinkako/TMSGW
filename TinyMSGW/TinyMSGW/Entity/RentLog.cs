using System;
using System.ComponentModel;

namespace TinyMSGW.Entity
{
    /// <summary>
    /// 实体类：借书记录
    /// </summary>
    [Serializable]
    internal sealed class RentLog
    {
        /// <summary>
        /// 执行还书动作
        /// </summary>
        public void Return(DateTime? returnDate = null)
        {
            this.ActualReturnTimestamp = returnDate == null ? DateTime.Now : returnDate;
        }

        /// <summary>
        /// 获取或设置借书记录ID
        /// </summary>
        [DefaultValue(0)]
        public int RentID { get; set; }

        /// <summary>
        /// 获取或设置借书人的借书卡ID
        /// </summary>
        [DefaultValue(-1)]
        public int BorrowUsercardID { get; set; }

        /// <summary>
        /// 获取或设置所借的书的ISBN
        /// </summary>
        [DefaultValue("")]
        public string RentBookISBN { get; set; }

        /// <summary>
        /// 获取或设置借出时间戳
        /// </summary>
        [DefaultValue(0)]
        public DateTime BorrowTimestamp { get; set; }

        /// <summary>
        /// 获取或设置应该还书时间戳
        /// </summary>
        [DefaultValue(0)]
        public DateTime OughtReturnTimestamp { get; set; }

        /// <summary>
        /// 获取或设置实际还书时间戳
        /// </summary>
        [DefaultValue(null)]
        public DateTime? ActualReturnTimestamp { get; set; }

        /// <summary>
        /// 获取这本借出的书是否已经还了
        /// </summary>
        public bool IsAlreadyReturned
        {
            get
            {
                return this.ActualReturnTimestamp != null;
            }
        }
    }
}
