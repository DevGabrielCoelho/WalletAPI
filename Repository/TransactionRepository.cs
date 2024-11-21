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
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDBContext _context;
        public TransactionRepository(ApplicationDBContext applicationDBContext, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = applicationDBContext;
        }
        public async Task AddAsync(Transaction transaction)
        {
            if (_context.Transactions == null)
                throw new InvalidOperationException("The Transactions DbSet is not initialized.");

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public Transaction AddIp(Transaction transaction)
        {
            var ipAddres = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            transaction.SenderIp = ipAddres;
            return transaction;
        }

        public async Task<List<TransactionDto>> GetAllDtoAsync()
        {
            if (_context.Transactions == null)
                throw new InvalidOperationException("The Transactions DbSet is not initialized.");
            var transactions = await _context.Transactions.Select(x => x.ToTransactionDto()).ToListAsync();
            return transactions;
        }

        public async Task<TransactionDto> GetByIdDtoAsync(string id)
        {
            if (_context.Transactions == null)
                throw new InvalidOperationException("The Transactions DbSet is not initialized.");
            var transaction = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == id);
            if (transaction == null)
                 throw new InvalidOperationException($"Transaction with ID {id} not found.");
            return transaction.ToTransactionDto();
        }

        public async Task<Transaction> GetByIdTransactionAsync(string id)
        {
            if (_context.Transactions == null)
                throw new InvalidOperationException("The Transactions DbSet is not initialized.");
            var transaction = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == id);
            if (transaction == null)
                throw new InvalidOperationException($"Transaction with ID {id} not found.");
            return transaction;
        }

        public async Task<AccountDto> GetFromAcoByTransferIdAsync(string id)
        {
            if (_context.Transactions == null)
                throw new InvalidOperationException("The Transactions DbSet is not initialized.");
            var transaction = await _context.Transactions.Include(x => x.FromAccount).FirstOrDefaultAsync(x => x.Id == id);
            if (transaction == null)
                throw new InvalidOperationException($"Transaction with ID {id} not found.");
            if (transaction.FromAccount == null)
                throw new InvalidOperationException("The FromAccount related to the transaction is null.");
            return transaction.FromAccount.ToAccountDto();
        }

        public async Task<RefundingDto> GetRefunByTransferIdAsync(string id)
        {
            if (_context.Transactions == null)
                throw new InvalidOperationException("The Transactions DbSet is not initialized.");
            var transaction = await _context.Transactions.Include(x => x.Refunding).FirstOrDefaultAsync(x => x.Id == id);
            if (transaction == null)
                throw new InvalidOperationException($"Transaction with ID {id} not found.");
            if (transaction.Refunding == null)
                throw new InvalidOperationException("The refunding related to the transaction is null.");
            return transaction.Refunding.ToRefundingDto();
        }

        public async Task<AccountDto> GetToAcoByTransferIdAsync(string id)
        {
            if (_context.Transactions == null)
                throw new InvalidOperationException("The Transactions DbSet is not initialized.");
            var transaction = await _context.Transactions.Include(x => x.ToAccount).FirstOrDefaultAsync(x => x.Id == id);
            if (transaction == null)
                throw new InvalidOperationException($"Transaction with ID {id} not found.");
            if (transaction.ToAccount == null)
                throw new InvalidOperationException("The ToAccount related to the transaction is null.");
            return transaction.ToAccount.ToAccountDto();
        }

        public async Task UpdateAsync(Transaction transaction, Transaction transactionAlt)
        {
            int alts = 0;
            if (transaction.RefundingId != transactionAlt.RefundingId || transaction.Refunding != transactionAlt.Refunding)
            {
                transaction.Refunding = transactionAlt.Refunding;
                if (_context.Transactions == null)
                    throw new InvalidOperationException("The Transactions DbSet is not initialized.");
                _context.Transactions.Update(transaction);
                await _context.SaveChangesAsync();
                await _context.Transactions.Where(x => x.Id == transaction.Id).ExecuteUpdateAsync(x => x
                    .SetProperty(x => x.RefundingId, transactionAlt.RefundingId)
                );
                await _context.SaveChangesAsync();
                alts++;
            }
            if (transaction.Status != transactionAlt.Status)
            {
                if (_context.Transactions == null)
                   throw new InvalidOperationException("The Transactions DbSet is not initialized.");
                await _context.Transactions.Where(x => x.Id == transaction.Id).ExecuteUpdateAsync(x => x
                    .SetProperty(x => x.Status, transactionAlt.Status)
                );
                alts++;
            }
            if (alts > 0)
            {
                if (_context.Transactions == null)
                    throw new InvalidOperationException("The Transactions DbSet is not initialized.");
                await _context.Transactions.Where(x => x.Id == transaction.Id).ExecuteUpdateAsync(x => x
                    .SetProperty(x => x.UpdatedAt, DateTime.Now)
                );
            }
            await _context.SaveChangesAsync();
        }
    }
}