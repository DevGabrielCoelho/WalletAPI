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
    public class RefundingRepository : IRefundingRepository
    {
        private readonly ApplicationDBContext _context;
        public RefundingRepository(ApplicationDBContext applicationDBContext)
        {
            _context = applicationDBContext;
        }
        public async Task AddAsync(Refunding refunding)
        {
            if (_context.Refundings == null)
                throw new InvalidOperationException("The Refundings DbSet is not initialized.");
            await _context.SaveChangesAsync();
        }

        public async Task<List<RefundingDto>> GetAllDtoAsync()
        {
            if (_context.Refundings == null)
                throw new InvalidOperationException("The Refundings DbSet is not initialized.");
            return await _context.Refundings.Select(x => x.ToRefundingDto()).ToListAsync();
        }

        public async Task<RefundingDto> GetByIdDtoAsync(string id)
        {
            if (_context.Refundings == null)
                throw new InvalidOperationException("The Refundings DbSet is not initialized.");
            var refunding = await _context.Refundings.FirstOrDefaultAsync(x => x.Id == id);
            if (refunding == null)
                throw new InvalidOperationException("Refunding not found with the provided ID.");
            return refunding.ToRefundingDto();
        }

        public async Task<Refunding> GetByIdRefundingAsync(string id)
        {
            if (_context.Refundings == null)
                throw new InvalidOperationException("The Refundings DbSet is not initialized.");
            var refound = await _context.Refundings.FirstOrDefaultAsync(x => x.Id == id);
            if (refound == null)
                throw new InvalidOperationException("Refunding not found with the provided ID.");
            return refound;
        }

        public async Task<Refunding> GetByTransactionIdAsync(string id)
        {
            if (_context.Refundings == null)
                throw new InvalidOperationException("The Refundings DbSet is not initialized.");
            var refound = await _context.Refundings.FirstOrDefaultAsync(x => x.TransactionId == id);
            if (refound == null)
                throw new InvalidOperationException("Refunding not found with the provided transaction ID.");
            return refound;
        }

        public async Task<TransactionDto> GetTransferByRefundingIdAsync(string id)
        {
            if (_context.Refundings == null)
                throw new InvalidOperationException("The Refundings DbSet is not initialized.");
            var refunding = await _context.Refundings.Include(x => x.Transaction).FirstOrDefaultAsync(x => x.Id == id);
            if (refunding == null)
                throw new InvalidOperationException("Refunding not found with the provided ID.");
            if (refunding.Transaction == null)
                throw new InvalidOperationException("No associated transaction found for the provided refunding ID.");
            return refunding.Transaction.ToTransactionDto();
        }
    }
}