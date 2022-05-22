using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1_AuthorsBooks.Models
{
    public class CacheBook
    {
        public int AuthorId { get; set; }
        public string BookName { get; set; }
        public string GenreName { get; set; }
        public int Pages { get; set; }
    }
}
