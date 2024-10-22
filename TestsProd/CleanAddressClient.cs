using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Runtime;
using System.Text.Json;
using AutoMapper;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;

namespace TestsProd
{
        public class CleanAddressClient : ICleanAddressService
        {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;
        private readonly APISettings _settings;

        public CleanAddressClient(IHttpClientFactory httpClientFactory, IMapper mapper, IOptions<APISettings> settings)
            {
                _httpClientFactory = httpClientFactory;
                _mapper = mapper;
                _settings = settings.Value;
            }
        


        public async Task<AddressResponse> GetCleanAddressAsync(string address)
        {
            HttpResponseMessage response = null;

            using var client = _httpClientFactory.CreateClient();
            
            client.BaseAddress = new Uri("https://cleaner.dadata.ru/");
            client.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
            if (string.IsNullOrEmpty(_settings.ApiKey) || string.IsNullOrEmpty(_settings.Secret)) throw new EmptyTokenException();
            client.DefaultRequestHeaders.Add("Authorization", "Token " + _settings.ApiKey);
            client.DefaultRequestHeaders.Add("X-Secret", _settings.Secret);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _settings.ApiPoint);
            request.Content = new StringContent($"[\"{address}\"]",
                                                Encoding.UTF8,
                                                "application/json");//CONTENT-TYPE header

            response = await client.SendAsync(request);

            var result = await response.Content.ReadFromJsonAsync<List<AddressResponse>>();

            return result.FirstOrDefault();
            

        }



    }
}
