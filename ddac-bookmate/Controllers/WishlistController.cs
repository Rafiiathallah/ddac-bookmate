using ddac_bookmate.Data;
using ddac_bookmate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ddac_bookmate.Controllers
{
    [Authorize]
    public class WishlistController : Controller
    {
        private readonly ddac_bookmateContext _context;

        public WishlistController(ddac_bookmateContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            var wishlist = await _context.Wishlists
                .Include(w => w.BookWishlists)
                    .ThenInclude(bw => bw.Book)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            return View(wishlist);
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishlist(int bookId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wishlist = await _context.Wishlists
                .Include(w => w.BookWishlists)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            // Create wishlist if it doesn't exist
            if (wishlist == null)
            {
                wishlist = new Wishlist { UserId = userId };
                _context.Wishlists.Add(wishlist);
                await _context.SaveChangesAsync();
            }

            // Check if book already in wishlist
            var existingItem = wishlist.BookWishlists?
                .FirstOrDefault(bw => bw.BookId == bookId);

            if (existingItem == null)
            {
                var bookWishlist = new BookWishlist
                {
                    BookId = bookId,
                    WishlistId = wishlist.WishlistId
                };
                _context.BookWishlists.Add(bookWishlist);
                wishlist.BookCount++;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromWishlist(int bookId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wishlist = await _context.Wishlists
                .Include(w => w.BookWishlists)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishlist != null)
            {
                var bookWishlist = wishlist.BookWishlists
                    .FirstOrDefault(bw => bw.BookId == bookId);

                if (bookWishlist != null)
                {
                    _context.BookWishlists.Remove(bookWishlist);
                    wishlist.BookCount--;
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}