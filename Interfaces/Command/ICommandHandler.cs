namespace TwitterStream.Interfaces.Command
{
    public interface ICommandHandler<T>
    {
        Task Handle(T command);
    }
    public interface ICommandDispatcher
    {
        void Send<T>(T command);
    }
}