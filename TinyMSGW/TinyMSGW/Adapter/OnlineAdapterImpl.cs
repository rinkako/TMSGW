﻿using System;
using System.Data;
using System.Collections.Generic;
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
            try
            {
                // 更新User记录
                string UpdateClause = "update tw_user set tw_user.CardID = -1 where tw_user.UserID = " + user.UserID;
                DataSet UpdateDs = DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, UpdateClause, null);
                // 更新Usercard记录
                string cardClause = "update tw_usercard set tw_usercard.Valid = 0 where tw_usercard.CardID = " + card.UsercardID;
                DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, cardClause, null);
                return true;
            }
            catch (Exception e)
            {
                LogUtil.Log("ERROR: Custom cancel usercard failed: " + e.ToString());
                card = null;
                return false;
            }
        }

        public bool CustomerHandleUsercard(User user, out Usercard card)
        {
            try
            {
                // 更新User记录
                string UpdateClause = "update tw_user set tw_user.CardID = " + (GlobalDataPackage.GlobalCounterUsercardID++) +
                    " where tw_user.UserID = " + user.UserID;
                DataSet UpdateDs = DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, UpdateClause, null);
                // 更新Usercard记录
                var ctime = DateTime.Now;
                string InsertClause = "insert into tw_usercard (UsercardID, UserID, RegisteTimestamp, Valid) values (" +
                     GlobalDataPackage.GlobalCounterUsercardID + ", " + user.UserID + ", '" +
                     ctime.ToString("yyyy-MM-dd HH:mm:ss") + "', 0)";
                DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, InsertClause, null);
                card = new Usercard()
                {
                    UsercardID = GlobalDataPackage.GlobalCounterUsercardID,
                    UserID = user.UserID,
                    RegisteTimestamp = ctime
                };
                return true;
            }
            catch (Exception e)
            {
                LogUtil.Log("ERROR: Custom handle usercard failed: " + e.ToString());
                card = null;
                return false;
            }
        }
        
        public bool CustomerRentBook(Book book, Usercard card)
        {
            try
            {
                // 更新BOOK记录
                string UpdateClause = "update tw_book set tw_book.NumberInRenting = tw_book.NumberInRenting + 1 where tw_book.ISBN = '" + book.ISBN + "'";
                DataSet UpdateDs = DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, UpdateClause, null);
                // 更新RENTLOG记录
                string LogClause = "insert into tw_rentlog (RentID, BorrowUsercardID, RentBookISBN, BorrowTimestamp, OughtReturnTimestamp, IsPaidDelayFee) values (" +
                     (GlobalDataPackage.GlobalCounterRentLogID++) + ", " + card.UsercardID + ", '" + book.ISBN + "', '" +
                     DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + (DateTime.Now.AddDays(GlobalDataPackage.ReturnDaySpan)).ToString("yyyy-MM-dd HH:mm:ss") +
                     "', 0)";
                DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, LogClause, null);
                return true;
            }
            catch (Exception e)
            {
                LogUtil.Log("ERROR: Custom rent book failed: " + e.ToString());
                return false;
            }
        }

        public bool CustomerReturnBook(Book book, Usercard card)
        {
            try
            {
                // 更新BOOK记录
                string UpdateClause = "update tw_book set tw_book.NumberInRenting = tw_book.NumberInRenting - 1 where tw_book.ISBN = '" + book.ISBN + "'";
                DataSet UpdateDs = DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, UpdateClause, null);
                // 更新RENTLOG记录
                string LogClause = "update tw_rentlog set tw_rentlog.ActualReturnTimestamp = '" +
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', tw_rentlog.IsPaidDelayFee = 1 where tw_rentlog.RentBookISBN = '" +
                    book.ISBN + "' and tw_rentlog.BorrowUsercardID = " + card.UsercardID;
                DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, LogClause, null);
                return true;
            }
            catch (Exception e)
            {
                LogUtil.Log("ERROR: Custom return book failed: " + e.ToString());
                return false;
            }
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
                LogUtil.Log("ERROR: cannot delete user" + e.ToString());
                return false;
            }
        }

        public bool EditBook(Book book, Book newbookDescriptor)
        {
            try
            {
                string updateClause = "update tw_book set tw_book.ISBN = '" + newbookDescriptor.ISBN + "', " +
                    "tw_book.Name = '" + newbookDescriptor.Name + "', " +
                    "tw_book.Author = '" + newbookDescriptor.Author + "', " +
                    "tw_book.Type = '" + newbookDescriptor.Type + "', " +
                    "tw_book.PublishYear = " + newbookDescriptor.PublishYear + ", " +
                    "tw_book.Value = " + newbookDescriptor.Value + ", " +
                    "tw_book.LocationOfLibrary = '" + newbookDescriptor.LocationOfLibrary + "', " +
                    "tw_book.NumberInLibrary = " + newbookDescriptor.NumberInLibrary + ", " +
                    "tw_book.NumberInRenting = " + newbookDescriptor.NumberInRenting +
                    " where tw_book.ISBN = '" + book.ISBN + "'";
                DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, updateClause, null);
                return true;
            }
            catch (Exception e)
            {
                LogUtil.Log("ERROR: Edit Library Book Failed: " + e.ToString());
                return false;
            }
        }

        public bool EditStoringBook(Warehouse whouse, StoringBook book, StoringBook newbookDescriptor)
        {
            try
            {
                string updateClause = "update tw_storing set tw_storing.ISBN = '" + newbookDescriptor.ISBN + "', " +
                    "tw_storing.Name = '" + newbookDescriptor.Name + "', " +
                    "tw_storing.Author = '" + newbookDescriptor.Author + "', " +
                    "tw_storing.Type = '" + newbookDescriptor.Type + "', " +
                    "tw_storing.PublishYear = " + newbookDescriptor.PublishYear + ", " +
                    "tw_storing.Value = " + newbookDescriptor.Value + ", " +
                    "tw_storing.NumberOfWarehouse = " + newbookDescriptor.NumberOfWarehouse + ", " +
                    "tw_storing.WarehouseID = " + whouse.WarehouseID +
                    " where tw_storing.ISBN = '" + book.ISBN + "'";
                DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, updateClause, null);
                return true;
            }
            catch (Exception e)
            {
                LogUtil.Log("ERROR: Edit Storing Book Failed: " + e.ToString());
                return false;
            }
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
            try
            {
                string bookClause = "select * from tw_storing where tw_storing.ISBN = " + descriptor.ISBN;
                var bookDS = DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, bookClause, null);
                // 如果已有记录就改变，否则插入
                string updateClause;
                if (bookDS.Tables[0].Rows.Count > 0)
                {
                    updateClause = "update tw_storing set tw_storing.NumberOfWarehouse = tw_storing.NumberOfWarehouse + " + numberToStore +
                        " where tw_storing.ISBN = " + descriptor.ISBN;
                }
                else
                {
                    updateClause = "insert into tw_storing (ISBN, Name, Author, Type, Value, PublishYear, NumberOfWarehouse, WarehouseID) values (" +
                        descriptor.ISBN + ", '" + descriptor.Name + "', '" + descriptor.Author + "', '" + descriptor.Type + "', " +
                        descriptor.Value + ", " + descriptor.PublishYear + ", " + numberToStore + ", " + whouse.WarehouseID + ")";
                }
                DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, updateClause, null);
                sbook = new StoringBook()
                {
                    WarehouseID = whouse.WarehouseID,
                    Name = descriptor.Name,
                    PublishYear = descriptor.PublishYear,
                    Type = descriptor.Type,
                    Author = descriptor.Author,
                    ISBN = descriptor.ISBN,
                    NumberOfWarehouse = numberToStore,
                };
                return true;
            }
            catch (Exception e)
            {
                LogUtil.Log("ERROR: Keeper Add Book failed: " + e.ToString());
                sbook = null;
                return false;
            }
        }

        public bool KeeperRemoveBook(Warehouse whouse, StoringBook sbook)
        {
            try
            {
                string updateClause = "delete from tw_storing where tw_storing.ISBN = " + sbook.ISBN;
                DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, updateClause, null);
                return true;
            }
            catch (Exception e)
            {
                LogUtil.Log("ERROR: Keeper Remove Book Failed: " + e.ToString());
                return false;
            }
        }

        public bool KeeperShopBook(Warehouse whouse, StoringBook sbook, int number)
        {
            try
            {
                string updateClause = "update tw_storing set tw_storing.NumberOfWarehouse = NumberOfWarehouse - " + number +
                    " where tw_storing.ISBN = " + sbook.ISBN;
                DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, updateClause, null);
                return true;
            }
            catch (Exception e)
            {
                LogUtil.Log("ERROR: Keeper Shop Book Failed: " + e.ToString());
                return false;
            }
        }

        public bool LibrarianAddBook(Book descriptor, int numberToAdd)
        {
            try
            {
                string bookClause = "select * from tw_book where tw_book.ISBN = " + descriptor.ISBN;
                var bookDS = DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, bookClause, null);
                // 如果已有记录就改变，否则插入
                string updateClause;
                if (bookDS.Tables[0].Rows.Count > 0)
                {
                    updateClause = "update tw_book set tw_book.NumberInLibrary = tw_book.NumberInLibrary + " + numberToAdd +
                        " where tw_book.ISBN = " + descriptor.ISBN;
                }
                else
                {
                    updateClause = "insert into tw_book (ISBN, Name, Author, Type, Value, PublishYear, LocationOfLibrary, StoreIntoLibraryTimestamp, NumberInLibrary, NumberInRenting) values (" +
                        descriptor.ISBN + ", '" + descriptor.Name + "', '" + descriptor.Author + "', '" + descriptor.Type + "', " + descriptor.Value + ", " +
                        descriptor.PublishYear + ", '" + descriptor.LocationOfLibrary + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                        descriptor.NumberInLibrary + ", " + descriptor.NumberInRenting + ")";
                }
                DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, updateClause, null);
                return true;
            }
            catch (Exception e)
            {
                LogUtil.Log("ERROR: Librarian Add Book Failed: " + e.ToString());
                return false;
            }
        }

        public bool LibrarianRecieveDelayFee(Usercard card)
        {
            // 恒为TRUE
            return true;
        }

        public bool LibrarianRemoveBook(Book book)
        {
            try
            {
                string updateClause = "delete from tw_book where tw_book.ISBN = " + book.ISBN;
                DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, updateClause, null);
                return true;
            }
            catch (Exception e)
            {
                LogUtil.Log("ERROR: Librarian Remove Book Failed: " + e.ToString());
                return false;
            }
        }

        public bool LibrarianRestoreBook(Book book)
        {
            try
            {
                string bookClause = "select * from tw_book where tw_book.ISBN = " + book.ISBN;
                var bookDS = DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, bookClause, null);
                var rowItem = bookDS.Tables[0].Rows[0];
                var nRent = (int)rowItem["NumberInRenting"];
                string updateClause = "update tw_book set tw_book.NumberInLibrary = " + nRent + " where tw_book.ISBN = " + book.ISBN;     
                DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, updateClause, null);
                return true;
            }
            catch (Exception e)
            {
                LogUtil.Log("ERROR: Librarian Restore Book failed: " + e.ToString());
                return false;
            }
        }

        public bool ListAllBook(out object outDataSet)
        {
            try
            {
                string clause = "select * from ((select ISBN, Name, Author, Type, PublishYear from tw_book where tw_book.NumberInLibrary != 0) union (select ISBN, Name, Author, Type, PublishYear from tw_storing where tw_storing.NumberOfWarehouse != 0)) distinct";
                outDataSet = DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, clause, null);
                return true;
            }
            catch (Exception e)
            {
                LogUtil.Log("ERROR: List all book failed: " + e.ToString());
                outDataSet = null;
                return false;
            }
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

        public bool ListAllRentingBook(string username, bool allFlag, out List<Book> outList, out List<RentLog> logList)
        {
            try
            {
                string userClause = "select * from tw_user where tw_user.UserName = '" + username +"'";
                var tbrowC = DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, userClause, null).Tables[0].Rows;
                if (tbrowC.Count == 0)
                {
                    logList = null;
                    outList = null;
                    return false;
                }
                var cardID = (int)tbrowC[0]["CardID"];
                if (cardID == -1)
                {
                    outList = null;
                    logList = null;
                    return false;
                }
                string jClause = "select * from tw_rentlog inner join tw_book on tw_rentlog.RentBookISBN = tw_book.ISBN where tw_rentlog.BorrowUsercardID = " + cardID;
                if (allFlag == false)
                {
                    jClause += " and tw_rentlog.ActualReturnTimestamp IS NULL";
                }
                var dataTable = DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, jClause, null).Tables[0];
                outList = new List<Book>();
                logList = new List<RentLog>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    var rObj = dataTable.Rows[i];
                    Book bk = new Book()
                    {
                        ISBN = (string)rObj["ISBN"],
                        Name = (string)rObj["Name"],
                        Author = (string)rObj["Author"],
                        Type = (string)rObj["Type"],
                        Value = (double)rObj["Value"],
                        PublishYear = (int)rObj["PublishYear"]
                    };
                    outList.Add(bk);
                    RentLog rl = new RentLog()
                    {
                       BorrowTimestamp = (DateTime)rObj["BorrowTimestamp"],
                       OughtReturnTimestamp = (DateTime)rObj["OughtReturnTimestamp"],
                       ActualReturnTimestamp = rObj["ActualReturnTimestamp"].GetType() == typeof(DBNull) ? null : (DateTime?)rObj["ActualReturnTimestamp"]
                    };
                    logList.Add(rl);
                }
                return true;
            }
            catch (Exception e)
            {
                LogUtil.Log("ERROR: List All Renting Book Failed: " + e.ToString());
                outList = null;
                logList = null;
                return false;
            }
        }

        public bool ListAllStoringBook(Warehouse w, string keyword, out object outDataSet)
        {
            try
            {
                string clause = "select * from tw_storing";
                if (keyword != String.Empty)
                {
                    clause += " where tw_storing.Name LIKE '%" + keyword + "%'";
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

        public bool RetrieveStoringBook(Warehouse w, string isbn, out StoringBook outBook)
        {
            try
            {
                string clause = "select * from tw_storing where tw_storing.ISBN = '" + isbn +
                    "' and tw_storing.WarehouseID = " + w.WarehouseID;
                DataTable resDt = DBUtil.CommitToDB(DBUtil.Conn, CommandType.Text, clause, null).Tables[0];
                if (resDt.Rows.Count > 0)
                {
                    var bObj = resDt.Rows[0];
                    StoringBook retObj = new StoringBook()
                    {
                        ISBN = isbn,
                        Author = (string)bObj["Author"],
                        Name = (string)bObj["Name"],
                        Type = (string)bObj["Type"],
                        PublishYear = (int)bObj["PublishYear"],
                        Value = (double)bObj["Value"],
                        WarehouseID = (int)bObj["WarehouseID"],
                        NumberOfWarehouse = (int)bObj["NumberOfWarehouse"]
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
                LogUtil.Log("ERROR: When retrieve storing book: " + e.ToString());
                return false;
            }
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
                    descriptor.Add("cardid", resDt.Rows[0]["CardID"].ToString());
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
