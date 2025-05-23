namespace Infrastructure.State.CMD.Abstract
{
	public interface ICommandHandler<TCommand>
		where TCommand : ICommand
	{
		bool Handle(TCommand command);
	}
}