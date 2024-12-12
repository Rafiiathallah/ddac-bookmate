using ddac_bookmate.Data;
using ddac_bookmate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ddac_bookmate.Controllers
{
    public class PublishersController : Controller
    {
        private readonly ddac_bookmateContext _context;

        public PublishersController(ddac_bookmateContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var publishers = await _context.Publishers
                .Include(p => p.BookPublishers)
                    .ThenInclude(bp => bp.Book)
                .OrderBy(p => p.Name)
                .AsNoTracking()
                .ToListAsync();

            return View(publishers);
        }

        public async Task<IActionResult> Details(int id, int? bookId)
        {
            var publisher = await _context.Publishers
                .Include(p => p.BookPublishers)
                    .ThenInclude(bp => bp.Book)
                        .ThenInclude(b => b.Language)
                .Include(p => p.BookPublishers)
                    .ThenInclude(bp => bp.Book)
                        .ThenInclude(b => b.BookGenres)
                            .ThenInclude(bg => bg.Genre)
                .Include(p => p.BookPublishers)
                    .ThenInclude(bp => bp.Book)
                        .ThenInclude(b => b.BookAuthors)
                            .ThenInclude(ba => ba.Author)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PublisherId == id);

            if (publisher == null)
            {
                return NotFound();
            }

            if (bookId.HasValue)
            {
                var selectedBook = publisher.BookPublishers
                    .FirstOrDefault(bp => bp.Book.BookID == bookId)?.Book;
                ViewData["SelectedBook"] = selectedBook;
            }

            return View(publisher);
        }
    }
}
