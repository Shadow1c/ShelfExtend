using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShelfExtend.Entity;

namespace ShelfExtend.MenuLogic.LibraryMenuLogic
{
    public class LibraryMenu : Menu
    {
        public static readonly string Name = "Menu Biblioteki";
        public LibraryMenu(User user) : base(user)
        {
            _menuOptions.Add(new BorrowBook(_user));
            _menuOptions.Add(new ReturnBook(_user));
            _menuOptions.Add(new CheckYourBooks(_user));
        }

        public static bool isCountBookTakenCorrect(User currentUser)
        {
            string sqlStringCountBooks = $"SELECT COUNT(*) FROM BookShelf WHERE user_id = {currentUser.User_id}";

            int? userCountBook = PostgresConnection.GetCount(sqlStringCountBooks);
            if (userCountBook == null)
            {
                return false;
            }
            else if (userCountBook >= 2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
