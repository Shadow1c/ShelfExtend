using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShelfExtend.Entity;
using Dapper;

namespace ShelfExtend.MenuLogic.JokeMenuLogic
{
    public class JokeMenu : Menu
    {
        public static readonly string Name = "Menu Dowcipów";
        public JokeMenu(User user) : base(user)
        {
            _user = user;
            _menuOptions.Add(new AddJoke(_user));
            _menuOptions.Add(new ReadJoke(_user));
            _menuOptions.Add(new ReadRandomJoke(_user));
            _menuOptions.Add(new CheckStatistic(_user));
        }

        public static void CountJoke(User user)
        {
            string SqlstringCountJoke = $"UPDATE JokeLocationStatistic SET readjokecount = readjokecount + 1 WHERE location_id = {user.Location_id}";
            using (var RegistrationConnection = PostgresConnection.EstablishConnection())
            {
                try
                {
                    RegistrationConnection.Execute(SqlstringCountJoke);
                }
                catch
                {
                    Console.WriteLine("Błąd naliczenia do statystyk. Wciśnij dowolny klawisz by kontynuować.");
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }
                Console.WriteLine("przeczytanie naliczone do statystyk. Wciśnij dowolny klawisz by kontynuować.");
                Console.ReadKey();
                Console.Clear();
                return;
            }

        }
    }

    

    
}
