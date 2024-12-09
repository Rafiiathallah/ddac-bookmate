using System.ComponentModel.DataAnnotations;

namespace ddac_bookmate.Models
{
    public class Language
    {
        [Key]
        public int LanguageId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }

        // Navigation properties
        public ICollection<Book> Books { get; set; }
    }
}
