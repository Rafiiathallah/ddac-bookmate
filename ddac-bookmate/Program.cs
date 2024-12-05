using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ddac_bookmate.Data;
using ddac_bookmate.Areas.Identity.Data;
using Microsoft.AspNetCore.Builder;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ddac_bookmateContextConnection") ?? throw new InvalidOperationException("Connection string 'ddac_bookmateContextConnection' not found.");

//builder.Services.AddDbContext<ddac_bookmateContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<ddac_bookmateContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddDefaultIdentity<ddac_bookmateUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ddac_bookmateContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
