using ShelfExtend.Entity;
using ShelfExtend.MenuLogic.AdministrationMenuLogic;
using ShelfExtend.MenuLogic.JokeMenuLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShelfExtend.MenuLogic.AdministrationMenulogic
{
    public class AdministrationMenu : Menu
    {
        public static readonly string Name = "Menu Administracji";

        public AdministrationMenu(User user) : base(user)
        {
            _user = user;
            _menuOptions.Add(new AddAdministrator(_user));
            _menuOptions.Add(new RemoveAdministrator(_user));
            _menuOptions.Add(new BecomeAdmin(_user));
        }
    }
}
