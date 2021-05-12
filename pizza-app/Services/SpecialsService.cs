using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using pizza_app.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace pizza_app.Services
{
    public class SpecialsService
    {
        private HttpClient Client;
        private readonly ILogger<SpecialsService> _logger;

        public SpecialsService(HttpClient client, IOptions<Settings> settings, ILogger<SpecialsService> logger)
        {
            _logger = logger;
            client.BaseAddress = settings.Value.SpecialsApi; //new Uri(settings.Value.SpecialsApi);
            Client = client;
        }

        public async Task<IEnumerable<PizzaSpecial>> GetSpecials()
        {
            var response = await Client.GetAsync("/specials");
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<IEnumerable<PizzaSpecial>>(responseStream);
        }
    }
}
