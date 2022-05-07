using SchoolLibrary.Database;

namespace SchoolLibrary.Services;

public interface IBookService
{
    void AddAuthor(string name); //додати нового автора

    void AddBook(string title, int authorId, int number); //додати нову книжку

    void ShowAllBooks(); //показати всі книжки

    /// <summary>
    /// Змінити кількість примірників
    /// </summary>
    /// <param name="bookId"></param>
    /// <param name="number">Нова кількість примірників</param>
    void ChangeNumberOfBooks(int bookId, int number);

    Book? FindBook(int bookId); //знайти книжку по ід

    void DeleteBook(int bookId); //видалити книжку, якщо жоден примірник не взято !!!
}

public class BookService : IBookService
{
    private readonly LibraryContext _libraryContext;

    public BookService(LibraryContext libraryContext)
    {
        _libraryContext = libraryContext;
    }

    public void AddAuthor(string name) //додати нового автора
    {
        var add = _libraryContext.Authors.FirstOrDefault(s => s.Name == name);
        if (add != null)
        {
            throw new Exception("Author already exists!");
        }
        var a = _libraryContext.Authors.Add(new Author {Name = name});
        _libraryContext.SaveChanges();
    }

    public void AddBook(string title, int authorId, int number) //додати нову книжку
    {
        var add = _libraryContext.Books.FirstOrDefault(s => s.Title == title);
        if (add != null)
            throw new Exception("Book already exists!");
        var a = _libraryContext.Books.Add(new Book
            {Title = title, AuthorId = authorId, Number = number, FirstNumberOfBooks = number});
        _libraryContext.SaveChanges();
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

    /// <summary>
    /// Змінити кількість примірників
    /// </summary>
    /// <param name="bookId"></param>
    /// <param name="number">Нова кількість примірників</param>
    public void ChangeNumberOfBooks(int bookId, int number)
    {
        var book = FindBook(bookId);
        if (book != null)
        {
            book.Number = number;
            Console.WriteLine(book.BookId + " " + book.Title + " " + book.Number);
        }
    }

    public Book? FindBook(int bookId) //знайти книжку по ід
    {
        var id = _libraryContext.Books.FirstOrDefault(s => s.BookId == bookId);
        if (id == null)
            throw new Exception("Book was not found!");
        var books =
            from b in _libraryContext.Books
            join ab in _libraryContext.Authors on b.AuthorId equals ab.AuthorId
            select new {b.BookId, b.Title, b.Number, ab.Name};

        var booksList = books.ToList();

        foreach (var book in booksList)
        {
            Console.WriteLine(book.BookId + " " + book.Title + " " + book.Number + "---" + book.Name);
        }
        return id;
    }
    
    public void DeleteBook(int bookId) //видалити книжку, якщо жоден примірник не взято
    {
        var book = _libraryContext.Books.FirstOrDefault(s => s.BookId == bookId);
        if (book == null || book.Number != book.FirstNumberOfBooks) return;
        _libraryContext.Books.Remove(book);
        _libraryContext.SaveChanges();
    }
}