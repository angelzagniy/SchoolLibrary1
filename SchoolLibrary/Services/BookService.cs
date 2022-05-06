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

    public void AddBook(string title, IEnumerable<int> authors)//додати нову книжку
    {
        var book = new Book
        {
            Title = title
        };
        _libraryContext.Books.Add(book);
        _libraryContext.SaveChanges();
        
        foreach (var id in authors)
        {
            _libraryContext.BookAuthors.Add(new BookAuthor {BookId = book.BookId, AuthorId = id});
        }
        _libraryContext.SaveChanges();
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

    public void ChangeNumberOfBooks()//змінити кількість примірників
    {
    }

    public void ShowAllPeople()//показати всіх користувачів
    {
    }

    public void FindPeople()//знайти користувача по імені
    {
    }

    public void FindBook(int bookId)//знайти книжку по ід
    {
        var id = _libraryContext.Books.FirstOrDefault(s => s.BookId == bookId);
        if(id == null)
            Console.WriteLine("Book was not found!");
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