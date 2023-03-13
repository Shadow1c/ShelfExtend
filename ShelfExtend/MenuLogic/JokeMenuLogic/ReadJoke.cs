using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShelfExtend.Entity;

namespace ShelfExtend.MenuLogic.JokeMenuLogic
{
    public class ReadJoke : MenuOption
    {
        public ReadJoke(User user, UserType userType = UserType.None, TeachingLevel teachingLevel = TeachingLevel.None) : base(user, userType, teachingLevel)
        {
            Name = "Przeczytaj Żart";
        }

        public override void Execute()
        {
            //Get list of joke type
            string Sqlstringgettype = $"SELECT * FROM JokeType";
            List<JokeType> jokeTypes = new List<JokeType>();
            jokeTypes = PostgresConnection.GetListFromDB<JokeType>(Sqlstringgettype);
            if (jokeTypes == null || jokeTypes.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("Aktualnie brak żartów w bazie. Tylko administratorzy mogą dodawać żarty. Wciśnij dowolny przycisk by kontynuować.");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine("Wpisz jaką kategorie żartu chcesz przeczytać");
            foreach (JokeType joke in jokeTypes)
            {
                Console.WriteLine($"{joke.category_joke_id}. {joke.joke_category}");
            }

            bool isVaild = int.TryParse(Console.ReadLine(), out int inputId);
            if (!isVaild)
            {
                Console.Clear();
                Console.WriteLine("Niepoprawna wartośc. Wciśnij dowolny klawisz by kontynuować.");
                Console.ReadKey();
                return;
            }

            //Based on user choice pick specific type of joke 
            JokeType? jokeTypeChosen = jokeTypes.FirstOrDefault(type => type.category_joke_id == inputId);
            if (jokeTypeChosen == null)
            {
                Console.Clear();
                Console.WriteLine("Nie znaleziono kategorii. Wciśnij dowolny klawisz by kontynuować.");
                Console.ReadKey();
                return;
            }

            //Get list of jokes with specific type
            string Sqlstringgetjoke = $"SELECT * FROM JokeBase WHERE category_joke_id = {jokeTypeChosen.category_joke_id};";
            List<JokeBase> JokeList = new List<JokeBase>();
            JokeList = PostgresConnection.GetListFromDB<JokeBase>(Sqlstringgetjoke);

            if (JokeList == null || JokeList.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("Przepraszamy ,aktualnie brak żartów w tej kategori. Wciśnij dowolny przycisk by kontynuować.");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine("Który numer z poniższych żartów chcesz przeczytać?");
            foreach (JokeBase joke in JokeList)
            {
                Console.WriteLine($"{joke.Joke_id}.");
            }

            bool isVaild2 = int.TryParse(Console.ReadLine(), out int inputId2);
            if (!isVaild)
            {
                Console.Clear();
                Console.WriteLine("Niepoprawna wartość. Wciśnij dowolny klawisz by kontynuować.");
                Console.ReadKey();
                return;
            }

            JokeBase? JokeChosen = JokeList.FirstOrDefault(jokes => jokes.Joke_id == inputId2);
            if (JokeChosen == null)
            {
                Console.Clear();
                Console.WriteLine("Nie znaleziono żartu. Wciśnij dowolny klawisz by kontynuować.");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine(JokeChosen.Joke_message);
            JokeMenu.CountJoke(_user);
        }
    }
}
