using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShelfExtend.Entity;

namespace ShelfExtend.MenuLogic.LessonMenuLogic
{
    public class LessonMenu : Menu
    {
        public static readonly string Name = "Menu Korepetycji";

        public LessonMenu(User user) : base(user)
        {
            _menuOptions.Add(new SignIn(_user));
            _menuOptions.Add(new SignOut(_user));
            _menuOptions.Add(new BecomeTeacher(_user));
            _menuOptions.Add(new SignYourClass(_user));
            _menuOptions.Add(new RemoveYourClass(_user));  
        }
    }
}
