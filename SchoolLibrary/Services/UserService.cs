using SchoolLibrary.Database;

namespace SchoolLibrary.Services;

public interface IUserService
{
    void AddUser(string name); //add user

    List<User> GetAllUsers(); //get all users

    List<(int Id, string UserName, int BookId, string Title)> GetAllUsersWithBooks(); //get all users with books
    
    List<User> FindUser(string userName); //find user by name

    void UserTakeBookAway(string userName, int bookId); //people take book away

    void TakeBookBack(string userName, int bookId); //people give book away 

    void DeleteUser(string userName); //delete user 
}

public class UserService : IUserService
{
    private readonly LibraryContext _libraryContext;

    public UserService(LibraryContext libraryContext)
    {
        _libraryContext = libraryContext;
    }
    
    /// <summary>
    /// add new user
    /// </summary>
    /// <param name="name"></param>
    public void AddUser(string name) 
    {
        var add = _libraryContext.Users.FirstOrDefault(s => s.UserName == name);
        if (add != null)
            throw new Exception("User already exists!");
        var a = _libraryContext.Users.Add(new User {UserName = name});
        _libraryContext.SaveChanges();
        Console.WriteLine("User id is " + a.Entity.Id);
    }
    
    /// <summary>
    /// get all users
    /// </summary>
    /// <returns></returns>
    public List<User> GetAllUsers() 
    {
        List<User> userList = _libraryContext.Users.ToList();
        return userList;
    }
    
    /// <summary>
    /// find user by name
    /// </summary>
    /// <param name="userName"></param>
    public List<User> FindUser(string userName)
    {
        List<User> user = _libraryContext.Users.Where(s => s.UserName.ToLower().Contains(userName)).ToList();
        return user;
    }
    
    /// <summary>
    /// people take book away
    /// </summary>
    /// <param name="userName">user name</param>
    /// <param name="bookId">book id</param>
    public void UserTakeBookAway(string userName, int bookId) 
    {
        var user = _libraryContext.Users.FirstOrDefault(s => s.UserName.ToLower() == userName);
        if (user != null)
        {
            var book = _libraryContext.Books.FirstOrDefault(s => s.BookId == bookId);
            if (book != null && book.Number > 0) book.Number--;
            _libraryContext.UserBooks.Add(new UserBook {Id = user.Id, BookId = bookId, UserName = userName});
            _libraryContext.SaveChanges();
        }
        else
        {
            throw new Exception("User was not found!");
        }
    }
    
    /// <summary>
    /// people give book away 
    /// </summary>
    /// <param name="userName">user name</param>
    /// <param name="bookId"></param>
    public void TakeBookBack(string userName, int bookId) 
    {
        var user = _libraryContext.UserBooks.FirstOrDefault(s => s.UserName.ToLower() == userName);
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

    /// <summary>
    /// get all users with books
    /// </summary>
    public List<(int Id, string UserName, int BookId, string Title)> GetAllUsersWithBooks()
    {
        var users =
            from b in _libraryContext.UserBooks
            join ab in _libraryContext.Books on b.BookId equals ab.BookId
            select new {b.Id, b.UserName, b.BookId, ab.Title};

        var booksList = users.ToList().Select(b => (b.Id, b.UserName, b.BookId, b.Title)).ToList();
        return booksList;
    }
    
    /// <summary>
    /// delete user
    /// </summary>
    /// <param name="userName"></param>
    public void DeleteUser(string userName) 
    {
        var userB = _libraryContext.UserBooks.FirstOrDefault(s => s.UserName.ToLower() == userName);
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
            var user1 = _libraryContext.Users.FirstOrDefault(s => s.UserName.ToLower() == userName);
            if(user1 != null) 
                _libraryContext.Users.Remove(user1);
            else
                throw new Exception("User was not found!");

            _libraryContext.SaveChanges();
        }
    }
}