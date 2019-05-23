using System.Threading.Tasks;
using GildedRose.BusinessLogic.DTOs;

namespace GildedRose.BusinessLogic.Interfaces.Services
{
    public interface IItemService : IBaseService<ItemDto>
    {
        Task<bool> BuyItem(int id, int count);
    }
}
