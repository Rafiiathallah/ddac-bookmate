using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ddac_bookmate.Models;
using Microsoft.AspNetCore.Authorization;
using ddac_bookmate.Data;
using System.Security.Claims;

namespace ddac_bookmate.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ddac_bookmateContext _context;

        public CartController(ddac_bookmateContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _context.Carts
                .Include(c => c.BookCarts)
                    .ThenInclude(bc => bc.Book)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int bookCartId, int quantity)
        {
            var bookCart = await _context.BookCarts.FindAsync(bookCartId);
            if (bookCart == null)
            {
                return NotFound();
            }

            if (quantity <= 0)
            {
                // Remove item if quantity is 0 or less
                _context.BookCarts.Remove(bookCart);
            }
            else
            {
                bookCart.Quantity = quantity;
            }

            await _context.SaveChangesAsync();

            // Recalculate cart total
            var cart = await _context.Carts
                .Include(c => c.BookCarts)
                .FirstOrDefaultAsync(c => c.CartId == bookCart.CartId);

            if (cart != null)
            {
                cart.TotalPrice = cart.BookCarts.Sum(bc => bc.UnitPrice * bc.Quantity);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(int bookCartId)
        {
            var bookCart = await _context.BookCarts.FindAsync(bookCartId);
            if (bookCart != null)
            {
                _context.BookCarts.Remove(bookCart);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int bookId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _context.Carts
                .Include(c => c.BookCarts)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            // Create cart if it doesn't exist
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            // Get book details
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                return NotFound();
            }

            // Check if book already in cart
            var existingItem = cart.BookCarts?
                .FirstOrDefault(bc => bc.BookId == bookId);

            if (existingItem != null)
            {
                existingItem.Quantity += 1;
            }
            else
            {
                // Add new book to cart
                var bookCart = new BookCart
                {
                    BookId = bookId,
                    CartId = cart.CartId,
                    Quantity = 1,
                    UnitPrice = book.BookPrice
                };
                _context.BookCarts.Add(bookCart);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public async Task<IActionResult> Purchase()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            // Get user's cart with books
            var cart = await _context.Carts
                .Include(c => c.BookCarts)
                    .ThenInclude(bc => bc.Book)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.BookCarts.Any())
            {
                return RedirectToAction(nameof(Index));
            }

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

            // Add books to library (skip if already exists)
            foreach (var bookCart in cart.BookCarts)
            {
                var existingBook = library.BookAuthors?
                    .FirstOrDefault(ba => ba.BookId == bookCart.BookId);

                if (existingBook == null)
                {
                    var bookLibrary = new BookLibrary
                    {
                        BookId = bookCart.BookId,
                        LibraryId = library.LibraryId,
                        IsFavourite = false
                    };
                    _context.BookLibraries.Add(bookLibrary);
                    library.BookCount++;
                }
            }

            // Clear the cart
            _context.BookCarts.RemoveRange(cart.BookCarts);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Library");
        }
    }
} 