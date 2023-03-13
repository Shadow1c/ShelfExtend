using ShelfExtend.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ShelfExtend.MenuLogic.AdministrationMenuLogic
{
    public class BecomeAdmin : MenuOption
    {
        private const string _password = "admin";
        public BecomeAdmin(User user, UserType userType = UserType.None, TeachingLevel teachingLevel = TeachingLevel.None) : base(user, userType, teachingLevel)
        {
            Name = "Zostań administratorem";
            UserType = UserType.User;
        }

        public override void Execute()
        {
            Console.WriteLine("Wpisz hasło by zmienić poziom dotępu");
            if (!string.Equals(Console.ReadLine(), _password))
            {
                Console.Clear();
                Console.WriteLine("Wpisane hasło jest nieprawidłowe, wciśnij dowolny przycisk by kontynuować.");
                Console.ReadKey();
                return;
            }
            else
            {
                string SqlStringAddAdmin = $"UPDATE Users SET levelofaccess = 2 WHERE user_id = {_user.User_id}";

                using (var RegistrationConnection = PostgresConnection.EstablishConnection())
                {
                    try
                    {
                        RegistrationConnection.Execute($"{SqlStringAddAdmin}");
                    }
                    catch
                    {
                        Console.WriteLine("Nie udało się nadać praw. Wciśnij dowolny klawisz by kontynuować");
                        Console.ReadKey();
                        Console.Clear();
                        return;
                    }
                    Console.Clear();
                    Console.WriteLine("Przyznano prawa administratora. Wciśnij dowolny klawisz by kontynuować");
                    _user.Levelofaccess = UserType.Admin;
                    Console.ReadKey();
                }
            }
        }
    }
}
