using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace TinyMSGW.Entity
{
    /// <summary>
    /// 实体类：用户借书卡
    /// </summary>
    [Serializable]
    internal sealed class Usercard
    {
        /// <summary>
        /// 构造器
        /// </summary>
        public Usercard()
        {
            this.BorrowingList = new List<RentLog>();
        }

        /// <summary>
        /// 获取或设置借书卡ID
        /// </summary>
        [DefaultValue(0)]
        public int UsercardID { get; set; }

        /// <summary>
        /// 获取或设置此借书卡绑定的用户ID
        /// </summary>
        [DefaultValue(0)]
        public int UserID { get; set; }

        /// <summary>
        /// 获取或设置开卡时间戳
        /// </summary>
        [DefaultValue(0)]
        public DateTime RegisteTimestamp { get; set; }

        /// <summary>
        /// 获取或设置注销卡时间戳
        /// </summary>
        [DefaultValue(null)]
        public DateTime? CancelTimestamp { get; set; }

        /// <summary>
        /// 获取借书向量
        /// </summary>
        public List<RentLog> BorrowingList { get; private set; }

        /// <summary>
        /// 获取该借书卡延期未归还图书的数量
        /// </summary>
        public int DelayNum
        {
            get
            {
                return this.BorrowingList.Count((x) => x.OughtReturnTimestamp <= DateTime.Now);
            }
        }

        /// <summary>
        /// 获取或设置该借书卡应缴纳的滞纳金
        /// </summary>
        public double DelayFee
        {
            get
            {
                this.iDelayfee = 0;
                this.BorrowingList.ForEach((x) =>
                {
                    if (x.OughtReturnTimestamp < DateTime.Now)
                    {
                        this.iDelayfee += GlobalDataPackage.DelayFeeADay * (DateTime.Now - x.OughtReturnTimestamp).Days;
                    }
                });
                return this.iDelayfee;
            }
            set
            {
                this.iDelayfee = value;
            }
        }
        
        /// <summary>
        /// 借书卡的滞纳金
        /// </summary>
        private double iDelayfee = 0;
    }
}
