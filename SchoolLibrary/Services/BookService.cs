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
        var add = _libraryContext.Users.FirstOrDefault(s => s.UserName == name);
        if (add != null)
            throw new Exception("User already exists!");
        var a = _libraryContext.Users.Add(new User {UserName = name});
        _libraryContext.SaveChanges();
        Console.WriteLine("User id is " + a.Entity.Id);
    }

    public void AddAuthor() //додати нового автора
    {
        Console.WriteLine("Enter name of author");
        var name = (Console.ReadLine() ?? string.Empty);
        var add = _libraryContext.Authors.FirstOrDefault(s => s.Name== name);
        if (add != null)
            throw new Exception("Author already exists!");
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
        var add = _libraryContext.Books.FirstOrDefault(s => s.Title == title);
        if (add != null)
            throw new Exception("Book already exists!");
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

    public void UserTakeBookAway(string userName, int bookId) //користувач забирає книжку додому
    {
        var user = _libraryContext.Users.FirstOrDefault(s => s.UserName == userName);
        if (user != null)
        {
            var book = _libraryContext.Books.FirstOrDefault(s => s.BookId == bookId);
                 book.Number--;
            var a = _libraryContext.UserBooks.Add(new UserBook {Id = user.Id, BookId = bookId, UserName = userName});
            _libraryContext.SaveChanges();
        }
        else
        {
            throw new Exception("User was not found!");
        }
    }

    public void TakeBookBack(string userName, int bookId) //користувач повертає книжку !!!
    {
        var user = _libraryContext.Users.FirstOrDefault(s => s.UserName == userName);
        if (user != null)
        {
            var book = _libraryContext.Books.FirstOrDefault(s => s.BookId == bookId);
            book.Number++;
            var b = new UserBook {Id = user.Id, BookId = book.BookId, UserName = user.UserName};
            _libraryContext.UserBooks.Remove(b);
            _libraryContext.SaveChanges();
        }
        else
        {
            throw new Exception("User was not found!");
        }
    }

    public void DeleteBook(int bookId) //видалити книжку, якщо жоден примірник не взято !!!
    {
        var book = new Book{BookId = bookId};
        if (book.Number == book.FirstNumberOfBooks)
            _libraryContext.Books.Remove(book);
        _libraryContext.SaveChanges();
    }

    public void DeleteUser(string userName) //видалити користувача та повернути всі примірники !!!
    {
        var user = _libraryContext.UserBooks.FirstOrDefault(s => s.UserName == userName);
        if (user != null)
        {
            var book = _libraryContext.Books.FirstOrDefault(s => s.BookId == user.BookId);
            if (book != null)
            {
                var b = new UserBook {Id = user.Id, BookId = book.BookId, UserName = user.UserName};
                _libraryContext.UserBooks.Remove(b);
            }
            _libraryContext.SaveChanges();
        }
        else
        {
            throw new Exception("User was not found!");
        }
    }
}