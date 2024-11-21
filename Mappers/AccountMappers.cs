using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletApi.Dtos;
using WalletApi.Models;

namespace WalletApi.Mappers
{
    public static class AccountMappers
    {
        public static AccountDto ToAccountDto(this Account account){
            return new AccountDto{
                Balance = account.Balance,
                CreatedAt = account.CreatedAt,
                Id = account.Id,
                UpdatedAt = account.UpdatedAt,
                UserId = account.UserId
            };
        }
    }
}