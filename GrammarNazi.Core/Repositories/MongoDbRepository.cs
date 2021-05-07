using GrammarNazi.Domain.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GrammarNazi.Core.Repositories
{
    public class MongoDbRepository<T> : IRepository<T> where T : class
    {
        private readonly IMongoCollection<T> _collection;

        public MongoDbRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("");
            _collection = database.GetCollection<T>(typeof(T).Name);
        }

        public async Task Add(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task<bool> Any(Expression<Func<T, bool>> filter = null)
        {
            var count = await _collection.CountDocumentsAsync(filter);

            return count > 0;
        }

        public async Task Delete(T entity)
        {
            // TODO: Implement this method
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter)
        {
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<T> GetFirst(Expression<Func<T, bool>> filter)
        {
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<TResult> Max<TResult>(Expression<Func<T, TResult>> selector)
        {
            var allItems = await _collection.Find(_ => true).ToListAsync();

            return allItems.Max(selector.Compile());
        }

        public async Task Update(T entity, Expression<Func<T, bool>> identifier)
        {
            await _collection.ReplaceOneAsync(identifier, entity);
        }
    }
}