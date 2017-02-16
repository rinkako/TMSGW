using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinyMSGW.ViewModel;
using TinyMSGW.Entity;
using TinyMSGW.Utils;
using TinyMSGW.Enums;
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
            // 有欠款不能借
            if (card.DelayFee == 0)
            {
                // 书的库存减少一本
                book.NumberInLibrary--;
                // 书的借出数多一本
                book.NumberInRenting++;
                // 把书放入借书卡中生成借书记录
                RentLog rlog = new RentLog()
                {
                    RentID = GDP.GlobalCounterRentLogID++,
                    BorrowTimestamp = DateTime.Now,
                    OughtReturnTimestamp = DateTime.Now.AddDays(GDP.ReturnDaySpan),
                    RentBookISBN = book.ISBN,
                    BorrowUsercardID = card.UsercardID
                };
                card.BorrowingList.Add(rlog);
                return true;
            }
            return false;
        }

        public bool CustomerReturnBook(Book book, Usercard card)
        {
            // 有欠款不能还
            if (card.DelayFee == 0)
            {
                // 书的库存增加一本
                book.NumberInLibrary++;
                // 书的借出数少一本
                book.NumberInRenting--;
                // 将借书记录标定为已归还
                RentLog rlog = card.BorrowingList.Find((x) => x.RentBookISBN == book.ISBN);
                rlog.ActualReturnTimestamp = DateTime.Now;
                return true;
            }
            return false;
        }

        public bool CustomerMissAndPayForBook(User user, Book book, out double pay)
        {
            // 注销借书记录
            Usercard ucard = user.Card;
            RentLog rlog = ucard.BorrowingList.Find((x) => x.RentBookISBN == book.ISBN);
            // 直接记为按时归还
            rlog.ActualReturnTimestamp = rlog.OughtReturnTimestamp;
            // 书的借出计数少一，但库存不增加
            book.NumberInRenting--;
            // 输出需要赔偿的价钱
            pay = book.Value;
            return true;
        }

        public bool DeleteUser(User user)
        {
            // 权限检查
            if (GDP.CurrentUser.Type == UserType.Admin)
            {
                User cuser = this.libMana.GetUser(user.UserName);
                // 系统管理员或者自己不可以被删掉
                if (cuser == null || cuser.Type == UserType.Admin ||
                    cuser.UserID == GDP.CurrentUser.UserID)
                {
                    return false;
                }
                // 移除用户（将用户设置为无效的）
                user.UserValid = false;
                return true;
            }
            return false;
        }

        public bool EditBook(Book book, Book newbookDescriptor)
        {
            // 依照描述子更新书的信息
            book.Author = newbookDescriptor.Author;
            book.ISBN = newbookDescriptor.ISBN;
            book.IsRemoved = newbookDescriptor.IsRemoved;
            book.LocationOfLibrary = newbookDescriptor.LocationOfLibrary;
            book.Name = newbookDescriptor.Name;
            book.NumberInLibrary = newbookDescriptor.NumberInLibrary;
            book.NumberInRenting = newbookDescriptor.NumberInRenting;
            book.PublishYear = newbookDescriptor.PublishYear;
            book.StoreIntoLibraryTimestamp = newbookDescriptor.StoreIntoLibraryTimestamp;
            book.Type = newbookDescriptor.Type;
            book.Value = newbookDescriptor.Value;
            return true;
        }

        public bool EditStoringBook(Warehouse whouse, StoringBook book, StoringBook newbookDescriptor)
        {
            // 依照描述子更新书的信息
            book.Author = newbookDescriptor.Author;
            book.ISBN = newbookDescriptor.ISBN;
            book.Name = newbookDescriptor.Name;
            book.NumberOfWarehouse = newbookDescriptor.NumberOfWarehouse;
            book.PublishYear = newbookDescriptor.PublishYear;
            book.Type = newbookDescriptor.Type;
            book.WarehouseID = newbookDescriptor.WarehouseID;
            return true;
        }

        public bool EditUser(User user, UserType newType, Dictionary<string, string> paras)
        {
            user.Type = newType;
            user.UserName = paras["username"];
            user.UserPhone = paras["userphone"];
            user.UserPasswordSHA1 = paras["userpasswordsha1"];
            user.UserValid = paras["uservalid"] == "true" ? true : false;
            return true;
        }

        public bool KeeperAddBook(Warehouse whouse, Book descriptor, int numberToStore, out StoringBook sbook)
        {
            // 检查是否初次入库
            StoringBook csb = whouse.StoringList.Find((t) => t.ISBN == descriptor.ISBN);
            // 该书初次入库要建立记录
            if (csb == null)
            {
                StoringBook newsb = new StoringBook()
                {
                    Author = descriptor.Author,
                    ISBN = descriptor.ISBN,
                    Name = descriptor.Name,
                    NumberOfWarehouse = numberToStore,
                    PublishYear = descriptor.PublishYear,
                    Type = descriptor.Type,
                    WarehouseID = whouse.WarehouseID
                };
                whouse.StoringList.Add(newsb);
                csb = newsb;
            }
            // 否则只需要更新信息
            else
            {
                csb.Author = descriptor.Author;
                csb.ISBN = descriptor.ISBN;
                csb.Name = descriptor.Name;
                csb.NumberOfWarehouse = csb.NumberOfWarehouse + numberToStore; // 数量增加
                csb.PublishYear = descriptor.PublishYear;
                csb.Type = descriptor.Type;
            }
            sbook = csb;
            return true;
        }

        public bool KeeperRemoveBook(Warehouse whouse, StoringBook sbook)
        {
            sbook.NumberOfWarehouse = 0;
            whouse.StoringList.Remove(sbook);
            return true;
        }

        public bool KeeperShopBook(Warehouse whouse, StoringBook sbook, int number)
        {
            // 库存不少于需求
            if (sbook.NumberOfWarehouse >= number)
            {
                // 减少库存，增加图书馆藏书量是图书馆柜员的任务，不在此处理
                sbook.NumberOfWarehouse -= number;
                return true;
            }
            return false;
        }

        public bool LibrarianAddBook(Book descriptor)
        {
            Book libBook = this.libMana.GetBook(descriptor.ISBN);
            // 该书初次入库要建立记录
            if (libBook == null)
            {
                Book newb = new Book()
                {
                    Author = descriptor.Author,
                    ISBN = descriptor.ISBN,
                    Name = descriptor.Name,
                    LocationOfLibrary = descriptor.LocationOfLibrary,
                    NumberInLibrary = descriptor.NumberInLibrary,
                    NumberInRenting = 0,
                    Value = descriptor.Value,
                    IsRemoved = false,
                    StoreIntoLibraryTimestamp = DateTime.Now,
                    PublishYear = descriptor.PublishYear,
                    Type = descriptor.Type
                };
                this.libMana.AddBook(newb);
                libBook = newb;
            }
            // 否则只更新信息
            else
            {
                libBook.Author = descriptor.Author;
                libBook.ISBN = descriptor.ISBN;
                libBook.Name = descriptor.Name;
                libBook.LocationOfLibrary = descriptor.LocationOfLibrary;
                // 增加上架数量，不改变借出量
                libBook.NumberInLibrary = libBook.NumberInLibrary + descriptor.NumberInLibrary;
                libBook.Value = descriptor.Value;
                libBook.IsRemoved = false;
                libBook.PublishYear = descriptor.PublishYear;
                libBook.Type = descriptor.Type;
            }
            return true;
        }

        public bool LibrarianRecieveDelayFee(Usercard card)
        {
            // 强制归还所有延时书籍
            var nowstamp = DateTime.Now;
            foreach (var rlog in card.BorrowingList)
            {
                if (rlog.OughtReturnTimestamp < nowstamp)
                {
                    Book book = this.libMana.GetBook(rlog.RentBookISBN);
                    // 书的库存增加一本
                    book.NumberInLibrary++;
                    // 书的借出数少一本
                    book.NumberInRenting--;
                    // 将借书记录标定为已归还
                    rlog.ActualReturnTimestamp = nowstamp;
                }
            }
            // 收钱完毕就将滞纳金消除
            card.PaidDelayFeeSucceed();
            return true;
        }

        public bool LibrarianRemoveBook(Book book)
        {
            return this.libMana.DeleteBook(book);
        }

        public bool LibrarianRestoreBook(Book book)
        {
            Book obj = this.libMana.GetBook(book.ISBN);
            // 置零，相当于下架，存入仓库由仓管负责，不在此处理
            obj.NumberInLibrary = 0;
            return true;
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
            if (userObj != null && userObj.UserValid == true)
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
            GDP.CurrentUser = null;
            return true;
        }
  
        public bool RetrieveDelayFee(Usercard card, out double fee)
        {
            fee = card.DelayFee;
            return true;
        }

        public bool RetrieveUser(string username, out Dictionary<string, string> descriptor)
        {
            User usr = this.libMana.GetUser(username);
            if (usr == null)
            {
                descriptor = null;
                return false;
            }
            Dictionary<string, string> resDict = new Dictionary<string, string>();
            resDict["cardid"] = usr.Card.UsercardID.ToString();
            resDict["type"] = usr.Type.ToString();
            resDict["userid"] = usr.UserID.ToString();
            resDict["username"] = usr.UserName;
            resDict["userpasswordsha1"] = usr.UserPasswordSHA1;
            resDict["userphone"] = usr.UserPhone;
            resDict["uservalid"] = usr.UserValid ? "true" : "false";
            descriptor = resDict;
            return true;
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

        public bool ListAllLibraryBook(out object outDataSet, string keyword, string type)
        {
            throw new NotImplementedException();
        }

        public bool ListAllStoringBook(Warehouse w, out object outDataSet)
        {
            throw new NotImplementedException();
        }

        public bool ListAllBook(out object outDataSet)
        {
            throw new NotImplementedException();
        }

        public bool ListUser(UserType utype, out object outDataSet)
        {
            throw new NotImplementedException();
        }

        public bool ListAllUser(out object outDataSet)
        {
            throw new NotImplementedException();
        }
    }
}
