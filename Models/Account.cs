using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WalletApi.Models
{
    public class Account
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? UserId { get; set; }
        [Precision(18,2)]
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public User? User { get; set; }
        public List<Transaction> IncomingTransactions { get; set; } = new();
        public List<Transaction> OutgoingTransactions { get; set; } = new();

        public override string ToString()
        {
            return $"Id: {Id}; UserId: {UserId}";
        }
    }
}