using Moq;
using TwitterStream.Contract.RepositoryContract;
using TwitterStream.Domain;

namespace TwitterStream.CommandHandlers.Tests
{
    public class HashTagStatisticCommandHandlerTests
    {
        private readonly Mock<IKeyValueRepository<string, int>> _mockRepo;

        public HashTagStatisticCommandHandlerTests()
        {
            _mockRepo = new();
        }

        [Fact()]
        public async void Handle_Should_Success_IfInputDataIsNotNull()
        {
            var handler = new HashTagStatisticCommandHandler(_mockRepo.Object);

            var exception = await Record.ExceptionAsync(() => handler.Handle(new HashTagStatisticCommandHandlerEntity("tag1)")));
            Assert.Null(exception);
        }

        [Fact()]
        public async void Handle_Should_Fail_IfInputDataIsNull()
        {
            var handler = new HashTagStatisticCommandHandler(_mockRepo.Object);

            var exception = await Record.ExceptionAsync(() => handler.Handle(null));
            Assert.NotNull(exception);
        }
    }
}