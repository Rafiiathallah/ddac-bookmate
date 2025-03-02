﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ddac_bookmate.Models
{
    public class BookLibrary
    {
        [Key]
        public int BookLibraryId { get; set; }
        public int BookId { get; set; }
        public int LibraryId { get; set; }
        public bool IsFavourite { get; set; } = false;

        // Navigation properties
        [ForeignKey("BookId")]
        public Book Book { get; set; }
        [ForeignKey("LibraryId")]
        public Library Library { get; set; }
    }
}
