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
            bool isExit = false;
            while (!isExit) 
            {
                Console.WriteLine("Po potwierdzeniu zdobędziesz rangę nauczyciela korepetycji bez możliwości powrotu");
                Console.WriteLine("Czy jesteś pewny wyboru?");
                Console.WriteLine("0. Anuluj");
                Console.WriteLine("1. Potwierdzam wybór!");

                int userConformationInt;
                bool isValid = int.TryParse(Console.ReadLine(), out userConformationInt);
                if (!isValid) 
                {
                    Console.Clear();
                    Console.WriteLine("Wpisano niepoprawną wartośc. Wciśnij dowolny klawisz by kontynuować.");
                    Console.ReadKey();
                    continue;
                }

                switch (userConformationInt)
                {
                    case 0:
                        return;
                    case 1:
                        isExit = true;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Wpisano nieprawidłową wartość. Wciśnij dowolny klawisz by kontynuować.");
                        Console.ReadKey();
                        break;
                }
            }

            string Sqlstringstatuschange = $"UPDATE Users SET learningstatus = 2 WHERE user_id = {_user.User_id};";
            if (PostgresConnection.isExecutSqlString(Sqlstringstatuschange))
            {
                Console.Clear();
                Console.WriteLine("Zostałeś nauczycielem. Wciśnij dowolny klawisz by kontynuować.");
                _user.Learningstatus = TeachingLevel.Teacher;
                Console.ReadKey();
            }
            else 
            { 
                Console.Clear();
                Console.WriteLine("Nie udało się zostać nauczycielem, Wcisnij dowolny klawisz by kontynuować.");
                Console.ReadKey();
            }
        }
    }
}
