using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ddac_bookmate.Areas.Identity.Data;

namespace ddac_bookmate.Models
{
    public class Wishlist
    {
        [Key]
        public int WishlistId { get; set; }
        public string UserId { get; set; }
        public int BookCount { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<BookWishlist> BookWishlists { get; set; }
        public ddac_bookmateUser User { get; set; }
    }
}
