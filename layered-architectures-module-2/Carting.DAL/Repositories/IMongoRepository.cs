using System.Linq.Expressions;
using Carting.DAL.Models;

namespace Carting.DAL.Repositories;

public interface IMongoRepository<T> where T:EntityBase
{
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task GetByIdAsync(string id);
    Task<IEnumerable<T>> GetAllAsync();
    Task CreateAsync(T obj);
    Task UpdateAsync(T obj);
    Task DeleteAsync(Expression<Func<T, bool>> predicate);
}