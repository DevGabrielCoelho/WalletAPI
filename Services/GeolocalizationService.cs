using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WalletApi.Interfaces;

namespace WalletApi.Services
{
    public class GeolocalizacaoService : IGeolocalizationService
    {
        private readonly HttpClient _httpClient;

        public GeolocalizacaoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> ObterLocalizacaoPorIp(string ip)
        {
            var url = $"https://ipinfo.io/{ip}/json";
            var resposta = await _httpClient.GetStringAsync(url);
            dynamic dados = JsonConvert.DeserializeObject(resposta) ?? "";

            if (dados.status == "fail")
            {
                return "";
            }

            return $"Localização: {dados.city}, {dados.regionName}, {dados.country}. " +
                   $"Latitude: {dados.lat}, Longitude: {dados.lon}.";
        }
    }
}