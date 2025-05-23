using Infrastructure.State.Entities.Player;
using Infrastructure.State.Entities.UserCamera;

namespace Infrastructure.State.Root
{
	public sealed class GameStateProxy
	{
		private readonly GameState _state;

		//public ObservableList<PlayerEntityProxy> players { get; } = new();
		public string Version { get; }
		public CameraSettingsDataProxy CameraSettings { get; }
		public PlayerEntityProxy Player { get; }


		public GameStateProxy(GameState state)
		{
			_state = state;
			Version = state.version;
			Player = new PlayerEntityProxy(state.player);
			CameraSettings = new CameraSettingsDataProxy(state.cameraSettings);

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

		public int GetEntityId() => ++_state.globalEntityId;
	}
}