using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterStream.Contract.RepositoryContract;
using TwitterStream.Interfaces.Command;

namespace TwitterStream.CommandHandlers
{
    public class HashTagStatisticCommandHandler : ICommandHandler<HashTagStatisticCommandHandlerEntity>
    {
        IKeyValueRepository<string, int> _repository;
        public HashTagStatisticCommandHandler(IKeyValueRepository<string, int> repository)
        {
            _repository = repository;
        }
        public async Task Handle(HashTagStatisticCommandHandlerEntity entity)
        {
            var tag = entity.Tag;
            if (!_repository.ContainsKey(tag))
            {
                _repository.Add(tag, 1);
            }
            else
            {
                var currentCount = _repository.Get(tag);
                _repository.Update(tag, currentCount + 1);
            }

            await Task.Run(() => { });
        }
    }

    public class HashTagStatisticCommandHandlerEntity
    {
        public HashTagStatisticCommandHandlerEntity(string tag)
        {
            Tag = tag;
        }

        public string Tag { get; set; }
    }
}
