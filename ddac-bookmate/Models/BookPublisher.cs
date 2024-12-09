using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ddac_bookmate.Models
{
    public class BookPublisher
    {
        [Key]
        public int BookPublisherId { get; set; }
        public int BookId { get; set; }
        public int PublisherId { get; set; }

        // Navigation properties
        [ForeignKey("BookId")]
        public Book Book { get; set; }
        [ForeignKey("PublisherId")]
        public Publisher Publisher { get; set; }
    }
}
