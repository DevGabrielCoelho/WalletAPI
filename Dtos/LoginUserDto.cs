using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WalletApi.Dtos
{
    public class LoginUserDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Email must be 5 characters")]
        [MaxLength(30, ErrorMessage = "Email cannot be over 30 characters")]
        [DataType(DataType.EmailAddress)]
        [Display(Prompt = "YourEmail@email.com")]
        public string? Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be 8 characters")]
        [MaxLength(25, ErrorMessage = "Password cannot be over 25 characters")]
        [DataType(DataType.Password)]
        [Display(Prompt = "YourPassword")]
        public string? Password { get; set; }
    }
}