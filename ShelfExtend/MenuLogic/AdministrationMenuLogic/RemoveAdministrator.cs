using ShelfExtend.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ShelfExtend.MenuLogic.AdministrationMenuLogic
{
    public class RemoveAdministrator : MenuOption
    {
        public RemoveAdministrator(User user, UserType userType = UserType.None, TeachingLevel teachingLevel = TeachingLevel.None) : base(user, userType, teachingLevel)
        {
            Name = "Usuń Administratora";
            UserType = UserType.Admin;
        }

        public override void Execute()
        {
            //Get Admin list
            string SqlStrigGetListOfUsers = $"SELECT * FROM Users WHERE levelofaccess = 2";
            List<User> ListUsers = PostgresConnection.GetListFromDB<User>(SqlStrigGetListOfUsers);
            if (ListUsers == null || ListUsers.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("Nie znaleziono administratorów. Wciśnij dowolny klawisz by kontynuować.");
                Console.ReadKey();
                return;
            }

            //Printout list of admins
            Console.WriteLine("Lista użytkowników");
            Console.WriteLine("0. Anuluj");
            foreach (User User in ListUsers)
            {
                Console.WriteLine($"{ListUsers.IndexOf(User) + 1}. {User.Name} {User.Surname}");
            }

            bool isValid = int.TryParse(Console.ReadLine(), out int userInputInt);
            if (ListUsers.Count < userInputInt || userInputInt < 0)
            {
                Console.Clear();
                Console.WriteLine("Wpisano złą wartość. Wciśnij dowolny klawisz by kontynuować.");
                Console.ReadKey();
                return;
            }
            if (userInputInt == 0)
            {
                return;
            }

            User PickedUser = ListUsers.ElementAt(userInputInt - 1);

            //User confirms action 
            bool isExit = false;
            while (!isExit)
            {
                Console.Clear();
                Console.WriteLine("Czy jesteś pewny że chcesz usunąć administratora:");
                Console.WriteLine($"{PickedUser.Name} {PickedUser.Surname}, Id {PickedUser.User_id}, login:{PickedUser.Login} ");
                Console.WriteLine("0. Anuluj");
                Console.WriteLine("1. Potwierdzenie");
                int userConformationInput;
                bool isConformationValid = int.TryParse(Console.ReadLine(), out userConformationInput);
                if (!isConformationValid)
                {
                    Console.Clear();
                    Console.WriteLine("Wprowadzono nieprawidłową wartość. Wciśnij dowolny klawisz by kontynuować.");
                    Console.ReadKey();
                    break;
                }

                switch (userConformationInput)
                {
                    case 0:
                        return;
                    case 1:
                        isExit = true;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Wprowadzono nieprawidłową wartość. Wciśnij dowolny klawisz by kontynuować.");
                        Console.ReadKey();
                        break;
                }
            }

            //Remove Administrator
            string SqlStringDeliteUser = $"UPDATE Users SET levelofaccess = 1 WHERE user_id = {PickedUser.User_id}";

            using (var RegistrationConnection = PostgresConnection.EstablishConnection())
            {
                try
                {
                    RegistrationConnection.Execute($"{SqlStringDeliteUser}");
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Nie udało się zminić praw");
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }
                Console.Clear();
                Console.WriteLine("Zmieniono prawa dostępu na użytkownik");
                if (_user.User_id == PickedUser.User_id)
                {
                    _user.Levelofaccess = UserType.User;
                }
                Console.ReadKey();
            }
        }
    }
}
