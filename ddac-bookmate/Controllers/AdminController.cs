using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ddac_bookmate.Areas.Identity.Data;
using ddac_bookmate.Models;
using ddac_bookmate.Data;

namespace ddac_bookmate.Controllers
{
    public class AdminController : Controller
    {
        private readonly ddac_bookmateContext _context;

        public AdminController(ddac_bookmateContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, int? genreFilter)
        {
            var books = from b in _context.Books
                        .Include(b => b.Language)
                        .Include(b => b.BookGenres)
                            .ThenInclude(bg => bg.Genre)
                        .Include(b => b.BookAuthors)
                            .ThenInclude(ba => ba.Author)
                        select b;

            // Apply search filter
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                books = books.Where(s => s.BookName.ToLower().Contains(searchString));
                ViewData["CurrentFilter"] = searchString;
            }

            // Apply genre filter
            if (genreFilter.HasValue)
            {
                books = books.Where(b => b.BookGenres.Any(bg => bg.GenreId == genreFilter));
                ViewData["GenreFilter"] = genreFilter;
            }

            // Load filter options
            ViewBag.Genres = await _context.Genres.ToListAsync();

            var filteredBooks = await books
                .OrderBy(b => b.BookName)
                .ToListAsync();

            return View(filteredBooks);
        }
    }
}