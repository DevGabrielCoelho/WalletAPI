using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletApi.Dtos;
using WalletApi.Models;

namespace WalletApi.Mappers
{
    public static class RefundingMappers
    {
        public static RefundingDto ToRefundingDto(this Refunding refunding){
            return new RefundingDto{
                CreatedAt = refunding.CreatedAt,
                CreatedBy = refunding.CreatedBy,
                Description = refunding.Description,
                Id = refunding.Id,
                TransactionId = refunding.TransactionId,
                UpdatedAt = refunding.UpdatedAt
            };
        }
    }
}