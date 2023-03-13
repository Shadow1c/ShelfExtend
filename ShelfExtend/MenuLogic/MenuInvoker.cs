using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using ShelfExtend.Entity;
using ShelfExtend.MenuLogic.JokeMenuLogic;
using ShelfExtend.MenuLogic.LessonMenuLogic;
using ShelfExtend.MenuLogic.LibraryMenuLogic;
using ShelfExtend.MenuLogic.LoginMenuLogic;
using ShelfExtend.MenuLogic.AdministrationMenulogic;

namespace ShelfExtend.MenuLogic
{
    public class MenuInvoker
    {
        private User? _user { get; set; }

        public void InvokeMenu()
        {
            bool isExit = false;
            while (!isExit)
            {
                _user = null;
                MenuBeforLogin();
                MenuAfterLogin();
            }
        }
        private void MenuBeforLogin()
        {
            bool isExit = false;
            while (!isExit)
            {
                Console.Clear();
                Console.WriteLine("0. Wyjdz z aplikacji");
                Console.WriteLine("1. Rejestracja");
                Console.WriteLine("2. Logowanie");
                Console.WriteLine("Wpisz numer opcji by kontynuować");

                int userInput;
                bool isValidInput = int.TryParse(Console.ReadLine(), out userInput);
                if (!isValidInput)
                {
                    Console.Clear();
                    Console.WriteLine("Wpisana wartość nieprawidłowa, wciśnij dowolny klawisz by kontynuować...");
                    Console.ReadKey();
                    continue;
                }

                User? logedUser = null;
                switch (userInput)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        new Registration().MenuRegistration();
                        break;
                    case 2:
                        logedUser = new Login().MenuLogin();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Wpisana wartość nieprawidłowa, wciśnij dowolny klawisz by kontynuować...");
                        Console.ReadKey();
                        break;
                }

                //If method return user data from DB assign date to _user and exit current method.
                if (logedUser != null)
                {
                    _user = logedUser;
                    return;
                }
            }
        }
        private void MenuAfterLogin()
        {
            bool isExit = false;
            while (!isExit)
            {
                Console.Clear();
                Console.WriteLine("Wybierz z poniższych menu");
                Console.WriteLine("0. Wyloguj");
                Console.WriteLine($"1. {JokeMenu.Name}");
                Console.WriteLine($"2. {LessonMenu.Name}");
                Console.WriteLine($"3. {LibraryMenu.Name}");
                Console.WriteLine($"4. {AdministrationMenu.Name}");

                int userValueNumber;
                bool isValidInput = int.TryParse(Console.ReadLine(), out userValueNumber);
                if (!isValidInput) 
                {
                    Console.Clear();
                    Console.WriteLine("Wprawdziłeś złą wartość. Wciśnij dowlny klawisz by kontynuować.");
                    Console.ReadKey();
                }

                switch (userValueNumber) 
                {
                    case 0:
                        return;
                    case 1:
                        new JokeMenu(_user).Execute();
                        break;
                    case 2:
                        LessonMenu lessonMenu = new LessonMenu(_user);
                        lessonMenu.Execute();
                        lessonMenu._user.Learningstatus = _user.Learningstatus;
                        break;
                    case 3:
                        new LibraryMenu(_user).Execute();
                        break;
                    case 4:
                        AdministrationMenu administrationMenu = new AdministrationMenu(_user);
                        administrationMenu.Execute();
                        administrationMenu._user.Levelofaccess = _user.Levelofaccess;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Wprawdziłeś złą wartość. Wciśnij dowlny klawisz by kontynuować.");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
