using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WalletApi.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? AccountId { get; set; }
        public string? Name { get; set; }
        public string? Document { get; set; }
        public DateTime Birthday { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? PasswordHash { get; set; }
        public string? SessionToken { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public Account? Account { get; set; }

        public override string ToString()
        {
            return "Id: " + Id + "; Accountid: " + AccountId;
        }
    }
}

