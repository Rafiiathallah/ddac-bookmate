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

        [HttpGet]
        public async Task<IActionResult> AddBook()
        {
            ViewBag.Languages = await _context.Languages.ToListAsync();
            ViewBag.Genres = await _context.Genres.ToListAsync();
            ViewBag.Authors = await _context.Authors.ToListAsync();
            ViewBag.Publishers = await _context.Publishers.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBook(Book book, int[] SelectedGenres, int[] SelectedAuthors)
        {
            if (ModelState.IsValid)
            {
                // Set default values
                book.UploadedDate = DateTime.UtcNow;
                book.IsTrending = false;
                
                // Add the book first
                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                // Add genre relationships
                foreach (var genreId in SelectedGenres)
                {
                    var bookGenre = new BookGenre
                    {
                        BookId = book.BookID,
                        GenreId = genreId
                    };
                    _context.BookGenres.Add(bookGenre);
                }

                // Add author relationships
                foreach (var authorId in SelectedAuthors)
                {
                    var bookAuthor = new BookAuthor
                    {
                        BookId = book.BookID,
                        AuthorId = authorId
                    };
                    _context.BookAuthors.Add(bookAuthor);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If we got this far, something failed, redisplay form
            ViewBag.Languages = await _context.Languages.ToListAsync();
            ViewBag.Genres = await _context.Genres.ToListAsync();
            ViewBag.Authors = await _context.Authors.ToListAsync();
            return View(book);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books
                .Include(b => b.BookAuthors)
                .Include(b => b.BookGenres)
                .Include(b => b.BookPublishers)
                .FirstOrDefaultAsync(b => b.BookID == id);

            if (book == null)
            {
                return NotFound();
            }

            try
            {
                // Remove related records first
                _context.BookAuthors.RemoveRange(book.BookAuthors);
                _context.BookGenres.RemoveRange(book.BookGenres);
                _context.BookPublishers.RemoveRange(book.BookPublishers);
                
                // Remove the book
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"Book '{book.BookName}' was successfully deleted.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deleting the book.";
                // Log the error
            }

            return RedirectToAction(nameof(Index));
        }
    }
}