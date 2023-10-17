using Carting.DAL.Models;

namespace Carting.DAL.Repositories;

public interface IMongoRepository<T> where T:EntityBase
{
    Task GetByIdAsync(string id);
    Task<IEnumerable<T>> GetAllAsync();
    Task CreateAsync(T obj);
    Task UpdateAsync(T obj);
}