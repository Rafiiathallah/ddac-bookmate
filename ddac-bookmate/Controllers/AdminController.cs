using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ddac_bookmate.Data;
using ddac_bookmate.Models;
using Microsoft.EntityFrameworkCore;
using ddac_bookmate.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace ddac_bookmate.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ddac_bookmateContext _context;
        private readonly UserManager<ddac_bookmateUser> _userManager;

        public AdminController(ddac_bookmateContext context, UserManager<ddac_bookmateUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || !user.IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            var books = await _context.Books
                .Include(b => b.Language)
                .ToListAsync();

            return View(books);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || !user.IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}