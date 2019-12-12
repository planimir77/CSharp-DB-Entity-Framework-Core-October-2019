using VaporStore.Data.Models.Enum;

namespace VaporStore.Data.Models
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Purchase
    {
        [Key]
        public int Id { get; set; }

        [Required] 
        public PurchaseType Type { get; set; }

        [Required]
        [RegularExpression("^[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}$")]
        public string ProductKey { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [ForeignKey(nameof(Card)), Required]
        public int CardId { get; set; }

        public Card Card { get; set; }

        [ForeignKey(nameof(Game)), Required]
        public int GameId { get; set; }

        public Game Game { get; set; }

    }
}