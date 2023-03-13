using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShelfExtend.Entity;

namespace ShelfExtend.MenuLogic.LibraryMenuLogic
{
    public class CheckYourBooks : MenuOption
    {
        public CheckYourBooks(User user, UserType userType = UserType.None, TeachingLevel teachingLevel = TeachingLevel.None) : base(user, userType, teachingLevel)
        {
            Name = "Sprawdz aktualnie wypożyczone książki";
        }

        public override void Execute()
        {
            CalculatePayment();
        }

        private void CalculatePayment()
        {
            //Get list of all current user books 
            string sqlStingCheckYourBooks = $"SELECT * FROM BookShelf WHERE user_id = {_user.User_id}";
            List<BookShelf>? ListAvailableBooks = PostgresConnection.GetListFromDB<BookShelf>(sqlStingCheckYourBooks);
            if (ListAvailableBooks == null || ListAvailableBooks.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("Nie masz żadnych wypożyczonych książek. Wciśnij dowolny klawisz by kontynuować.");
                Console.ReadKey();
                return;
            }

            //Show all user books with debt
            Console.Clear();
            foreach (BookShelf book in ListAvailableBooks)
            {
                Console.WriteLine($"{ListAvailableBooks.IndexOf(book) + 1}. {book.bookname} Zaległa kwota to {CalculateDebt(book.borrow_date)}");
            }
            Console.WriteLine("Wciśnij dowolny klawisz by kontynuować");
            Console.ReadKey();
        }

        //Calculate payment
        public static decimal CalculateDebt(DateTime booktime)
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan diff1 = currentTime.Subtract(booktime);
            decimal moneyPerWeek = 10M;
            decimal moneyToPay = ((int)diff1.TotalDays / 7) * moneyPerWeek;
            return moneyToPay;
        }
    }
}
