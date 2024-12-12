using Microsoft.AspNetCore.Mvc;
using ddac_bookmate.Models;
using ddac_bookmate.Data;
using Microsoft.EntityFrameworkCore;

namespace ddac_bookmate.Controllers
{
    public class GenresController : Controller
    {
        private readonly ddac_bookmateContext _context;

        public GenresController(ddac_bookmateContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var genres = await _context.Genres.ToListAsync();
            return View(genres);
        }
    }
}
