using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ddac_bookmate.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        
        [Required]
        public string UserId { get; set; }  // To link with the user
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; } = 0;  // Default to 0
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation property
        public ICollection<BookCart> BookCarts { get; set; }
    }
} 