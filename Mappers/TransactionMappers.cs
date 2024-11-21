using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletApi.Models;
using WalletApi.Dtos;

namespace WalletApi.Mappers
{
    public static class TransactionMappers
    {
        public static TransactionDto ToTransactionDto(this Transaction transaction){
            return new TransactionDto{
                CreatedAt = transaction.CreatedAt,
                FromAccountId = transaction.FromAccountId,
                Geolocation = transaction.Geolocation,
                Id = transaction.Id,
                RefundingId = transaction.RefundingId,
                SenderIp = transaction.SenderIp,
                Status = transaction.Status,
                ToAccountId = transaction.ToAccountId,
                UpdatedAt = transaction.UpdatedAt,
                Value = transaction.Value
            };
        }
    }
}