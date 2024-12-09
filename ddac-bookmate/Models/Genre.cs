using System.ComponentModel.DataAnnotations;

namespace ddac_bookmate.Models
{
    public class Genre
    {
        [Key]
        public int GenreId { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }

        // Navigation properties
        public ICollection<BookGenre> BookGenres { get; set; }
    }
}
