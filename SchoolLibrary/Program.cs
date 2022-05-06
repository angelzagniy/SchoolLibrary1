using SchoolLibrary.Database;
using SchoolLibrary.Services;

namespace SchoolLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            string dbName = "LibraryDataBase.db";
            using var dbContext = new LibraryContext(dbName);
            var userService = new UserService(dbContext);
            var bookService = new BookService(dbContext);

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            dbContext.SaveChanges();
           
            User user = null;
            while (user == null)
            {
                Console.WriteLine("Do you want login or sign up?\n(put 1 to login, 2 - sign up)");
                int userInput = int.Parse(Console.ReadLine());
                switch (userInput)
                {
                    case 1:
                        while (user == null)
                        {
                            Console.WriteLine("Enter your username:");
                            string userName = Console.ReadLine();
                            user = userService.Login(userName);
                            if (user == null)
                            {
                                Console.WriteLine("Username not found. Please try again.");
                            }
                        }

                        Console.WriteLine("You successfully logged in!");
                        break;
                    case 2:
                        while (user == null)
                        {
                            Console.WriteLine("Enter your username:");
                            string userName = Console.ReadLine();
                            try
                            {
                                user = userService.SignUp(userName);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                user = null;
                            }
                        }

                        Console.WriteLine("You successfully signed up!");
                        break;
                    default:
                        Console.WriteLine("Invalid input! Please try again.");
                        break;
                }
            }

            while (true)
            {
                Console.WriteLine("Please, chose a command" +
                                  "\n0 - quit" +
                                  "\n1 - add new author" +
                                  "\n2 - add new book" +
                                  "\n3 - change number of books" +
                                  "\n4 - show all people" +
                                  "\n5 - show all books" +
                                  "\n6 - find username" +
                                  "\n7 - find book by id" +
                                  "\n8 - take book" + //people take book away
                                  "\n9 - give book away" + //people give book away
                                  "\n10 - delete book" +
                                  "\n11 - delete user");

                var choice = int.Parse(Console.ReadLine() ?? string.Empty);

                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        bookService.AddAuthor();
                        break;
                    case 2:
                        Console.WriteLine("Enter title of book");
                        var title = Console.ReadLine();
                        Console.WriteLine("Enter authorId of the book");
                        var authorId = int.Parse(Console.ReadLine() ?? string.Empty);
                        bookService.AddBook(title, new []{authorId, 2, 3});
                        Console.WriteLine("Enter number of the books");
                        var number = int.Parse(Console.ReadLine() ?? string.Empty);
                        //Console.WriteLine("Book id is " + entry.Entity.BookId);
                        break;
                    case 3:
                        bookService.ChangeNumberOfBooks();
                        break;
                    case 4:
                        bookService.ShowAllPeople();
                        break;
                    case 5:
                        bookService.ShowAllBooks();
                        break;
                    case 6:
                        bookService.FindPeople();
                        break;
                    case 7:
                        Console.WriteLine("Enter bookId for searching:");
                        int bookId = int.Parse(Console.ReadLine());
                        bookService.FindBook(bookId);
                        break;
                    case 8:
                        bookService.PeoleTakeBookAway();
                        break;
                    case 9:
                        bookService.TakeBookBack();
                        break;
                    case 10:
                        bookService.DeleteBook();
                        break;
                    case 11:
                        bookService.DeletePeople();
                        break;
                    default:
                        Console.WriteLine("Wrong number!");
                        break;
                }
            }
        }
    }
}