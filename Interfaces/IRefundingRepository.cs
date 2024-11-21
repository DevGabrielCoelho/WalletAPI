using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletApi.Dtos;
using WalletApi.Models;

namespace WalletApi.Interfaces
{
    public interface IRefundingRepository
    {
        Task<List<RefundingDto>> GetAllDtoAsync();
        Task<RefundingDto> GetByIdDtoAsync(string id);
        Task<Refunding> GetByIdRefundingAsync(string id);
        Task<Refunding> GetByTransactionIdAsync(string id);
        Task AddAsync(Refunding refunding);
        Task<TransactionDto> GetTransferByRefundingIdAsync(string id);
    }
}