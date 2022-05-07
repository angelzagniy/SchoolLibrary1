using SchoolLibrary.Database;

namespace SchoolLibrary.Services;

public interface IUserService
{
    void AddUser(string name); //додати користувача

    List<User> ShowAllUsers(); //показати всіх користувачів

    List<User> FindUser(string userName); //знайти користувача по імені

    void UserTakeBookAway(string userName, int bookId); //користувач забирає книжку додому

    void TakeBookBack(string userName, int bookId); //користувач повертає книжку 

    void DeleteUser(string userName); //видалити користувача та повернути всі примірники !!!
}

public class UserService : IUserService
{
    private readonly LibraryContext _libraryContext;

    public UserService(LibraryContext libraryContext)
    {
        _libraryContext = libraryContext;
    }
    
    public void AddUser(string name) //додати користувача
    {
        var add = _libraryContext.Users.FirstOrDefault(s => s.UserName == name);
        if (add != null)
            throw new Exception("User already exists!");
        var a = _libraryContext.Users.Add(new User {UserName = name});
        _libraryContext.SaveChanges();
    }
    
    public List<User> ShowAllUsers() //показати всіх користувачів
    {
        List<User> userList = _libraryContext.Users.ToList();
        return userList;
    }
    
    public List<User> FindUser(string userName) //знайти користувача по імені
    {
        List<User> user = _libraryContext.Users.Where(s => s.UserName.ToLower().Contains(userName)).ToList();
        return user;
    }
    
    public void UserTakeBookAway(string userName, int bookId) //користувач забирає книжку додому
    {
        var user = _libraryContext.Users.FirstOrDefault(s => s.UserName == userName);
        if (user != null)
        {
            var book = _libraryContext.Books.FirstOrDefault(s => s.BookId == bookId);
            if (book != null) book.Number--;
            _libraryContext.UserBooks.Add(new UserBook {Id = user.Id, BookId = bookId, UserName = userName});
            _libraryContext.SaveChanges();
        }
        else
        {
            throw new Exception("User was not found!");
        }
    }
    
    public void TakeBookBack(string userName, int bookId) //користувач повертає книжку 
    {
        var user = _libraryContext.UserBooks.FirstOrDefault(s => s.UserName == userName);
        if (user != null)
        {
            var book = _libraryContext.Books.FirstOrDefault(s => s.BookId == bookId);
            if (book != null) book.Number++;
            _libraryContext.SaveChanges();
        }
        else
        {
            throw new Exception("User was not found!");
        }
    }

    public void DeleteUser(string userName) //видалити користувача та повернути всі примірники !!!
    {
        var userB = _libraryContext.UserBooks.FirstOrDefault(s => s.UserName == userName);
        if (userB != null)
        {
            var book = _libraryContext.Books.FirstOrDefault(s => s.BookId == userB.BookId);
            if (book != null)
            {
                book.Number++;
            }
            _libraryContext.UserBooks.Remove(userB);
            var user = _libraryContext.Users.FirstOrDefault(s => s.UserName == userB.UserName);
            if (user != null) _libraryContext.Users.Remove(user);
            _libraryContext.SaveChanges();
        }
        else
        {
            throw new Exception("User was not found!");
        }
    }
}