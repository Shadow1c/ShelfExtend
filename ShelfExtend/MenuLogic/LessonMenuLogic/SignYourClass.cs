using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShelfExtend.Entity;
using Dapper;

namespace ShelfExtend.MenuLogic.LessonMenuLogic
{
    public class SignYourClass : MenuOption
    {
        public SignYourClass(User user, UserType userType = UserType.None, TeachingLevel teachingLevel = TeachingLevel.None) : base(user, userType, teachingLevel)
        {
            Name = "Dodaj przedmiot który będziesz uczysz";
            TeachingLevel = TeachingLevel.Teacher;
        }
        public override void Execute()
        {
            //Get list of subject 
            string Sqlstringgetsubjects = $"SELECT * FROM SchoolSubjects WHERE id_subject NOT IN (SELECT id_subject FROM Teachers WHERE teacher_id = {_user.User_id});";
            List<SchoolSubjects>? schoolSubjects = PostgresConnection.GetListFromDB<SchoolSubjects>(Sqlstringgetsubjects);
            if (schoolSubjects == null || schoolSubjects.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("Brak Listy przedmiotów na serwerze. Wciśnij dowolny klawisz by kontynuować.");
                Console.ReadKey();
                return;
            }

            //Ask user to pick subject which will be teaching
            Console.Clear();
            Console.WriteLine("Który przedmiot chcesz uczyć?");
            foreach (SchoolSubjects subject in schoolSubjects)
            {
                Console.WriteLine($"{schoolSubjects.IndexOf(subject) + 1}. {subject.subject_name}");
            }
            Console.WriteLine("0. Anuluj");

            string? userInput = Console.ReadLine();
            bool isVaild = int.TryParse(userInput, out int userInputInt);
            if (!isVaild || userInputInt > schoolSubjects.Count || userInputInt < 0)
            {
                Console.Clear();
                Console.WriteLine("Wpisana wartość jest nie poprawna. Wciśnij dowolny klawisz by kontynuować.");
                Console.ReadKey();
                return;
            }
            if (userInputInt == 0)
            {
                return;
            }

            SchoolSubjects subjects = schoolSubjects.ElementAt(userInputInt - 1);
            string Sqlstringaddsubject = $"INSERT INTO Teachers VALUES({_user.User_id}, NULL, {subjects.id_subject})";

            using (var RegistrationConnection = PostgresConnection.EstablishConnection())
            {
                try
                {
                    RegistrationConnection.Execute($"{Sqlstringaddsubject}");
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Dodanie przedmiotu nie powioło się. Wciśnij dowolny klawisz by kontynuować.");
                    Console.ReadKey();
                    return;
                }
                Console.Clear();
                Console.WriteLine("Dodanie przedmiotu powiodło się. Wciśnij dowolny klawisz by kontynuować.");
                Console.ReadKey();
                return;
            }
        }
    }
}
