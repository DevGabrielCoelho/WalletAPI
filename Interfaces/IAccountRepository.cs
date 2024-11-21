using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletApi.Dtos;
using WalletApi.Models;

namespace WalletApi.Interfaces
{
    public interface IAccountRepository
    {
        Task<List<AccountDto>> GetAllAsync();
        Task<AccountDto> GetByIdDtoAsync(string id);
        Task<Account> GetByIdAccountAsync(string id);
        Task AddIncommingAsync(string id, Transaction transaction);
        Task AddoutcommingAsync(string id, Transaction transaction);
        Task UpdateBalance(string id, decimal value, char op /*'a' || 's'*/);
        Task AddAsync(Account account);
        Task<List<Transaction>> GetAllInAsync(string id);
        Task<List<Transaction>> GetAllOutAsync(string id);
        Task<bool> ValidationToken(string id, string token);
    }
}