using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace FeeApi.Models
{
    public class FeeContext : DbContext
    {
        public FeeContext(DbContextOptions<FeeContext> options)
            : base(options)
        {
        }

        public DbSet<FeeItem> FeeItems { get; set; } = null!;
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Segment> Segments { get; set; } = null!;
    }

}