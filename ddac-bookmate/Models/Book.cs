using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ddac_bookmate.Models
{
    public class Book
    {
        [Key]
        public int BookID { get; set; }
        public string? BookName { get; set; }
        public string? BookAuthor { get; set; }
        public DateTime BookPublishedDate { get; set; }
        public decimal BookPrice { get; set; }
        public string? Synopsis { get; set; }

        // Foreign key for Language
        [Required]
        public int LanguageId { get; set; }

        // Navigation property for Language
        [ForeignKey("LanguageId")]
        public Language Language { get; set; }

        // Navigation properties
        public ICollection<BookAuthor> BookAuthors { get; set; }
        public ICollection<BookPublisher> BookPublishers { get; set; }
        public ICollection<BookGenre> BookGenres { get; set; }
        public ICollection<BookLibrary> BookLibraries { get; set; }
        public ICollection<BookWishlist> BookWishlists { get; set; }
    }
}
