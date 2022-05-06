namespace SchoolLibrary.Database;

public class Book
{
    public int BookId { get; set; }
    public string? Title { get; set; }
    public int Number { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; }
    public List<User> People { get; set; }
}