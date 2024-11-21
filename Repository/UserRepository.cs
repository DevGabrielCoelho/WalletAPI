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
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public UserRepository(ApplicationDBContext applicationDBContext, IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
            _context = applicationDBContext;
        }

        public async Task AddAsync(User user)
        {
            if (_context.Users == null)
            {
                throw new InvalidOperationException("The Users DbSet is not initialized. Ensure the database context is properly configured.");
            }
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserDto>> GetAllAsyncDto()
        {
            if (_context.Users == null)
            {
                throw new InvalidOperationException("The Users DbSet is not initialized. Ensure the database context is properly configured.");
            }
            var listUser = await _context.Users.Select(x => x.ToUserDto()).ToListAsync();
            return listUser;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            if (_context.Users == null)
            {
                throw new InvalidOperationException("The Users DbSet is not initialized. Ensure the database context is properly configured.");
            }
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                throw new InvalidOperationException("User not found for the provided email.");
            }
            return user;
        }

        public async Task<UserDto> GetByIdDtoAsync(string id)
        {
            if (_context.Users == null)
            {
                throw new InvalidOperationException("The Users DbSet is not initialized. Ensure the database context is properly configured.");
            }
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                throw new InvalidOperationException("User not found for the provided ID.");
            }
            return user.ToUserDto();
        }

        public async Task<User> GetByIdUserAsync(string id)
        {
            if (_context.Users == null)
            {
                throw new InvalidOperationException("The Users DbSet is not initialized. Ensure the database context is properly configured.");
            }
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                throw new InvalidOperationException("User not found for the provided ID.");
            }
            return user;
        }

        public User GetByIdUser(string id)
        {
            if (_context.Users == null)
            {
                throw new InvalidOperationException("The Users DbSet is not initialized. Ensure the database context is properly configured.");
            }
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                throw new InvalidOperationException("User not found for the provided ID.");
            }
            return user;
        }

        public async Task<bool> SearchDocumentAsync(string document)
        {
            if (_context.Users == null)
            {
                throw new InvalidOperationException("The Users DbSet is not initialized. Ensure the database context is properly configured.");
            }
            return await _context.Users.AnyAsync(x => x.Document == document);
        }

        public async Task<bool> SearchEmailAsync(string email)
        {
            if (_context.Users == null)
            {
                throw new InvalidOperationException("The Users DbSet is not initialized. Ensure the database context is properly configured.");
            }
            return await _context.Users.AnyAsync(x => x.Email == email);
        }

        public async Task<bool> SearchPhoneAsync(string phone)
        {
            if (_context.Users == null)
            {
                throw new InvalidOperationException("The Users DbSet is not initialized. Ensure the database context is properly configured.");
            }
            return await _context.Users.AnyAsync(x => x.Phone == phone);
        }

        public async Task UpdateAsync(User user, CreateUserDto createUserDto)
        {
            if (_context.Users == null)
            {
                throw new InvalidOperationException("The Users DbSet is not initialized. Ensure the database context is properly configured.");
            }
            int alts = 0;
            
            if (createUserDto.Birthday.ToString() != null && createUserDto.Birthday != user.Birthday)
            {
                await _context.Users.Where(x => x.Id == user.Id).ExecuteUpdateAsync(x => x
                    .SetProperty(x => x.Birthday, createUserDto.Birthday)
                );
                alts++;
            }

            if (createUserDto.Document != null && createUserDto.Document != "string" && createUserDto.Document != user.Document)
            {
                await _context.Users.Where(x => x.Id == user.Id).ExecuteUpdateAsync(x => x
                    .SetProperty(x => x.Document, createUserDto.Document)
                );
                alts++;
            }

            if (createUserDto.Email != null && createUserDto.Email != "string" && createUserDto.Email != user.Email)
            {
                await _context.Users.Where(x => x.Id == user.Id).ExecuteUpdateAsync(x => x
                    .SetProperty(x => x.Email, createUserDto.Email)
                );
                alts++;
            }

            if (createUserDto.Name != null && createUserDto.Name != "string" && createUserDto.Name != user.Name)
            {
                await _context.Users.Where(x => x.Id == user.Id).ExecuteUpdateAsync(x => x
                    .SetProperty(x => x.Name, createUserDto.Name)
                );
                alts++;
            }

            if (user.PasswordHash == null)
                throw new InvalidOperationException("The PasswordHash property of the user is empty. Ensure it is initialized.");

            if (createUserDto.Password == null)
                throw new InvalidOperationException("The provided password is null. Please provide a valid password.");
            
            var verificacao = _passwordHasher.Verify(user.PasswordHash, createUserDto.Password);

            if (createUserDto.Password != null && createUserDto.Password != "string" && !verificacao)
            {
                await _context.Users.Where(x => x.Id == user.Id).ExecuteUpdateAsync(x => x
                    .SetProperty(x => x.PasswordHash, _passwordHasher.Hash(createUserDto.Password))
                );
                alts++;
            }

            if (createUserDto.Phone != null && createUserDto.Phone != "string" && createUserDto.Phone != user.Phone)
            {
                await _context.Users.Where(x => x.Id == user.Id).ExecuteUpdateAsync(x => x
                    .SetProperty(x => x.Phone, createUserDto.Phone)
                );
                alts++;
            }
            if (alts > 0)
            {
                await _context.Users.Where(x => x.Id == user.Id).ExecuteUpdateAsync(x => x
                    .SetProperty(x => x.UpdatedAt, DateTime.Now)
                );
            }
            await _context.SaveChangesAsync();

        }

        public async Task UpdateToken(User user, string token)
        {
            if (_context.Users == null)
            {
                throw new InvalidOperationException("The Users DbSet is not initialized. Ensure the database context is properly configured.");
            }

            await _context.Users.Where(x => x.Id == user.Id).ExecuteUpdateAsync(x => x
                .SetProperty(x => x.SessionToken, token)
            );
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidationToken(string id, string token)
        {
            var user = await GetByIdUserAsync(id);
            return user.SessionToken == token;
        }
    }
}