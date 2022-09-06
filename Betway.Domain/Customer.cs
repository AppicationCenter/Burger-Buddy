using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betway.Domain
{
    public class Customer : ModelBase
    {
        public string FullNames { get; set; } = string.Empty;

        public string ContactNumber { get; set; } = string.Empty;

        public int UserTypeId { get; set; }

        [ForeignKey(nameof(UserTypeId))]
        public UserType? UserType { get; set; }
    }
}
