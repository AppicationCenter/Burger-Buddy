using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betway.Domain
{
    public class Company : ModelBase
    {
        public string Name { get; set; } = string.Empty;

        public TimeSpan OpeningTime { get; private set; } = TimeSpan.FromHours(09);

        public TimeSpan ClosingTime { get; private set; } = TimeSpan.FromHours(17);
    }
}
