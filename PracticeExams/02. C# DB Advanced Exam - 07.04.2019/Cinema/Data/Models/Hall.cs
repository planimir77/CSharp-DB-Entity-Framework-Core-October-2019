namespace Cinema.Data.Models
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Hall
    {
        [Key]
        public int Id { get; set; }

        [MinLength(3), MaxLength(20),Required] // Add Property length for example : [MinLength(minNumber), MaxLength(maxNumber), ]
        public string Name { get; set; }

        public bool Is4Dx { get; set; }

        public bool Is3D { get; set; }

        public ICollection<Projection> Projections { get; set; } = new HashSet<Projection>();

        public ICollection<Seat> Seats { get; set; } = new HashSet<Seat>();

    }
}