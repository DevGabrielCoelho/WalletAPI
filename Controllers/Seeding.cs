
using System.Text;
using Microsoft.AspNetCore.Mvc;
using WalletApi.Data;
using WalletApi.Enums;
using WalletApi.Models;

namespace WalletApi.Controllers
{
  [ApiController]
  [Route("api/seeding")]
  public class Seeding : ControllerBase
  {
    private readonly ApplicationDBContext _context;
    public Seeding(ApplicationDBContext applicationDBContext)
    {
      _context = applicationDBContext;
    }

    [HttpPost("seed")]
    public IActionResult SeedData()
    {
      if (_context == null)
        throw new Exception("Db is not initialized");

      if (_context.Users == null)
        throw new Exception("Users is not initialized");
      if (_context.Accounts == null)
        throw new Exception("Accounts is not initialized");
      if (_context.Transactions == null)
        throw new Exception("Transactions is not initialized");
      if (_context.Refundings == null)
        throw new Exception("Refundings is not initialized");

      if (_context.Users.Any())
      {
        return BadRequest("Database already contains data.");
      }

      var user1 = new User
      {
        Name = "Rodrigo Thiago",
        Document = "12345678900",
        Birthday = new DateTime(1995, 5, 20),
        Email = "rodrigo@example.com",
        Phone = "123456789",
        PasswordHash = "hashed_password_1"
      };

      var user2 = new User
      {
        Name = "Maria Silva",
        Document = "98765432100",
        Birthday = new DateTime(1990, 10, 15),
        Email = "maria@example.com",
        Phone = "987654321",
        PasswordHash = "hashed_password_2"
      };

      _context.Users.AddRange(user1, user2);

      var account1 = new Account
      {
        UserId = user1.Id,
        Balance = 1000.50M,
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now
      };

      var account2 = new Account
      {
        UserId = user2.Id,
        Balance = 250.75M,
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now
      };



      _context.Accounts.AddRange(account1, account2);

      user1.AccountId = account1.Id;
      user2.AccountId = account2.Id;

      var transaction1 = new Transaction
      {
        FromAccountId = account1.Id,
        ToAccountId = account2.Id,
        Value = 200.00M,
        Status = TransactionStatus.Completed,
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now
      };

      var transaction2 = new Transaction
      {
        FromAccountId = account2.Id,
        ToAccountId = account1.Id,
        Value = 50.00M,
        Status = TransactionStatus.Pending,
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now
      };

      _context.Transactions.AddRange(transaction1, transaction2);

      var refunding1 = new Refunding
      {
        TransactionId = transaction2.Id,
        Description = "Refund for incorrect payment",
        CreatedBy = "Admin",
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now
      };

      _context.Refundings.Add(refunding1);

      _context.SaveChanges();

      return Ok("Seed data added successfully.");
    }
  }
}