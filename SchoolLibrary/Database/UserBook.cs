namespace SchoolLibrary.Database;

public class UserBook
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public int BookId { get; set; }
    public User User { get; set; }
}