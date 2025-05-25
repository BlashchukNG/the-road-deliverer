using System.Collections.Generic;
using System.Linq;
using Infrastructure.State.Entities.Player;
using Infrastructure.State.Entities.UserCamera;
using Infrastructure.State.GameResources;
using Infrastructure.State.GameResources.Configs;
using ObservableCollections;
using R3;

namespace Infrastructure.State.Root
{
	public sealed class GameStateProxy
	{
		private readonly GameState _state;
		private readonly List<ResourceConfig> _configs;
		public string Version { get; }

		public ObservableList<ResourceProxy> Resources { get; } = new();
		public CameraSettingsDataProxy CameraSettings { get; }
		public PlayerEntityProxy Player { get; }


		public GameStateProxy(GameState state, List<ResourceConfig> configs)
		{
			_state = state;
			_configs = configs;
			Version = state.version;
			Player = new PlayerEntityProxy(state.player);
			CameraSettings = new CameraSettingsDataProxy(state.cameraSettings);

			InitResources(state);
		}

		private void InitResources(GameState state)
		{
			state.resources.ForEach(resource => Resources.Add(new ResourceProxy(resource, _configs.First(c=>c.type == resource.type))));

			Resources.ObserveAdd().Subscribe(e => { state.resources.Add(e.Value.origin); });

			Resources.ObserveAdd().Subscribe(e => 
				{ state.resources.Remove(state.resources.FirstOrDefault(resource => resource.type == e.Value.Type)); });
		}

		public int GetEntityId() => _state.GetEntityId();
	}
}