namespace Cinema.Data.Models
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [MinLength(3), MaxLength(20), Required] // Add Property length for example : [MinLength(minNumber), MaxLength(maxNumber), ]
        public string FirstName { get; set; }

        [MinLength(3), MaxLength(20), Required] // Add Property length for example : [MinLength(minNumber), MaxLength(maxNumber), ]
        public string LastName { get; set; }

        [Range(12, 110), Required] // Add Property range for example: [Range(MinValue, MaxValue)]
        public int Age { get; set; }

        [Range(0, Double.MaxValue), Required] // Add Property range for example: [Range(MinValue, MaxValue)]
        public decimal  Balance { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();

    }
}