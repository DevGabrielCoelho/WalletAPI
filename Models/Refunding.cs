using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalletApi.Models
{
    public class Refunding
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? TransactionId { get; set; }
        public string? Description { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public Transaction? Transaction { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}; TransactionId: {TransactionId}";
        }
    }
}