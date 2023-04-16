using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MStarSupply.Models;

namespace MStarSupply.Data
{
    public class MStarSupplyContext : DbContext
    {
        public MStarSupplyContext (DbContextOptions<MStarSupplyContext> options)
            : base(options)
        {
        }

        public DbSet<MStarSupply.Models.Produto> Produto { get; set; } = default!;

        public DbSet<MStarSupply.Models.Movimentacoes>? Movimentacoes { get; set; }

    }
}
