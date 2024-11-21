using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletApi.Dtos;
using WalletApi.Models;

namespace WalletApi.Interfaces
{
    public interface IUserRepository
    {
        Task<List<UserDto>> GetAllAsyncDto();
        Task<UserDto> GetByIdDtoAsync(string id);
        Task<User> GetByIdUserAsync(string id);
        User GetByIdUser(string id);
        Task<User> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user, CreateUserDto createUserDto);
        Task UpdateToken(User user, string token);
        Task<bool> SearchEmailAsync(string email);
        Task<bool> SearchPhoneAsync(string phone);
        Task<bool> SearchDocumentAsync(string document);
        Task<bool> ValidationToken(string id, string token);
    }
}