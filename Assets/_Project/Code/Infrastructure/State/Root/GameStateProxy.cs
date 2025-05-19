using Infrastructure.State.Entities.Player;

namespace Infrastructure.State.Root
{
	public sealed class GameStateProxy
	{
		//public ObservableList<PlayerEntityProxy> players { get; } = new();
		public PlayerEntityProxy Player { get; }

		public GameStateProxy(GameState state)
		{
			Player = new PlayerEntityProxy(state.player);

			//state.players.ForEach(p => players.Add(new PlayerEntityProxy(p)));
			// players.ObservableAdd().Subscribe(e =>
			// {
			// 	var addEntity = e.Value;
			// 	state.players.Add(new PlayerEntity
			// 	{
			// 		id = addEntity.Id,
			// 		typeId = addEntity.Id
			// 	});
			// });

			// players.ObservableRemove().Subscribe(e =>
			// {
			// 	var removeEntityProxy = e.Value;
			// 	var removeEntity = state.Players.FirstOrDefault(b=> b.id == removeEntityProxy.Id);
			// 	state.players.Remove(removeEntity);
			// });
		}
	}
}