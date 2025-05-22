using System;

namespace Infrastructure.State.Entities.Player.Characteristics.Data
{
	[Serializable]
	public sealed class PrimaryCharacteristics : ICloneable
	{
		public Characteristic strength;
		public Characteristic dexterity;
		public Characteristic stamina;
		public Characteristic perception;
		public Characteristic intelligence;
		public Characteristic luck;
		
		public object Clone() => MemberwiseClone();
	}
}