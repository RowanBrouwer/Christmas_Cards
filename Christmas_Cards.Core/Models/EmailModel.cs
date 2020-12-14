using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Christmas_Cards.Models
{
    public class EmailModel
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string MiddelName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string FullName() => (MiddelName == null) ? $"{FirstName}{LastName}" : $"{FirstName}{MiddelName}{LastName}";

    }
}
