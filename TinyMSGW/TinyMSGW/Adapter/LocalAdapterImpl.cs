using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinyMSGW.ViewModel;
using TinyMSGW.Entity;
using TinyMSGW.Utils;
using TinyMSGW.Enum;
using GDP = TinyMSGW.GlobalDataPackage;

namespace TinyMSGW.Adapter
{
    /// <summary>
    /// 实现单机模式的控制器
    /// </summary>
    internal sealed class LocalAdapterImpl : IActionAdapter
    {
        /// <summary>
        /// 图书馆管理器唯一实例
        /// </summary>
        private LibraryManager libMana = LibraryManager.GetInstance();

        public bool AddUser(string name, UserType type, Dictionary<string, string> paras, out User user)
        {
            // 检查用户是否重名
            var userobj = this.libMana.GetUser(name);
            if (userobj != null)
            {
                user = null;
                return false;
            }
            User nuser = new User()
            {
                UserID = GDP.GlobalCounterUserID++,
                Card = null,
                Type = type,
                UserName = name,
                UserPasswordSHA1 = CommonUtil.EncryptToSHA1(paras["password"]),
                UserPhone = paras["phone"],
                UserValid = true
            };
            // 离线模式不需要马上更新稳定储存器上的实体，直接把更新提交给图书馆
            this.libMana.AddToUserList(nuser);
            user = nuser;
            return true;
        }

        public bool CustomerCancelUsercard(User user, Usercard card)
        {
            // 不欠款且没有借书时才可以注销卡片
            if (card.DelayFee == 0 && card.BorrowingList.All((x) => x.IsAlreadyReturned == true))
            {
                user.Card = null;
                card.CancelTimestamp = DateTime.Now;
                return true;
            }
            return false;
        }

        public bool CustomerHandleUsercard(User user, out Usercard card)
        {
            // 没有借书卡才能办
            if (user.Card == null)
            {
                Usercard ucard = new Usercard()
                {
                    DelayFee = 0,
                    RegisteTimestamp = DateTime.Now,
                    UsercardID = GDP.GlobalCounterUsercardID++,
                    UserID = user.UserID
                };
                // 把卡捆绑到用户
                user.Card = ucard;
                card = ucard;
                return true;
            };
            card = null;
            return false;
        }

        public bool CustomerRentBook(Book book, Usercard card)
        {
            throw new NotImplementedException();
        }

        public bool CustomerReturnBook(Book book, Usercard card)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool EditBook(Book book, Book newbookDescriptor)
        {
            throw new NotImplementedException();
        }

        public bool EditStoringBook(Warehouse whouse, StoringBook book, StoringBook newbookDescriptor)
        {
            throw new NotImplementedException();
        }

        public bool EditUser(User user, UserType newType, Dictionary<string, string> paras)
        {
            throw new NotImplementedException();
        }

        public bool KeeperAddBook(Warehouse whouse, Book descriptor, out StoringBook sbook)
        {
            throw new NotImplementedException();
        }

        public bool KeeperRemoveBook(Warehouse whouse, StoringBook sbook)
        {
            throw new NotImplementedException();
        }

        public bool KeeperShopBook(Warehouse whouse, StoringBook sbook)
        {
            throw new NotImplementedException();
        }

        public bool LibrarianAddBook(Book book)
        {
            throw new NotImplementedException();
        }

        public bool LibrarianRecieveDelayFee(Usercard card)
        {
            throw new NotImplementedException();
        }

        public bool LibrarianRemoveBook(Book book)
        {
            throw new NotImplementedException();
        }

        public bool LibrarianRestoreBook(Book book)
        {
            throw new NotImplementedException();
        }

        public bool LoginSuccess(string username)
        {
            // 将GDP中的用户实例替换为登陆的用户实例
            GDP.CurrentUser = this.libMana.GetUser(username);
            return GDP.CurrentUser != null;
        }

        public bool LoginValid(string username, string passwordWithSHA1, out bool allowLogin)
        {
            // 获取用户实例
            var userObj = this.libMana.GetUser(username);
            if (userObj != null)
            {
                // 验证密码的SHA1是否一样
                allowLogin = userObj.UserPasswordSHA1 == passwordWithSHA1;
            }
            allowLogin = false;
            return true; // 单机模式始终能够验证用户
        }

        public bool Logout()
        {
            // 先保存所有修改
            SettingManager.WriteSettingToStable();
            // 登出用户
            GlobalDataPackage.CurrentUser = null;
            return true;
        }

        public bool RetrieveBook(Book book, out Dictionary<string, string> descriptor)
        {
            throw new NotImplementedException();
        }

        public bool RetrieveBookNumber(Book book, out int count)
        {
            throw new NotImplementedException();
        }

        public bool RetrieveDelayFee(Usercard card, out double fee)
        {
            throw new NotImplementedException();
        }

        public bool RetrieveUser(User user, out Dictionary<string, string> descriptor)
        {
            throw new NotImplementedException();
        }

        public void Terminate()
        {
            // 保存修改
            SettingManager.WriteSettingToStable();
            // 保存数据
            this.WriteDataToStableStorage();
            // 退出程序
            LogUtil.Log("ACK: Terminate called, system shutdown.");
            Environment.Exit(0);
        }

        public bool WriteDataToStableStorage()
        {
            return LocalIOUtil.Serialization(libMana.GetLibraryInstance(), LocalIOUtil.ParseURItoURL(GDP.DataFileName, true));
        }
    }
}
