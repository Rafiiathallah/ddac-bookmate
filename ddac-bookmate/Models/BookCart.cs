using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ddac_bookmate.Models
{
    public class BookCart
    {
        [Key]
        public int BookCartId { get; set; }
        
        public int BookId { get; set; }
        public int CartId { get; set; }
        
        [Required]
        public int Quantity { get; set; } = 1;  // Default to 1
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }  // Price at the time of adding to cart
        
        // Navigation properties
        [ForeignKey("BookId")]
        public Book Book { get; set; }
        
        [ForeignKey("CartId")]
        public Cart Cart { get; set; }
    }
} 