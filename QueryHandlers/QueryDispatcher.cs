using TwitterStream.Interfaces;
using TwitterStream.Interfaces.Query;

namespace TwitterStream.QueryHandlers
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider _service;

        public QueryDispatcher(IServiceProvider service)
        {
            _service = service;
        }

        public TResult Send<T, TResult>(T query)
        {
            var handler = _service.GetService(typeof(IQueryHandler<T, TResult>));
            if (handler != null)
                return ((IQueryHandler<T, TResult>)handler).Handle(query);
            else
                throw new Exception($"Query doesn't have any handler {typeof(T).Name}");
        }

        public TResult Send<TResult>()
        {
            var handler = _service.GetService(typeof(IQueryHandler<TResult>));
            if (handler != null)
                return ((IQueryHandler<TResult>)handler).Handle();
            else
                throw new Exception($"Result entity doesn't have any handler {typeof(IResult).Name}");
        }
    }
}
