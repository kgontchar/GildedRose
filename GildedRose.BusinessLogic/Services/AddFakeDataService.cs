using System.Collections.Generic;
using GildedRose.BusinessLogic.Interfaces.Services;
using GildedRose.DataAccess.DatabaseContext;
using GildedRose.DataAccess.Entities;

namespace GildedRose.BusinessLogic.Services
{
    public class AddFakeDataService : IAddFakeDataService
    {
        readonly GildedRoseContext _context;

        public AddFakeDataService(GildedRoseContext context)
        {
            _context = context;
        }

        public void AddFakeData()
        {
            var items = new List<ItemEntity>
            {
                new ItemEntity { Id = 1, Name = "Item1", Price = 10000, ItemsLeft = 1000 },
                new ItemEntity { Id = 2, Name = "Item2", Price = 20000, ItemsLeft = 1000  },
                new ItemEntity { Id = 3, Name = "Item3", Price = 30000, ItemsLeft = 1000  },
                new ItemEntity { Id = 4, Name = "Item4", Price = 40000, ItemsLeft = 1000  },
                new ItemEntity { Id = 5, Name = "Item5", Price = 50000, ItemsLeft = 1000  }
            };

            var users = new List<UserEntity>
            {
                new UserEntity {Id = 1, Name = "User1", Username = "user1", Password = "user1", Role = "admin" },
                new UserEntity {Id = 2, Name = "User2", Username = "user2", Password = "user2", Role = "user" },
                new UserEntity {Id = 3, Name = "User3", Username = "user3", Password = "user3", Role = "user" },
                new UserEntity {Id = 4, Name = "User4", Username = "user4", Password = "user4", Role = "user" },
                new UserEntity {Id = 5, Name = "User5", Username = "user5", Password = "user5", Role = "user" }
            };

            _context.Items.AddRange(items);
            _context.Users.AddRange(users);
            _context.SaveChanges();
        }
    }
}
