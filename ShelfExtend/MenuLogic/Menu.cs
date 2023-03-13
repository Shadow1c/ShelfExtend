using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using ShelfExtend.Entity;

namespace ShelfExtend.MenuLogic
{
    public abstract class Menu
    {
        public User? _user { get; protected set; }
        protected List<MenuOption>? _menuOptions = new List<MenuOption>();

        public Menu(User user)
        {
            _user = user;
        }

        public void Execute()
        {
            ChooseOption();
        }

        protected List<MenuOption>? PrintOptions()
        {
            List<MenuOption>? optionForCurrentUser = new List<MenuOption>();

            Console.Clear();
            foreach (MenuOption option in _menuOptions) 
            {
                if ((option.UserType == 0 || option.UserType == _user.Levelofaccess) && (option.TeachingLevel == 0 || option.TeachingLevel == _user.Learningstatus))
                {
                    optionForCurrentUser.Add(option);

                    Console.WriteLine($"{optionForCurrentUser.Count}. {option.Name}");
                }
            }
            return optionForCurrentUser;
        }

        protected void ChooseOption()
        {
            bool isExit = false;
            while(!isExit)
            {
                Console.Clear();
                List<MenuOption>? chooseOption = PrintOptions();
                if (chooseOption == null)
                {
                    Console.Clear();
                    Console.WriteLine("Brak opcji w tym menu z twoimi obecnymi uprawnieniami. Wciśnij dowolny przycisk by kontynuować.");
                    Console.ReadKey();
                    continue;
                }
                Console.WriteLine("0. Wróć");


                int userInput;
                bool isInputValid = int.TryParse(Console.ReadLine(), out userInput);

                if (!isInputValid || chooseOption.Count < userInput || userInput < 0)
                {
                    Console.Clear();
                    Console.WriteLine("Wpisana wartość jest nie prawidłowa. Wciśnij dowolny klawisz by kontynuować.");
                    Console.ReadKey();
                    continue;
                }

                if (userInput == 0)
                {
                    return;
                }

                Console.Clear();
                chooseOption[userInput-1].Execute();
            }
        }
    }
}
