﻿using BookShop.Data.Models.Enums;

namespace BookShop.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required] 
        [MinLength(3), MaxLength(30)]
        public string Name { get; set; }

        [Required]
        public Genre Genre { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal  Price { get; set; }

        [Range(50, 5000)]
        public int Pages { get; set; }

        [Required]
        public DateTime PublishedOn { get; set; }

        public ICollection<AuthorBook> AuthorsBooks { get; set; } = new HashSet<AuthorBook>();

    }
}