using TwitterStream.Domain;
using TwitterStream.Contract.RepositoryContract;
using TwitterStream.Interfaces.Command;

namespace TwitterStream.CommandHandlers
{
    public class AddNewTweetCommandHandler : ICommandHandler<Tweet>
    {
        IRepository<Tweet> _repository;
        public AddNewTweetCommandHandler(IRepository<Tweet> repository)
        {
            _repository = repository;
        }
        public async Task Handle(Tweet tweet)
        {
            if (tweet == null)
            {
                throw new ArgumentNullException(nameof(tweet));
            }
            _repository.Add(tweet);

            await Task.Run(() => { });
        }
    }
}