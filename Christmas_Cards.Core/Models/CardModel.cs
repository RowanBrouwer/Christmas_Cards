using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Christmas_Cards.Models
{
    public class CardModel
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public Images Image { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public FontType FontType { get; set; }

        //public Music { get; set; }

        public ICollection<EmailModel> Emails { get; set; }
    }

    public enum FontType
    {
        Times_New_Roman,
        Comic_Sans,
        Cursive
    }
}
