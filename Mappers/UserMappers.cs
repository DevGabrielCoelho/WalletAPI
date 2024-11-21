using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WalletApi.Dtos;
using WalletApi.Models;

namespace WalletApi.Mappers
{
    public static class UserMappers
    {
        public static UserDto ToUserDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                AccountId = user.AccountId,
                Birthday = user.Birthday,
                CreatedAt = user.CreatedAt,
                Email = user.Email,
                Name = user.Name,
                Phone = user.Phone,
                UpdatedAt = user.UpdatedAt
            };
        }

        public static User ToUserFromCreateUserDto(this CreateUserDto createUserDto)
        {
            var timeNow = DateTime.Now;
            return new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = createUserDto.Name,
                Document = createUserDto.Document,
                Birthday = createUserDto.Birthday,
                Email = createUserDto.Email,
                Phone = createUserDto.Phone,
                PasswordHash = createUserDto.Password,
                CreatedAt = timeNow,
                UpdatedAt = timeNow,
            };
        }
    }
}