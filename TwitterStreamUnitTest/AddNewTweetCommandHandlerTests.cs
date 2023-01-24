using Moq;
using TwitterStream.Contract.RepositoryContract;
using TwitterStream.Domain;

namespace TwitterStream.CommandHandlers.Tests
{
    public class AddNewTweetCommandHandlerTests
    {
        private readonly Mock<IRepository<Tweet>> _mockRepo;

        public AddNewTweetCommandHandlerTests()
        {
            _mockRepo = new();
        }
        [Fact()]
        public async void Handle_Should_Success_IfTweetDataIsNotNull()
        {
            var handler = new AddNewTweetCommandHandler(_mockRepo.Object);            

            var exception = await Record.ExceptionAsync(() => handler.Handle(new Tweet()));
            Assert.Null(exception);
        }

        [Fact()]
        public async void Handle_Should_Fail_IfTweetDataIsNull()
        {
            var handler = new AddNewTweetCommandHandler(_mockRepo.Object);          

            var exception = await Record.ExceptionAsync(() => handler.Handle(null));
            Assert.NotNull(exception);
        }
    }
}