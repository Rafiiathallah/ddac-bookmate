using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ddac_bookmate.Models
{
    public class BookAuthor
    {
        [Key]
        public int BookAuthorId { get; set; }
        public int BookId { get; set; }
        public int AuthorId { get; set; }

        // Navigation properties
        [ForeignKey("BookId")]
        public Book Book { get; set; }
        [ForeignKey("AuthorId")]
        public Author Author { get; set; }
    }
}
