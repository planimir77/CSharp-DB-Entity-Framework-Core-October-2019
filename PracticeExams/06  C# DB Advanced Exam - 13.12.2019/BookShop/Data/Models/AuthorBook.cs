namespace BookShop.Data.Models
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class AuthorBook
    {
        [ForeignKey(nameof(Author)), Required]
        public int AuthorId { get; set; }
        public Author Author { get; set; }


        [ForeignKey(nameof(Book)), Required]
        public int BookId { get; set; }
        public Book Book { get; set; }

    }
}