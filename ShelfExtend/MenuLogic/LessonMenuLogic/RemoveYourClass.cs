using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShelfExtend.Entity;
using Dapper;

namespace ShelfExtend.MenuLogic.LessonMenuLogic
{
    public class RemoveYourClass : MenuOption
    {
        public RemoveYourClass(User user, UserType userType = UserType.None, TeachingLevel teachingLevel = TeachingLevel.None) : base(user, userType, teachingLevel)
        {
            Name = "Usuń zajęcia których uczysz";
            TeachingLevel = TeachingLevel.Teacher;
        }
        public override void Execute()
        {
            string getListOfClassQuery = $"SELECT Users.user_id, Users.name, Users.surname, SchoolSubjects.id_subject, SchoolSubjects.subject_name FROM Teachers JOIN Users ON Teachers.student_id = Users.user_id JOIN SchoolSubjects ON Teachers.id_subject = SchoolSubjects.id_subject WHERE Teachers.teacher_id = {_user.User_id} AND Teachers.student_id IS NOT NULL;";

            List<TeacherClasses> ListOfCurrentClasses = PostgresConnection.GetListFromDB<TeacherClasses>(getListOfClassQuery);

            if (ListOfCurrentClasses == null || ListOfCurrentClasses.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("Nie masz jeszcze zaplanowanych żadnych korepetycji. Wciśnij dowolny klawisz by kontynuować");
                Console.ReadKey();
                return;
            }

            //Printout all users classes where user is as teacher
            Console.Clear();
            Console.WriteLine("Wybierz które zajęcia chcesz usunąć");
            Console.WriteLine("0. Anuluj");
            foreach (TeacherClasses classes in ListOfCurrentClasses)
            {
                Console.WriteLine($"{ListOfCurrentClasses.IndexOf(classes) + 1}. {classes.Name} {classes.Surname} uczysz go {classes.subject_name}");
            }


            //Take user input 
            string? userInput = Console.ReadLine();
            bool isValid = int.TryParse(userInput, out int userInputInt);
            if (!isValid || userInputInt > ListOfCurrentClasses.Count || userInputInt < 0)
            {
                Console.Clear();
                Console.WriteLine("Nie wybrano nauczyciela. Wciśnij dowolny klawisz by kontynuować.");
                Console.ReadKey();
                return;
            }
            if (userInputInt == 0)
            {
                return;
            }

            //Assign user choice
            TeacherClasses PickedClass = ListOfCurrentClasses.ElementAt(userInputInt - 1);

            //Let user confirm his choice
            bool isPickedAnswer = false;
            while (!isPickedAnswer)
            {
                Console.Clear();
                Console.WriteLine($"Czy jesteś pewny że chcesz usunąć zajęcia z {PickedClass.Name} {PickedClass.Surname} którego uczysz {PickedClass.subject_name}?");
                Console.WriteLine("Wpisz 1 by potwierdzić.");
                Console.WriteLine("Wpisz 0 by anulować.");

                int userInputConformation;
                bool isConfirmed = int.TryParse(Console.ReadLine(), out userInputConformation);

                switch (userInputConformation)
                {
                    case 0:
                        return;
                    case 1:
                        isPickedAnswer = true;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Wpisana wartość nie poprawna. Wciśnij dowolny klawisz by kontynuować");
                        Console.ReadKey();
                        break;
                }
            }

            //Delete class
            string deliteUserClass = $"DELETE FROM Teachers WHERE Teachers.teacher_id = {_user.User_id} AND Teachers.student_id = {PickedClass.User_id} AND Teachers.id_subject = {PickedClass.id_subject};";
            using (var RegistrationConnection = PostgresConnection.EstablishConnection())
            {
                try
                {
                    RegistrationConnection.Execute($"{deliteUserClass}");
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Nie udało się Usunąc zajęć. Wciśnij dowolny klawisz by kontynuować.");
                    Console.ReadKey();
                    return;
                }
                Console.Clear();
                Console.WriteLine("Usunięcie powiodło się. Wciśnij dowolny klawisz by kontynuować.");
                Console.ReadKey();
                return;
            }
        }
    }
}
