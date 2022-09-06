using Betway.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betway.Domain
{
    public class Order : ModelBase
    {
        public int OrderNumber { get; set; }

        public OrderState OrderState { get; set; } = OrderState.Cooking;

        public TimeSpan CookingTime { get; set; } = TimeSpan.FromMinutes(1);

        public int CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Customer? Customer { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
