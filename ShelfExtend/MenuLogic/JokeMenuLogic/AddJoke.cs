using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShelfExtend.Entity;
using ShelfExtend.Entity.Extension;
using Dapper;

namespace ShelfExtend.MenuLogic.JokeMenuLogic
{
    public class AddJoke : MenuOption
    {
        public AddJoke(User user, UserType userType = UserType.None, TeachingLevel teachingLevel = TeachingLevel.None) : base(user, userType, teachingLevel)
        {
            Name = "Dodaj żart";
            UserType = UserType.Admin;
        }

        public override void Execute()
        {
            string Sqlstringaddjoke = $"INSERT INTO JokeBase VALUES(DEFAULT, )";

            //Get list of joke types
            string Sqlstringgetjokecategory = $"SELECT * FROM JokeType";
            List<JokeType> jokeTypes = new List<JokeType>();
            jokeTypes = PostgresConnection.GetListFromDB<JokeType>(Sqlstringgetjokecategory);
            if (jokeTypes == null || jokeTypes.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("Baza żartów jest pusta, nie można dodać żartu. Dodaj najpierw kategorie żartów nim opublikujesz żart.(Brak implementacji dodania kategori w aplikacji, potrzebna ingerencja w baze danych) Wciśnij dowolny przycisk by kontynuować.");
                Console.ReadKey();
                return;
            }

            //Printout categories of jokes 
            Console.Clear();
            Console.WriteLine("Wybierz do jakiej kategori chcesz dodac żart");
            Console.WriteLine("0. Anuluj");
            foreach (JokeType joke in jokeTypes)
            {
                Console.WriteLine($"{joke.category_joke_id}. {joke.joke_category}");
            }

            //ask user about type
            bool isVaild = int.TryParse(Console.ReadLine(), out int inputId);
            if (!isVaild)
            {
                Console.Clear();
                Console.WriteLine("Niepoprawna wartośc. Wciśnij dowolny kalwisz by kontynuować.");
                Console.ReadKey();
                return;
            }
            if (inputId == 0)
            {
                return;
            }

            JokeType jokeTypeChosen = jokeTypes.FirstOrDefault(type => type.category_joke_id == inputId);
            if (jokeTypeChosen == null)
            {
                Console.Clear();
                Console.WriteLine("Nie znaleziono kategorii. Wciśnij dowolny klawisz by kontynuować.");
                Console.ReadKey();
                return;
            }

            //ask user about content of joke
            Console.Clear();
            Console.WriteLine("Wpisz traść żartu.");
            string inputJokeMessage = Console.ReadLine();
            if (!inputJokeMessage.isStringLengthCorrect(5000) || string.IsNullOrEmpty(inputJokeMessage)) 
            {
                Console.Clear();
                Console.WriteLine("Wpisana treść jest nieprawidłowa, upewnij się że dobrze wprowadziłeś tekst. Wciśnij dowolny klawisz by kontynuować.");
                Console.ReadKey();
                return; 
            }

            //User confirms action 
            bool isExit = false;
            while (!isExit)
            {
                Console.Clear();
                Console.WriteLine("Czy jesteś pewny że chcesz dodać żart?");
                Console.WriteLine("0. Anuluj");
                Console.WriteLine("1. Potwierdzenie");

                int userConformationInput;
                bool isConformationValid = int.TryParse(Console.ReadLine(), out userConformationInput);
                if (!isConformationValid)
                {
                    Console.Clear();
                    Console.WriteLine("Wprowadzono nieprawidłową wartość. Wciśnij dowolny klawisz by kontynuować.");
                    Console.ReadKey();
                    continue;
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

            //insert joke
            using (var AddJokeConnection = PostgresConnection.EstablishConnection())
            {
                try
                {
                    AddJokeConnection.Execute($"INSERT INTO JokeBase VALUES(DEFAULT, '{inputJokeMessage}', {jokeTypeChosen.category_joke_id});");
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Nie udało się wysłać formularza. Wciśnij dowolny klawisz by kontynuować.");
                    Console.ReadKey();
                    return;
                }
                Console.Clear();
                Console.WriteLine("Rejestracja powiodła się. Wciśnij dowolny klawisz by kontynuować.");
                Console.ReadKey();
                return;
            }
        }
    }
}
