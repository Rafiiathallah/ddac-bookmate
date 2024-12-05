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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
