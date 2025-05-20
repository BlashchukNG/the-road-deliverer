using System;
using Infrastructure.State.Entities.Player;
using Infrastructure.State.Entities.UserCamera;

namespace Infrastructure.State.Root
{
	[Serializable]
	public sealed class GameState : ICloneable
	{
		public string version = "1.0";
		public CameraSettingsData cameraSettings;
		public PlayerEntity player;
		public object Clone() => MemberwiseClone();
	}
}