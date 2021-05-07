using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pizza_app
{
    public class Settings
    {
        public string ToppingsApi { get; set; }
        public string SpecialsApi { get; set; }
        public bool IsContained { get; set; }
        public bool Development { get; set; }
    }
}