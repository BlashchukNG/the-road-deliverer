using System;
using System.Collections.Generic;
using Infrastructure.State.Entities.Player;
using Infrastructure.State.Entities.UserCamera;
using Infrastructure.State.GameResources;

namespace Infrastructure.State.Root
{
	[Serializable]
	public sealed class GameState : ICloneable
	{
		public string version = "1.0";
		public int globalEntityId;
		public List<ResourceData> resources = new();
		public CameraSettingsData cameraSettings;
		public PlayerEntity player;

		public int GetEntityId() => ++globalEntityId;

		public object Clone()
		{
			return new GameState
			{
				cameraSettings = this.cameraSettings.Clone() as CameraSettingsData,
				player = this.player.Clone() as PlayerEntity
			};
		}
	}
}