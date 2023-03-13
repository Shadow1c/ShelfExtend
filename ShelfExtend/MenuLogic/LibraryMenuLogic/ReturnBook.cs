using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShelfExtend.Entity;

namespace ShelfExtend.MenuLogic.LibraryMenuLogic
{
    public class ReturnBook : MenuOption
    {
        public ReturnBook(User user, UserType userType = UserType.None, TeachingLevel teachingLevel = TeachingLevel.None) : base(user, userType, teachingLevel)
        {
            Name = "Zwróć książkę";
        }

        public override void Execute()
        {
            string sqlStingCheckYourBooks = $"SELECT * FROM BookShelf WHERE user_id = {_user.User_id}";
            List<BookShelf>? ListAvailableBooks = PostgresConnection.GetListFromDB<BookShelf>(sqlStingCheckYourBooks);
            if (ListAvailableBooks == null || ListAvailableBooks.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("Nie masz żadnej ksiązki do zwrócenia. Wciśij dowolny klawisz by kontynuować.");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine("Wybierz którą ksiażkę chcesz zwrócić.");
            foreach (BookShelf book in ListAvailableBooks)
            {
                Console.WriteLine($"{ListAvailableBooks.IndexOf(book) + 1}. {book.bookname} Zaległa kwota to {CheckYourBooks.CalculateDebt(book.borrow_date)}");
            }

            bool isValid = int.TryParse(Console.ReadLine(), out int userInput);
            if (!isValid || ListAvailableBooks.Count < userInput || 0 >= userInput)
            {
                Console.Clear();
                Console.WriteLine("Błąd wpisanej wartości. Wciśnij dowolny klawisz by kontynuować.");
                Console.ReadKey();
                return;
            }

            BookShelf PickedBook = ListAvailableBooks.ElementAt(userInput - 1);

            //User confirms action 
            bool isExit = false;
            while (!isExit)
            {
                Console.Clear();
                Console.WriteLine("Ksiązkę którą chcesz oddać to:");
                Console.WriteLine($"{PickedBook.bookname}");
                Console.WriteLine("0. Anuluj");
                Console.WriteLine("1. Potwierdzenie Oddania");

                int userInputInt;
                bool isConformationValid = int.TryParse(Console.ReadLine(), out userInputInt);
                if (!isConformationValid)
                {
                    Console.Clear();
                    Console.WriteLine("Wprowadzono nieprawidłową wartość. Wciśnij dowolny klawisz by kontynuować.");
                    Console.ReadKey();
                    continue;
                }

                switch (userInputInt)
                {
                    case 0:
                        return;
                    case 1:
                        isExit = true;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Wprowadzono nieprawidłową wartość. Wciśnij dowolny klaiwsz by kontynuować.");
                        Console.ReadKey();
                        break;
                }
            }

            string sqlStringReturnBook = $"UPDATE BookShelf SET user_id = NULL, borrow_date = NULL WHERE book_id = {PickedBook.book_id}";
            if (!PostgresConnection.isExecutSqlString(sqlStringReturnBook))
            {
                Console.Clear();
                Console.WriteLine("Wystapił błąd, nie udało się oddać ksiązki. Wciśnij dowolny klawisz by kontynuować");
                Console.ReadKey();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Udało się oddać książke. Wciśnij dowolny klawisz by kontynuować.");
                Console.ReadKey();
            }
        }
    }
}
