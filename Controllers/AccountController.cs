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

namespace WalletApi.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        [HttpGet("balance/")]
        [Authorize]
        public async Task<IActionResult> Balance([FromQuery] string id, [FromQuery] string token)
        {

            if (!ModelState.IsValid)
                return BadRequest("The provided model state is invalid.");

            var tokenValid = await _accountRepository.ValidationToken(id, token);
            if (!tokenValid)
            {
                return Unauthorized("Token validation failed or is invalid.");
            }

            var accountDto = await _accountRepository.GetByIdDtoAsync(id);
            if (accountDto == null)
                throw new InvalidOperationException("Account not found for the given ID.");

            return Ok(new
            {
                balance = accountDto.Balance
            });
        }
    }
}