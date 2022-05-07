using SchoolLibrary.Database;

namespace SchoolLibrary.Services;

public class BookService
{
    private readonly LibraryContext _libraryContext;

    public BookService(LibraryContext libraryContext)
    {
        _libraryContext = libraryContext;
    }

    public void AddPeople() //додати користувача
    {
        Console.WriteLine("Enter name of user");
        var name = (Console.ReadLine() ?? string.Empty);
        var a = _libraryContext.Users.Add(new User {UserName = name});
        _libraryContext.SaveChanges();
        Console.WriteLine("User id is " + a.Entity.Id);
    }

    public void AddAuthor() //додати нового автора
    {
        Console.WriteLine("Enter name of author");
        var name = (Console.ReadLine() ?? string.Empty);
        var a = _libraryContext.Authors.Add(new Author {Name = name});
        _libraryContext.SaveChanges();
        Console.WriteLine("Author id is " + a.Entity.AuthorId);
    }

    public void AddBook() //додати нову книжку
    {
        Console.WriteLine("Enter title of book");
        var title = Console.ReadLine();
        Console.WriteLine("Enter authorId of the book");
        var authorId = int.Parse(Console.ReadLine() ?? string.Empty);
        Console.WriteLine("Enter number of the books");
        var number = int.Parse(Console.ReadLine() ?? string.Empty);
        var a = _libraryContext.Books.Add(new Book
            {Title = title, AuthorId = authorId, Number = number, FirstNumberOfBooks = number});
        _libraryContext.SaveChanges();
        Console.WriteLine("Book id is " + a.Entity.BookId);
    }

    public void ShowAllBooks() //показати всі книжки
    {
        var books =
            from b in _libraryContext.Books
            join ab in _libraryContext.Authors on b.AuthorId equals ab.AuthorId
            select new {b.BookId, b.Title, b.Number, ab.Name};

        var booksList = books.ToList();

        foreach (var book in booksList)
        {
            Console.WriteLine(book.BookId + " " + book.Title + " " + book.Number + "---" + book.Name);
        }
    }

    public void ChangeNumberOfBooks(int bookId, int number) //змінити кількість примірників
    {
        var book = FindBook(bookId);
        book.Number = number;
        Console.WriteLine(book.BookId + " " + book.Title + " " + book.Number);
    }

    public void ShowAllPeople() //показати всіх користувачів
    {
        var userList = _libraryContext.Users.ToList();
        foreach (var user in userList)
        {
            Console.WriteLine(user.Id + " " + user.UserName);
        }
    }

    public void FindUser(string userName) //знайти користувача по імені
    {
        var user = _libraryContext.Users.Where(s => s.UserName.ToLower().Contains(userName)).ToList();
        foreach (var u in user)
        {
            Console.WriteLine(u.Id + " " + u.UserName);
        }
    }

    public Book FindBook(int bookId) //знайти книжку по ід
    {
        var id = _libraryContext.Books.FirstOrDefault(s => s.BookId == bookId);
        if (id == null)
            Console.WriteLine("Book was not found!");
        else
        {
            var books =
                from b in _libraryContext.Books
                join ab in _libraryContext.Authors on b.AuthorId equals ab.AuthorId
                select new {b.BookId, b.Title, b.Number, ab.Name};

            var booksList = books.ToList();

            foreach (var book in booksList)
            {
                Console.WriteLine(book.BookId + " " + book.Title + " " + book.Number + "---" + book.Name);
            }
        }

        return id;
    }

    public void PeoleTakeBookAway(string userName, int bookId) //користувач забирає книжку додому
    {
        var user = _libraryContext.Users.FirstOrDefault(s => s.UserName == userName);
        if (user != null)
        {
            var book = _libraryContext.Books.FirstOrDefault(s => s.BookId == bookId);
                 book.Number--;
            var a = _libraryContext.UserBooks.Add(new UserBook {Id = user.Id, BookId = bookId, UserName = userName});
        }
        else
        {
            Console.WriteLine("User was not found!");
            return;
        }
        _libraryContext.SaveChanges();
    }

    public void TakeBookBack(string userName, int bookId) //користувач повертає книжку !!!
    {
        var user = _libraryContext.Users.FirstOrDefault(s => s.UserName == userName);
        if (user != null)
        {
            var book = _libraryContext.Books.FirstOrDefault(s => s.BookId == bookId);
            book.Number++;
            var a = _libraryContext.UserBooks.Remove(new UserBook {Id = user.Id, BookId = bookId, UserName = userName});
        }
        else
        {
            Console.WriteLine("User was not found!");
            return;
        }
        _libraryContext.SaveChanges();
    }

    public void DeleteBook() //видалити книжку, якщо жоден примірник не взято !!!
    {
        var book = new Book();
        if (book.Number == book.FirstNumberOfBooks)
            _libraryContext.Books.Remove(book);
        _libraryContext.SaveChanges();
    }

    public void DeletePeople(string userName) //видалити користувача та повернути всі примірники !!!
    {
        var user = _libraryContext.UserBooks.FirstOrDefault(s => s.UserName == userName);
        if (user != null)
        {
            var book = _libraryContext.UserBooks.FirstOrDefault(s => s.BookId == user.BookId);
            var a = _libraryContext.UserBooks.Remove(new UserBook {Id = user.Id, BookId = book.BookId, UserName = userName});
        }
        else
        {
            Console.WriteLine("User was not found!");
            return;
        }
        _libraryContext.SaveChanges();
    }
}