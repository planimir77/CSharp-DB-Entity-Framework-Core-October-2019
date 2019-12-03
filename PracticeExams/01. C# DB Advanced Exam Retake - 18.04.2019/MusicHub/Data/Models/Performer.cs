namespace MusicHub.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Performer
    {
        [Key]
        public int Id { get; set; }

        [MinLength(3),MaxLength(20), Required] // Add Property length for example : [MinLength(minNumber),MaxLength(maxNumber), ]
        public string FirstName { get; set; }

        [MinLength(3),MaxLength(20), Required] // Add Property length for example : [MinLength(minNumber),MaxLength(maxNumber)]
        public string LastName { get; set; }

        [Range(18, 70), Required] // Add Property range for example: [Range(MinValue, MaxValue), ]
        public int Age { get; set; }

        [Range(0, double.MaxValue), Required] // Add Property range for example: [Range(MinValue, MaxValue)]
        public decimal  NetWorth { get; set; }

        public ICollection<SongPerformer> PerformerSongs { get; set; } = new HashSet<SongPerformer>();

    }
}