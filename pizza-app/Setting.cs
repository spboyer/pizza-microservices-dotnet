using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pizza_app
{
    public class Settings
    {
        public Uri ToppingsApi { get; set; }
        public Uri SpecialsApi { get; set; }
        public Uri ProxyUri { get; set; }
        public bool IsContained { get; set; }
        public bool Development { get; set; }
    }
}