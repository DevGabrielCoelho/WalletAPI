using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WalletApi.Dtos
{
    public class CreateUserDto
    {
        [Required]
        [MinLength(2, ErrorMessage = "Name must be 2 characters")]
        [MaxLength(50, ErrorMessage = "Name cannot be over 50 characters")]
        [Display(Prompt = "Your Name")]
        public string? Name { get; set; }

        [Required]
        [MinLength(12, ErrorMessage = "Document must be 12 characters")]
        [MaxLength(15, ErrorMessage = "Document cannot be over 15 characters")]
        [Display(Prompt = "123.456.789-10")]
        public string? Document { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        [Display(Prompt = "DD/MM/AAAA")]
        public DateTime Birthday { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Email must be 5 characters")]
        [MaxLength(30, ErrorMessage = "Email cannot be over 30 characters")]
        [DataType(DataType.EmailAddress)]
        [Display(Prompt = "YourEmail@email.com")]
        public string? Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Phone must be 8 characters")]
        [MaxLength(25, ErrorMessage = "Phone cannot be over 25 characters")]
        [DataType(DataType.PhoneNumber)]
        [Display(Prompt = "+CC (DDD) 1234-5678")]
        public string? Phone { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be 8 characters")]
        [MaxLength(25, ErrorMessage = "Password cannot be over 25 characters")]
        [DataType(DataType.Password)]
        [Display(Prompt = "YourPassword")]
        public string? Password { get; set; }
    }
}