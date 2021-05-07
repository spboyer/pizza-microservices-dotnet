using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        private readonly ILogger<IndexModel> _logger;
        private readonly SpecialsService _service;
        public bool GetSpecialsError { get; private set; }
        public bool HasSpecials => Specials.Any();
        public IEnumerable<PizzaSpecial> Specials { get; private set;}

        public IndexModel(ILogger<IndexModel> logger, SpecialsService service)
        {
            _logger = logger;
            _service = service;
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

        }
    }
}
