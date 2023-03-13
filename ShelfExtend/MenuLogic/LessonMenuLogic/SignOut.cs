using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShelfExtend.Entity;
using Dapper;

namespace ShelfExtend.MenuLogic.LessonMenuLogic
{
    public class SignOut : MenuOption
    {
        public SignOut(User user, UserType userType = UserType.None, TeachingLevel teachingLevel = TeachingLevel.None) : base(user, userType, teachingLevel)
        {
            Name = "Wypisz się z zajęć";
        }
        public override void Execute()
        {
            string Sqlstringgetyourlesson = $"SELECT user_id, name, surname, SchoolSubjects.subject_name, SchoolSubjects.id_subject FROM Users JOIN Teachers ON Teachers.teacher_id = users.user_id JOIN SchoolSubjects ON SchoolSubjects.id_subject = Teachers.id_subject WHERE Teachers.student_id = {_user.User_id};";

            //Get list of assign classes
            List<StudentTeachers>? TeachersOfStudent = PostgresConnection.GetListFromDB<StudentTeachers>(Sqlstringgetyourlesson);
            if (TeachersOfStudent == null || TeachersOfStudent.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("Nigdzie jeszcze nie jesteś zapisany. Wciśnij dowolny klawisz by kontynuować");
                Console.ReadKey();
                return;
            }

            //Make user choose which class wants to remove
            bool isUserExit = false;
            while (!isUserExit)
            {
                Console.Clear();
                Console.WriteLine("Wybierz które zajęcia chcesz usunąć");
                Console.WriteLine("0. Anuluj kasowanie");
                foreach (StudentTeachers Teacher in TeachersOfStudent)
                {
                    Console.WriteLine($"{TeachersOfStudent.IndexOf(Teacher) + 1}. Korepetytor to {Teacher.name} {Teacher.surname}, uczy cię {Teacher.subject_name}");
                }

                string? userInput = Console.ReadLine();
                bool isValid = int.TryParse(userInput, out int userInputInt);
                if (!isValid || userInputInt > TeachersOfStudent.Count || userInputInt < 0)
                {
                    Console.Clear();
                    Console.WriteLine("Błędna wartość. Wciśnij dowolny klawisz by kontynuować.");
                    Console.ReadKey();
                    continue;
                }
                if (userInputInt == 0)
                {
                    return;
                }
                StudentTeachers PickedStudent = TeachersOfStudent.ElementAt(userInputInt - 1);

                //Update database 
                string Sqlstringdelitelesson = $"UPDATE Teachers SET student_id = NULL WHERE teacher_id = {PickedStudent.user_id} AND student_id = {_user.User_id} AND id_subject = {PickedStudent.id_subject};";

                using (var RegistrationConnection = PostgresConnection.EstablishConnection())
                {
                    try
                    {
                        RegistrationConnection.Execute($"{Sqlstringdelitelesson}");
                    }
                    catch
                    {
                        Console.Clear();
                        Console.WriteLine("Nie udało się usunąć. Wciśnij dowolny klawisz by kontynuować.");
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
}
