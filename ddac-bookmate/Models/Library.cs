using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ddac_bookmate.Areas.Identity.Data;

namespace ddac_bookmate.Models
{
    public class Library
    {
        [Key]
        public int LibraryId { get; set; }
        public string UserId { get; set; }
        [Required]
        public int BookCount { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.Now;

        // Navigation properties
        public ICollection<BookLibrary> BookAuthors { get; set; }

        public ddac_bookmateUser User { get; set; }

    }
}
