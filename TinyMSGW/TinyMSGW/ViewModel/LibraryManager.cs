
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
            Utils.LogUtil.Log("ACK: Library instance replaced.");
            bool retFlag = this.ourLibrary != null;
            this.ourLibrary = instance;
            return retFlag;
        }

        /// <summary>
        /// 获取图书馆实体
        /// </summary>
        /// <returns>图书馆实例的引用</returns>
        public Library GetLibraryInstance()
        {
            return this.ourLibrary;
        }
        
        /// <summary>
        /// 获取一个用户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>该用户名对应的用户实例</returns>
        public User GetUser(string username)
        {
            return this.ourLibrary.UserList.Find((x) => x.UserName == username);
        }

        /// <summary>
        /// 通过用户名取得她的借书卡
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>该用户的借书卡</returns>
        public Usercard GetUsercardByUsername(string username)
        {
            var usr = this.ourLibrary.UserList.Find((x) => x.UserName == username);
            if (usr != null)
            {
                return usr.Card;
            }
            return null;
        }


        /// <summary>
        /// 获取一个仓库
        /// </summary>
        /// <param name="id">仓库id</param>
        /// <returns>仓库的实例</returns>
        public Warehouse GetWarehouse(int id)
        {
            return this.ourLibrary.WarehouseList.Find((x) => x.WarehouseID == id);
        }

        /// <summary>
        /// 获取一本书的实例
        /// </summary>
        /// <param name="isbn">书的唯一标示ISBN</param>
        /// <returns>书在图书馆的实例</returns>
        public Book GetBook(string isbn)
        {
            return this.ourLibrary.OnShopBookList.Find((x) => x.ISBN == isbn);
        }

        /// <summary>
        /// 上架一本书
        /// </summary>
        /// <param name="book">要上架的书籍</param>
        public void AddBook(Book book)
        {
            this.ourLibrary.OnShopBookList.Add(book);
        }

        /// <summary>
        /// 删除一本书
        /// </summary>
        /// <param name="book">书籍实例</param>
        /// <returns>操作成功与否</returns>
        public bool DeleteBook(Book book)
        {
            return this.ourLibrary.OnShopBookList.Remove(book);
        }

        /// <summary>
        /// 删除一个用户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>操作成功与否</returns>
        public bool DeleteUser(User user)
        {
            return this.ourLibrary.UserList.Remove(user);
        }

        /// <summary>
        /// 将一个用户加到图书馆的用户列表中
        /// </summary>
        /// <param name="user">用户实例</param>
        public void AddToUserList(User user)
        {
            this.ourLibrary.UserList.Add(user);
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
