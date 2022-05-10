using SchoolLibrary.Database;

namespace SchoolLibrary.Services;

public interface IBookService
{
    void AddAuthor(string name); //add new author

    void AddBook(string title, int authorId, int number); //add new book

    List<(int BookId, string Title, int Number, string Name)> GetAllBooks(); //get all books
    
    Book ChangeNumberOfBooks(string title, int number); //change number of books

    Book FindBook(string title); //find book by title 

    void DeleteBook(int bookId); //delete book if book wasn`t taken
}

public class BookService : IBookService
{
    private readonly LibraryContext _libraryContext;

    public BookService(LibraryContext libraryContext)
    {
        _libraryContext = libraryContext;
    }

    /// <summary>
    /// add new author
    /// </summary>
    /// <param name="name"></param>
    public void AddAuthor(string name) 
    {
        var add = _libraryContext.Authors.FirstOrDefault(s => s.Name == name);
        if (add != null)
        {
            throw new Exception("Author already exists!");
        }
        var a = _libraryContext.Authors.Add(new Author {Name = name});
        _libraryContext.SaveChanges();
        Console.WriteLine("Author id is " + a.Entity.AuthorId);
    }

    /// <summary>
    /// add new book
    /// </summary>
    /// <param name="title">book title</param>
    /// <param name="authorId"></param>
    /// <param name="number">number of books</param>
    public void AddBook(string title, int authorId, int number) 
    {
        var add = _libraryContext.Books.FirstOrDefault(s => s.Title == title);
        if (add != null)
            throw new Exception("Book already exists!");
        var a = _libraryContext.Books.Add(new Book
            {Title = title, AuthorId = authorId, Number = number});
        _libraryContext.SaveChanges();
        Console.WriteLine("Book id is " + a.Entity.BookId);
    }
    
    /// <summary>
    /// get all books
    /// </summary>
    /// <returns></returns>
    public List<(int BookId, string Title, int Number, string Name)> GetAllBooks()
    {
        var books =
            from b in _libraryContext.Books
            join ab in _libraryContext.Authors on b.AuthorId equals ab.AuthorId
            select new {b.BookId, b.Title, b.Number, ab.Name};

        var booksList = books.ToList().Select(b => (b.BookId, b.Title, b.Number, b.Name)).ToList();
        return booksList;
    }

    /// <summary>
    /// change number of books
    /// </summary>
    /// <param name="title"></param>
    /// <param name="number">new number of books</param>
    public Book ChangeNumberOfBooks(string title, int number)
    {
        var book = FindBook(title);
        book.Number = number;
        _libraryContext.SaveChanges();
        return book;
    }

    /// <summary>
    /// find book by title
    /// </summary>
    /// <param name="title">book title</param>
    public Book FindBook(string title) 
    {
        var book = _libraryContext.Books.FirstOrDefault(s => s.Title.ToLower().Contains(title));
        
        if (book == null)
            throw new Exception("Book was not found!");
        return book;
    }
    
    /// <summary>
    /// delete book if book wasn`t taken
    /// </summary>
    /// <param name="bookId"></param>
    public void DeleteBook(int bookId) 
    {
        var book = _libraryContext.Books.FirstOrDefault(s => s.BookId == bookId);
        var b = _libraryContext.UserBooks.FirstOrDefault(s => s.BookId == bookId);
        if (book == null || b!=null) return;
        _libraryContext.Books.Remove(book);
        _libraryContext.SaveChanges();
    }
}