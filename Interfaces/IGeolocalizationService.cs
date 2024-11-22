using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalletApi.Interfaces
{
    public interface IGeolocalizationService
    {
        Task<string> ObterLocalizacaoPorIp(string ip);
    }
}