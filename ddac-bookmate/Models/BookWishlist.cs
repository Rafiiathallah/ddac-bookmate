using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ddac_bookmate.Models
{
    public class BookWishlist
    {
        [Key]
        public int BookWishlistId { get; set; }
        public int BookId { get; set; }
        public int WishlistId { get; set; }

        // Navigation properties
        [ForeignKey("BookId")]
        public Book Book { get; set; }
        [ForeignKey("WishlistId")]
        public Wishlist Wishlist { get; set; }
    }
}
