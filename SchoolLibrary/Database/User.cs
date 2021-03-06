namespace SchoolLibrary.Database;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public IEnumerable<UserBook>? Books { get; set; }
}