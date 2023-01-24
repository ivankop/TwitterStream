using TwitterStream.Contract.RepositoryContract;
using TwitterStream.Interfaces;
using TwitterStream.Interfaces.Query;

namespace TwitterStream.QueryHandlers
{
    public class TopHashTagsQueryHandler : IQueryHandler<TopHashTagsQuery, TopHashTagsQueryHandlerResult>
    {
        IKeyValueRepository<string, int> _repository;
        public TopHashTagsQueryHandler(IKeyValueRepository<string, int> repository)
        {
            _repository = repository;
        }

        public TopHashTagsQueryHandlerResult Handle(TopHashTagsQuery query)
        {
            var repo = _repository.GetAll();
            var tmp = repo.OrderByDescending(kv => kv.Value).Select(kv => kv).ToList();
            var tags = repo.OrderByDescending(kv => kv.Value).Select(kv => kv.Key).Take(query.Take).ToList();
            TopHashTagsQueryHandlerResult result = new TopHashTagsQueryHandlerResult(tags);
            return result;
        }
    }

    public class TopHashTagsQuery
    {
        public TopHashTagsQuery(int take)
        {
            Take = take;
        }

        public int Take { get; }
    }

    public class TopHashTagsQueryHandlerResult
    {
        public TopHashTagsQueryHandlerResult(IList<string> tags)
        {
            this.Tags = tags;
        }

        public IList<string> Tags { get; set; }
    }
}
