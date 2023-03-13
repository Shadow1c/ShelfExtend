using ShelfExtend.Entity;
using ShelfExtend.Entity.Extension;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using Dapper;

namespace ShelfExtend.MenuLogic.LoginMenuLogic
{
    public class Registration
    {
        private User? _user { get; set; } = null;

        public void MenuRegistration()
        {
            GetRegistrationFormula();
            if (_user == null)
            {
                return;
            }
            
            Console.Clear();
            SendFormula(_user);
        }

        private void GetRegistrationFormula()
        {
            bool isExit = false;
            while (!isExit) 
            {
                //User fill registration formula
                //Send formula to DB
                //DB check if user login is already taken
                //if its free write user to DB and return true
                Console.Clear();
                Console.WriteLine("Podaj login nowego użytkownika");
                string? inputLogin = Console.ReadLine();
                if (string.IsNullOrEmpty(inputLogin) || !inputLogin.isStringLengthCorrect(40))
                {
                    Console.WriteLine("Nieprawidłowa wpisana wartość. Wcisnij dowolny klawisz by kontynuować");
                    Console.ReadKey();
                    continue;
                }

                Console.WriteLine("Podaj hasło");
                string? inputPassword = Console.ReadLine();
                if (string.IsNullOrEmpty(inputPassword) || !inputPassword.isStringLengthCorrect(40))
                {
                    Console.WriteLine("Nieprawidłowa wpisana wartość. Wcisnij dowolny klawisz by kontynuować");
                    Console.ReadKey();
                    continue;
                }

                Console.WriteLine("Podaj Imię");
                string? inputName = Console.ReadLine();
                if (string.IsNullOrEmpty(inputName) || !inputName.isStringLengthCorrect(40))
                {
                    Console.WriteLine("Nieprawidłowa wpisana wartość. Wcisnij dowolny klawisz by kontynuować");
                    Console.ReadKey();
                    continue;
                }

                Console.WriteLine("Podaj Nazwisko");
                string? inputSurname = Console.ReadLine();
                if (string.IsNullOrEmpty(inputSurname) || !inputSurname.isStringLengthCorrect(40))
                {
                    Console.WriteLine("Nieprawidłowa wpisana wartość. Wcisnij dowolny klawisz by kontynuować");
                    Console.ReadKey();
                    continue;
                }

                Console.WriteLine("Wpisz 1 jeżeli jesteś uczniem ,2 jeżeli jesteś nauczycielem");
                string? inputStatus = Console.ReadLine();
                TeachingLevel StatusConverted;
                bool isEnum = Enum.TryParse<TeachingLevel>(inputStatus, out StatusConverted);
                if (!isEnum) 
                {
                    Console.WriteLine("Nieprawidłowa wpisana wartość. Wcisnij dowolny klawisz by kontynuować");
                    Console.ReadKey();
                    continue; 
                }

                if (!isCityTakenFromDB() || Location.All.Count == 0)
                {
                    Console.WriteLine("Błąd Połączenia lub lista miast pusta ,skontaktuj się z administrotorem sieci. Wcisnij dowolny klawisz by kontynuować.");
                    Console.ReadKey();
                    return;
                }

                PrintOutCities();

                string? inputLocation = Console.ReadLine();
                int Locationint;
                bool isNumber = int.TryParse(inputLocation, out Locationint);
                if (!isNumber)
                {
                    Console.WriteLine("Nieprawidłowy numer miasta. Wcisnij dowolny klawisz by kontynuować");
                    Console.ReadKey();
                    continue;
                }
                if (!isListContainsInput(Locationint))
                {
                    Console.WriteLine("Brak miasta w bazie. Wciśnij dowolny klawisz by kontynuować.");
                    Console.ReadKey();
                    continue;
                }
                User User = new User(inputLogin, inputPassword, inputName, inputSurname, StatusConverted, Locationint);

                Console.Clear();
                Console.WriteLine("Sprawdz czy wprowadzone dane zgadzają się");
                Console.WriteLine($"Login: {User.Login}");
                Console.WriteLine($"Hasło: {User.Password}");
                Console.WriteLine($"Imię: {User.Name}");
                Console.WriteLine($"Nazwisko: {User.Surname}");
                Console.WriteLine($"Status Szkolny: {User.Learningstatus}");
                Console.WriteLine($"Miasto: {Location.All[Locationint-1].City}");

                Console.WriteLine("0. Anuluj wprowadzanie.");
                Console.WriteLine("1. Potwierdzam poprawność danych.");
                Console.WriteLine("2. Wprowadz dane jeszcze raz.");

                int userConformationInput;
                bool isValidConformation = int.TryParse(Console.ReadLine(),out userConformationInput);
                if (!isValidConformation)
                {
                    Console.Clear();
                    Console.WriteLine("Nieprawidłowa wpisana wartość. Wcisnij dowolny klawisz by kontynuować");
                    Console.ReadKey();
                    continue;
                }
                switch (userConformationInput)
                {
                    case 0:
                        return;
                    case 1: 
                        _user = User;
                        return;
                    case 2:
                        continue;
                    default:
                        Console.Clear();
                        Console.WriteLine("Wpisano nieprawidłową wartość. Wciśnij dowolny klawisz by kontynuować");
                        Console.ReadKey();
                        continue;
                }
            }
        }

        private void PrintOutCities()
        {
            Console.WriteLine("Wpisz z jakiego miasta się logujesz");
            foreach (var list in Location.All)
            {
                Console.WriteLine($"{list.Location_id} to {list.City}");
            }
        }
        private void SendFormula(User currentUser)
        {
            using (var RegistrationConnection = PostgresConnection.EstablishConnection())
            {
                try
                {
                    RegistrationConnection.Execute("INSERT INTO Users VALUES(DEFAULT, @Login, @Password, @Name, @Surname, @LearningStatus, @location_id, @LevelOfAccess)", currentUser);
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Nie udało się wysłać formularza. Wciśnij dowolny klawisz by kontynuować");
                    Console.ReadKey();
                    return;
                }
                Console.Clear();
                Console.WriteLine("Rejestracja powiodła się. Wciśnij dowolny klawisz by kontynuować");
                Console.ReadKey();
                return;
            }
        }
        private bool isCityTakenFromDB()
        {
            using (var RegistrationConnection = PostgresConnection.EstablishConnection())
            {
                try
                {
                    Location.All = RegistrationConnection.Query<Location>($"SELECT * FROM location").ToList();
                }
                catch
                {
                    Location.All.Clear();
                    return false;
                }
                return true;
            }
        }
        private bool isListContainsInput(int input)
        {
            foreach (var list in Location.All)
            {
                if (list.Location_id.Equals(input))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
