using SchoolLibrary.Database;

namespace SchoolLibrary.Services;

public class UserService
{
    private readonly LibraryContext _libraryContext;

    public UserService(LibraryContext libraryContext)
    {
        _libraryContext = libraryContext;
    }
    
    public User Login(string userName)
    {
        return _libraryContext.Users.FirstOrDefault(s => s.UserName == userName);
    }
        
    public User SignUp(string userName)
    {
        var user = _libraryContext.Users.FirstOrDefault(s => s.UserName == userName);
        if (user != null)
            throw new Exception("User already exists!");
        user = new User{UserName = userName};
        _libraryContext.Users.Add(user);
        _libraryContext.SaveChanges();
        return user;
    }

}