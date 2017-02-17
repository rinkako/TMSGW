using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinyMSGW.Entity;
using TinyMSGW.Enums;
using TinyMSGW.Utils;
using TinyMSGW.ViewModel;
using GDP = TinyMSGW.GlobalDataPackage;

namespace TinyMSGW.Adapter
{
    /// <summary>
    /// 实现联机模式的控制器
    /// </summary>
    internal sealed class OnlineAdapterImpl : IActionAdapter
    {
        public bool AddUser(string name, UserType type, Dictionary<string, string> paras, out User user)
        {
            try
            {
                // 检查用户是否重名
                string FindClause = "select * from tw_user where tw_user.UserName = '" + name + "'";
                DataSet FindDs = DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, FindClause, null);
                if (FindDs.Tables.Count > 0 && FindDs.Tables[0].Rows.Count > 0)
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
                // 立即更新数据库
                string AddClause = "insert into tw_user (UserID, UserName, UserPasswordSHA1, UserPhone, Type, UserValid, CardID) values (" +
                    nuser.UserID +", '" + nuser.UserName + "', '" + nuser.UserPasswordSHA1 + "', '" + nuser.UserPhone +"', " +
                    (int)nuser.Type + ", 1, -1)";
                DataSet AddDs = DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, AddClause, null);
                user = nuser;
                return true;
            }
            catch (Exception e)
            {
                LogUtil.Log("ERROR: Add user failed: " + e.ToString());
                user = null;
                return false;
            }
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
            var deleteUserId = user.UserID;
            try
            {
                string FindClause = "select * from tw_user where tw_user.UserID = " + deleteUserId;
                DataSet FindDs = DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, FindClause, null);
                var CardId = (int)FindDs.Tables[0].Rows[0]["CardID"];
                string deleteClause = "update tw_user set tw_user.UserValid = 0 where tw_user.UserID = " + deleteUserId;
                DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, deleteClause, null);
                string deleteClause2 = "update tw_usercard set tw_usercard.Valid = 0 where tw_usercard.UsercardID = " + CardId;
                DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, deleteClause2, null);
                return true;
            }
            catch (Exception e)
            {
                LogUtil.Log("ERROR: " + e.ToString());
                return false;
            }
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
            var uid = user.UserID;
            try
            {
                string Clause;
                // 不改密码时
                if (paras["password"] == String.Empty)
                {
                    Clause = "update tw_user set tw_user.UserName = '" + paras["username"] + "', tw_user.UserPhone = '" + paras["phone"] +
                        "', tw_user.Type = " + paras["type"] + " where tw_user.UserID = " + uid;
                }
                // 改密码时
                else
                {
                    Clause = "update tw_user set tw_user.UserName = '" + paras["username"] + "', tw_user.UserPhone = '" + paras["phone"] +
                        "', tw_user.Type = " + paras["type"] + ", tw_user.UserPasswordSHA1 = '" + Utils.CommonUtil.EncryptToSHA1(paras["password"]) +
                        "' where tw_user.UserID = " + uid;
                }
                DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, Clause, null);
                return true;
            }
            catch (Exception e)
            {
                LogUtil.Log("ERROR: Edit user failed: " + e.ToString());
                return false;
            }
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

        public bool ListAllBook(out object outDataSet)
        {
            throw new NotImplementedException();
        }

        public bool ListAllLibraryBook(out object outDataSet, string keyword, string type)
        {
            try
            {
                string clause = "select * from tw_book";
                if (keyword != String.Empty)
                {
                    clause += " where tw_book.Name LIKE '%" + keyword + "%'";
                }
                outDataSet = DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, clause, null);
                return true;
            }
            catch (Exception e)
            {
                LogUtil.Log("ERROR: " + e.ToString());
                outDataSet = null;
                return false;
            }
        }

        public bool ListAllStoringBook(Warehouse w, out object outDataSet)
        {
            throw new NotImplementedException();
        }

        public bool ListAllUser(out object outDataSet, string keyword)
        {
            try
            {
                string clause = "select * from tw_user";
                if (keyword != String.Empty)
                {
                    clause += " where tw_user.UserName LIKE '%" + keyword + "%' and tw_user.UserValid = 1";
                }
                else
                {
                    clause += " where tw_user.UserValid = 1";
                }
                outDataSet = DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, clause, null);
                return true;
            }
            catch (Exception e)
            {
                LogUtil.Log("ERROR: " + e.ToString());
                outDataSet = null;
                return false;
            }
        }

        public bool ListUser(UserType utype, out object outDataSet)
        {
            try
            {
                string clause = "select * from tw_user where tw_user.Type = " + ((int)utype).ToString();
                outDataSet = DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, clause, null);
                return true;
            }
            catch (Exception e)
            {
                LogUtil.Log("ERROR: " + e.ToString());
                outDataSet = null;
                return false;
            }
        }

        public bool LoginSuccess(string username)
        {
            LogUtil.Log("ACK: Login Succeed with username: " + username);
            return true;
        }

        public bool LoginValid(string username, string passwordWithSHA1, out bool allowLogin)
        {
            try
            {
                DataTable resDt = DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, "select * from tw_user where tw_user.UserName = \"" + username + "\"", null).Tables[0];
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
                LogUtil.Log("ERROR: Login Valid Online Failure：" + e.ToString());
                return allowLogin = false;
            }
        }

        public bool Logout()
        {
            // 注销
            GlobalDataPackage.CurrentUser = null;
            return true;
        }

        public bool RetrieveBook(string isbn, out Book outBook)
        {
            try
            {
                DataTable resDt = DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, "select * from tw_book where tw_book.ISBN = \"" + isbn + "\"", null).Tables[0];
                if (resDt.Rows.Count > 0)
                {
                    var bObj = resDt.Rows[0];
                    Book retObj = new Book()
                    {
                        ISBN = isbn,
                        Author = (string)bObj["Author"],
                        Name = (string)bObj["Name"],
                        Type = (string)bObj["Type"],
                        PublishYear = (int)bObj["PublishYear"],
                        Value = (double)bObj["Value"],
                        LocationOfLibrary = (string)bObj["LocationOfLibrary"],
                        NumberInLibrary = (int)bObj["NumberInLibrary"],
                        NumberInRenting = (int)bObj["NumberInRenting"]
                    };
                    outBook = retObj;
                }
                else
                {
                    outBook = null;
                }
                return true;
            }
            catch (Exception e)
            {
                outBook = null;
                LogUtil.Log("ERROR: When retrieve book: " + e.ToString());
                return false;
            }
        }

        public bool RetrieveDelayFee(Usercard card, out double fee)
        {
            throw new NotImplementedException();
        }

        public bool RetrieveUser(string username, out Dictionary<string, string> descriptor)
        {
            try
            {
                DataTable resDt = DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, "select * from tw_user where tw_user.UserName = \"" + username + "\"", null).Tables[0];
                descriptor = null;
                if (resDt.Rows.Count > 0)
                {
                    descriptor = new Dictionary<string, string>();
                    descriptor.Add("phone", (string)resDt.Rows[0]["UserPhone"]);
                    descriptor.Add("type", resDt.Rows[0]["Type"].ToString());
                }
                return true;
            }
            catch (Exception e)
            {
                LogUtil.Log("ERROR: Retrieve user failed：" + e.ToString());
                descriptor = null;
                return false;
            }
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
            // online模式的修改总是即时的
            return true;
        }
    }
}
