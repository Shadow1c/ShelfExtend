using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShelfExtend.Entity;

namespace ShelfExtend.MenuLogic.LibraryMenuLogic
{
    public class BorrowBook : MenuOption
    {
        public BorrowBook(User user, UserType userType = UserType.None, TeachingLevel teachingLevel = TeachingLevel.None) : base(user, userType, teachingLevel)
        {
            Name = "Wypożycz książkę";
        }

        public override void Execute()
        {
            //Check if book limit not reached
            if (LibraryMenu.isCountBookTakenCorrect(_user) == false)
            {
                Console.Clear();
                Console.WriteLine("Osiągnieto już limit pobranych książek lub błąd pobrania danych. Wciśnij dowolny klawisz by kontynuować.");
                Console.ReadKey();
                return;
            }

            //Get list of books
            string sqlStingCheckBooks = $"SELECT * FROM BookShelf WHERE user_id IS NULL";
            List<BookShelf>? ListAvailableBooks = PostgresConnection.GetListFromDB<BookShelf>(sqlStingCheckBooks);
            if (ListAvailableBooks == null ||  ListAvailableBooks.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("Nie ma wolnych książek do wypozyczenia. Wciśnij dowolny klawisz by kontynuować.");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            foreach (BookShelf book in ListAvailableBooks)
            {
                Console.WriteLine($"{ListAvailableBooks.IndexOf(book) + 1}. {book.bookname}");
            }

            bool isValid = int.TryParse(Console.ReadLine(), out int userInput);
            if (!isValid || ListAvailableBooks.Count < userInput || 0 >= userInput)
            {
                Console.Clear();
                Console.WriteLine("Błąd wpisanej wartości. Wciśnij dowolny klawisz by kontynuować.");
                Console.ReadKey();
                return;
            }

            string currentTime = DateTime.Now.ToString("yyyy-MM-dd");
            BookShelf pickedBook = ListAvailableBooks.ElementAt(userInput - 1);

            //User confirms action 
            bool isExit = false;
            while (!isExit)
            {
                Console.Clear();
                Console.WriteLine("Ksiązkę którą chcesz wypożyczyć to:");
                Console.WriteLine($"{pickedBook.bookname} jej rodzaj to {pickedBook.book_type}");
                Console.WriteLine("0. Anuluj");
                Console.WriteLine("1. Potwierdzenie wypożyczenia");

                int userInputInt;
                bool isConformationValid = int.TryParse(Console.ReadLine(), out userInputInt);
                if (!isConformationValid) 
                {
                    Console.Clear();
                    Console.WriteLine("Wprowadzono nieprawidłową wartość. Wciśnij dowolny klaiwsz by kontynuować.");
                    Console.ReadKey();
                    break;
                }

                switch (userInput)
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

            //set query
            string sqlStringTakeBook = $"UPDATE BookShelf SET user_id = {_user.User_id}, borrow_date = '{currentTime}' WHERE book_id = {pickedBook.book_id};";
            PostgresConnection.isExecutSqlString(sqlStringTakeBook);
        }
    }
}
