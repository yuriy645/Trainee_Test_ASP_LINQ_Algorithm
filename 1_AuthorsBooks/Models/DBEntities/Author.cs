using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace _1_AuthorsBooks
{
    public partial class Author
    {
        public Author()
        {
            AuthorsBooks = new List<AuthorsBook>();
        }

        public int AuthorId { get; set; }

        [Required]
        [Display(Name = "Second Name ")]
        public string AuthorSecondName { get; set; }
        public int FirstNameId { get; set; }
        public int PatronymicId { get; set; }

        public virtual FirstName FirstName { get; set; }
        public virtual Patronymic Patronymic { get; set; }
        public virtual List<AuthorsBook> AuthorsBooks { get; set; }
    }
}
