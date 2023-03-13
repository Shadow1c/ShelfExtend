using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShelfExtend.Entity;
using Dapper;

namespace ShelfExtend.MenuLogic.JokeMenuLogic
{
    public class ReadRandomJoke : MenuOption
    {
        public ReadRandomJoke(User user, UserType userType = UserType.None, TeachingLevel teachingLevel = TeachingLevel.None) : base(user, userType, teachingLevel)
        {
            Name = "Przeczytaj Losowy żart";
        }
        public override void Execute()
        {
            string SqlstringForIdJoke = $"SELECT joke_id FROM JokeBase";
            List<int>? listJokeId = new List<int>();
            listJokeId = PostgresConnection.GetListFromDB<int>(SqlstringForIdJoke);

            if (listJokeId == null || listJokeId.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("Przepraszamy ,aktualnie brak żartów w bazie. Wciśnij dowolny przycisk by kontynuować.");
                Console.ReadKey();
                return;
            }

            var random = new Random();
            int indexOfRandomJoke = random.Next(0, listJokeId.Count);
            int idOfRandomJoke = listJokeId.ElementAt(indexOfRandomJoke);

            string? jokemessage;
            using (var JokeConnection = PostgresConnection.EstablishConnection())
            {
                try
                {
                    jokemessage = JokeConnection.QuerySingleOrDefault<string>($"SELECT joke_message FROM JokeBase WHERE joke_id = {idOfRandomJoke}");
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Błąd pobrania losowego żartu. Wciśnij dowolny klawisz by kontynuować..");
                    Console.ReadKey();
                    return;
                }
            }

            Console.Clear();
            Console.WriteLine(jokemessage);
            JokeMenu.CountJoke(_user);
        }
    }
}
