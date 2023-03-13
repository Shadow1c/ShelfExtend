using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShelfExtend.Entity;

namespace ShelfExtend.MenuLogic.LessonMenuLogic
{
    public class BecomeTeacher : MenuOption
    {
        public BecomeTeacher(User user, UserType userType = UserType.None, TeachingLevel teachingLevel = TeachingLevel.None) : base(user, userType, teachingLevel)
        {
            Name = "Zostań nauczycielem";
            TeachingLevel = TeachingLevel.Student;
        }

        public override void Execute()
        {
            string Sqlstringstatuschange = $"UPDATE Users SET learningstatus = 2 WHERE user_id = {_user.User_id};";
            if (PostgresConnection.isExecutSqlString(Sqlstringstatuschange))
            {
                _user.Learningstatus = TeachingLevel.Teacher;
            }
        }
    }
}
