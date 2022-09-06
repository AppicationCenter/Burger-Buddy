using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betway.Domain
{
    public class Product : ModelBase
    {
        public string Name { get; set; } = string.Empty;

        public double Price { get; set; }
    }
}
