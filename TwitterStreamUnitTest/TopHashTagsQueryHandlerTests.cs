using Moq;
using TwitterStream.Contract.RepositoryContract;
using TwitterStream.Domain;

namespace TwitterStream.QueryHandlers.Tests
{
    public class TopHashTagsQueryHandlerTests
    {
        private readonly Mock<IKeyValueRepository<string, int>> _mockRepo;

        public TopHashTagsQueryHandlerTests()
        {
            _mockRepo = new();
        }

        [Fact()]
        public void Handle_Should_ReturnInCorrectOrders()
        {
            Dictionary<string, int> tags = new Dictionary<string, int>();
            tags.Add("tag1", 5);
            tags.Add("tag2", 4);
            tags.Add("tag3", 10);
            _mockRepo.Setup(x => x.GetAll()).Returns(tags.AsQueryable());

            TopHashTagsQueryHandler handler = new TopHashTagsQueryHandler(_mockRepo.Object);
            var result = handler.Handle(new TopHashTagsQuery(10));

            List<string> expected = new List<string>();
            expected.Add("tag3");
            expected.Add("tag1");
            expected.Add("tag2");
            // should be in ordered descending
            Assert.True(expected.SequenceEqual(result.Tags));
        }

        [Fact()]
        public void Handle_Should_ReturnTopTen()
        {
            Dictionary<string, int> tags = new Dictionary<string, int>();
            // generate 20 tags
            for (int i = 0; i < 20; i++)
            {
                tags.Add("tag" + i, i);
            }
            _mockRepo.Setup(x => x.GetAll()).Returns(tags.AsQueryable());

            TopHashTagsQueryHandler handler = new TopHashTagsQueryHandler(_mockRepo.Object);
            var result = handler.Handle(new TopHashTagsQuery(10));

            // check top item value
            Assert.Equal("tag19", result.Tags[0]);
            // should only returns top 10
            Assert.Equal(10, result.Tags.Count);
        }

        [Fact()]
        public void Handle_Should_ReturnTopFive()
        {
            Dictionary<string, int> tags = new Dictionary<string, int>();
            // generate 20 tags
            for (int i = 0; i < 20; i++)
            {
                tags.Add("tag" + i, i);
            }
            _mockRepo.Setup(x => x.GetAll()).Returns(tags.AsQueryable());

            TopHashTagsQueryHandler handler = new TopHashTagsQueryHandler(_mockRepo.Object);
            var result = handler.Handle(new TopHashTagsQuery(5));

            // should only returns top 5
            Assert.Equal(5, result.Tags.Count);
        }
    }
}