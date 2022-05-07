using SchoolLibrary.Database;

namespace SchoolLibrary.Services;

public interface IAdminService
{
    Admin Login(string adminName);
    Admin SignUp(string adminName);
}

public class AdminService : IAdminService
{
    private readonly LibraryContext _libraryContext;

    public AdminService(LibraryContext libraryContext)
    {
        _libraryContext = libraryContext;
    }
    
    public Admin Login(string adminName)
    {
        return _libraryContext.Admins.FirstOrDefault(s => s.AdminName == adminName);
    }
        
    public Admin SignUp(string adminName)
    {
        var admin = _libraryContext.Admins.FirstOrDefault(s => s.AdminName == adminName);
        if (admin != null)
            throw new Exception("User already exists!");
        admin = new Admin{AdminName = adminName};
        _libraryContext.Admins.Add(admin);
        _libraryContext.SaveChanges();
        return admin;
    }

}