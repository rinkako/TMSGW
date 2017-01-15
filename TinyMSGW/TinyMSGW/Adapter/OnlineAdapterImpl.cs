using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinyMSGW.Entity;
using TinyMSGW.Enum;

namespace TinyMSGW.Adapter
{
    /// <summary>
    /// 实现联机模式的控制器
    /// </summary>
    internal sealed class OnlineAdapterImpl : IActionAdapter
    {
        bool IActionAdapter.AddUser(string name, UserType type, Dictionary<string, string> paras, out User user)
        {
            throw new NotImplementedException();
        }

        bool IActionAdapter.CustomerCancelUsercard(User user, Usercard card)
        {
            throw new NotImplementedException();
        }

        bool IActionAdapter.CustomerHandleUsercard(User user, out Usercard card)
        {
            throw new NotImplementedException();
        }

        bool IActionAdapter.CustomerRentBook(Book book, Usercard card)
        {
            throw new NotImplementedException();
        }

        bool IActionAdapter.CustomerReturnBook(Book book, Usercard card)
        {
            throw new NotImplementedException();
        }

        bool IActionAdapter.DeleteUser(User user)
        {
            throw new NotImplementedException();
        }

        bool IActionAdapter.EditBook(Book book, Book newbookDescriptor)
        {
            throw new NotImplementedException();
        }

        bool IActionAdapter.EditStoringBook(Warehouse whouse, StoringBook book, StoringBook newbookDescriptor)
        {
            throw new NotImplementedException();
        }

        bool IActionAdapter.EditUser(User user, UserType newType, Dictionary<string, string> paras)
        {
            throw new NotImplementedException();
        }

        bool IActionAdapter.KeeperAddBook(Warehouse whouse, Book descriptor, out StoringBook sbook)
        {
            throw new NotImplementedException();
        }

        bool IActionAdapter.KeeperRemoveBook(Warehouse whouse, StoringBook sbook)
        {
            throw new NotImplementedException();
        }

        bool IActionAdapter.KeeperShopBook(Warehouse whouse, StoringBook sbook)
        {
            throw new NotImplementedException();
        }

        bool IActionAdapter.LibrarianAddBook(Book book)
        {
            throw new NotImplementedException();
        }

        bool IActionAdapter.LibrarianRecieveDelayFee(Usercard card)
        {
            throw new NotImplementedException();
        }

        bool IActionAdapter.LibrarianRemoveBook(Book book)
        {
            throw new NotImplementedException();
        }

        bool IActionAdapter.LibrarianRestoreBook(Book book)
        {
            throw new NotImplementedException();
        }

        bool IActionAdapter.LoginSuccess(string username)
        {
            throw new NotImplementedException();
        }

        bool IActionAdapter.LoginValid(string username, string passwordWithSHA1, out bool allowLogin)
        {
            throw new NotImplementedException();
        }

        bool IActionAdapter.Logout()
        {
            throw new NotImplementedException();
        }

        bool IActionAdapter.RetrieveBook(Book book, out Dictionary<string, string> descriptor)
        {
            throw new NotImplementedException();
        }

        bool IActionAdapter.RetrieveBookNumber(Book book, out int count)
        {
            throw new NotImplementedException();
        }

        bool IActionAdapter.RetrieveDelayFee(Usercard card, out double fee)
        {
            throw new NotImplementedException();
        }

        bool IActionAdapter.RetrieveUser(User user, out Dictionary<string, string> descriptor)
        {
            throw new NotImplementedException();
        }

        bool IActionAdapter.WriteDataToStableStorage()
        {
            throw new NotImplementedException();
        }

        void IActionAdapter.Terminate()
        {
            throw new NotImplementedException();
        }
    }
}
