using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinyMSGW.Entity;
using TinyMSGW.Enums;

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

        public bool LoginSuccess(string username)
        {
            throw new NotImplementedException();
        }

        public bool LoginValid(string username, string passwordWithSHA1, out bool allowLogin)
        {
            throw new NotImplementedException();
        }

        public bool Logout()
        {
            throw new NotImplementedException();
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
