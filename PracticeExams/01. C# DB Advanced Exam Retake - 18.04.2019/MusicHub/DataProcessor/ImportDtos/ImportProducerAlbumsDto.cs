using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using MusicHub.Data.Models;

namespace MusicHub.DataProcessor.ImportDtos
{
    public class ImportProducerAlbumsDto
    {
        [Required]
        [MinLength(3),MaxLength(30)] 
        public string Name { get; set; }

        [RegularExpression("^[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+")]
        public string Pseudonym { get; set; }

        [RegularExpression(@"\+359 [0-9]{3} [0-9]{3} [0-9]{3}")]
        public string PhoneNumber { get; set; }
        
        public ImportAlbumDto[] Albums { get; set; } //= new HashSet<ImportAlbumDto>();
    }
}
