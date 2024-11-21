using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletApi.Models;

namespace WalletApi.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}