using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using pizza_app.Model;
using pizza_app.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace pizza_app.Pages
{
    public class IndexModel : PageModel
    {
        public string SelectedTopping { get; set; }
        private readonly ILogger<IndexModel> _logger;
        private readonly SpecialsService _service;
        private readonly ToppingsService _toppingsService;
        public bool GetSpecialsError { get; private set; }
        public bool HasSpecials => Specials.Any();
        public IEnumerable<PizzaSpecial> Specials { get; private set;}
        public IEnumerable<SelectListItem> Toppings { get; private set; }

        public IndexModel(ILogger<IndexModel> logger, SpecialsService service, ToppingsService toppingsService)
        {
            _logger = logger;
            _service = service;
            _toppingsService = toppingsService;
        }


        public async Task OnGet()
        {
            try
            {
                Specials = await _service.GetSpecials();
            } catch (HttpRequestException)
            {
                GetSpecialsError = true;
                Specials = Array.Empty<PizzaSpecial>();
            }

            try
            {
                _logger.LogInformation("Getting toppings list");
                var toppings = await _toppingsService.GetToppings();
                Toppings = toppings.OrderBy(t => t.name)
                    .Select(i =>
                        new SelectListItem { 
                            Text = $"{i.name} ({i.GetFormattedPrice()})"
                        }).ToList();

            }
            catch (HttpRequestException)
            {
                Toppings = Array.Empty<SelectListItem>();
            }

        }
    }
}
