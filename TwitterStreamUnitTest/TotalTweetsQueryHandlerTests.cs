using Moq;
using TwitterStream.Contract.RepositoryContract;
using TwitterStream.Domain;

namespace TwitterStream.QueryHandlers.Tests
{
    public class TotalTweetsQueryHandlerTests
    {
        private readonly Mock<IRepository<Tweet>> _mockRepo;

        public TotalTweetsQueryHandlerTests()
        {
            _mockRepo = new();
        }

        [Fact()]
        public void Handle_Should_Return_CorrectTotalCount()
        {
            List<Tweet> tweets = new List<Tweet>();
            tweets.Add(new Tweet());
            tweets.Add(new Tweet());
            tweets.Add(new Tweet());
            _mockRepo.Setup(x => x.GetAll()).Returns(tweets.AsQueryable());

            TotalTweetsQueryHandler handler = new TotalTweetsQueryHandler(_mockRepo.Object);
            var result = handler.Handle();

            Assert.Equal(3, result.Total);

        }

        [Fact()]
        public void Handle_Should_Return_Zero()
        {
            List<Tweet> tweets = new List<Tweet>();
            
            _mockRepo.Setup(x => x.GetAll()).Returns(tweets.AsQueryable());

            TotalTweetsQueryHandler handler = new TotalTweetsQueryHandler(_mockRepo.Object);
            var result = handler.Handle();

            Assert.Equal(0, result.Total);

        }
    }
}