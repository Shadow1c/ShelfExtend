using ShelfExtend.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShelfExtend.MenuLogic.LessonMenuLogic
{
    public class CheckAssigment : MenuOption
    {
        public CheckAssigment(User user, UserType userType = UserType.None, TeachingLevel teachingLevel = TeachingLevel.None) : base(user, userType, teachingLevel)
        {
            Name = "Sprawdz kto cię uczy";
        }

        public override void Execute()
        {
            string Sqlstringcheckyourlesson = $"SELECT user_id, name, surname, SchoolSubjects.subject_name, SchoolSubjects.id_subject FROM Users JOIN Teachers ON Teachers.teacher_id = users.user_id JOIN SchoolSubjects ON SchoolSubjects.id_subject = Teachers.id_subject WHERE Teachers.student_id = {_user.User_id};";


            List<StudentTeachers>? StudentTeachers = PostgresConnection.GetListFromDB<StudentTeachers>(Sqlstringcheckyourlesson);
            if (StudentTeachers == null || StudentTeachers.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("Jeszcze nigdzie nie jesteś zapisany. Wciśnij dowolny klawisz by kontynuować.");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            foreach (StudentTeachers teacher in StudentTeachers)
            {
                Console.WriteLine($"{teacher.name} {teacher.surname} uczy cię przedmiotu {teacher.subject_name}");
            }
        }
    }
}
