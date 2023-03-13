using ShelfExtend.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ShelfExtend.MenuLogic.AdministrationMenuLogic
{
    public class AddAdministrator : MenuOption
    {
        public AddAdministrator(User user, UserType userType = UserType.None, TeachingLevel teachingLevel = TeachingLevel.None) : base(user, userType, teachingLevel)
        {
            Name = "Dodaj Administratora";
            UserType = UserType.Admin;
        }

        public override void Execute()
        {
            //Get list of all users with access user
            string SqlStrigGetListOfUsers = $"SELECT * FROM Users WHERE levelofaccess = 1 OR levelofaccess = 0";
            List<User> ListUsers = PostgresConnection.GetListFromDB<User>(SqlStrigGetListOfUsers);
            if (ListUsers == null || ListUsers.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("Lista użytkowników jest pusta. Wciśnij dowolny klawisz by kontynuować");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine("Lista użytkowników");
            Console.WriteLine("0. Anuluj");
            foreach (User User in ListUsers)
            {
                Console.WriteLine($"{ListUsers.IndexOf(User) + 1}. {User.Name} {User.Surname} {User.Levelofaccess}");
            }

            bool isValid = int.TryParse(Console.ReadLine(), out int userInputInt);
            if (ListUsers.Count < userInputInt || userInputInt < 0)
            {
                Console.Clear();
                Console.WriteLine("Wpisana zła wartość. Wciśnij dowolny klawisz by kontynuować.");
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
                Console.WriteLine("Czy jesteś pewny że chesz przyznać prawa administratora :");
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

            //Set Admin access
            string SqlStringAddAdmin = $"UPDATE Users SET levelofaccess = 2 WHERE user_id = {PickedUser.User_id}";

            using (var RegistrationConnection = PostgresConnection.EstablishConnection())
            {
                try
                {
                    RegistrationConnection.Execute($"{SqlStringAddAdmin}");
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Nie udało się nadać praw. Wciśnij dowolny klawisz by kontynuować");
                    Console.ReadKey();
                    return;
                }
                Console.Clear();
                Console.WriteLine("Zmieniono prawa na administratora. Wciśnij dowolny klawisz by kontynuować");
                Console.ReadKey();
                return;
            }
        }
    }
}
