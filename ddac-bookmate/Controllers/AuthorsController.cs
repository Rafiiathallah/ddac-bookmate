using ddac_bookmate.Data;
using ddac_bookmate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ddac_bookmate.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ddac_bookmateContext _context;

        public AuthorsController(ddac_bookmateContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var authors = await _context.Authors
                .Include(a => a.BookAuthors)
                    .ThenInclude(ba => ba.Book)
                .OrderBy(a => a.Name)
                .AsNoTracking()
                .ToListAsync();

            return View(authors);
        }

        public async Task<IActionResult> Details(int id, int? bookId)
        {
            var author = await _context.Authors
                .Include(a => a.BookAuthors)
                    .ThenInclude(ba => ba.Book)
                        .ThenInclude(b => b.Language)
                .Include(a => a.BookAuthors)
                    .ThenInclude(ba => ba.Book)
                        .ThenInclude(b => b.BookGenres)
                            .ThenInclude(bg => bg.Genre)
                .Include(a => a.BookAuthors)
                    .ThenInclude(ba => ba.Book)
                        .ThenInclude(b => b.BookPublishers)
                            .ThenInclude(bp => bp.Publisher)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.AuthorId == id);

            if (author == null)
            {
                return NotFound();
            }

            if (bookId.HasValue)
            {
                var selectedBook = author.BookAuthors
                    .FirstOrDefault(ba => ba.Book.BookID == bookId)?.Book;
                ViewData["SelectedBook"] = selectedBook;
            }

            return View(author);
        }
    }
}
