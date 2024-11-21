using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WalletApi.Data;
using WalletApi.Dtos;
using WalletApi.Interfaces;
using WalletApi.Mappers;
using WalletApi.Models;

namespace WalletApi.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDBContext _context;
        public AccountRepository(ApplicationDBContext applicationDBContext)
        {
            _context = applicationDBContext;
        }

        public async Task AddAsync(Account account)
        {
            if (_context.Accounts == null)
                throw new InvalidOperationException("Accounts DbSet is not initialized.");
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task AddIncommingAsync(string id, Transaction transaction)
        {
            if (_context.Accounts == null)
                throw new InvalidOperationException("Accounts DbSet is not initialized.");
            var account = await _context.Accounts.Include(x => x.IncomingTransactions).FirstOrDefaultAsync(x => x.Id == id);

            if (account == null)
                throw new InvalidOperationException("Account not found.");

            if (account.IncomingTransactions == null)
                throw new InvalidOperationException("Incoming transactions collection is not initialized.");

            var accountTransaction = account.IncomingTransactions.FirstOrDefault(x => x.Id == transaction.Id);

            if (accountTransaction == null)
            {
                account.IncomingTransactions.Add(transaction);
            }
            else
            {
                account.IncomingTransactions.Remove(accountTransaction);
                account.IncomingTransactions.Add(transaction);
            }
            await _context.SaveChangesAsync();
            await _context.Accounts.Where(x => x.Id == id).ExecuteUpdateAsync(x => x
                .SetProperty(x => x.UpdatedAt, DateTime.Now)
            );
        }

        public async Task AddoutcommingAsync(string id, Transaction transaction)
        {
            if (_context.Accounts == null)
                throw new InvalidOperationException("Accounts DbSet is not initialized.");
            var account = await _context.Accounts.Include(x => x.OutgoingTransactions).FirstOrDefaultAsync(x => x.Id == id);

            if (account == null)
                throw new InvalidOperationException("Account not found.");

            if (account.OutgoingTransactions == null)
                throw new InvalidOperationException("Outgoing transactions collection is not initialized.");

            var accountTransaction = account.OutgoingTransactions.FirstOrDefault(x => x.Id == transaction.Id);

            if (accountTransaction == null) account.OutgoingTransactions.Add(transaction);
            else
            {
                account.OutgoingTransactions.Remove(accountTransaction);
                account.OutgoingTransactions.Add(transaction);
            }
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            await _context.Accounts.Where(x => x.Id == id).ExecuteUpdateAsync(x => x
                .SetProperty(x => x.UpdatedAt, DateTime.Now)
            );
        }

        public async Task<List<AccountDto>> GetAllAsync()
        {
            if (_context.Accounts == null)
                throw new InvalidOperationException("Accounts DbSet is not initialized.");
            return await _context.Accounts.Select(x => x.ToAccountDto()).ToListAsync();
        }

        public async Task<List<Transaction>> GetAllInAsync(string id)
        {
            if (_context.Accounts == null)
                throw new InvalidOperationException("Accounts DbSet is not initialized.");
            var account = await _context.Accounts.Include(x => x.IncomingTransactions).FirstOrDefaultAsync(x => x.Id == id);
            if (account == null)
                throw new InvalidOperationException("Account not found.");
            return account.IncomingTransactions;
        }
        public async Task<List<Transaction>> GetAllOutAsync(string id)
        {
            if (_context.Accounts == null)
                throw new InvalidOperationException("Accounts DbSet is not initialized.");
            var account = await _context.Accounts.Include(x => x.OutgoingTransactions).FirstOrDefaultAsync(x => x.Id == id);
            if (account == null)
                throw new InvalidOperationException("Account not found.");
            return account.OutgoingTransactions;
        }

        public async Task<Account> GetByIdAccountAsync(string id)
        {
            if (_context.Accounts == null)
                throw new InvalidOperationException("Accounts DbSet is not initialized.");
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id);
            if (account == null)
                throw new InvalidOperationException("Account not found.");
            return account;
        }

        public async Task<AccountDto> GetByIdDtoAsync(string id)
        {
            if (_context.Accounts == null)
                throw new InvalidOperationException("Accounts DbSet is not initialized.");
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id);
            if (account == null)
                throw new InvalidOperationException("Account not found.");
            return account.ToAccountDto();
        }

        public async Task UpdateBalance(string id, decimal value, char op)
        {
            if (_context.Accounts == null)
                throw new InvalidOperationException("Accounts DbSet is not initialized.");
            var account = await this.GetByIdAccountAsync(id);
            if (op == 's')
            {
                await _context.Accounts.Where(x => x.Id == id).ExecuteUpdateAsync(x => x
                    .SetProperty(x => x.Balance, account.Balance - value)
                );
            }
            else if (op == 'a')
            {
                await _context.Accounts.Where(x => x.Id == id).ExecuteUpdateAsync(x => x
                    .SetProperty(x => x.Balance, account.Balance + value)
                );
            }
            await _context.Accounts.Where(x => x.Id == id).ExecuteUpdateAsync(x => x
                .SetProperty(x => x.UpdatedAt, DateTime.Now)
            );
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidationToken(string id, string token)
        {
            if (_context.Accounts == null)
                throw new InvalidOperationException("DbSet is not initialized.");
            var account = await _context.Accounts.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
            if (account == null)
                throw new InvalidOperationException("Account is not initialized.");
            if (account.User == null)
                throw new InvalidOperationException("Accounts DbSet is not initialized.");
            return account.User.SessionToken == token;
        }
    }
}