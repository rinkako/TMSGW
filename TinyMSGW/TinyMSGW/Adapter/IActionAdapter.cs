using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinyMSGW.Entity;
using TinyMSGW.Enums;

namespace TinyMSGW.Adapter
{
    /// <summary>
    /// 接口：程序前端与后台动作的接口
    /// 前端只需要把数据传递给接口而不需考虑是单机还是联机的细节
    /// </summary>
    internal interface IActionAdapter
    {
        #region 客户的动作
        /// <summary>
        /// 客户借走一本书
        /// </summary>
        /// <param name="book">要借的书</param>
        /// <param name="card">受理这个业务的借书卡</param>
        /// <returns>操作成功与否</returns>
        bool CustomerRentBook(Book book, Usercard card);

        /// <summary>
        /// 客户还一本之前借走的书
        /// </summary>
        /// <param name="book">要还的书</param>
        /// <param name="card">受理这个业务的借书卡</param>
        /// <returns>操作成功与否</returns>
        bool CustomerReturnBook(Book book, Usercard card);

        /// <summary>
        /// 客户注销一张借书卡
        /// </summary>
        /// <param name="user">办理业务的用户</param>
        /// <param name="card">要被处理的借书卡</param>
        /// <returns>操作成功与否</returns>
        bool CustomerCancelUsercard(User user, Usercard card);

        /// <summary>
        /// 客户办理一张借书卡
        /// </summary>
        /// <param name="user">要办理的用户</param>
        /// <param name="card">[out] 办理得到的借书卡</param>
        /// <returns>操作成功与否</returns>
        bool CustomerHandleUsercard(User user, out Usercard card);
        #endregion

        #region 管理员动作
        /// <summary>
        /// 添加一个用户
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="type">用户类型</param>
        /// <param name="paras">用户参数字典</param>
        /// <param name="user">[out] 产生的用户实例</param>
        /// <returns>操作成功与否</returns>
        bool AddUser(string name, UserType type, Dictionary<string, string> paras, out User user);

        /// <summary>
        /// 删除一个用户
        /// </summary>
        /// <param name="user">要删掉的用户</param>
        /// <returns>操作成功与否</returns>
        bool DeleteUser(User user);

        /// <summary>
        /// 编辑一个用户
        /// </summary>
        /// <param name="user">要修改的用户</param>
        /// <param name="newType">新的用户类型</param>
        /// <param name="paras">新的用户参数字典</param>
        /// <returns>操作成功与否</returns>
        bool EditUser(User user, UserType newType, Dictionary<string, string> paras);

        /// <summary>
        /// 查询一个用户的详细信息
        /// </summary>
        /// <param name="username">要查询的用户名</param>
        /// <param name="descriptor">[out] 该用户的描述字典</param>
        /// <returns>操作成功与否</returns>
        bool RetrieveUser(string username, out Dictionary<string, string> descriptor);
        #endregion

        #region 图书馆柜员动作
        /// <summary>
        /// 柜员收入滞纳金并恢复借书卡功能
        /// </summary>
        /// <param name="card">要处理的借书卡</param>
        /// <returns>操作成功与否</returns>
        bool LibrarianRecieveDelayFee(Usercard card);

        /// <summary>
        /// 柜员将一种书加到图书馆中
        /// </summary>
        /// <param name="descriptor">待添加的书的描述子</param>
        /// <param name="numberToAdd">数量</param>
        /// <returns>操作成功与否</returns>
        bool LibrarianAddBook(Book descriptor, int numberToAdd);

        /// <summary>
        /// 柜员将一种书从图书馆中下架并抛弃
        /// </summary>
        /// <param name="book">待移除的书</param>
        /// <returns>操作成功与否</returns>
        bool LibrarianRemoveBook(Book book);

        /// <summary>
        /// 柜员将一种书从图书馆中下架以后续存入仓库
        /// </summary>
        /// <param name="book">待移除的书</param>
        /// <returns>操作成功与否</returns>
        bool LibrarianRestoreBook(Book book);
        #endregion

        #region 仓库管理员动作
        /// <summary>
        /// 仓管将一种新采购的书入库
        /// </summary>
        /// <param name="whouse">要入库的仓库</param>
        /// <param name="descriptor">书的描述子</param>
        /// <param name="numberToStore">要入库的数量</param>
        /// <param name="sbook">入库生成的未上架书籍实例</param>
        /// <returns>操作成功与否</returns>
        bool KeeperAddBook(Warehouse whouse, Book descriptor, int numberToStore, out StoringBook sbook);

        /// <summary>
        /// 仓管将一种库存里的书扔掉
        /// </summary>
        /// <param name="whouse">要移出的仓库</param>
        /// <param name="sbook">要扔掉的的未上架书籍</param>
        /// <returns>操作成功与否</returns>
        bool KeeperRemoveBook(Warehouse whouse, StoringBook sbook);

        /// <summary>
        /// 仓管将一种库存里的书拿出以后续在图书馆上架
        /// </summary>
        /// <param name="whouse">要移出的仓库</param>
        /// <param name="sbook">要处理的未上架书籍</param>
        /// <param name="number">要上架的本数</param>
        /// <returns>操作成功与否</returns>
        bool KeeperShopBook(Warehouse whouse, StoringBook sbook, int number);

        /// <summary>
        /// 修改一本库存书的信息
        /// </summary>
        /// <param name="whouse">所在的仓库</param>
        /// <param name="sbook">要处理的未上架书籍</param>
        /// <param name="newbookDescriptor">要将第二个参数的书改成形如这个的书</param>
        /// <returns>操作成功与否</returns>
        bool EditStoringBook(Warehouse whouse, StoringBook book, StoringBook newbookDescriptor);
        #endregion

        #region 公共动作
        /// <summary>
        /// 修改一本书的信息
        /// </summary>
        /// <param name="book">要改的书</param>
        /// <param name="newbookDescriptor">要将第一个参数的书改成形如这个的书</param>
        /// <returns>操作成功与否</returns>
        bool EditBook(Book book, Book newbookDescriptor);

        /// <summary>
        /// 获取一本书的信息
        /// </summary>
        /// <param name="isbn">要查询的图书的ISBN</param>
        /// <param name="outBook">[out] 查询到的图书实例</param>
        /// <returns>操作成功与否</returns>
        bool RetrieveBook(string isbn, out Book outBook);

        /// <summary>
        /// 获取一本库存书的信息
        /// </summary>
        /// <param name="w">要查询的仓库</param>
        /// <param name="isbn">要查询的图书的ISBN</param>
        /// <param name="outBook">[out] 查询到的图书实例</param>
        /// <returns>操作成功与否</returns>
        bool RetrieveStoringBook(Warehouse w, string isbn, out StoringBook outBook);

        /// <summary>
        /// 列出所有上架图书
        /// </summary>
        /// <param name="outDataSet">[out] 要传出的数据集</param>
        /// <param name="keyword">查询关键词</param>
        /// <param name="type">查询类型</param>
        /// <returns>操作成功与否</returns>
        bool ListAllLibraryBook(out object outDataSet, string keyword, string type);

        /// <summary>
        /// 列出所有库存图书
        /// </summary>
        /// <param name="w">要查询的仓库</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="outDataSet">[out] 要传出的数据集</param>
        /// <returns>操作成功与否</returns>
        bool ListAllStoringBook(Warehouse w, string keyword, out object outDataSet);

        /// <summary>
        /// 列出用户所借所有图书
        /// </summary>
        /// <param name="username">要查询的用户名</param>
        /// <param name="allFlag">是否列出已还记录</param>
        /// <param name="outList">[out] 所借书籍向量</param>
        /// <param name="logList">[out] 对应借书记录向量</param>
        /// <returns>操作成功与否</returns>
        bool ListAllRentingBook(string username, bool allFlag, out List<Book> outList, out List<RentLog> logList);

        /// <summary>
        /// 列出所有图书
        /// </summary>
        /// <param name="outDataSet">[out] 要传出的数据集</param>
        /// <returns>操作成功与否</returns>
        bool ListAllBook(out object outDataSet);

        /// <summary>
        /// 列出所有指定类型的用户
        /// </summary>
        /// <param name="utype">用户类型</param>
        /// <param name="outDataSet">[out] 要传出的数据集</param>
        /// <returns>操作成功与否</returns>
        bool ListUser(UserType utype, out object outDataSet);

        /// <summary>
        /// 列出所有用户
        /// </summary>
        /// <param name="outDataSet">[out] 要传出的数据集</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>操作成功与否</returns>
        bool ListAllUser(out object outDataSet, string keyword);
        #endregion

        #region 系统的辅助函数
        /// <summary>
        /// 验证用户登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="passwordWithSHA1">SHA1过的密码串</param>
        /// <param name="allowLogin">[out] 是否允许登录</param>
        /// <returns>操作成功与否（此处表现为能否成功运行验证）</returns>
        bool LoginValid(string username, string passwordWithSHA1, out bool allowLogin);
        
        /// <summary>
        /// 登陆一个用户，用于成功验证之后
        /// </summary>
        /// <param name="username">要登录的用户名</param>
        /// <returns>操作成功与否</returns>
        bool LoginSuccess(string username);

        /// <summary>
        /// 登出系统
        /// </summary>
        /// <returns>操作成功与否</returns>
        bool Logout();

        /// <summary>
        /// 结束应用程序
        /// </summary>
        void Terminate();

        /// <summary>
        /// 将数据写入稳定储存器
        /// </summary>
        /// <returns>操作成功与否</returns>
        bool WriteDataToStableStorage();
        #endregion
    }
}
