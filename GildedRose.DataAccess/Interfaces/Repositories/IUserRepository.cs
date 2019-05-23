using GildedRose.DataAccess.Entities;

namespace GildedRose.DataAccess.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<UserEntity>
    {
        UserEntity SignIn(string username, string password);
    }
}
