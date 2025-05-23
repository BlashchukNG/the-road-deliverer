using System;
using System.Collections.Generic;

namespace Infrastructure.State.CMD
{
	public interface ICommandProcessor
	{
		void RegisterCommand<TCommand>(TCommand command)
			where TCommand : ICommand;

		bool ProcessCommand<TCommand>(TCommand command)
			where TCommand : ICommand;
	}

	public class CommandProcessor : ICommandProcessor
	{
		private readonly IGameStateProvider _gameStateProvider;
		private readonly Dictionary<Type, object> _handlesMap = new();


		public CommandProcessor(IGameStateProvider gameStateProvider)
		{
			_gameStateProvider = gameStateProvider;
		}

		public void RegisterCommand<TCommand>(TCommand command)
			where TCommand : ICommand
		{
			_handlesMap[typeof(TCommand)] = command;
		}

		public bool ProcessCommand<TCommand>(TCommand command)
			where TCommand : ICommand
		{
			if (_handlesMap.TryGetValue(command.GetType(), out object handler))
			{
				var handlerType = (ICommandHandler<TCommand>)handler;
				var result = handlerType.Handle(command);
				if (result) _gameStateProvider.SaveGameState();
				return result;
			}

			return false;
		}
	}
}