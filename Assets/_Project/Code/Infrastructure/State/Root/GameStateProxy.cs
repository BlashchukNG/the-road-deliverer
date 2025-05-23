using System.Linq;
using Infrastructure.State.Entities.Player;
using Infrastructure.State.Entities.UserCamera;
using Infrastructure.State.GameResources;
using ObservableCollections;
using R3;

namespace Infrastructure.State.Root
{
	public sealed class GameStateProxy
	{
		private readonly GameState _state;
		public string Version { get; }

		public ObservableList<ResourceProxy> Resources { get; } = new();
		public CameraSettingsDataProxy CameraSettings { get; }
		public PlayerEntityProxy Player { get; }


		public GameStateProxy(GameState state)
		{
			_state = state;
			Version = state.version;
			Player = new PlayerEntityProxy(state.player);
			CameraSettings = new CameraSettingsDataProxy(state.cameraSettings);

			InitResources(state);
		}

		private void InitResources(GameState state)
		{
			state.resources.ForEach(resource => Resources.Add(new ResourceProxy(resource)));

			Resources.ObserveAdd().Subscribe(e => { state.resources.Add(e.Value.origin); });

			Resources.ObserveAdd().Subscribe(e => 
				{ state.resources.Remove(state.resources.FirstOrDefault(resource => resource.type == e.Value.Type)); });
		}

		public int GetEntityId() => _state.GetEntityId();
	}
}