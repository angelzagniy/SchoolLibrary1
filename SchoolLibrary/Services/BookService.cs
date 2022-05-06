using SchoolLibrary.Database;

namespace SchoolLibrary.Services;

public class BookService
{
    private readonly LibraryContext _libraryContext;

    public BookService(LibraryContext libraryContext)
    {
        _libraryContext = libraryContext;
    }

    public void AddAuthor()//додати нового автора
    {
        Console.WriteLine("Enter name of author");
        var name = (Console.ReadLine() ?? string.Empty);
        var a = _libraryContext.Authors.Add(new Author {Name = name});
        _libraryContext.SaveChanges();
        Console.WriteLine("Author id is " + a.Entity.AuthorId);
    }

    public void AddBook()//додати нову книжку
    {
        Console.WriteLine("Enter title of book");
        var title = Console.ReadLine();
        Console.WriteLine("Enter authorId of the book");
        var authorId = int.Parse(Console.ReadLine() ?? string.Empty);
        Console.WriteLine("Enter number of the books");
        var number = int.Parse(Console.ReadLine() ?? string.Empty);
        var a = _libraryContext.Books.Add(new Book {Title = title, AuthorId = authorId, Number = number});
        _libraryContext.SaveChanges();
        Console.WriteLine("Book id is " + a.Entity.BookId);
    }

    public void ShowAllBooks()//показати всі книжки
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

    public void ChangeNumberOfBooks(int bookId, int number)//змінити кількість примірників
    {
        var book = FindBook(bookId);
        book.Number = number;
        Console.WriteLine(book.BookId + " " + book.Title + " " + book.Number);
    }

    public void ShowAllPeople()//показати всіх користувачів
    {
    }

    public void FindPeople()//знайти користувача по імені
    {
    }

    public Book FindBook(int bookId)//знайти книжку по ід
    {
        var id = _libraryContext.Books.FirstOrDefault(s => s.BookId == bookId);
        if(id == null)
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

    public void PeoleTakeBookAway()//користувач забирає книжку додому
    {
    }

    public void TakeBookBack()//користувач повертає книжку
    {
    }

    public void DeleteBook()//видалити книжку, якщо жоден примірник не взято
    {
    }

    public void DeletePeople()//видалити користувача та повернути всі примірники
    {
        
    }
}