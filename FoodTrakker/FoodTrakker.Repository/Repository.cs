﻿using FoodTrakker.Core;
using FoodTrakker.Repository.Contracts;
using FoodTrakker.Repository.Data;
using Microsoft.EntityFrameworkCore;


namespace FoodTrakker.Repository
{
    public class Repository<T, IndexType> : IRepository<T, IndexType> 
        where T : class, Iindexable<IndexType> 
    {
        protected readonly FoodTrakkerContext _context;
        public Repository(FoodTrakkerContext context)
        {
            _context = context;
        }
        public async Task AddAsync(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(IndexType id)
        {
            var objectToDelete = await GetAsync(id);
            _context.Remove(objectToDelete);
            await _context.SaveChangesAsync(true);
        }

        public Task<List<T>> GetAsync()
        {
            return Task.FromResult(_context.Set<T>().ToList());
        }

        public async Task<T> GetAsync(IndexType id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(t => t.Id.Equals(id));
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
