using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GildedRose.BusinessLogic.Exceptions;
using GildedRose.BusinessLogic.Interfaces.Services;
using GildedRose.DataAccess.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Internal;

namespace GildedRose.BusinessLogic.Services
{
    public abstract class BaseService<TEntity, TResource> : IBaseService<TResource> where TEntity : class where TResource : class
    {
        private const string VALUE_IS_ZERO_OR_LESS_EXCEPTION_MESSAGE = "Value can't be zero or less";
        protected readonly IMapper Mapper;
        private readonly IBaseRepository<TEntity> _repository;

        protected BaseService(IBaseRepository<TEntity> repository, IMapper mapper)
        {
            Mapper = mapper;
            _repository = repository;
        }

        public async Task<IEnumerable<TResource>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();

            if (entities == null || !entities.Any())
            {
                return new List<TResource>();
            }

            var resources = Mapper.Map<IEnumerable<TResource>>(entities);

            return resources;
        }

        public async Task<TResource> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(VALUE_IS_ZERO_OR_LESS_EXCEPTION_MESSAGE);
            }

            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
            {
                throw new ItemNotFoundException("Can't find entity with those id");
            }

            var resource = Mapper.Map<TResource>(entity);

            return resource;
        }

        public async Task<TResource> CreateAsync(TResource item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Item to create can't be null");
            }

            var entity = await _repository.CreateAsync(Mapper.Map<TEntity>(item));

            if (entity == null)
            {
                throw new ItemNotFoundException("Entity wasn't created");
            }

            var resource = Mapper.Map<TResource>(entity);

            return resource;
        }

        public async Task<TResource> UpdateAsync(TResource item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Item to update can't be null");
            }
            
            var entity = await _repository.UpdateAsync(Mapper.Map<TEntity>(item));

            if (entity == null)
            {
                throw new ItemNotFoundException("Entity wasn't updated");
            }

            var resource = Mapper.Map<TResource>(entity);

            return resource;
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(VALUE_IS_ZERO_OR_LESS_EXCEPTION_MESSAGE);
            }

            await _repository.DeleteAsync(id);
        }
    }
}
