using System;
using System.Collections.Generic;

#nullable disable

namespace _1_AuthorsBooks
{
    public partial class AuthorsBook
    {
        //public int AuthorsBookId { get; set; }
        public int AuthorId { get; set; }
        public int BookId { get; set; }
        public virtual Author Author { get; set; }
        public virtual Book Book { get; set; }
    }
}
