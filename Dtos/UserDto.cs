using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalletApi.Dtos
{
    public class UserDto
    {
        public string? Id { get; set; }
        public string? AccountId { get; set; }
        public string? Name { get; set; }
        public DateTime Birthday { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}