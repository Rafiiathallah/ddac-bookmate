using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ddac_bookmate.Models;
using Microsoft.AspNetCore.Authorization;
using ddac_bookmate.Data;
using System.Security.Claims;
using Amazon.SimpleNotificationService;
using ddac_bookmate.Services;

namespace ddac_bookmate.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ddac_bookmateContext _context;
        private readonly ISNSService _snsService;

        public CartController(ddac_bookmateContext context, ISNSService snsService)
        {
            _context = context;
            _snsService = snsService;
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

            // Add this after line 246, before clearing the cart
            foreach (var bookCart in cart.BookCarts)
            {
                var book = bookCart.Book;
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var message = $"Book '{book.BookName}' has been added to your library.";
                var notificationSent = await _snsService.PublishMessageAsync(
                    message,
                    "New Books Added to Library",
                    userEmail
                );
                
                if (!notificationSent)
                {
                    Console.WriteLine($"Failed to send notification for book: {book.BookName}");
                }
            }

            return RedirectToAction("Index", "Library");
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _context.Carts
                .Include(c => c.BookCarts)
                    .ThenInclude(bc => bc.Book)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.BookCarts.Any())
            {
                return RedirectToAction(nameof(Index));
            }

            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            // Simulate payment processing delay
            await Task.Delay(1000);
            
            // Add books to library (existing logic)
            var cart = await _context.Carts
                .Include(c => c.BookCarts)
                    .ThenInclude(bc => bc.Book)
                .FirstOrDefaultAsync(c => c.UserId == userId);

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

            return Json(new { success = true, message = "Payment successful! Books added to your library." });
        }

        [HttpPost]
        public async Task<IActionResult> BuyNow(int bookId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _context.Carts
                .Include(c => c.BookCarts)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            // Clear existing cart items
            if (cart?.BookCarts != null)
            {
                _context.BookCarts.RemoveRange(cart.BookCarts);
            }

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

            // Add book to cart
            var bookCart = new BookCart
            {
                BookId = bookId,
                CartId = cart.CartId,
                Quantity = 1,
                UnitPrice = book.BookPrice
            };
            _context.BookCarts.Add(bookCart);
            await _context.SaveChangesAsync();

            // Redirect to checkout
            return RedirectToAction(nameof(Checkout));
        }
    }
} 