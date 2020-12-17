using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Christmas_Cards.Models
{
    [Serializable]
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
        [Required]
        public FontSize FontSize { get; set; }
        [Required]
        public string FontColour { get; set; }

        //public Music { get; set; }
        public ICollection<EmailModel> Emails { get; set; }

    }

    public enum FontType
    {
        [Display(Name = "The Perfect Christmas")]
        ThePerfectChristmas,
        [Display(Name = "Christmas Bell's")]
        ChristmasBellPersonalUse,
        [Display(Name = "Christmas Lights")]
        ChristmasLightsOutdoor,
        [Display(Name = "Holly and Berries")]
        holly_and_berries,
        [Display(Name = "Merry Christmas Flakes")]
        MerryChristmasFlake,
        [Display(Name = "Christmas Font")]
        PWChristmasfont,
        [Display(Name = "Happy Christmas")]
        PWHappyChristmas

    }




    public enum FontSize
    {
      //pt = px
        pt20 = 25,
        pt30 = 40,
        pt40 = 50,
        pt50 = 66,
        pt60 = 80,
        pt70 = 90,
        pt80 = 106,
        pt90 = 120,
        pt100 = 133,
        pt120 = 160,
    }
}
