
namespace TwitterStream.Interfaces.Query
{

    public interface IQueryHandler<TQuery, TResult> {
        TResult Handle(TQuery query);
    }

    public interface IQueryHandler<TResult> {
        TResult Handle();
    }

    public interface IQueryDispatcher
    {
        TResult Send<T, TResult>(T query);
        TResult Send<TResult>();
    }
}