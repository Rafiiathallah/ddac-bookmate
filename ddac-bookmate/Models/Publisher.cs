using System.ComponentModel.DataAnnotations;

namespace ddac_bookmate.Models
{
    public class Publisher
    {
        [Key]
        public int PublisherId { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Bio { get; set; }

        // Navigation properties
        public ICollection<BookPublisher> BookPublishers { get; set; }
    }
}
