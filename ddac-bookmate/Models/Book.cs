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

        // Navigation properties
        public ICollection<BookAuthor> BookAuthors { get; set; }
        public ICollection<BookPublisher> BookPublishers { get; set; }
        public ICollection<BookGenre> BookGenres { get; set; }
        public ICollection<BookLibrary> BookLibraries { get; set; }
    }
}
