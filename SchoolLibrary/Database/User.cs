namespace SchoolLibrary.Database;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; }
}