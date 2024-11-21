using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WalletApi.Enums;

namespace WalletApi.Dtos
{
    public class TransactionDto
    {
        public string? Id { get; set; }
        public string? ToAccountId { get; set; }
        public string? FromAccountId { get; set; }
        public string? RefundingId { get; set; }
        public string? SenderIp { get; set; }
        public string? Geolocation { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public TransactionStatus Status { get; set; }

        [Precision(18,2)]
        public decimal Value { get; set; }
    }
}