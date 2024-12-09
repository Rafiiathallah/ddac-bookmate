using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ddac_bookmate.Models
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Bio { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public int? GenreId { get; set; }

        // Navigation properties
        [ForeignKey("GenreId")]
        public Genre? Genre { get; set; }
        public ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
