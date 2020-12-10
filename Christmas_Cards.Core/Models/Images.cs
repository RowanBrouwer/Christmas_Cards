using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Christmas_Cards.Models
{
    public class Images
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string ImagePath { get; set; }
    }
}
