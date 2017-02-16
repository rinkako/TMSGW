using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinyMSGW.Entity;
using TinyMSGW.Enums;
using TinyMSGW.Utils;

namespace TinyMSGW.Adapter
{
    /// <summary>
    /// 实现联机模式的控制器
    /// </summary>
    internal sealed class OnlineAdapterImpl : IActionAdapter
    {
        public bool AddUser(string name, UserType type, Dictionary<string, string> paras, out User user)
        {
            throw new NotImplementedException();
        }

        public bool CustomerCancelUsercard(User user, Usercard card)
        {
            throw new NotImplementedException();
        }

        public bool CustomerHandleUsercard(User user, out Usercard card)
        {
            throw new NotImplementedException();
        }

        public bool CustomerMissAndPayForBook(User user, Book book, out double pay)
        {
            throw new NotImplementedException();
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

        public bool KeeperAddBook(Warehouse whouse, Book descriptor, int numberToStore, out StoringBook sbook)
        {
            throw new NotImplementedException();
        }

        public bool KeeperRemoveBook(Warehouse whouse, StoringBook sbook)
        {
            throw new NotImplementedException();
        }

        public bool KeeperShopBook(Warehouse whouse, StoringBook sbook, int number)
        {
            throw new NotImplementedException();
        }

        public bool LibrarianAddBook(Book descriptor)
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

        public bool ListAllBook()
        {
            throw new NotImplementedException();
        }

        public bool ListAllLibraryBook()
        {
            throw new NotImplementedException();
        }

        public bool ListAllStoringBook(Warehouse w)
        {
            throw new NotImplementedException();
        }

        public bool ListAllUser()
        {
            throw new NotImplementedException();
        }

        public bool ListUser(UserType utype)
        {
            throw new NotImplementedException();
        }

        public bool LoginSuccess(string username)
        {
            // 恒为TRUE
            return true;
        }

        public bool LoginValid(string username, string passwordWithSHA1, out bool allowLogin)
        {
            try
            {
                DataTable resDt = DBUtil.GetDataSet(DBUtil.Conn, CommandType.Text, "select * from tw_user where tw_user.UserName = \"" + username + "\"", null).Tables[0];
                allowLogin = false;
                if (resDt.Rows.Count > 0)
                {
                    string checkpw = (string)resDt.Rows[0]["UserPasswordSHA1"];
                    if (String.Equals(checkpw, passwordWithSHA1, StringComparison.CurrentCultureIgnoreCase))
                    {
                        allowLogin = true;
                        GlobalDataPackage.CurrentUser = new User()
                        {
                            UserID = (int)resDt.Rows[0]["UserID"],
                            CardID = (int)resDt.Rows[0]["CardID"],
                            UserName = username,
                            Type = (UserType)((int)resDt.Rows[0]["Type"]),
                            UserPasswordSHA1 = passwordWithSHA1,
                            UserPhone = (string)resDt.Rows[0]["UserPhone"],
                            UserValid = (int)resDt.Rows[0]["UserValid"] == 1
                        };
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                LogUtil.Log("ERROR - Login Valid Online Failure：" + e.ToString());
                return allowLogin = false;
            }
        }

        public bool Logout()
        {
            GlobalDataPackage.CurrentUser = null;
            return true;
        }

        public bool RetrieveDelayFee(Usercard card, out double fee)
        {
            throw new NotImplementedException();
        }

        public bool RetrieveUser(string username, out Dictionary<string, string> descriptor)
        {
            throw new NotImplementedException();
        }

        public void Terminate()
        {
            throw new NotImplementedException();
        }

        public bool WriteDataToStableStorage()
        {
            throw new NotImplementedException();
        }
    }
}
