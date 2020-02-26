using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ABCCC.Models
{
    public class CCInformation
    {
        public int CreditCardId { get; set; }
        public int OrderId { get; set; }
        public Transaction Transaction { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [CreditCard]
        public string Number { get; set; }

        [Required]
        [Range(1,12, ErrorMessage = "Please Enter a valid month number")]
        [Display(Name = "Expiry Month")]
        public int ExpiryMonth { get; set; }

        [RegularExpression(@"^(\d{2}$)", ErrorMessage = "Enter a two digit year")]
        [Required]
        [Display(Name="Expiry Year")]
        public int ExpiryYear { get; set; }

        [Required]
        [RegularExpression(@"^(\d{3}$)", ErrorMessage = "Enter a three digit CCV")]
        public int CCV { get; set; }
    }
}
