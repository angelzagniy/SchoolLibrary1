using Microsoft.EntityFrameworkCore;

namespace SchoolLibrary.Database
{ 
    public class LibraryContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        
        private readonly string _fileName;

        public LibraryContext(string fileName)
        {
            _fileName = fileName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("FileName=" + _fileName);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Book>().HasKey(s => s.BookId);
            modelBuilder.Entity<Author>().HasKey(s => s.AuthorId);
            modelBuilder.Entity<BookAuthor>().HasKey(ba => new {ba.BookId, ba.AuthorId});

            modelBuilder.Entity<BookAuthor>()
                .HasOne<Book>()
                .WithMany()
                .HasForeignKey(ba => ba.BookId);
            
            modelBuilder.Entity<BookAuthor>()
                .HasOne<Author>()
                .WithMany()
                .HasForeignKey(ba => ba.AuthorId);

            modelBuilder.Entity<User>().HasKey(s => s.Id);
        }
    }
}