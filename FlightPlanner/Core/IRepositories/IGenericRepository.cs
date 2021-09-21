using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using System.Threading.Tasks;

namespace FlightPlanner.Core.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> All();
        Task<T> GetById(int id);
        Task<bool> Add(T entity);
        Task<bool> Delete(int id);
        Task<bool> Upsert(T entity);

        Task DeleteAllEntries();
    }
}