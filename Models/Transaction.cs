using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WalletApi.Enums;

namespace WalletApi.Models
{
    public class Transaction
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? ToAccountId { get; set; }
        public string? FromAccountId { get; set; }
        public string? SenderIp { get; set; }
        public string? Geolocation { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public TransactionStatus Status { get; set; }

        [Precision(18,2)]
        public decimal Value { get; set; }
        public Account? ToAccount { get; set; }
        public Account? FromAccount { get; set; }
        public Refunding? Refunding { get; set; }
        public string? RefundingId { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}; ToAccountId: {ToAccountId}; FromAccountId: {FromAccountId}; RefundingId: {RefundingId}; Status: {Status}";
        }
    }
}