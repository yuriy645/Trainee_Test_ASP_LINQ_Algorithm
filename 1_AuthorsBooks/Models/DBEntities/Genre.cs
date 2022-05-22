using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace _1_AuthorsBooks
{
    public partial class Genre
    {
        public Genre()
        {
            Books = new HashSet<Book>();
        }

        public int GenreId { get; set; }

        [Required]
        [UIHint("HiddenInput")]
        public string GenreName { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
