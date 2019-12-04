using Cinema.Data.Models.Enums;

namespace Cinema.Data.Models
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [MinLength(3), MaxLength(20),Required] // Add Property length for example : [MinLength(minNumber), MaxLength(maxNumber), ]
        public string Title { get; set; }

        [Required]
        public Genre Genre { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        [Range(1, 10), Required] // Add Property range for example: [Range(MinValue, MaxValue)]
        public double Rating { get; set; }

        [MinLength(3), MaxLength(20),Required] // Add Property length for example : [MinLength(minNumber), MaxLength(maxNumber), ]
        public string Director { get; set; }

        public ICollection<Projection> Projections { get; set; } = new HashSet<Projection>();

    }
}