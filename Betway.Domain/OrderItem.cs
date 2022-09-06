﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betway.Domain
{
    public class OrderItem
    {
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }

        public int Quantity { get; set; }
    }
}
