using SchoolLibrary.Database;
using SchoolLibrary.Services;

namespace SchoolLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            const string dbName = "LibraryDataBase.db";
            using var dbContext = new LibraryContext(dbName);
            IAdminService adminService = new AdminService(dbContext);
            IBookService bookService = new BookService(dbContext);
            IUserService userService = new UserService(dbContext);

            var controller = new Controller(userService, bookService, adminService);

            //delete DataBase
            //dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            
            controller.RunMainLoop();
            
            dbContext.SaveChanges();
        }
    }
}