namespace SoftJail.Data.Models
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3), MaxLength(25) ]
        public string Name { get; set; }

        public ICollection<Cell> Cells { get; set; } = new HashSet<Cell>();

    }
}