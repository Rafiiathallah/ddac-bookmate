using ddac_bookmate.Data;
using ddac_bookmate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ddac_bookmate.Services;

namespace ddac_bookmate.Controllers
{
    [Authorize]
    public class LibraryController : Controller
    {
        private readonly ddac_bookmateContext _context;
        private readonly ISNSService _snsService;

        public LibraryController(ddac_bookmateContext context, ISNSService snsService)
        {
            _context = context;
            _snsService = snsService;
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

        [HttpPost]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var library = await _context.Libraries
                .FirstOrDefaultAsync(l => l.UserId == userId);

            if (library != null)
            {
                var libraryBook = await _context.Set<BookLibrary>()
                    .FirstOrDefaultAsync(bl => bl.BookId == id && bl.LibraryId == library.LibraryId);

                if (libraryBook != null)
                {
                    _context.Set<BookLibrary>().Remove(libraryBook);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Book removed from your library.";
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddToLibrary(int bookId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            // Get or create user's library
            var library = await _context.Libraries
                .Include(l => l.BookAuthors)
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

            // Check if book already exists in library
            var existingBook = library.BookAuthors?
                .FirstOrDefault(ba => ba.BookId == bookId);

            if (existingBook == null)
            {
                var bookLibrary = new BookLibrary
                {
                    BookId = bookId,
                    LibraryId = library.LibraryId,
                    IsFavourite = false
                };
                _context.BookLibraries.Add(bookLibrary);
                library.BookCount++;
                await _context.SaveChangesAsync();

                // Send test SNS notification
                var book = await _context.Books.FindAsync(bookId);
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var message = $"Book '{book.BookName}' has been added to your library.";
                await _snsService.PublishMessageAsync(
                    message,
                    "New Book Added to Library",
                    userEmail
                );
                
                TempData["Success"] = "Book added to your library!";
            }
            else
            {
                TempData["Info"] = "This book is already in your library.";
            }

            return RedirectToAction("Index", "Library");
        }
    }
}