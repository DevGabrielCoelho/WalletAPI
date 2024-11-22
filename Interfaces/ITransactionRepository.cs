using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletApi.Dtos;
using WalletApi.Models;

namespace WalletApi.Interfaces
{
    public interface ITransactionRepository
    {
        Task<List<TransactionDto>> GetAllDtoAsync();
        Task<TransactionDto> GetByIdDtoAsync(string id);
        Task<Transaction> GetByIdTransactionAsync(string id);
        Task AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction, Transaction transactionAlt);
        Task<RefundingDto> GetRefunByTransferIdAsync(string id);
        Task<AccountDto> GetToAcoByTransferIdAsync(string id);
        Task<AccountDto> GetFromAcoByTransferIdAsync(string id);
        Transaction AddIp(Transaction transaction);
        Task<Transaction> AddGeo(Transaction transaction);
    }
}