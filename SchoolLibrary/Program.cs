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
            var adminService = new AdminService(dbContext);
            var bookService = new BookService(dbContext);

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            dbContext.SaveChanges();

            Admin admin = null;
            while (admin == null)
            {
                Console.WriteLine("Do you want login or sign up?\n(put 1 to login, 2 - sign up)");
                int userInput = int.Parse(Console.ReadLine());
                switch (userInput)
                {
                    case 1:
                        while (admin == null)
                        {
                            Console.WriteLine("Enter your username:");
                            string adminName = Console.ReadLine();
                            admin = adminService.Login(adminName);
                            if (admin == null)
                            {
                                Console.WriteLine("Username not found. Please try again.");
                            }
                        }

                        Console.WriteLine("You successfully logged in!");
                        break;
                    case 2:
                        while (admin == null)
                        {
                            Console.WriteLine("Enter your username:");
                            string adminName = Console.ReadLine();
                            try
                            {
                                admin = adminService.SignUp(adminName);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                admin = null;
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
                                  "\n1 - add user"+
                                  "\n2 - add new author" +
                                  "\n3 - add new book" +
                                  "\n4 - change number of books" +
                                  "\n5 - show all people" +
                                  "\n6 - show all books" +
                                  "\n7 - find username" +
                                  "\n8 - find book by id" +
                                  "\n9 - take book" + //people take book away
                                  "\n10 - give book away" + //people give book away
                                  "\n11 - delete book" +
                                  "\n12 - delete user");

                var choice = int.Parse(Console.ReadLine() ?? string.Empty);

                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        bookService.AddUser();
                        break;
                    case 2:
                        bookService.AddAuthor();
                        break;
                    case 3:
                        bookService.AddBook();
                        break;
                    case 4:
                        Console.WriteLine("Enter bookId for searching:");
                        int bookId = int.Parse(Console.ReadLine());
                        bookService.FindBook(bookId);
                        Console.WriteLine("Enter new number of books:");
                        int n = int.Parse(Console.ReadLine());
                        bookService.ChangeNumberOfBooks(bookId, n);
                        break;
                    case 5:
                        bookService.ShowAllUsers();
                        break;
                    case 6:
                        bookService.ShowAllBooks();
                        break;
                    case 7:
                        Console.WriteLine("Enter userName:");
                        string userName = Console.ReadLine().ToLower();
                        bookService.FindUser(userName);
                        break;
                    case 8:
                        Console.WriteLine("Enter bookId for searching:");
                        int bId = int.Parse(Console.ReadLine());
                        bookService.FindBook(bId);
                        break;
                    case 9:
                        Console.WriteLine("Enter userName:");
                        string uName = Console.ReadLine().ToLower();
                        Console.WriteLine("Enter bookId:");
                        int bookId1 = int.Parse(Console.ReadLine());
                        bookService.UserTakeBookAway(uName, bookId1);
                        break;
                    case 10:
                        Console.WriteLine("Enter userName:");
                        string uN = Console.ReadLine().ToLower();
                        Console.WriteLine("Enter bookId:");
                        int bookId2 = int.Parse(Console.ReadLine());
                        bookService.TakeBookBack(uN, bookId2);
                        break;
                    case 11:
                        Console.WriteLine("Enter bookId for searching:");
                        int book = int.Parse(Console.ReadLine());
                        bookService.DeleteBook(book);
                        break;
                    case 12:
                        Console.WriteLine("Enter userName:");
                        string u = Console.ReadLine().ToLower();
                        bookService.DeleteUser(u);
                        break;
                    default:
                        Console.WriteLine("Wrong number!");
                        break;
                }
            }
        }
    }
}