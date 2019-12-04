using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Cinema.Data.Models.Enums;

namespace Cinema.DataProcessor.ImportDto
{
    public class ImportMovieDto
    {
        //[MinLength(3), MaxLength(20),Required] 
        public string Title { get; set; }

        //[Required]
        public string Genre { get; set; }

        //[Required]
        public string Duration { get; set; }

        //[Range(1, 10), Required] 
        public double Rating { get; set; }

        //[MinLength(3), MaxLength(20),Required] 
        public string Director { get; set; }
    }
}
