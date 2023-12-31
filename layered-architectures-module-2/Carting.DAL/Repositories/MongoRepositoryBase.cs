﻿using System.Linq.Expressions;
using Carting.DAL.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Carting.DAL.Repositories;

public class MongoRepositoryBase<T> : IMongoRepository<T> where T:EntityBase
{
    protected readonly IMongoDatabase _mongoDatabase;
    protected readonly IMongoCollection<T> _itemCollection;
    public MongoRepositoryBase(IMongoDatabase mongoDatabase)
    {
        _mongoDatabase = mongoDatabase;
        _itemCollection = _mongoDatabase.GetCollection<T>(GetCollectionName());
    }
    
    public virtual Task CreateAsync(T obj)
    {
        return _itemCollection.InsertOneAsync(obj);
    }

    public virtual Task UpdateAsync(T obj)
    {
        return _itemCollection.ReplaceOneAsync(entity => entity.Id == obj.Id, obj);
    }
    
    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return (await _itemCollection.FindAsync(predicate)).ToList();
    }

    public virtual Task GetByIdAsync(string id)
    {
        return _itemCollection.FindAsync(entity => entity.Id == id);
    }
    
    public virtual Task<IEnumerable<T>> GetAllAsync()
    {
        return _itemCollection.FindAsync<T>(new BsonDocument()).Result.ToListAsync().ContinueWith(task => (IEnumerable<T>)task.Result);
    }

    public virtual Task DeleteAsync(Expression<Func<T,bool>> predicate)
    {
        return _itemCollection.DeleteOneAsync(predicate);
    }

    protected virtual string GetCollectionName() => typeof(T).Name;
}