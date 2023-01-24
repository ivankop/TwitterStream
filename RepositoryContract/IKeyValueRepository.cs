namespace TwitterStream.Contract.RepositoryContract
{
    public interface IKeyValueRepository<TKey, TValue>
    {
        public TValue Get(TKey key);
        public void Add(TKey key, TValue value);
        public void Update(TKey key, TValue value);
        public IQueryable<KeyValuePair<TKey, TValue>> GetAll();
        public bool ContainsKey(TKey key);
    }
}
