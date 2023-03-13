using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShelfExtend.Entity;
using Dapper;

namespace ShelfExtend.MenuLogic.LessonMenuLogic
{
    public class SignIn : MenuOption
    {
        public SignIn(User user, UserType userType = UserType.None, TeachingLevel teachingLevel = TeachingLevel.None) : base(user, userType, teachingLevel)
        {
            Name = "Zapisz się na zajęcia";
        }
        public override void Execute()
        {
            //Check for available subject
            string Sqlstringgetsubjects = $"SELECT * FROM SchoolSubjects;";
            List<SchoolSubjects>? ListSubjects = PostgresConnection.GetListFromDB<SchoolSubjects>(Sqlstringgetsubjects);

            //Look For errors
            if (ListSubjects == null || ListSubjects.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("Nie ma aktualnie żadnego przedmiotu do wybrania. Wciśnij dowolny klawisz by kontynuować.");
                Console.ReadKey();
                return;
            }
            //

            //Printout all available Subjects
            Console.Clear();
            Console.WriteLine("Wybier jakiego przedmiotu chcesz się uczyć z niżej wymienionych:");
            foreach (SchoolSubjects subjects in ListSubjects)
            {
                Console.WriteLine($"{subjects.id_subject}. {subjects.subject_name}");
            }
            Console.WriteLine("0. Anuluj");

            //Ask about subject value and check errors
            string? inputUser = Console.ReadLine();
            bool isValid = int.TryParse(inputUser, out int inputUserInt);
            if (!isValid)
            {
                Console.Clear();
                Console.WriteLine("Wpisana niepoprawna wartość. Wiciśnij dowolny klawisz by kontynuować.");
                Console.ReadKey();
                return;
            }

            if (inputUserInt == 0) { return; }
            SchoolSubjects? pickedsubject = ListSubjects.FirstOrDefault<SchoolSubjects>(sub => sub.id_subject == inputUserInt);
            if (pickedsubject == null)
            {
                Console.Clear();
                Console.WriteLine("Nie udało się pobrać wybranego przedmiotu. Wciśnij dowolny klawisz by kontynuować.");
                Console.ReadKey();
                return;
            }
            //

            //Get all available teachers who learn picked subject
            string Sqlstringavailableteachersubject = $"SELECT users.user_id, users.login, users.password, users.name, users.surname, users.learningstatus, users.location_id, users.levelofaccess " +
                                                      $"FROM Users RIGHT JOIN (SELECT Foo.teacher_id " +
                                                      $"FROM (SELECT teacher_id, COUNT(student_id) AS StudentCount FROM Teachers WHERE Teachers.id_subject = {pickedsubject.id_subject} AND Teachers.teacher_id <> '{_user.User_id}' GROUP BY teacher_id HAVING COUNT(student_id) < 3 ORDER BY teacher_id) AS Foo) " +
                                                      $"AS Faa ON user_id = Faa.teacher_id;";
            List<User>? teachersList = PostgresConnection.GetListFromDB<User>(Sqlstringavailableteachersubject);
            if (teachersList == null || teachersList.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("Nie ma aktualnie wolnego korepetytora dla tego przedmiotu. Wciśnij dowolny klawisz by kontynuować");
                Console.ReadKey();
                return;
            }
            //

            //Ask user to pick teacher for subject
            Console.Clear();
            Console.WriteLine($"Wybierz nauczyciela z którym chcesz uczyć się przedmiotu {pickedsubject.subject_name}:");
            foreach (User teacher in teachersList)
            {
                Console.WriteLine($"{teachersList.IndexOf(teacher) + 1}.{teacher.Name} {teacher.Surname}");
            }
            Console.WriteLine("0. Anuluj");


            //Take user input 
            string? userInput = Console.ReadLine();
            bool isValid3 = int.TryParse(userInput, out int userInputInt);
            if (!isValid3 || userInputInt > teachersList.Count || userInputInt < 0)
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
            User PickedTeacher = teachersList.ElementAt(userInputInt - 1);

            //Let user confirm his choice
            bool isPickedAnswer = false;
            while (!isPickedAnswer)
            {
                Console.Clear();
                Console.WriteLine($"Wybrany nauczyciel to {PickedTeacher.Name} {PickedTeacher.Surname}");
                Console.WriteLine("Wpisz 1 by potwierdzić.");
                Console.WriteLine("Wpisz 0 by anulować.");

                int userInputConformation;
                bool isConfirmed = int.TryParse(Console.ReadLine(), out userInputConformation);

                if (!isConfirmed) 
                {
                    Console.Clear();
                    Console.WriteLine("Wpisana wartość nie poprawna. Wciśnij dowolny klawisz by kontynuować");
                    Console.ReadKey();
                    continue;
                }
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
            //

            //Insert into database
            string Sqlstringinsertstudent = $"INSERT INTO Teachers VALUES ({PickedTeacher.User_id}, {_user.User_id}, {pickedsubject.id_subject});";
            using (var RegistrationConnection = PostgresConnection.EstablishConnection())
            {
                try
                {
                    RegistrationConnection.Execute($"{Sqlstringinsertstudent}");
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Nie udało się zapisać. Wciśnij dowolny klawisz by kontynuować.");
                    Console.ReadKey();
                    return;
                }
                Console.Clear();
                Console.WriteLine("Zapisanie powiodło się. Wciśnij dowolny klawisz by kontynuować.");
                Console.ReadKey();
                return;
            }
        }
    }
}
