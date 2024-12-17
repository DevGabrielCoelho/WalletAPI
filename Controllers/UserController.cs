using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WalletApi.Data;
using WalletApi.Dtos;
using WalletApi.Interfaces;
using WalletApi.Mappers;
using WalletApi.Models;

namespace WalletApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        public UserController(IPasswordHasher passwordHasher, IUserRepository userRepository, IAccountRepository accountRepository, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromQuery] CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (createUserDto.Document == null)
                throw new InvalidOperationException("Document field is missing or not properly initialized.");
            if (await _userRepository.SearchDocumentAsync(createUserDto.Document))
                return BadRequest("Document already registered");

            if (createUserDto.Email == null)
                throw new InvalidOperationException("Email field is missing or not properly initialized.");
            if (await _userRepository.SearchEmailAsync(createUserDto.Email))
                return BadRequest("Email already registered");

            if (createUserDto.Phone == null)
                return BadRequest("Email already registered");
            if (await _userRepository.SearchPhoneAsync(createUserDto.Phone))
                throw new InvalidOperationException("Phone number field is missing or not properly initialized.");

            if (createUserDto.Password == null)
                throw new InvalidOperationException("Password field is missing or not properly initialized.");
            createUserDto.Password = _passwordHasher.Hash(createUserDto.Password);

            var user = createUserDto.ToUserFromCreateUserDto();
            var account = new Account();
            account.User = user;
            account.UserId = user.Id;
            user.Account = account;
            user.AccountId = account.Id;
            user.SessionToken = _tokenService.CreateToken(user);
            await _userRepository.AddAsync(user);
            if (user.Name == null)
                throw new InvalidOperationException("User's name is missing or not properly initialized.");
            if (user.Email == null)
                throw new InvalidOperationException("User's email is missing or not properly initialized.");
            return Ok(new NewUserDto
            {
                Name = user.Name,
                Email = user.Email,
                Token = user.SessionToken
            });
        }

        [HttpPut("edit-user/")]
        [Authorize]
        public async Task<IActionResult> EditUser([FromQuery] string id, [FromBody] CreateUserDto createUserDto, [FromQuery] string token)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (!(await _userRepository.ValidationToken(id, token)))
            {
                return Unauthorized();
            }

            var user = await _userRepository.GetByIdUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            if (createUserDto.Document == null)
                throw new InvalidOperationException("Document field is missing or not properly initialized.");
            if (await _userRepository.SearchDocumentAsync(createUserDto.Document) && user.Document != createUserDto.Document)
                return BadRequest("Document already registered");

            if (createUserDto.Email == null)
                throw new InvalidOperationException("Email field is missing or not properly initialized.");
            if (await _userRepository.SearchEmailAsync(createUserDto.Email) && user.Email != createUserDto.Email)
                return BadRequest("Email already registered");

            if (createUserDto.Phone == null)
                throw new InvalidOperationException("Phone number field is missing or not properly initialized.");
            if (await _userRepository.SearchPhoneAsync(createUserDto.Phone) && user.Phone != createUserDto.Phone)
                return BadRequest("Phone number already registered");

            await _userRepository.UpdateAsync(user, createUserDto);

            user = await _userRepository.GetByIdUserAsync(id);
            return Ok(user.ToUserDto());
        }
    }
}