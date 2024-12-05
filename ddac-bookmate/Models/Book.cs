using System.ComponentModel.DataAnnotations;

namespace ddac_bookmate.Models
{
    public class Book
    {
        [Key]
        public int BookID { get; set; }
        public string? BookName { get; set; }
        public string? BookAuthor{ get; set; }
        public DateTime BookPublishedDate { get; set; }
        public decimal BookPrice { get; set; }
    }
}
