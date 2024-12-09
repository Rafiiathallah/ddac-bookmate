using ddac_bookmate.Areas.Identity.Data;
using ddac_bookmate.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ddac_bookmate.Data;

public class ddac_bookmateContext : IdentityDbContext<ddac_bookmateUser>
{
    public ddac_bookmateContext(DbContextOptions<ddac_bookmateContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Publisher> Publishers { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Library> Libraries { get; set; }
    public DbSet<BookAuthor> BookAuthors { get; set; }
    public DbSet<BookPublisher> BookPublishers { get; set; }
    public DbSet<BookGenre> BookGenres { get; set; }
    public DbSet<BookLibrary> BookLibraries { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure relationships for junction tables
        builder.Entity<BookAuthor>()
            .HasKey(ba => new { ba.BookId, ba.AuthorId });

        builder.Entity<BookAuthor>()
            .HasOne(ba => ba.Book)
            .WithMany(b => b.BookAuthors)
            .HasForeignKey(ba => ba.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<BookAuthor>()
            .HasOne(ba => ba.Author)
            .WithMany(a => a.BookAuthors)
            .HasForeignKey(ba => ba.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<BookPublisher>()
            .HasKey(bp => new { bp.BookId, bp.PublisherId });

        builder.Entity<BookPublisher>()
            .HasOne(bp => bp.Book)
            .WithMany(b => b.BookPublishers)
            .HasForeignKey(bp => bp.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<BookPublisher>()
            .HasOne(bp => bp.Publisher)
            .WithMany(p => p.BookPublishers)
            .HasForeignKey(bp => bp.PublisherId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<BookGenre>()
            .HasKey(bg => new { bg.BookId, bg.GenreId });

        builder.Entity<BookGenre>()
            .HasOne(bg => bg.Book)
            .WithMany(b => b.BookGenres)
            .HasForeignKey(bg => bg.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<BookGenre>()
            .HasOne(bg => bg.Genre)
            .WithMany(g => g.BookGenres)
            .HasForeignKey(bg => bg.GenreId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<BookLibrary>()
            .HasKey(bl => new { bl.BookId, bl.LibraryId });

        builder.Entity<BookLibrary>()
            .HasOne(bl => bl.Book)
            .WithMany(b => b.BookLibraries)
            .HasForeignKey(bl => bl.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<BookLibrary>()
            .HasOne(bl => bl.Library)
            .WithMany(l => l.BookAuthors)
            .HasForeignKey(bl => bl.LibraryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure User relationship
        builder.Entity<Library>()
            .HasOne(l => l.User)
            .WithMany()
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
