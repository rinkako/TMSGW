using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TinyMSGW.Entity;

namespace TinyMSGW.ViewModel
{
    /// <summary>
    /// 控制器类：图书馆管理类
    /// 她是一个单例类
    /// </summary>
    internal sealed class LibraryManager
    {
        /// <summary>
        /// 为图书馆实体设置实例
        /// </summary>
        /// <param name="instance">图书馆实例，她和她的附庸必须都完全地反序列化了</param>
        /// <returns>是否为替换操作</returns>
        public bool SetLibraryInstance(Library instance)
        {
            bool retFlag = this.ourLibrary != null;
            this.ourLibrary = instance;
            return retFlag;
        }
        
        /// <summary>
        /// 私有的构造器
        /// </summary>
        private LibraryManager()
        {

        }

        /// <summary>
        /// 工厂方法：获得类的唯一实例
        /// </summary>
        /// <returns>图书馆管理器唯一实例</returns>
        public static LibraryManager GetInstance()
        {
            return LibraryManager.Instance;
        }

        /// <summary>
        /// 图书馆实体的实例
        /// </summary>
        private Library ourLibrary = null;

        /// <summary>
        /// 唯一实例
        /// </summary>
        private static readonly LibraryManager Instance = new LibraryManager();
    }
}
