using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace _1_AuthorsBooks
{
    public partial class Book
    {
        public Book()
        {
            AuthorsBooks = new HashSet<AuthorsBook>();
        }
        public int BookId { get; set; }
        [UIHint("HiddenInput")]
        public string BookName { get; set; }
        public int GenreId { get; set; }
        [UIHint("HiddenInput")]
        public int Pages { get; set; }
        public virtual Genre Genre { get; set; }
        public virtual ICollection<AuthorsBook> AuthorsBooks { get; set; }
    }
}
