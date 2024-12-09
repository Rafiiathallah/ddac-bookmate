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

    public DbSet<Book> BookTable { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Publisher> Publishers { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Library> Libraries { get; set; }
    public DbSet<BookAuthor> BookAuthors { get; set; }
    public DbSet<BookPublisher> BookPublishers { get; set; }
    public DbSet<BookGenre> BookGenres { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure relationships
        builder.Entity<BookAuthor>()
            .HasKey(ba => new { ba.BookId, ba.AuthorId });

        builder.Entity<BookPublisher>()
            .HasKey(bp => new { bp.BookId, bp.PublisherId });

        builder.Entity<BookGenre>()
            .HasKey(bg => new { bg.BookId, bg.GenreId });

        // Configure cascade delete behavior
        builder.Entity<Library>()
            .HasOne(l => l.Book)
            .WithMany(b => b.Libraries)
            .HasForeignKey(l => l.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Library>()
            .HasOne(l => l.User)
            .WithMany()
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
