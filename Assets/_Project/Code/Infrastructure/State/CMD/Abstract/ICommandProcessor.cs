namespace Infrastructure.State.CMD.Abstract
{
	public interface ICommandProcessor
	{
		void RegisterHandler<TCommand>(ICommandHandler<TCommand> command)
			where TCommand : ICommand;

		bool ProcessCommand<TCommand>(TCommand command)
			where TCommand : ICommand;
	}
}