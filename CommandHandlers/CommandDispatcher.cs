using TwitterStream.Interfaces.Command;

namespace TwitterStream.CommandHandlers
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _service;

        public CommandDispatcher(IServiceProvider service)
        {
            _service = service;
        }

        public void Send<T>(T command)
        {

            var handler = _service.GetService(typeof(ICommandHandler<T>));

            if (handler != null)
                ((ICommandHandler<T>)handler).Handle(command);
            else
                throw new Exception($"This entity doesn't have any handler { typeof(T).Name }");

        }
    }
}
