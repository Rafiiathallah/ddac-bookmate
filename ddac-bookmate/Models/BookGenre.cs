using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ddac_bookmate.Models
{
    public class BookGenre
    {
        [Key]
        public int BookGenreId { get; set; }
        public int BookId { get; set; }
        public int GenreId { get; set; }

        // Navigation properties
        [ForeignKey("BookId")]
        public Book Book { get; set; }
        [ForeignKey("GenreId")]
        public Genre Genre { get; set; }
    }
}
