using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ddac_bookmate.Models;
using Microsoft.AspNetCore.Authorization;
using ddac_bookmate.Data;

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
            var userId = User.Identity.Name;
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
            if (bookCart != null)
            {
                bookCart.Quantity = quantity;
                await _context.SaveChangesAsync();
                
                // Recalculate cart total
                var cart = await _context.Carts
                    .Include(c => c.BookCarts)
                    .FirstOrDefaultAsync(c => c.CartId == bookCart.CartId);
                    
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
    }
} 