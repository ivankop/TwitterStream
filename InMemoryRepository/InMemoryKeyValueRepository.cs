using TwitterStream.Contract.RepositoryContract;
using System.Collections.Concurrent;

namespace TwitterStream.Repository
{
    public class InMemoryKeyValueRepository<TKey, TValue> : IKeyValueRepository<TKey, TValue>
    {
        static ConcurrentDictionary<TKey, TValue> dictionary;

        static InMemoryKeyValueRepository()
        {
            dictionary = new ConcurrentDictionary<TKey, TValue>();
        }

        public void Add(TKey key, TValue value)
        {
            dictionary.TryAdd(key, value);
        }

        public TValue Get(TKey key)
        {
            return dictionary.GetValueOrDefault(key);
        }

        public IQueryable<KeyValuePair<TKey, TValue>> GetAll()
        {
            return dictionary.AsQueryable();
        }

        public void Update(TKey key, TValue value)
        {
            var oldValue = dictionary.GetValueOrDefault(key);
            dictionary.TryUpdate(key, value, oldValue);
        }

        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }
    }
}
