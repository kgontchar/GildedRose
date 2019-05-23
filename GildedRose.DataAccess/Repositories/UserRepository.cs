using System.Linq;
using GildedRose.DataAccess.DatabaseContext;
using GildedRose.DataAccess.Entities;
using GildedRose.DataAccess.Interfaces.Repositories;

namespace GildedRose.DataAccess.Repositories
{
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
        public UserRepository(GildedRoseContext dbContext) : base(dbContext)
        {
        }

        public UserEntity SignIn(string username, string password)
        {
            var user = DbSet.FirstOrDefault(x => x.Username == username && x.Password == password);
            return user;
        }
    }
}
