using Betway.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Betway.Data.Contexts
{
    public class BurgerBuddyContext : DbContext
    {
        public BurgerBuddyContext(DbContextOptions<BurgerBuddyContext> options) : base(options)
{
}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ModelBase>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
