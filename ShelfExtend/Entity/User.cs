using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShelfExtend.Entity
{
    public class User
    {
        public int? User_id { get; set; } = null;
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public TeachingLevel? Learningstatus { get; set; }
        public int? Location_id { get; set; }
        public UserType? Levelofaccess { get; set; } = UserType.User;

        public User(int user_id, string login, string password, string name, string surname, TeachingLevel learningstatus, int location_id)
        {
            User_id = user_id;
            Login = login;
            Password = password;
            Name = name;
            Surname = surname;
            Learningstatus = learningstatus;
            Location_id = location_id;
        }

        public User(string login, string password, string name, string surname, TeachingLevel learningstatus, int location_id)
        {
            Login = login;
            Password = password;
            Name = name;
            Surname = surname;
            Learningstatus = learningstatus;
            Location_id = location_id;
        }
        public User() 
        {

        }
    }

    public enum UserType
    {
        None,
        User,
        Admin
    }

    public enum TeachingLevel
    {
        None,
        Student,
        Teacher
    }
}
