using ddac_bookmate.Data;
using ddac_bookmate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ddac_bookmate.Controllers
{
    [Authorize]
    public class LibraryController : Controller
    {
        private readonly ddac_bookmateContext _context;

        public LibraryController(ddac_bookmateContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var library = await _context.Libraries
                .Include(l => l.BookAuthors)  // This is actually BookLibrary items despite the name
                    .ThenInclude(bl => bl.Book)
                        .ThenInclude(b => b.Language)
                .FirstOrDefaultAsync(l => l.UserId == userId);

            if (library == null)
            {
                library = new Library 
                { 
                    UserId = userId,
                    BookCount = 0,
                    AddedDate = DateTime.UtcNow
                };
                _context.Libraries.Add(library);
                await _context.SaveChangesAsync();
            }

            var books = library.BookAuthors?
                .Select(bl => bl.Book)
                .ToList() ?? new List<Book>();

            return View(books);
        }
    }
}