using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShelfExtend.Entity;

namespace ShelfExtend.MenuLogic.JokeMenuLogic
{
    public class CheckStatistic : MenuOption
    {
        public CheckStatistic(User user, UserType userType = UserType.None, TeachingLevel teachingLevel = TeachingLevel.None) : base(user, userType, teachingLevel)
        {
            Name = "Sprawdź statystykę ";
        }

        public override void Execute()
        {
            string sqlStringCheckCityStatistics = $"SELECT Location.location_id, JokeLocationStatistic.readjokecount,  Location.City FROM JokeLocationStatistic FULL JOIN Location ON Location.location_id = JokeLocationStatistic.location_id ORDER BY Location.location_id ";

            List<StatisticCities> CitiesList = PostgresConnection.GetListFromDB<StatisticCities>(sqlStringCheckCityStatistics);
            if (CitiesList == null || CitiesList.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("Błąd,nie można wyświetlić listy statystyk. Możliwym powodem jest brak miast w bazie danych, Skontaktuj się z administracją aplikacji.");
                Console.WriteLine("Wciśnij dowolny przycisk by kontynuować");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            foreach (StatisticCities city in CitiesList)
            {
                Console.WriteLine($"{city.city} = {city.readjokecount}");
            }
            Console.WriteLine("Wciśnij dowolny klawisz by kontynuować.");
            Console.ReadKey();
        }
    }
}
