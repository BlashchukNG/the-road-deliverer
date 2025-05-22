using System;

namespace Infrastructure.State.Entities.Player.Characteristics.Data
{
	[Serializable]
	public sealed class SecondaryCharacteristics : ICloneable
	{
		public Characteristic athleticism;
		public Characteristic driving;
		
		public object Clone() => MemberwiseClone();
	}
}