using TwitterStream.Domain;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using TwitterStream.Contract.RepositoryContract;

namespace TwitterStream.Repository
{
    public class InMemoryRepository : IRepository<Tweet>
    {
        static ConcurrentDictionary<string, Tweet> dictionary;

        static InMemoryRepository()
        {
            dictionary = new ConcurrentDictionary<string, Tweet>();
        }
        void IRepository<Tweet>.Add(Tweet entity)
        {
            dictionary.TryAdd(entity.id, entity);
        }

        void IRepository<Tweet>.Delete(Tweet entity)
        {
            throw new NotImplementedException();
        }

        IQueryable<Tweet> IRepository<Tweet>.Get(Expression<Func<Tweet, bool>> expression)
        {
            throw new NotImplementedException();
        }

        IQueryable<Tweet> IRepository<Tweet>.GetAll()
        {
            return dictionary.Values.AsQueryable();
        }

        void IRepository<Tweet>.Update(Tweet entity)
        {
            throw new NotImplementedException();
        }

        
    }
}