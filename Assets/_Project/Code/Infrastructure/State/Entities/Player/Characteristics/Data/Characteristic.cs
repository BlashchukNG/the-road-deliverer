using System;

namespace Infrastructure.State.Entities.Player.Characteristics.Data
{
	[Serializable]
	public sealed class Characteristic
	{
		public string titleLocKey;
		public string descriptionLocKey;
		
		public CharacteristicType type;
		public int level;
		public float progress;
		public float modificator;
	}
}