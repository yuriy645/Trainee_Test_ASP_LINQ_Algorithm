using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace _1_AuthorsBooks
{
    public partial class Patronymic
    {
        public Patronymic()
        {
            Authors = new HashSet<Author>();
        }
        public int PatronymicId { get; set; }
        [Required]
        [Display(Name = "Third Name ")]
        public string Patronymic1 { get; set; }
        public virtual ICollection<Author> Authors { get; set; }
    }
}
