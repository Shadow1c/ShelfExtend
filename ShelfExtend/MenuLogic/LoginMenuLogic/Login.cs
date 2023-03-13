using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShelfExtend.Entity;
using Dapper;

namespace ShelfExtend.MenuLogic.LoginMenuLogic
{
    public class Login
    {
        public User MenuLogin()
        {
            Console.Clear();
            User? currentUser = new User();

            //Take user login and password
            Console.WriteLine("Podaj swój login.");
            string? inputLogin = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Podaj swoje hasło");
            string? inputPassword = Console.ReadLine();

            if (string.IsNullOrEmpty(inputLogin) || string.IsNullOrEmpty(inputPassword))
            {
                Console.Clear();
                Console.WriteLine("Wpisana wartość nie prawidłowa. Wcisnij dowolny klawisz by kontynuować.");
                Console.ReadKey();
                currentUser = null;
                return currentUser;
            }

            return currentUser = GetUserDB(currentUser, inputLogin, inputPassword);
        }
        private User? GetUserDB(User? user, string Login, string password)
        {
            using (var loginConnection = PostgresConnection.EstablishConnection())
            {
                try
                {
                    user = loginConnection.QuerySingleOrDefault<User>($"SELECT * FROM users WHERE login = '{Login}' AND password = '{password}'");//Stored Procedure

                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Błąd logowania, Wciśnij dowolny klawisz.");
                    Console.ReadKey();
                    return user = null;
                }
            }
            if (user == null)
            {
                Console.Clear();
                Console.WriteLine("Błąd logowania, Wciśnij dowolny klawisz.");
                Console.ReadKey();
            }
            return user;
        }
    }
}
