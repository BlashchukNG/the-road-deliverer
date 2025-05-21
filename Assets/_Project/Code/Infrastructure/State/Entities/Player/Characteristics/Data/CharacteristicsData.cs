using System;

namespace Infrastructure.State.Entities.Player.Characteristics.Data
{
	[Serializable]
	public sealed class CharacteristicsData : ICloneable
	{
		public PrimaryCharacteristics primary;
		public SecondaryCharacteristics secondary;


		public CharacteristicsData(PrimaryCharacteristics primary, SecondaryCharacteristics secondary)
		{
			this.primary = primary;
			this.secondary = secondary;
		}

		public object Clone() => new CharacteristicsData(primary.Clone() as PrimaryCharacteristics, secondary.Clone() as SecondaryCharacteristics);
	}
}