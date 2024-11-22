using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using WalletApi.Data;
using WalletApi.Interfaces;
using WalletApi.Mappers;
using WalletApi.Models;

namespace WalletApi.Controllers
{
    [ApiController]
    [Route("api/transaction")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IRefundingRepository _refundingRepository;

        public TransactionController(ITransactionRepository transactionRepository, IRefundingRepository refundingRepository, IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            _refundingRepository = refundingRepository;
            _transactionRepository = transactionRepository;
        }
        /*
                [HttpGet("getall")]
                public async Task<IActionResult> GetAll()
                {
                    return Ok(await _transactionRepository.GetAllDtoAsync());
                }

                [HttpGet("getid/{id}")]
                public async Task<IActionResult> GetById([FromRoute] string id)
                {
                    var x = await _transactionRepository.GetByIdDtoAsync(id);
                    return Ok(await _transactionRepository.GetByIdDtoAsync(id));
                }
        */
        [HttpPost("transfer/")]
        [Authorize]
        public async Task<IActionResult> Transfer([FromQuery] string toAccountId, [FromQuery] string fromAccountId, [FromQuery] decimal value, [FromQuery] string token)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            
            if (value <= 0)
                return BadRequest("Invalid Transaction Value!");

            if (!(await _accountRepository.ValidationToken(toAccountId, token)))
            {
                return Unauthorized();
            }

            var toAccount = await _accountRepository.GetByIdAccountAsync(toAccountId);
            var fromAccount = await _accountRepository.GetByIdAccountAsync(fromAccountId);
            if (toAccount == null || fromAccount == null) return NotFound();
            var timenow = DateTime.Now;
            var transaction = new WalletApi.Models.Transaction
            {
                CreatedAt = timenow,
                FromAccount = fromAccount,
                FromAccountId = fromAccount.Id,
                Geolocation = "!!ADICIONAR AINDA!!",
                Id = Guid.NewGuid().ToString(),
                SenderIp = "!!ADICIONAR AINDA!!",
                Status = Enums.TransactionStatus.Pending,
                ToAccount = toAccount,
                ToAccountId = toAccount.Id,
                UpdatedAt = timenow,
                Value = value
            };
            transaction = _transactionRepository.AddIp(transaction);
            transaction = await _transactionRepository.AddGeo(transaction);

            if (transaction.ToAccountId == null)
                throw new Exception();
            if (transaction.FromAccountId == null)
                throw new Exception();

            await _accountRepository.AddIncommingAsync(transaction.ToAccountId, transaction);
            await _accountRepository.AddoutcommingAsync(transaction.FromAccountId, transaction);
            transaction = await _transactionRepository.GetByIdTransactionAsync(transaction.Id);

            if (toAccount.Balance < value)
            {
                timenow = DateTime.Now;
                var refunding = new Models.Refunding
                {
                    CreatedAt = timenow,
                    CreatedBy = toAccount.Id,
                    Description = "Insufficient Balance",
                    Id = Guid.NewGuid().ToString(),
                    Transaction = transaction,
                    TransactionId = transaction.Id,
                    UpdatedAt = timenow
                };
                await _refundingRepository.AddAsync(refunding);
                await _transactionRepository.UpdateAsync(transaction, new Models.Transaction
                {
                    Refunding = refunding,
                    RefundingId = refunding.Id,
                    Status = Enums.TransactionStatus.Fail
                });
                return BadRequest(refunding.ToRefundingDto());
            }
            if (transaction.ToAccountId == null)
                throw new InvalidOperationException("The recipient's account ID is not initialized.");
            if (transaction.FromAccountId == null)
                throw new InvalidOperationException("The sender's account ID is not initialized.");
            await _accountRepository.UpdateBalance(transaction.ToAccountId, value, 's');
            await _accountRepository.UpdateBalance(transaction.FromAccountId, value, 'a');
            await _transactionRepository.UpdateAsync(transaction, new Models.Transaction
            {
                Status = Enums.TransactionStatus.Completed
            });
            return Ok();
        }
        /*
                [HttpGet("GetRefundingByTransferId/{id}")]
                public async Task<IActionResult> GetRefundingByTransferId(string id)
                {
                    var x = await _transactionRepository.GetRefunByTransferIdAsync(id);
                    return Ok(x);
                }
                [HttpGet("GetToAcoByTransferId/{id}")]
                public async Task<IActionResult> GetToAcoByTransferId(string id)
                {
                    var x = await _transactionRepository.GetToAcoByTransferIdAsync(id);
                    return Ok(x);
                }
                [HttpGet("GetFromAcoByTransferId/{id}")]
                public async Task<IActionResult> GetFromAcoByTransferId(string id)
                {
                    var x = await _transactionRepository.GetFromAcoByTransferIdAsync(id);
                    return Ok(x);
                }
        */
    }
}