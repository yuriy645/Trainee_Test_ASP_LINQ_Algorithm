using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace _1_AuthorsBooks
{
    public partial class FirstName
    {
        public FirstName()
        {
            Authors = new HashSet<Author>();
        }
        public int FirstNameId { get; set; }
        [Required]
        [Display(Name = "First Name ")]
        public string FirstName1 { get; set; }
        public virtual ICollection<Author> Authors { get; set; }
    }
}
