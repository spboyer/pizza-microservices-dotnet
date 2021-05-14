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
    public class ToppingsService
    {
        private HttpClient Client;

        public ToppingsService(HttpClient client, IOptions<Settings> settings)
        {
            //client.BaseAddress = settings.Value.ToppingsApi; //new Uri(settings.Value.SpecialsApi);
            client.BaseAddress = settings.Value.ProxyUri;
            Client = client;
        }

        public async Task<IEnumerable<Topping>> GetToppings()
        {
            var response = await Client.GetAsync("/toppings");
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<IEnumerable<Topping>>(responseStream);
        }
    }
}
