using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShelfExtend.Entity;

namespace ShelfExtend.MenuLogic
{
    public abstract class MenuOption
    {
        public User? _user { get; set; } = null;
        public string? Name { get; set; }
        public UserType UserType { get; set; }
        public TeachingLevel TeachingLevel { get; set; }

        public MenuOption(User user, UserType userType = 0, TeachingLevel teachingLevel = 0)
        {
            _user = user;
            UserType = userType;
            TeachingLevel = teachingLevel;
        }

        public abstract void Execute();
    }
}
