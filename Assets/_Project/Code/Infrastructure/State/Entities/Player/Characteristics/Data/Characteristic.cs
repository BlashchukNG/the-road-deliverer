using System;

namespace Infrastructure.State.Entities.Player.Characteristics.Data
{
	[Serializable]
	public sealed class Characteristic
	{
		public CharacteristicType type;
		public int level;
		public float progress;
		public float modificator;
	}
}