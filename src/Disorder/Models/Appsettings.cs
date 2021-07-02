using System.Collections.Generic;

namespace Disorder.Models
{
    public class Appsettings
    {
        public IEnumerable<string> StartupArgs { get; set; }

        public OrderType OrderType { get; set; }
    }
}
