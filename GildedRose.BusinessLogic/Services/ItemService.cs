using System;
using System.Threading.Tasks;
using AutoMapper;
using GildedRose.BusinessLogic.DTOs;
using GildedRose.BusinessLogic.Interfaces.Services;
using GildedRose.DataAccess.Entities;
using GildedRose.DataAccess.Interfaces.Repositories;
using GildedRose.BusinessLogic.Exceptions;

namespace GildedRose.BusinessLogic.Services
{
    public class ItemService : BaseService<ItemEntity, ItemDto>, IItemService
    {
        private readonly object _lock = new object();
        public ItemService(IItemRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public async Task<bool> BuyItem(int id, int count)
        {
            var item = await GetByIdAsync(id);

            if (item == null)
            {
                throw new ItemNotFoundException("Can't find entity with those id");
            }

            lock (_lock)
            {
                if (count <= 0)
                {
                    throw new ArgumentException("Count of item's can't be zero or less");
                }

                if (item.ItemsLeft <= 0)
                {
                    throw new ThereAreNoItemsOnStockException("These items no longer remains");
                }

                if (item.ItemsLeft < count)
                {
                    throw new ThereAreNoItemsOnStockException("Not enough items in stock");
                }

                item.ItemsLeft -= count;
                UpdateAsync(item);
            }
            return true;
        }
    }
}
