using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WalletApi.Data;
using WalletApi.Dtos;
using WalletApi.Interfaces;

namespace WalletApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class Auth : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public Auth(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Authenticator([FromBody] LoginUserDto loginUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (loginUserDto.Email == null)
                throw new InvalidOperationException("Email is required but was not provided.");
            var user = await _userRepository.GetByEmailAsync(loginUserDto.Email);
            if (user == null)
                throw new InvalidOperationException("User with the provided email was not found.");
            if (user.PasswordHash == null)
                throw new InvalidOperationException("User's password is not initialized.");
            if (loginUserDto.Password == null)
                throw new InvalidOperationException("Password is required but was not provided.");
            var testPass = _passwordHasher.Verify(user.PasswordHash, loginUserDto.Password);

            if(testPass){
                user.SessionToken = _tokenService.CreateToken(user);
            }

            if(!testPass){
                throw new InvalidOperationException("Error!");
            }

            if (user.SessionToken == null)
                throw new InvalidOperationException("Failed to generate a session token for the user.");
            await _userRepository.UpdateToken(user, user.SessionToken);

            if (user.Name == null)
                throw new InvalidOperationException("User's name is not initialized.");
            if (user.Email == null)
                throw new InvalidOperationException("User's email is not initialized.");
            return Ok(new NewUserDto{
                Name = user.Name,
                Email = user.Email,
                Token = user.SessionToken
            });
        }

    }
}