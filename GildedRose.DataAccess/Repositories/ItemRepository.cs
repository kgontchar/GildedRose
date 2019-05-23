using GildedRose.DataAccess.DatabaseContext;
using GildedRose.DataAccess.Entities;
using GildedRose.DataAccess.Interfaces.Repositories;

namespace GildedRose.DataAccess.Repositories
{
    public class ItemRepository : BaseRepository<ItemEntity>, IItemRepository
    {
        public ItemRepository(GildedRoseContext dbContext) : base(dbContext)
        {

        }
    }
}
