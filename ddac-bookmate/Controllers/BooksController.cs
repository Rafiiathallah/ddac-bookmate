using ddac_bookmate.Data;
using ddac_bookmate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ddac_bookmate.Controllers
{
    public class BooksController : Controller
    {

        private readonly ddac_bookmateContext _context;
        public BooksController(ddac_bookmateContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Book> books = await _context.BookTable.ToListAsync();
            return View(books);
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult AddData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddData(Book book)
        {
            // Convert to UTC before saving
            book.BookPublishedDate = DateTime.SpecifyKind(book.BookPublishedDate, DateTimeKind.Utc);

            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        //load an update page for the user
        public async Task<IActionResult> EditData(int? BookID)
        {
            if (BookID == null)
            {
                return NotFound();
            }
            var book = await _context.BookTable.FindAsync(BookID);
            if (book == null)
            {
                return BadRequest(BookID + " is not found in the table!");
            }
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateData(Book book)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Convert to UTC before saving
                    book.BookPublishedDate = DateTime.SpecifyKind(book.BookPublishedDate, DateTimeKind.Utc);
                    
                    _context.BookTable.Update(book);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Books");
                }
                return View("EditData", book);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        public async Task<IActionResult> DeleteData(int? BookID)
        {
            if (BookID == null)
            {
                return NotFound();
            }
            var book = await _context.BookTable.FindAsync(BookID);
            if (book == null)
            {
                return BadRequest(BookID + " is not found in the list!");
            }
            _context.BookTable.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Books");
        }

    }
}
