using System;

namespace Infrastructure.State.Root
{
	[Serializable]
	public sealed class GameSettingsState : ICloneable
	{
		public float volumeMusic;
		public float volumeSFX;
		
		public object Clone() => MemberwiseClone();
	}
}