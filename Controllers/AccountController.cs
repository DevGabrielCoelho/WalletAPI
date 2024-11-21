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
        /*
                [HttpGet("getall")]
                public async Task<IActionResult> GetAll()
                {
                    return Ok(await _accountRepository.GetAllAsync());
                }

                [HttpGet("getid/{id}")]
                public async Task<IActionResult> GetById([FromRoute] string id)
                {
                    return Ok(await _accountRepository.GetByIdDtoAsync(id));
                }

                [HttpGet("getAllInId/{id}")]
                public async Task<IActionResult> GetAllInById([FromRoute] string id)
                {
                    var x = await _accountRepository.GetAllInAsync(id);
                    return Ok(x.Select(x => x.ToTransactionDto()));
                }
                [HttpGet("getAllOutId/{id}")]
                public async Task<IActionResult> GetAllOutById([FromRoute] string id)
                {
                    var x = await _accountRepository.GetAllOutAsync(id);
                    return Ok(x.Select(x => x.ToTransactionDto()));
                }
        */
        [HttpGet("balance/")]
        [Authorize]
        public async Task<IActionResult> Balance([FromQuery] string id, [FromQuery] string token)
        {

            if (!ModelState.IsValid)
                return BadRequest("The provided model state is invalid.");

            var tokenValid = await _accountRepository.ValidationToken(id, token);
            if(!tokenValid){
                return Unauthorized("Token validation failed or is invalid.");
            }

            var accountDto = await _accountRepository.GetByIdDtoAsync(id);
            if (accountDto == null)
                throw new InvalidOperationException("Account not found for the given ID.");

            return Ok(accountDto.Balance);
        }
    }
}