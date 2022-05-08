using SchoolLibrary.Database;
using SchoolLibrary.Services;

namespace SchoolLibrary;

public class Controller
{
    private readonly IUserService _userService;
    private readonly IBookService _bookService;
    private readonly IAdminService _adminService;

    public Controller(IUserService userService, IBookService bookService, IAdminService adminService)
    {
        _userService = userService;
        _bookService = bookService;
        _adminService = adminService;
    }

    public void RunMainLoop()
    {
        Admin admin = null;
        while (admin == null)
        {
            Console.WriteLine("Do you want login or sign up?\n(put 1 to login, 2 - sign up)");
            var userInput = int.Parse(Console.ReadLine());
            switch (userInput)
            {
                case 1:
                    while (admin == null)
                    {
                        Console.WriteLine("Enter your username:");
                        var adminName = Console.ReadLine();
                        admin = _adminService.Login(adminName);
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
                            admin = _adminService.SignUp(adminName);
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

        Console.WriteLine("Please type 22 to show commands");
        
        while (true)
        {
            var choice = int.Parse(Console.ReadLine() ?? string.Empty);

            switch (choice)
            {
                case 22:
                    Console.WriteLine("Please, chose a command" +
                                      "\n0 - quit" +
                                      "\n1 - add user" +
                                      "\n2 - add new author" +
                                      "\n3 - add new book" +
                                      "\n4 - change number of books" +
                                      "\n5 - show all users" +
                                      "\n6 - show all books" +
                                      "\n7 - find username" +
                                      "\n8 - find book by id" +
                                      "\n9 - people take book away" +
                                      "\n10 - people give book away" +
                                      "\n11 - delete book" +
                                      "\n12 - delete user" +
                                      "\n22 - show commands");
                    break;
                case 0:
                    return;
                case 1:
                    Console.WriteLine("Enter name of user");
                    var uNam = (Console.ReadLine() ?? string.Empty);
                    _userService.AddUser(uNam);
                    break;
                case 2:
                    Console.WriteLine("Enter name of author");
                    var name = (Console.ReadLine() ?? string.Empty);
                    _bookService.AddAuthor(name);
                    break;
                case 3:
                    Console.WriteLine("Enter title of book");
                    var title = (Console.ReadLine()?? string.Empty);
                    Console.WriteLine("Enter authorId of the book");
                    var authorId = int.Parse(Console.ReadLine() ?? string.Empty);
                    Console.WriteLine("Enter number of the books");
                    var number = int.Parse(Console.ReadLine() ?? string.Empty);
                    _bookService.AddBook(title, authorId, number);
                    break;
                case 4:
                    Console.WriteLine("Enter bookId for searching:");
                    var bookId = int.Parse(Console.ReadLine());
                    _bookService.FindBook(bookId);
                    Console.WriteLine("Enter new number of books:");
                    var n = int.Parse(Console.ReadLine());

                    var boo = _bookService.ChangeNumberOfBooks(bookId, n);
                    Console.WriteLine(boo.BookId + " " + boo.Title + " " + boo.Number);
                    break;
                case 5:
                    List<User> uList = _userService.ShowAllUsers();

                    foreach (var user in uList)
                    {
                        Console.WriteLine(user.Id + " " + user.UserName);
                    }

                    break;
                case 6:
                    _bookService.ShowAllBooks();
                    break;
                case 7:
                    Console.WriteLine("Enter userName:");
                    var userName = Console.ReadLine().ToLower();

                    List<User> user1 = _userService.FindUser(userName);

                    foreach (var ur in user1)
                    {
                        Console.WriteLine(ur.Id + " " + ur.UserName);
                    }

                    break;
                case 8:
                    Console.WriteLine("Enter bookId for searching:");
                    var bId = int.Parse(Console.ReadLine());
                    var d = _bookService.FindBook(bId);
                    Console.WriteLine(d.BookId + " " + d.Title + " " + d.Number);
                    break;
                case 9:
                    Console.WriteLine("Enter userName:");
                    var uName = Console.ReadLine().ToLower();
                    Console.WriteLine("Enter bookId:");
                    var bookId1 = int.Parse(Console.ReadLine());
                    _userService.UserTakeBookAway(uName, bookId1);
                    break;
                case 10:
                    Console.WriteLine("Enter userName:");
                    var uN = Console.ReadLine().ToLower();
                    Console.WriteLine("Enter bookId:");
                    var bookId2 = int.Parse(Console.ReadLine());
                    _userService.TakeBookBack(uN, bookId2);
                    break;
                case 11:
                    Console.WriteLine("Enter bookId for searching:");
                    var book = int.Parse(Console.ReadLine());
                    _bookService.DeleteBook(book);
                    break;
                case 12:
                    Console.WriteLine("Enter userName:");
                    var u = Console.ReadLine().ToLower();
                    _userService.DeleteUser(u);
                    break;
                default:
                    Console.WriteLine("Wrong number!");
                    break;
            }

            Console.WriteLine("Enter next command (22 - show all commands)");
        }
    }
}