using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GildedRose.DataAccess.DatabaseContext;
using GildedRose.DataAccess.Interfaces.Entities;
using GildedRose.DataAccess.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GildedRose.DataAccess.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly GildedRoseContext _dbContext;

        protected readonly DbSet<T> DbSet;
        protected BaseRepository(GildedRoseContext dbContext)
        {
            _dbContext = dbContext;
            DbSet = _dbContext.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<T> CreateAsync(T item)
        {
            await DbSet.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            var entity = item as IItem;
            return await DbSet.FindAsync(entity.Id);
        }

        public async Task<T> UpdateAsync(T item)
        {
            DbSet.Update(item);
            await _dbContext.SaveChangesAsync();
            var entity = item as IItem;
            return await DbSet.FindAsync(entity.Id);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await DbSet.FindAsync(id);
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Can't find entity with those id");
            }
            DbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
