using TwitterStream.Contract.RepositoryContract;
using TwitterStream.Domain;
using TwitterStream.Interfaces;
using TwitterStream.Interfaces.Query;

namespace TwitterStream.QueryHandlers
{
    public class TotalTweetsQueryHandler : IQueryHandler<TotalTweetsQueryHandlerResult>
    {
        IRepository<Tweet> _repository;
        public TotalTweetsQueryHandler(IRepository<Tweet> repository)
        {
            _repository = repository;
        }
        public TotalTweetsQueryHandlerResult Handle()
        {
            var result = new TotalTweetsQueryHandlerResult(_repository.GetAll().Count());
            return result;
        }
    }

    public class TotalTweetsQueryHandlerResult : IResult
    {
        public int Total;

        public TotalTweetsQueryHandlerResult(int total)
        {
            Total = total;
        }
    }
}