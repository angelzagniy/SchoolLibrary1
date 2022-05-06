using Microsoft.EntityFrameworkCore;

namespace SchoolLibrary.Database
{
    public class LibraryContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<User> Users { get; set; }
        
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
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(s => s.BookId);
                entity
                    .HasOne(s => s.Author)
                    .WithMany(s => s.Books)
                    .HasForeignKey(s => s.AuthorId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity
                    .HasOne(s => s.Book)
                    .WithMany(s => s.People)
                    .HasForeignKey(s => s.BookId);
            });
            
            modelBuilder.Entity<Admin>().HasKey(s => s.Id);
            modelBuilder.Entity<Author>().HasKey(s => s.AuthorId);
        }
    }
}