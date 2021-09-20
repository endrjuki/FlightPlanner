using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlightPlanner.Core.IRepositories;
using FlightPlanner.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightPlanner.Core.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected ApplicationDbContext _context;
        protected DbSet<T> _dbSet;
        protected readonly ILogger _logger;

        public GenericRepository(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public virtual async Task<IEnumerable<T>> All()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T> GetById(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<bool> Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            return true;
        }

        public virtual Task<bool> Delete(Guid id)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<bool> Upsert(T entity)
        {
            throw new System.NotImplementedException();
        }

        public async Task DeleteAllEntries(string tableName)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($"delete from ${tableName}");
        }
    }
}