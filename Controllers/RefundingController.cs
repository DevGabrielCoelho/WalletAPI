using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WalletApi.Data;
using WalletApi.Interfaces;
using WalletApi.Mappers;
using WalletApi.Models;

namespace WalletApi.Controllers
{
    [ApiController]
    [Route("api/refunding")]
    public class RefundingController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRefundingRepository _refundingRepository;
        private readonly ITransactionRepository _transactionRepository;
        public RefundingController(IAccountRepository accountRepository, IRefundingRepository refundingRepository, ITransactionRepository transactionRepository)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _refundingRepository = refundingRepository;
        }
        /*
                [HttpGet("getall")]
                public async Task<IActionResult> GetAll()
                {
                    return Ok(await _refundingRepository.GetAllDtoAsync());
                }

                [HttpGet("getid/{id}")]
                public async Task<IActionResult> GetById([FromRoute] string id)
                {
                    return Ok(await _refundingRepository.GetByIdDtoAsync(id));
                }
        */
        [HttpPost("refound/")]
        [Authorize]
        public async Task<IActionResult> Refound([FromQuery] string transactionId, [FromQuery] string createdBy, [FromQuery] string token)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if(!(await _accountRepository.ValidationToken(createdBy, token))){
                return Unauthorized();
            }

            var existingRefunding = await _refundingRepository.GetByTransactionIdAsync(transactionId);
            if (existingRefunding != null)
            {
                return Conflict("A refunding already exists for this transaction.");
            }

            var transaction = await _transactionRepository.GetByIdTransactionAsync(transactionId);
            if (transaction == null)
            {
                return NotFound("Transaction not found.");
            }

            if (transaction.ToAccountId == null)
                throw new InvalidOperationException("The recipient's account ID is not initialized.");
            if (transaction.FromAccountId == null)
                throw new InvalidOperationException("The sender's account ID is not initialized.");
            var toAccount = await _accountRepository.GetByIdAccountAsync(transaction.ToAccountId);
            var fromAccount = await _accountRepository.GetByIdAccountAsync(transaction.FromAccountId);

            if (toAccount == null || fromAccount == null)
            {
                return NotFound("Accounts involved in the transaction were not found.");
            }

            var datenow = DateTime.Now;

            await _refundingRepository.AddAsync(new Refunding
            {
                CreatedAt = datenow,
                CreatedBy = createdBy,
                Description = "Refunding",
                Id = Guid.NewGuid().ToString(),
                Transaction = transaction,
                TransactionId = transaction.Id,
                UpdatedAt = datenow
            });

            var refunding = await _refundingRepository.GetByTransactionIdAsync(transactionId);


            var x = new Models.Transaction
            {
                Refunding = refunding,
                RefundingId = refunding.Id,
                Status = Enums.TransactionStatus.Refunded
            };
            await _transactionRepository.UpdateAsync(transaction, new Models.Transaction
            {
                Refunding = refunding,
                RefundingId = refunding.Id,
                Status = Enums.TransactionStatus.Refunded
            });


            await _accountRepository.AddIncommingAsync(transaction.ToAccountId, transaction);
            await _accountRepository.AddoutcommingAsync(transaction.FromAccountId, transaction);
            await _accountRepository.UpdateBalance(transaction.ToAccountId, transaction.Value, 'a');
            await _accountRepository.UpdateBalance(transaction.FromAccountId, transaction.Value, 's');

            return Ok(refunding.ToRefundingDto());
        }
        /*
                [HttpGet("tranferById/{id}")]
                public async Task<IActionResult> GetTransferByRefundingId([FromRoute]string id){
                    var x = await _refundingRepository.GetTransferByRefundingIdAsync(id);
                    return Ok(x);
                }
        */
    }
}