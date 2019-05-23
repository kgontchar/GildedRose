using Microsoft.EntityFrameworkCore;
using GildedRose.DataAccess.Entities;

namespace GildedRose.DataAccess.DatabaseContext
{
    public class GildedRoseContext : DbContext
    {
        public GildedRoseContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ItemEntity> Items { get; set; }
    }
}
