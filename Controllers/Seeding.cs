// using System;
// using System.Collections.Generic;
// using System.Diagnostics;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Logging;
// using WalletApi.Data;
// using WalletApi.Models;

// namespace WalletApi.Controllers
// {
//     [ApiController]
//     [Route("api/seeding")]
//     public class Seeding : ControllerBase
//     {
//         private readonly ApplicationDBContext _context;

//         public Seeding(ILogger<Seeding> logger, ApplicationDBContext applicationDBContext)
//         {
//             _context = applicationDBContext;
//         }

//         [HttpPost("seeder")]
//         public async Task<IActionResult> Seedings()
//         {
//             User user = new();
//             User user2 = new();
//             Account account = new();
//             Account account2 = new();
//             user.Account = account;
//             user.AccountId = account.Id;
//             user2.Account = account2;
//             user2.AccountId = account2.Id;
//             account.User = user;
//             account.UserId = user.Id;
//             account2.User = user2;
//             account2.UserId = user2.Id;


//             Transaction transaction = new();
//             transaction.ToAccount = account;
//             transaction.ToAccountId = account.Id;
//             transaction.FromAccount = account2;
//             transaction.FromAccountId = account2.Id;

//             Refunding refunding = new();
//             refunding.Transaction = transaction;
//             refunding.TransactionId = transaction.Id;

//             transaction.Refunding = refunding;
//             transaction.RefundingId = refunding.Id;

//             account.IncomingTransactions.Add(transaction);
//             account2.OutgoingTransactions.Add(transaction);

//             _context.Users.Add(user);
//             _context.Users.Add(user2);
//             _context.Accounts.Add(account);
//             _context.Accounts.Add(account2);
//             _context.Transactions.Add(transaction);
//             _context.Refundings.Add(refunding);
//             await _context.SaveChangesAsync();

//             return Ok();
//         }

//         [HttpGet]
//         public IActionResult GetAll()
//         {
//             string retorno = "Users:\n";
//             foreach (User x in _context.Users) retorno += "  " + x.ToString() + "\n";
//             retorno += "\nAccounts:\n";
//             foreach (Account x in _context.Accounts) retorno += "  " + x.ToString() + "\n";
//             retorno += "\nTransactions:\n";
//             foreach (Transaction x in _context.Transactions) retorno += "  " + x.ToString() + "\n";
//             retorno += "\nRefunding:\n";
//             foreach (Refunding x in _context.Refundings) retorno += "  " + x.ToString() + "\n";
//             return Ok(retorno);
//         }

//         [HttpDelete]
//         public async Task<IActionResult> DropAll()
//         {
//             foreach (Refunding x in _context.Refundings) _context.Refundings.Remove(x);
//             foreach (Transaction x in _context.Transactions) _context.Transactions.Remove(x);
//             foreach (Account x in _context.Accounts) _context.Accounts.Remove(x);
//             foreach (User x in _context.Users) _context.Users.Remove(x);

//             return Ok(await _context.SaveChangesAsync());
//         }

//     }
// }